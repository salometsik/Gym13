# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:6 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Gym13/*.csproj ./Gym13/
RUN dotnet restore

# copy everything else and build app
COPY Gym13/. ./Gym13/
WORKDIR /source/Gym13
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:6
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Gym13.dll"]