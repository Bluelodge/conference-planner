# Conference Planner

Simple conference schedule planner implementation with **ASP.NET Core 6.0**.


## Setup

:white_medium_square: Create intial Sqlite database schema for BackEnd.
```
cd BackEnd
dotnet ef database update
```

:white_medium_square: Run BackEnd and add conference info using [Techorama template](BackEnd/Data/TechoramaDataLoader.cs).

:white_medium_square: Create intial Sqlite database schema for FrontEnd.
```
cd FrontEnd
dotnet ef database update
```

:white_medium_square: Add API port in FrontEnd appsettings.json.

:white_medium_square: Setup [Google OAuth](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-6.0) on FrontEnd for development environment.


## Get started

:white_medium_square: [Run both](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022) BackEnd and Frontend.

:white_medium_square: Register/login Admin and Attendees (only the first user is admin).


### Admin User
:heavy_check_mark: Edit conference info.

:heavy_check_mark: Delete conference from general agenda.

### Attendee User
:heavy_check_mark: Add or delete conference to personal agenda.

