using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<Object> GetUsers();

        Task<Object> GetUser(long id);

        Task<bool> UserExists(string username);

        Task<bool> CreateUser(User user);

        Task<bool> UserExists(long id);

        Task<bool> UpdateUser(User user);

        Task<bool> DeleteUser(User user);

        Task<User> GetIndUser(long id);
    }
}
