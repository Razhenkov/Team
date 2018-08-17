using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Team.Data.Dao;
using Team.Services.Builders;
using Team.Services.Domains;
using System.Linq;

namespace Team.Services
{
    public interface IUserServices
    {
        Task Add(string firstName, string lastName, string emailAddress, string password);
        Task<UserLoginResult> Login(string emailAddress, string password);
        Task<User> Get(string emailAddress);
        Task<List<User>> List();
    }

    public class UserServices : IUserServices
    {
        private readonly IUserDao userDao;
        private readonly UserBuilder userBuilder;

        public UserServices(IUserDao userDao, UserBuilder userBuilder)
        {
            this.userDao = userDao;
            this.userBuilder = userBuilder;
        }

        public async Task<UserLoginResult> Login(string emailAddress, string password)
        {
            var user = await userDao.Get(emailAddress);

            if (user == null || user.Deleted)
                return UserLoginResult.NotFound;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return UserLoginResult.InvalidCredentials;

            return UserLoginResult.Success;
        }

        public async Task Add(string firstName, string lastName, string emailAddress, string password)
        {
            var passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);

            await userDao.Add(firstName, lastName, emailAddress, hashedPassword, passwordSalt);
        }

        public async Task<User> Get(string emailAddress)
        {
            var user = await userDao.Get(emailAddress);

            return userBuilder.Build(user);
        }

        public async Task<List<User>> List()
        {
            var users = await userDao.List();

            return users.Select(userBuilder.Build).ToList();
        }
    }
}
