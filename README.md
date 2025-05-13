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

### 1. Clone the Repository

```bash
git clone <repository-url>
cd clubpos
```

### 2. Database Setup

1. Start MySQL using Docker:
```bash
cd docker
docker-compose up -d
```

2. Wait for MySQL to be ready (usually takes about 30 seconds)

### 3. Backend Setup

1. Navigate to the backend directory:
```bash
cd backend/src
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Update the database connection string in `appsettings.json` if needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=clubpos;User=root;Password=your_password;"
  }
}
```

5. Run the backend:
```bash
dotnet run --project ClubPOS.API
```

The backend will be available at `http://localhost:5000`

### 4. Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Start the development server:
```bash
npm start
```

The frontend will be available at `http://localhost:3000`

### 5. Access the Application

1. Open your browser and navigate to `http://localhost:3000`
2. Log in with the default credentials:
   - Username: admin
   - Password: admin123

## Development

### Backend Development

- The backend uses ASP.NET Core MVC
- API endpoints are available at `http://localhost:5000/api/*`
- Swagger documentation is available at `http://localhost:5000/swagger`

### Frontend Development

- The frontend uses React with TypeScript
- Material-UI is used for the UI components
- API requests are automatically proxied to the backend
- Hot reloading is enabled for development

### Database Management

- MySQL runs in a Docker container
- Default credentials:
  - Host: localhost
  - Port: 3306
  - Database: clubpos
  - Username: root
  - Password: your_password

## Building for Production

### Backend

1. Navigate to the backend directory:
```bash
cd backend/src
```

2. Build the release version:
```bash
dotnet publish -c Release
```

3. The published files will be in `bin/Release/net7.0/publish`

### Frontend

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Build the production version:
```bash
npm run build
```

3. The built files will be in the `build` directory

## Troubleshooting

### Common Issues

1. Database Connection Issues
   - Ensure MySQL container is running: `docker ps`
   - Check connection string in `appsettings.json`
   - Verify MySQL port is not blocked

2. Backend Build Issues
   - Clear NuGet cache: `dotnet nuget locals all --clear`
   - Delete bin and obj folders
   - Run `dotnet restore` again

3. Frontend Build Issues
   - Delete node_modules folder
   - Clear npm cache: `npm cache clean --force`
   - Run `npm install` again

## Security

- JWT-based authentication
- Role-based access control
- Secure password hashing
- API rate limiting
- CORS configuration

## License

MIT 