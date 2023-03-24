using Microsoft.EntityFrameworkCore;
using PhoneBookAppApi.Models;

namespace PhoneBookAppApi
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
