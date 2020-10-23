#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Dennis.Ordering.API/Dennis.Ordering.API.csproj", "Dennis.Ordering.API/"]
COPY ["Dennis.Ordering.Infrastructure/Dennis.Ordering.Infrastructure.csproj", "Dennis.Ordering.Infrastructure/"]
COPY ["Dennis.Ordering.Domain/Dennis.Ordering.Domain.csproj", "Dennis.Ordering.Domain/"]
COPY ["Dennis.Domain.Abstractions/Dennis.Domain.Abstractions.csproj", "Dennis.Domain.Abstractions/"]
COPY ["Dennis.Infrastructure.Core/Dennis.Infrastructure.Core.csproj", "Dennis.Infrastructure.Core/"]
RUN dotnet restore "Dennis.Ordering.API/Dennis.Ordering.API.csproj"
COPY . .
WORKDIR "/src/Dennis.Ordering.API"
RUN dotnet build "Dennis.Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dennis.Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dennis.Ordering.API.dll"]