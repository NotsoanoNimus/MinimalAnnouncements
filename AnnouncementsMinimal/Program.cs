using AnnouncementsMinimal.Models.Announcement;


var builder = WebApplication.CreateBuilder(args);


// Create the database context, and set up a new local SQLite database if one doesn't already exist.
var dbCtx = new AnnouncementDb();

if(builder.Environment.IsDevelopment())
    dbCtx.DatabaseName = AnnouncementDb.DEV_DBNAME;

dbCtx.Database.EnsureCreated();

// Add dummy data if this is the first time the database is being created.
if(!dbCtx.Announcements.Any() && builder.Environment.IsDevelopment()) {
    // TODO: This is not working for test cases.
    dbCtx.Announcements.AddRange(new Announcement[] {
        new ( new(), "Admin", "Test Sample 1", "This is the first test sample." ),
        new ( new(), "Admin", "Test Sample 2", "This is the __second__ test sample."),
        new ( new(), "Admin", "Test Sample 3", "This is the _third_ test sample."),
        new ( new(), "Admin", "Test Sample 4", "This is the ___fourth___ test sample."),
        new ( new(), "AdminX", "Test Sample 5", "This is the __fifth__ test sample."),
        new ( new(), "AdminY", "Test Sample 6", "This is the __sixth__ test sample."),
        new ( new(), "AdminZ", "Test Sample 7", "This is the __seventh__ test sample."),
        new ( new(), "AdminA", "Test Sample 8", "This is the __eighth__ test sample."),
        new ( new(), "AdminA", "Test Sample 9", "This is the ninth `test` sample."),
        new ( new(), "AdminA", "Test Sample 10", "This is the *tenth* test sample."),
        new ( new(), "AdminA", "Test Sample 11", "This is \\*the **eleventh** test sample."),
        new ( new(), "AdminA", "Test Sample 12", "This is the `twelfth` test sample."),
    });

    dbCtx.SaveChanges();
}


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
        Title = "Announcements API", Description = "Announcements API for website portal."
    });
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Add routes for API operations.

// TODO: Add async capability
var announcementController = new AnnouncementMiniController(ref dbCtx);

app.MapDelete("/announcement/{id:int}", async (int id) => await announcementController.DeleteAnnouncement(id));

app.MapPost("/announcement", (Announcement? a) => announcementController.CreateAnnouncement(a));

app.MapPut("/announcement/{id:int}", async (int id, Announcement a) => await announcementController.UpdateAnnouncement(id, a));

app.MapGet("/announcement/{id:int}", (int id, uint? pageNumber, uint? resultsPerPage) => announcementController.GetAnnouncement(id));

app.MapGet("/announcement", (uint? pageNumber, uint? resultsPerPage) => announcementController.GetAnnouncements(pageNumber, resultsPerPage));


app.Run();