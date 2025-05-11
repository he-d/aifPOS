# Club POS System

A Point of Sale (POS) system for clubs, built with ASP.NET Core MVC backend and React frontend.

## Features

- Secure login system with JWT authentication
- Role-based access control (Admin, Seller)
- Product management
- Sales tracking and reporting
- Receipt printing integration
- MySQL database in Docker container

## Project Structure

```
.
├── backend/           # ASP.NET Core MVC backend
│   ├── src/          # Source code
│   └── tests/        # Unit tests
├── frontend/         # React frontend
│   ├── src/          # Source code
│   └── public/       # Static files
├── docker/           # Docker configuration
└── docs/             # Documentation
```

## Prerequisites

- .NET Core SDK 7.0 or later
- Node.js 16.x or later
- Docker Desktop
- MySQL 8.0
- Windows 10/11

## Getting Started

1. Clone the repository
2. Set up the MySQL Docker container
3. Configure the backend
4. Start the frontend development server
5. Run the application

## Development

### Backend Setup

```bash
cd backend/src
dotnet restore
dotnet build
dotnet run
```

### Frontend Setup

```bash
cd frontend
npm install
npm start
```

### Database Setup

```bash
cd docker
docker-compose up -d
```

## Security

- JWT-based authentication
- Role-based access control
- Secure password hashing
- API rate limiting
- CORS configuration

## License

MIT 