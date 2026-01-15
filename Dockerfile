FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY ./src/RateLimiter.Api/bin/Release/net8.0 .
ENTRYPOINT ["dotnet", "RateLimiter.Api.dll"]
