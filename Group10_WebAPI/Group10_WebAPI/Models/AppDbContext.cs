using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Group10_WebAPI.Models
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Game> SpikeballGames { get; set; }
        public DbSet<GameResponse> GameResponses { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Game>().ToTable("SpikeballGames");
            modelBuilder.Entity<User>().ToTable("Users").Property(u => u.UserId).ValueGeneratedOnAdd(); // Ensures auto-increment;
            modelBuilder.Entity<GameResponse>().ToTable("GameResponses");
            modelBuilder.Entity<Feedback>().ToTable("Feedbacks");
        }

        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string username = "admin";
            string password = "Admin123!";
            string roleName = "Admin";

            // If the role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // If the user doesn't exist, create it
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User { UserName = username, Email = "admin@example.com" }; // Don't set UserId here

                // Create the user without manually setting the UserId
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // No need to manually set the UserId or change it here
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }



    }

}
