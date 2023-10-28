# #See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# XELI AR MOKIDOT
# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src

#  COPY ["Gym13.csproj", "Gym13/"]
# # COPY ["Gym13.Application.csproj", "./Gym13.Application/"]
 

# COPY ["Gym13.Application/Gym13.Application.csproj", "Gym13.Application/"]
# COPY ["Gym13.Domain.csproj", "./Gym13.Domain/"]
# COPY ["Gym13.Common.csproj", "./Gym13.Common/"]

# RUN dotnet restore "Gym13/Gym13.csproj"
# COPY . .
# WORKDIR "/src/Gym13"
# RUN dotnet build "Gym13.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "Gym13.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "Gym13.dll"]


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
COPY *.sln .
COPY Gym13/*.csproj ./Gym13/
COPY Gym13.Application/*.csproj ./Gym13.Application/
COPY Gym13.Domain/*.csproj ./Gym13.Domain/
COPY Gym13.Common/*.csproj ./Gym13.Common/ 
# Restore as distinct layers
RUN dotnet restore

COPY . .

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Gym13.dll"]