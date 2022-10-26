using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace AnnouncementsMinimalAPI.Models.Announcement;


/// <summary>
/// Represents an Announcement record in the DB.
/// </summary>
/// <param name="Id"></param>
/// <param name="Date"></param>
/// <param name="Author"></param>
/// <param name="Subject"></param>
/// <param name="Message"></param>
public sealed record Announcement(
    [Required] int Id,
    [Required] DateTime Date,
    [Required, MaxLength(128)] string Author,
    [Required, MaxLength(256)] string Subject,
    [Required] string Message
);


/// <summary>
/// Database Context class extension for Announcements model. Should set up SQLite db for app use.
/// </summary>
public sealed class AnnouncementDb : DbContext {
    //public AnnouncementDb(DbContextOptions<AnnouncementDb> options) : base(options) { }

    /// <summary>
    /// Used to add dummy data to the DB when the mode is development.
    /// </summary>
    /// <param name="ctx"></param>
    public static void AddDummyData(AnnouncementDb ctx) {
        ctx.Announcements.AddRange(new Announcement[] {
            new ( 0, new(), "Admin", "Test Sample 1", "This is the first test sample." ),
            new ( 1, new(), "Admin", "Test Sample 2", "This is the __second__ test sample."),
            new ( 2, new(), "Admin", "Test Sample 3", "This is the _third_ test sample."),
            new ( 3, new(), "Admin", "Test Sample 4", "This is the ___fourth___ test sample."),
            new ( 4, new(), "AdminX", "Test Sample 5", "This is the __fifth__ test sample."),
            new ( 5, new(), "AdminY", "Test Sample 6", "This is the __sixth__ test sample."),
            new ( 6, new(), "AdminZ", "Test Sample 7", "This is the __seventh__ test sample."),
            new ( 7, new(), "AdminA", "Test Sample 8", "This is the __eighth__ test sample."),
            new ( 8, new(), "AdminA", "Test Sample 9", "This is the ninth `test` sample."),
            new ( 9, new(), "AdminA", "Test Sample 10", "This is the *tenth* test sample."),
            new ( 10, new(), "AdminA", "Test Sample 11", "This is \\*the **eleventh** test sample."),
            new ( 11, new(), "AdminA", "Test Sample 12", "This is the `twelfth` test sample."),
        });

        ctx.SaveChanges();
    }

    public string DatabaseName { get; set; } = "AnnouncementsDB";

    public DbSet<Announcement> Announcements => Set<Announcement>();

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


    public Announcement? CreateAnnouncement(Announcement? announcement) {
        // Check authentication and authorization (use decorator?).

        return null;
    }

    public Task<Announcement[]> GetAnnouncements()
        => this._ctx.Announcements.ToArrayAsync();

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