# AI Agent Instructions for Lottery API Project

## Project Overview
This is a .NET Web API project implementing a lottery system with user management, lottery ticket sales, and prize claiming functionality. The project uses Entity Framework Core with MySQL for data persistence.

## Key Architecture Components

### Database Schema
- Uses Entity Framework Core with MySQL (MariaDB 10.6)
- Key entities:
  - `User`: Account management with roles (admin/user)
  - `Lottery`: Lottery tickets with numbers and status
  - `Order`: Lottery ticket purchase records
  - `Result`: Lottery drawing results
  - `WalletTxn`: User wallet transactions

### Authentication & Authorization
- Basic email/password authentication via `AuthController`
- Role-based access control (`admin` vs regular users)
- Password hashing using BCrypt (see `PasswordHelper.cs`)

### Core Business Flows
1. User Management (`AuthController`)
   - Registration with validation for duplicate email/phone/bank account
   - Login with password verification
   - User profile management

2. Lottery Management (`AdminController`) 
   - Admin-only operations for lottery management
   - Drawing results and prize distribution
   - System reset functionality

3. User Operations (`LottoController`)
   - Ticket purchase
   - Balance checks
   - Prize claiming
   - Transaction history

## Development Workflow

### Local Setup
1. Install .NET 9.0+ SDK
2. Configure MySQL connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=your_db;user=your_user;password=your_password"
  }
}
```

### Testing
No unit tests currently exist. When adding tests:
- Focus on business logic in controllers
- Mock `ApplicationDBContext` for database operations
- Test lottery number validation and prize calculations

### Common Patterns
1. Controller Response Format:
```csharp
return Ok(new { 
    message = "Success message in Thai", 
    data = result 
});
```

2. Database Operations:
- Use async/await with EF Core operations
- Include proper error handling and validation
- Check user role for admin operations

3. Error Handling:
```csharp
if (condition) 
    return BadRequest(new { message = "Error message in Thai" });
```

## Important Conventions
1. Thai Language Support
   - All user-facing messages are in Thai
   - Data uses UTF-8 encoding

2. Decimal Handling
   - Money values use `decimal` type
   - Currency amounts have 2 decimal places

3. Date/Time
   - Uses Thai timezone for date operations
   - Timestamps stored in UTC

## Integration Points
1. Database Connection
   - MySQL/MariaDB connection via Pomelo.EntityFrameworkCore.MySql
   - Connection string in Program.cs

2. External Dependencies
   - BCrypt.Net-Next for password hashing
   - Newtonsoft.Json for JSON serialization

## Common Gotchas
1. Database connections may need retry logic
2. Thai character encoding must be preserved
3. Admin role check required for privileged operations
4. Currency calculations must use decimal type

## Security Notes
- Never expose raw passwords or connection strings
- Validate all user input
- Check user roles for privileged operations
- Use parameterized queries (handled by EF Core)