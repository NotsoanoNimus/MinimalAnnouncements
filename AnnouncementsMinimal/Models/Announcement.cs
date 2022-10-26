using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

using AnnouncementsMinimal.Pager;


namespace AnnouncementsMinimal.Models.Announcement;


/// <summary>
/// Represents an Announcement record in the DB.
/// </summary>
/// <param name="Id"></param>
/// <param name="Date"></param>
/// <param name="Author"></param>
/// <param name="Subject"></param>
/// <param name="Message"></param>
public sealed record Announcement(
    [Required] DateTime Date,
    [Required, MaxLength(128)] string Author,
    [Required, MaxLength(256)] string Subject,
    [Required] string Message
) {
    [Key] public int Id { get; init; }
};


/// <summary>
/// Database Context class extension for Announcements model. Should set up SQLite db for app use.
/// </summary>
public sealed class AnnouncementDb : DbContext {
    public static readonly string DEV_DBNAME = "AnnouncementsDB_DEV";

    //public AnnouncementDb(DbContextOptions<AnnouncementDb> options) : base(options) { }

    public string DatabaseName { get; set; } = "AnnouncementsDB";

    public DbSet<Announcement> Announcements { get; set; } = null;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite($"Filename={this.DatabaseName}.sqlite", opts => {
            opts.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Announcement>().ToTable($"{nameof(Announcement)}s");
        modelBuilder.Entity<Announcement>(ent => {
            ent.HasKey(ent => ent.Id);
            ent.HasIndex(ent => ent.Author);
            ent.Property(ent => ent.Date).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        base.OnModelCreating(modelBuilder);
    }
}


public sealed class AnnouncementMiniController {

    private readonly AnnouncementDb _ctx;

    public AnnouncementMiniController(ref AnnouncementDb ctx) => this._ctx = ctx;


    public Announcement CreateAnnouncement(Announcement announcement) {
        // Check authentication and authorization (use decorator?).

        // TODO: Async and exception handling.
        this._ctx.Announcements.Add(announcement);
        this._ctx.SaveChanges();

        return announcement;
    }

    // DESIGN SHORTFALL TODO TODO: This grabs the ENTIRE dataset before returning the paginated results.
    //   Thus, this is horribly inefficient and needs some fixing.
    public ApiResponse<Announcement> GetAnnouncements(uint? pageNumber, uint? resultsPerPage)
        => (new PaginatedResponse<Announcement>(this._ctx.Announcements.ToArray(), pageNumber, resultsPerPage)).GetResponse();

    public async Task<Announcement?> GetAnnouncement(int id)
        => (id < 0) ? null : await this._ctx.Announcements.FirstAsync(x => x.Id == id);

    public Announcement? UpdateAnnouncement(int id, Announcement a) {
        return null;
    }

    public Announcement? DeleteAnnouncement(int id) {
        // Check authentication and authorization (use decorator?).

        var ann = this.GetAnnouncement(id);

        if(ann != null) {

        }

        return null;
    }
};