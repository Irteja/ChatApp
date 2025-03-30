using Microsoft.EntityFrameworkCore;
using chatapp.Entities;
namespace  chatapp.Data; // Replace with your project's namespace

	public class ChatAppDbContext : DbContext
	{
    	public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
        	: base(options)
    	{
    	}

    	// Add DbSet properties for your entities (database tables) here:
    	// public DbSet<YourEntity> YourEntities { get; set; } // Example
    	public DbSet<User> Users { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Conversation> Conversations { get; set; }
	}

