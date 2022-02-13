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