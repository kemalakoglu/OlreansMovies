#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["Movies.Server/Movies.Server.csproj", "Movies.Server/"]
COPY ["Movies.GrainClients/Movies.GrainClients.csproj", "Movies.GrainClients/"]
COPY ["Movies.Aggregates/Movies.Aggregates.csproj", "Movies.Aggregates/"]
COPY ["Movies.Contracts/Movies.Contracts.csproj", "Movies.Contracts/"]
COPY ["Movies.Core/Movies.Core.csproj", "Movies.Core/"]
COPY ["Movies.Grains/Movies.Grains.csproj", "Movies.Grains/"]
RUN dotnet restore "Movies.Server/Movies.Server.csproj"
COPY . .
WORKDIR "/src/Movies.Server"
RUN dotnet build "Movies.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Movies.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Movies.Server.dll"]