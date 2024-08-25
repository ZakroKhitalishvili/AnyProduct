#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AnyProduct.Orders.Api/AnyProduct.Orders.Api.csproj", "AnyProduct.Orders.Api/"]
COPY ["AnyProduct.Orders.Application/AnyProduct.Orders.Application.csproj", "AnyProduct.Orders.Application/"]
COPY ["AnyProduct.Orders.Domain/AnyProduct.Orders.Domain.csproj", "AnyProduct.Orders.Domain/"]
COPY ["AnyProduct.Orders.Infrastructure/AnyProduct.Orders.Infrastructure.csproj", "AnyProduct.Orders.Infrastructure/"]
RUN dotnet restore "./AnyProduct.Orders.Api/AnyProduct.Orders.Api.csproj"
COPY . .
WORKDIR "/src/AnyProduct.Orders.Api"
RUN dotnet build "./AnyProduct.Orders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AnyProduct.Orders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnyProduct.Orders.Api.dll"]