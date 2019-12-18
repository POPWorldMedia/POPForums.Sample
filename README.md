POPForums Sample
================

This is a sample project showing how to light up POP Forums in an ASP.NET Core app by way of package references. For the backend, you need at a minimum these NuGet packages:

https://www.nuget.org/packages/PopForums.Mvc/

https://www.nuget.org/packages/PopForums.Sql/ 

You'll also need this NPM package:

https://www.npmjs.com/package/@popworldmedia/popforums 

The included gulpfile.js copies the client files as necessary. You also need the `Views/Shared/_Layout.cshtml` layout. Check the main project for options for the `PopForums.json` file and the `Startup` class.

## Project Links

For the latest information and documentation, and how to get started, check the docs:
https://popworldmedia.github.io/POPForums/

The main project on Github:
https://github.com/POPWorldMedia/POPForums

CI build of master, running on .NET Core is demo'ing here:
https://popforumsdev.azurewebsites.net/Forums

[![Build status](https://popw.visualstudio.com/POP%20Forums/_apis/build/status/popforumsdev)](https://popw.visualstudio.com/POP%20Forums/_build/latest?definitionId=2)

Latest release:
https://github.com/POPWorldMedia/POPForums/releases/tag/v16.0.0

The latest CI build packages can be found with these feeds on MyGet:
https://www.myget.org/F/popforums/api/v3/index.json
https://www.myget.org/F/popforums/npm/

