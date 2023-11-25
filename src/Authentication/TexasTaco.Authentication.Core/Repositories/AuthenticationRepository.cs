using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasTaco.Authentication.Core.Data.EF;

namespace TexasTaco.Authentication.Core.Repositories
{
    internal class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AuthDbContext _dbContext;

        public AuthenticationRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void TestMethod()
        {
            var data = _dbContext.Accounts.FirstOrDefault();
        }
    }
}
