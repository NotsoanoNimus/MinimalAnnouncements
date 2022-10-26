# MinimalAnnouncements
A minimal .NET Core (6.0) Web API for paginated Markdown Announcements.

This is supposed to represent simple CRUD functionality for a messaging/notifications API that displays announcements to users when they log onto a system.

The requirement for this prompt also included a blurb about data `pagination`, as eventually the list of _Announcement_ entities could get pretty large.

___TIME LIMIT: 2 HOURS___. After this limit, commits (except this README update) stopped, per the instructions given.


### Dependencies
Just need `Swagger` and the latest `EntityFramework` packs for core and SQLite. See the project file for details on versioning.


### Assumptions
Here's a list of assumptions that were made when sketching out the project initially:

- The backend database can be swapped around, but SQLite was used in this example for its minimal setup and its simplicity.
- As mentioned in the prompt, Authentication and Authorization can be set up or handled elsewhere, or this CRUD API is walled off some other way so that it's already protected.
- This is specifically an Announcements microservice; it is not part of a larger service. Ergo the minimal API boilerplate and the snap choice to skip out on using complex controllers or other abstractions.


### Missed Marks
This section lists a few marks that were missed, whether from requirements or in general, due to time constraints.

- Bad error handling. There are likely many avenues to stack traces in this.
- Poor API documentation, if any at all. Literally just included a small Swagger addition in the main `Program.cs` and nothing more. But all the in-Swagger testing works and is super helpful.
- The API uses some kind of minimalistic pseudo-Controller for `Announcements` because it's a microservice API (noted above), but this would probably need to be entirely rewritten elsewhere in a real scenario.
- There wasn't really a consideration for Markdown, though I didn't see any requirement about parsing and displaying it anywhere.
- There's probably a much simpler and more efficient way to page results.
- Docker was not tested.


-----

Thank you!