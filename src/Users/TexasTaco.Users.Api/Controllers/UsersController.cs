using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TexasTaco.Shared.Authentication;
using TexasTaco.Users.Core.Dtos;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Repositories;
using TexasTaco.Users.Core.ValueObjects;

namespace TexasTaco.Users.Api.Controllers
{
    [ApiVersion(1)]
    [Route("api/users/v{v:apiVersion}")]
    [ApiController]
    public class UsersController(IUsersRepository _usersRepository) : ControllerBase
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

            if(user.AccountId.ToString() != currentUserAccountId)
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

            await _usersRepository.UpdateUserAsync(user);

            return NoContent();
        }
    }
}
