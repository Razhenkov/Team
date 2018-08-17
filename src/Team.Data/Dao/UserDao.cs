using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Team.Data.Entities;

namespace Team.Data.Dao
{
    public interface IUserDao
    {
        Task Add(string firstName, string lastName, string emailAddress, string password, string passwordSalt);
        Task<User> Get(string emailAddress);
        Task<List<User>> List();
    }

    public class UserDao: IUserDao
    {
        private readonly DbSet<User> dbSet;
        private readonly DbContext dbContext;

        public UserDao(DbContext context)
        {
            this.dbSet = context.Set<User>();
            this.dbContext = context;
        }

        public async Task Add(string firstName, string lastName, string emailAddress, string password, string passwordSalt)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Password = password,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<User> Get(string emailAddress)
        {
            return await dbSet.SingleOrDefaultAsync(x => x.EmailAddress == emailAddress);
        }

        public async Task<List<User>> List()
        {
            return await dbSet.ToListAsync();
        }
    }
}
