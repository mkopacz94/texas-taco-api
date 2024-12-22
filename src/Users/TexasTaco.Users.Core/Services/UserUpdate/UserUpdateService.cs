using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Exceptions;
using TexasTaco.Users.Core.Repositories;

namespace TexasTaco.Users.Core.Services.UserUpdate
{
    internal class UserUpdateService(
        IUnitOfWork _unitOfWork,
        IUsersRepository _usersRepository,
        IOutboxMessagesRepository<OutboxMessage<UserUpdatedEventMessage>>
            _outboxRepository) : IUserUpdateService
    {
        public async Task UpdateUser(User user)
        {
            try
            {
                using var transaction = await _unitOfWork.BeginTransactionAsync();

                await _usersRepository.UpdateUserAsync(user);

                var outboxMessageBody = new UserUpdatedEventMessage(
                    Guid.NewGuid(),
                    user.AccountId,
                    user.FirstName!,
                    user.LastName!,
                    user.Address.AddressLine,
                    user.Address.PostalCode,
                    user.Address.City,
                    user.Address.Country);

                var outboxMessage = new OutboxMessage<UserUpdatedEventMessage>(
                    outboxMessageBody);

                await _outboxRepository.AddAsync(outboxMessage);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new FailedToUpdateUserDataException(user, ex);
            }
        }
    }
}
