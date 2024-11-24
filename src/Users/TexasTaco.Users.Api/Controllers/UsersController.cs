using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Users.Core.Data.EF;
using TexasTaco.Users.Core.Dtos;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Repositories;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/users")]
    [ApiController]
    public class UsersController(
        IUnitOfWork _unitOfWork,
        IUsersRepository _usersRepository,
        IUserUpdatedOutboxMessagesRepository _userUpdatedOutboxMessagesRepository)
        : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetUser(string accountId)
        {
            var user = await _usersRepository
                .GetByAccountIdAsync(Guid.Parse(accountId));

            if (user is null)
            {
                return NotFound($"User with associated {accountId} account id not found.");
            }

            var userDto = new UserDto(
                user.Id.Value.ToString(),
                user.Email.Value.ToString(),
                user.FirstName,
                user.LastName,
                new AddressDto(
                    user.Address.AddressLine,
                    user.Address.PostalCode,
                    user.Address.City,
                    user.Address.Country));

            return Ok(userDto);
        }

        [MapToApiVersion(1)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserToUpdateDto userToUpdateDto)
        {
            var user = await _usersRepository
                .GetByIdAsync(new UserId(Guid.Parse(id)));

            if (user is null)
            {
                return NotFound($"User with {id} id not found.");
            }

            string currentUserAccountId = User.FindFirst(TexasTacoClaimNames.AccountId)!.Value;

            if (user.AccountId.ToString() != currentUserAccountId)
            {
                return Unauthorized();
            }

            var address = new Address(
                userToUpdateDto.Address.AddressLine,
                userToUpdateDto.Address.PostalCode,
                userToUpdateDto.Address.City,
                userToUpdateDto.Address.Country);

            user.UpdateUser(
                userToUpdateDto.FirstName,
                userToUpdateDto.LastName,
                address);

            var transaction = await _unitOfWork.BeginTransactionAsync();

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

            var outboxMessage = new UserUpdatedOutboxMessage(outboxMessageBody);

            await _userUpdatedOutboxMessagesRepository.AddAsync(outboxMessage);

            await transaction.CommitAsync();

            return NoContent();
        }
    }
}
