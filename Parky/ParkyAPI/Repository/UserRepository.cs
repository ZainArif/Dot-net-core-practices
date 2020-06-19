using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using ParkyAPI.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly AppSettings _appSettings;
        private readonly IDataProtector _dataProtector;

        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings, IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _db = db;
            _appSettings = appSettings.Value;
            _dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.SecurePasswordKey);
        }
        public User Authenticate(string username, string password)
        {
            //var user = _db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            var user = _db.Users.SingleOrDefault(x => x.Username == username.Trim());

            //user not found
            if (user == null)
            {
                return null;
            }

            var unSecurePassword = _dataProtector.Unprotect(user.Password);
            if (unSecurePassword != password)
            {
                return null;
            }


            //if user was found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescrirptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                                         (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescrirptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == username);

            //user not found
            if (user == null)
            {
                return true;
            }

            return false;
        }

        public User Register(string username, string password)
        {
            var securePassword = _dataProtector.Protect(password.Trim());

            User userObj = new User()
            {
                Username = username.Trim(),
                Password = securePassword,
                Role = "Admin"
            };

            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return userObj;
        }
    }
}
