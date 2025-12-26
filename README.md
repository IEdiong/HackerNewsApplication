# HackerNews API

A clean, modern ASP.NET Core Web API that wraps the official [Hacker News API](https://github.com/HackerNews/API), providing a typed, paginated, and searchable interface for retrieving the latest Hacker News stories.

## 📋 Overview

This project serves as a backend service that:
- Fetches the latest stories from Hacker News
- Provides pagination and search capabilities
- Returns standardized JSON responses
- Includes built-in caching for improved performance
- Offers interactive API documentation via Swagger

## 🚀 Features

- **Pagination**: Control how many stories to retrieve per request
- **Search**: Filter stories by title keywords
- **CORS Enabled**: Ready for cross-origin requests from web clients
- **Swagger UI**: Interactive API documentation and testing
- **Docker Support**: Containerized deployment ready
- **Memory Caching**: Optimized performance with in-memory caching
- **Input Validation**: Automatic parameter validation and sanitization

## 🛠️ Tech Stack

- **.NET 8.0**: Latest LTS version of .NET
- **ASP.NET Core**: Web API framework
- **Newtonsoft.Json**: JSON serialization
- **Swashbuckle**: Swagger/OpenAPI documentation
- **Docker**: Containerization support

## 📦 Prerequisites

Before running this project, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed
- (Optional) [Docker Desktop](https://www.docker.com/products/docker-desktop) for containerized deployment

## 🏃 Running Locally

### Option 1: Using .NET CLI

1. **Clone the repository** (if not already done):
   ```bash
   git clone <repository-url>
   cd HackerNewsApplication
   ```

2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   cd HackerNews.API
   dotnet run
   ```

4. **Access the API**:
   - Swagger UI: `https://localhost:5001/swagger` or `http://localhost:5000/swagger`
   - API Endpoint: `https://localhost:5001/api/hackernews/stories`

### Option 2: Using Docker

1. **Build the Docker image**:
   ```bash
   docker build -t hackernews-api -f HackerNews.API/Dockerfile .
   ```

2. **Run the container**:
   ```bash
   docker run -p 8080:8080 hackernews-api
   ```

3. **Access the API**:
   - Swagger UI: `http://localhost:8080/swagger`
   - API Endpoint: `http://localhost:8080/api/hackernews/stories`

## 📖 API Documentation

### Get Latest Stories

**Endpoint**: `GET /api/hackernews/stories`

**Query Parameters**:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `limit` | integer | 10 | Number of stories to return (1-50) |
| `page` | integer | 1 | Page number for pagination (min: 1) |
| `search` | string | "" | Filter stories by title (optional) |

**Example Request**:
```bash
GET /api/hackernews/stories?limit=20&page=1&search=javascript
```

**Example Response**:
```json
{
  "message": "Request Successful",
  "success": true,
  "data": {
    "items": [
      {
        "id": 12345,
        "title": "Example Story Title",
        "url": "https://example.com",
        "by": "username",
        "time": 1609459200,
        "score": 100,
        "descendants": 25
      }
    ],
    "pagination": {
      "currentPage": 1,
      "pageSize": 20,
      "totalCount": 100
    }
  }
}
```

## 🧪 Running Tests

```bash
dotnet test
```

## 🏗️ Project Structure

```
HackerNewsApplication/
├── HackerNews.API/
│   ├── Controllers/          # API endpoints
│   ├── Services/             # Business logic
│   ├── HttpClientServices/   # External API communication
│   ├── Models/               # Data transfer objects
│   ├── Interfaces/           # Service contracts
│   ├── Helpers/              # Utility classes
│   ├── Extensions/           # Service configuration extensions
│   ├── Program.cs            # Application entry point
│   └── appsettings.json      # Configuration
├── HackerNews.Test/          # Unit and integration tests
└── README.md
```

## ⚙️ Configuration

The application can be configured via `appsettings.json`:

```json
{
  "HackerNewsApi": {
    "BaseUrl": "https://hacker-news.firebaseio.com"
  }
}
```

## 🔧 Development

### Adding New Features

1. Define interfaces in `Interfaces/`
2. Implement services in `Services/`
3. Add controllers in `Controllers/`
4. Update models in `Models/` as needed

### Code Style

- Follow C# naming conventions
- Use dependency injection for services
- Keep controllers thin, business logic in services
- Write unit tests for new features

## 📝 License

This project is licensed under the MIT License.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📧 Contact

For questions or feedback, please open an issue in the repository.

---

Built with ❤️ using ASP.NET Core
