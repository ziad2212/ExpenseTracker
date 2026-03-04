# 💸 Expense Tracker API

A RESTful API built with **ASP.NET Core** that allows users to manage their personal expenses. Features JWT authentication, expense CRUD operations, and date-based filtering.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core (.NET 10)
- **Database:** SQL Server (Entity Framework Core)
- **Authentication:** JWT Bearer Tokens
- **Password Hashing:** BCrypt
- **Documentation:** Swagger UI

---

## 🚀 Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (local or remote)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/ziad2212/ExpenseTracker.git
   cd ExpenseTracker
   ```

2. **Configure `appsettings.json`**

   edit the file in the root of the project and add your settings:
   ```json
   {
     "ConnectionStrings": {
       "Default": "Your SQL Server connection string here"
     },
     "JwtSettings": {
       "SecretKey": "your-secret-key-here",
       "ExpiresInMinutes": 60
     }
   }
   ```

3. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the project**
   ```bash
   dotnet run
   ```

5. **Open Swagger UI**
   ```
   https://localhost:{port}/swagger
   ```

---

## 🔐 Authentication

This API uses **JWT Bearer Token** authentication.

### Register
```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "password123"
}
```

### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGci..."
}
```

Use this token in the `Authorization` header for all protected endpoints:
```
Authorization: Bearer eyJhbGci...
```

---

## 📋 Expense Endpoints

All expense endpoints require a valid JWT token.

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/expense` | Get all expenses (with optional filters) |
| `GET` | `/api/expense/{id}` | Get a single expense by ID |
| `POST` | `/api/expense` | Create a new expense |
| `PUT` | `/api/expense/{id}` | Update an existing expense |
| `DELETE` | `/api/expense/{id}` | Delete an expense |

### Filtering

Use query parameters to filter expenses by date range:

```http
GET /api/expense?filter=past_week
GET /api/expense?filter=past_month
GET /api/expense?filter=last_3_months
GET /api/expense?filter=custom&start=2024-01-01&end=2024-03-01
```

### Create Expense
```json
{
  "name": "Grocery run",
  "description": "Weekly groceries",
  "amount": 85.50,
  "category": "Groceries",
  "date": "2024-03-01T00:00:00Z"
}
```

### Update Expense (all fields optional)
```json
{
  "name": "Updated name",
  "amount": 99.99
}
```

---

## 🗂️ Expense Categories

| Value | Category |
|-------|----------|
| `Groceries` | Groceries |
| `Leisure` | Leisure |
| `Electronics` | Electronics |
| `Utilities` | Utilities |
| `Clothing` | Clothing |
| `Health` | Health |
| `Education` | Education |
| `Transportation` | Transportation |
| `Entertainment` | Entertainment |
| `Other` | Other |

---

## 🏗️ Project Structure

```
ExpenseTracker/
├── Application/
│   ├── DTOs/
│   │   ├── AuthResponse.cs
│   │   ├── LoginRequest.cs
│   │   ├── RegisterRequest.cs
│   │   └── Expense/
│   │       ├── CreateExpenseRequest.cs
│   │       ├── UpdateExpenseRequest.cs
│   │       └── ExpenseResponse.cs
│   └── Services/
│       ├── IAuthService.cs
│       ├── AuthService.cs
│       ├── IExpenseService.cs
│       └── ExpenseService.cs
├── Controllers/
│   ├── AuthController.cs
│   └── ExpenseController.cs
├── Domain/
│   ├── Users/
│   │   └── User.cs
│   ├── Expenses/
│   │   └── Expense.cs
│   └── Enums/
│       └── Category.cs
├── Infrastructure/
│   └── Data/
│       └── AppDbContext.cs
├── Migrations/
├── Program.cs
└── appsettings.json
```

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
