# Rate Limiter Service using .NET 8

## Overview
A lightweight, in-memory rate limiting service built using .NET 8 Minimal APIs.
Designed to be extensible, observable, and production-ready.

## Features
- Token Bucket algorithm
- Per-user configurable rules
- Hot reload configuration (IOptionsMonitor)
- Thread-safe in-memory storage
- Prometheus metrics
- Clean architecture
- Docker support

### Design Decisions & Trade-offs
## Rate-Limiting Algorithm Choice
# Rate-Limiting Algorithm Choice - Algorithm: Token Bucket
- Widely used in production systems
- Easy to extend to distributed systems


### Code Structure & Architecture
- API → Application → Infrastructure

## API Layer
- Minimal API endpoints

## Application Layer
Rate-limiting logic ,Algorithm implementation ,Rule resolution

## Infrastructure Layer
- Storage implementations (In-Memory / Redis)

## Performance vs Accuracy
- In-memory store prioritizes performance
- Token Bucket accuracy is sufficient for most API workloads

### Command to Restore,Build,Test,Run
- dotnet restore
- dotnet build
- dotnet test
- dotnet run --project src/RateLimiter.Api

## API
### POST /check
- json{"userId": "premium-user" }
- http://localhost:5048/check

### Responses
- 200 OK
- 429 Too Many Requests
- http://localhost:5048/index.html
- http://localhost:5048/metrics