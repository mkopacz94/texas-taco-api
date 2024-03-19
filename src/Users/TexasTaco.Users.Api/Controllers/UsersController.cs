using Microsoft.AspNetCore.Mvc;
using TexasTaco.Shared.Authentication;
using TexasTaco.Users.Core.Dtos;
using TexasTaco.Users.Core.Entities;
using TexasTaco.Users.Core.Repositories;

namespace TexasTaco.Users.Api.Controllers
{
    [Route("api/users")]
    public class UsersController(IUsersRepository _usersRepository) : ControllerBase
    {
        [HttpPut("{accountId}")]
        public async Task<IActionResult> UpdateUser(string accountId, [FromBody] UserToUpdateDto userToUpdateDto)
        {
            string currentUserAccountId = User.FindFirst(TexasTacoClaimNames.AccountId)!.Value;

            if(accountId != currentUserAccountId)
            {
                return Unauthorized();
            }

            var user = await _usersRepository
                .GetByAccountIdAsync(Guid.Parse(accountId));

            if(user is null)
            {
                return NotFound($"User with associated " +
                    $"{accountId} accountId has not been found.");
            }

            var address = new Address(
                userToUpdateDto.Address.AddressLine,
                userToUpdateDto.Address.PostalCode,
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
