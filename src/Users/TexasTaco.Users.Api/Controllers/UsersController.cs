using Asp.Versioning;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Shared.Authentication;
using TexasTaco.Shared.Authentication.Attributes;
using TexasTaco.Shared.EventBus.Users;
using TexasTaco.Shared.Outbox;
using TexasTaco.Shared.Outbox.Repository;
using TexasTaco.Shared.Pagination;
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
        IOutboxMessagesRepository<OutboxMessage<UserUpdatedEventMessage>>
            _userUpdatedOutboxMessagesRepository)
        : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpGet("for-account/{accountId}")]
        public async Task<IActionResult> GetUserByAccountId(string accountId)
        {
            var user = await _usersRepository
                .GetByAccountIdAsync(Guid.Parse(accountId));

            if (user is null)
            {
                return NotFound($"User with associated {accountId} account id not found.");
            }

            var userDto = new UserDto(
                user.Id.Value,
                user.Email.Value.ToString(),
                user.FullName,
                new AddressDto(
                    user.Address.AddressLine,
                    user.Address.PostalCode,
                    user.Address.City,
                    user.Address.Country),
                user.PointsCollected);

            return Ok(userDto);
        }

        [MapToApiVersion(1)]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var id = new UserId(Guid.Parse(userId));
            var user = await _usersRepository.GetByIdAsync(id);

            if (user is null)
            {
                return NotFound($"User with id {userId} not found.");
            }

            var userDto = new UserDto(
                user.Id.Value,
                user.Email.Value.ToString(),
                user.FullName,
                new AddressDto(
                    user.Address.AddressLine,
                    user.Address.PostalCode,
                    user.Address.City,
                    user.Address.Country),
                user.PointsCollected);

            return Ok(userDto);
        }

        [AuthorizeRole(Role.Admin)]
        [MapToApiVersion(1)]
        [HttpGet()]
        public async Task<IActionResult> GetUsers(
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] string? searchQuery)
        {
            IEnumerable<User> users;

            if (pageNumber is null
                && pageSize is null
                && searchQuery is null)
            {
                users = await _usersRepository
                    .GetUsers();

                var response = GetUsersListDto(users);

                return Ok(response);
            }

            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be a positive number.");
            }

            if (pageSize <= 0)
            {
                return BadRequest("Page size must be a positive number.");
            }

            var pagedUsers = await _usersRepository
                .GetPagedUsersAsync(
                    (int)pageNumber!,
                    (int)pageSize!,
                    searchQuery);

            var usersDtos = GetUsersListDto(pagedUsers.Items);

            var pagedResultDto = new PagedResult<UsersListDto>(
                usersDtos,
                pagedUsers.TotalCount,
                pagedUsers.PageSize,
                pagedUsers.CurrentPage);

            return Ok(pagedResultDto);
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

            await _userUpdatedOutboxMessagesRepository.AddAsync(outboxMessage);

            await transaction.CommitAsync();

            return NoContent();
        }

        private static IEnumerable<UsersListDto> GetUsersListDto(
            IEnumerable<User> users)
        {
            return users
                .Select(u => new UsersListDto(
                    u.Id.Value,
                    u.Email.Value.ToString(),
                    u.FullName,
                    new AddressDto(
                        u.Address.AddressLine,
                        u.Address.PostalCode,
                        u.Address.City,
                        u.Address.Country),
                    u.PointsCollected)
                );
        }
    }
}
