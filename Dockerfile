#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gym13.csproj", "Gym13/"]
COPY ["Gym13.Application.csproj", "Gym13.Application/"]
COPY ["Gym13.Domain.csproj", "Gym13.Domain/"]
COPY ["Gym13.Common.csproj", "Gym13.Common/"]
RUN dotnet restore "Gym13/Gym13.csproj"
COPY . .
WORKDIR "/src/Gym13"
RUN dotnet build "Gym13.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gym13.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gym13.dll"]


