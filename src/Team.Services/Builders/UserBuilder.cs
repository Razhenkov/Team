using Team.Services.Domains;

namespace Team.Services.Builders
{
    public class UserBuilder
    {
        public User Build(Data.Entities.User entity)
        {
            return new User
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                EmailAddress = entity.EmailAddress
            };
        }
    }
}
