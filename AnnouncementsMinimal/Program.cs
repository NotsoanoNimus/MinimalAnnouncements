using AnnouncementsMinimalAPI.Models.Announcement;


var builder = WebApplication.CreateBuilder(args);


// Create the database context, and set up a new local SQLite database if one doesn't already exist.
var dbCtx = new AnnouncementDb();

if(builder.Environment.IsDevelopment())
    dbCtx.DatabaseName = "AnnouncementsDB_DEV";

dbCtx.Database.EnsureCreated();

// Add dummy data if this is the first time the database is being created.
if(!dbCtx.Announcements.Any() && builder.Environment.IsDevelopment())
    AnnouncementDb.AddDummyData(dbCtx);


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
var announcementController = new AnnouncementMiniController(ref dbCtx);

app.MapDelete("/announcement/{id:int}", (int id) => announcementController.DeleteAnnouncement(id));
app.MapPost("/announcement", (Announcement a) => announcementController.CreateAnnouncement(a));
app.MapPut("/announcement/{id:int}", (int id, Announcement a) => announcementController.UpdateAnnouncement(id, a));
app.MapGet("/announcement/{id:int}", (int id, uint? pageNumber, uint? resultsPerPage) => announcementController.GetAnnouncement(id));
app.MapGet("/announcement", async (uint? pageNumber, uint? resultsPerPage) => await announcementController.GetAnnouncements());


app.Run();