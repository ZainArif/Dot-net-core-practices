using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Repository.IRepository;
using WebApi.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WebApiContext _context;

        public UserRepository(WebApiContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            _context.User.Add(user);
            return await Save();
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.User.Update(user);
            return await Save();
        }

        public async Task<Object> GetUsers()
        {
            var users = await (from u in _context.User
                               join r in _context.Roles on u.RoleId equals r.RoleId
                               select new { u.Id, u.Name, r.RoleId, r.RoleName }
                              ).ToListAsync();
            return users;

            //using (var query = _context.Database.GetDbConnection().CreateCommand())
            //{
            //    query.CommandText = "select * from dbo.roles";
            //    _context.Database.OpenConnection();
            //    var result = query.ExecuteReader();
            //    var user = result.GetString(1);
            //    _context.Database.CloseConnection();
            //    return user;
            //}
        }

        public async Task<Object> GetUser(long id)
        {
            var user = await (from u in _context.User
                               join r in _context.Roles on u.RoleId equals r.RoleId
                               where u.Id == id
                               select new { u.Id, u.Name, r.RoleId, r.RoleName }
                              ).ToListAsync();
            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.User.AnyAsync(u => u.Name == username);
        }

        public async Task<bool> UserExists(long id)
        {
            return await _context.User.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() >= 0 ? true : false;
        }

        public async Task<bool> DeleteUser(User user)
        {
            _context.User.Remove(user);
            return await Save();
        }

        public async Task<User> GetIndUser(long id)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
