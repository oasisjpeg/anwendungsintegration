# Backend Setup & Development Guide

## 📌 Project Overview
This backend is built using **ASP.NET Core** with **Entity Framework Core** and **MySQL**. It provides APIs for managing users and energy consumption records.

---


## 🚀 Setting Up the Backend

### 1️⃣ Clone the Repository
```bash
git clone <your-repo-url>
cd <your-backend-folder>
```

### 2️⃣ Configure the Database Connection
Update the `appsettings.json` file with your database connection string:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=your_database;User=root;Password=your_password;"
}
```

### 3️⃣ Install Dependencies
```bash
dotnet restore
```

### 4️⃣ Run the Application
```bash
dotnet run
```
This will start the API on `http://localhost:5000` (or another available port).

---

## 🔄 Database Management

### 🛠 Apply Migrations & Update Database
Whenever you make changes to models, you must update the database:
```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```
Example:
```bash
dotnet ef migrations add AddConsumptionRecordTable
dotnet ef database update
```

### 📜 List Existing Migrations
```bash
dotnet ef migrations list
```

### ❌ Remove Last Migration (Before Applying It)
```bash
dotnet ef migrations remove
```

### 🔄 Revert to a Previous Migration
```bash
dotnet ef database update <PreviousMigrationName>
```
Example:
```bash
dotnet ef database update InitialCreate
```

Example 24H values for Database
```mysql
INSERT INTO wattwiseapi.consumptionrecords (userId, timestamp, kWValue) VALUES
(1, '2025-03-13 00:00:00', 0.5),
(1, '2025-03-13 01:00:00', 0.8),
(1, '2025-03-13 02:00:00', 0.4),
(1, '2025-03-13 03:00:00', 0.3),
(1, '2025-03-13 04:00:00', 0.2),
(1, '2025-03-13 05:00:00', 0.4),
(1, '2025-03-13 06:00:00', 0.6),
(1, '2025-03-13 07:00:00', 1.2),
(1, '2025-03-13 08:00:00', 2.1),
(1, '2025-03-13 09:00:00', 1.8),
(1, '2025-03-13 10:00:00', 1.5),
(1, '2025-03-13 11:00:00', 1.3),
(1, '2025-03-13 12:00:00', 2.5),
(1, '2025-03-13 13:00:00', 2.8),
(1, '2025-03-13 14:00:00', 3.2),
(1, '2025-03-13 15:00:00', 3.5),
(1, '2025-03-13 16:00:00', 3.0),
(1, '2025-03-13 17:00:00', 2.7),
(1, '2025-03-13 18:00:00', 2.4),
(1, '2025-03-13 19:00:00', 2.0),
(1, '2025-03-13 20:00:00', 1.8),
(1, '2025-03-13 21:00:00', 1.5),
(1, '2025-03-13 22:00:00', 1.0),
(1, '2025-03-13 23:00:00', 0.7);
```
---

## 📝 API Endpoints

### **User Authentication**
#### 🔹 Register a User
**POST** `/api/users/register`
```json
{
    "name": "John Doe",
    "email": "john@example.com",
    "passwordHash": "mypassword"
}
```

#### 🔹 Login a User
**POST** `/api/users/login`
```json
{
    "email": "john@example.com",
    "passwordHash": "mypassword"
}
```

### **Energy Consumption Records**
#### 🔹 Get User's Consumption Records
**GET** `/api/consumption-records/{userId}`

#### 🔹 Add a New Record
**POST** `/api/consumption-records`
```json
{
    "userId": 1,
    "timestamp": "2025-03-13T12:00:00Z",
    "kWValue": 2.5
}
```

#### 🔹 Update a Record
**PUT** `/api/consumption-records/{id}`
```json
{
    "id": 1,
    "userId": 1,
    "timestamp": "2025-03-13T13:00:00Z",
    "kWValue": 3.0
}
```

#### 🔹 Delete a Record
**DELETE** `/api/consumption-records/{id}`

---

## 🧪 Running Tests
To run unit tests:
```bash
dotnet test
```

---

## 🛠 Deployment
For production, build and publish the project:
```bash
dotnet publish -c Release -o out
```

Run the published app:
```bash
cd out
./your-app-name
```

---

## 💡 Need Help?
If you run into issues, feel free to open an issue on GitHub or contact the development team.

🚀 **Happy coding!**

