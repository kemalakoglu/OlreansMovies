# Changelog
All notable changes to this project will be documented in this file.

## [1.0.0] - 12-02-2022
### Added
-changeLog.md file

### Changed
- All projects versions raised to .net5

### Added
-Movies.GrainClients.Test project
-Movies.Grains.Test project
-Movies.Server.Test project

## [1.0.0] - 13-02-2022
### Added
-GrapphQL and Controller unit tests were defined into Movies.Server.Test project but not filled yet
-Task<IEnumerable<MovieModel>> GetList(string genre); interface was added into IMovieGrainClient interface
-Task<IEnumerable<MovieModel>> GetRatedFilms(); interface was added into IMovieGrainClient interface
-Task Update(string key, string name); interface was added into IMovieGrainClient interface
-MovieGrainClientTest unit tests were defined into Movies.Server.Test project but not filled yet
-MovieGrainClientTest unit tests were defined into Movies.GrainClients.Test project but not filled yet
-MovieGrainsTest unit tests were defined into Movies.Grains.Test project but not filled yet

## [1.0.0] - 14-02-2022
### Added
-MongoDB Driver Added
-Mongo Repository was written
-Redis Cache was added for all query operations
-MovieGrain and MovieGrainClient methods were written
-Movies Controller end-points were written
-ErrorHandlingMiddleware were created, all web api calls were logged by ELK
-ELK, Swagger, ZeppelingFramework.Core.Abstraction libraries were installed as extensions
-Base Response class, base error class were defined
-Predicator class were defined
-Mongo Repository Test project was opened
-Data migrations was done into MongoDB

## [1.0.0] - 15-02-2022
### Added
-MovieGrainClientTest.cs tests were written
-MoviesControllerTest.cs tests were written
-MoviesRepositoryTest.cs tests were written
-MoviesGrainsTest.cs tests were written
-Graph Mutations are added for add/update operations
-Graph Input Object is added
-MongoDb Seeder was added
-MongoDb and Redis configurations were mapped into appSettings.json

### Changed
-ErrorHandlingMiddleware logs are changed 
-GetRatedMovies response was checked from cache at first
-Get response was checked from cache at first
-Code Clean-up
-appSettings were changed by environments


## [1.0.0] - 15-02-2022
### Added
-Authentication added as refresh token with JWT
-Dockerfile and docker-compose.yml are added to build up docker container

### Changed
-range filter and search tasks were merged into a single end-point
-readme.md file is updated
-nuget packages were updated