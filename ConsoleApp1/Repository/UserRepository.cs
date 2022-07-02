using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository
{
    public class UserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
        public bool IsExist(string login)
        {
            return _context.Users.Any(e => e.Login.Equals(login));
        }
        public async Task AddUser(string name, string login)
        {
            _context.Users.Add(new User
            {
                Login = login,
                Name = name
            });
            await _context.SaveChangesAsync();
        }
    }
}
