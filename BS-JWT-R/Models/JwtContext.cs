using BS_JWT_R.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BS_JWT_R.Models
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {

        }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
