# 🚗 DriveSync

DriveSync is a full-stack vehicle reservation and management system built using **ASP.NET Core Web API (Backend)** and **ASP.NET MVC (Frontend)**. The project demonstrates a clean separation of backend services and frontend UI while following modern .NET development practices.

---

## 📌 Project Overview

DriveSync allows users to browse available vehicles, make reservations, and manage bookings through a user-friendly web interface. The backend provides RESTful APIs for authentication, vehicle management, and reservation handling, while the MVC frontend consumes these APIs and delivers the user experience.

This project is designed as a **portfolio-ready full-stack .NET application**, suitable for academic submissions, internships, and placement demonstrations.

---

## 🏗️ Architecture

**Solution Structure:**

```
DriveSync (Solution)
├── DriveSync        → ASP.NET Core Web API (Backend)
├── DriveSyncMVC     → ASP.NET MVC (Frontend)
└── Shared Database  → SQL Server
```

### 🔹 Backend (DriveSync API)

* Built with ASP.NET Core Web API
* Entity Framework Core for ORM
* RESTful endpoints
* JWT-based authentication & role-based access
* Vehicle and reservation management

### 🔹 Frontend (DriveSyncMVC)

* ASP.NET MVC (Razor Views)
* Consumes backend APIs
* User login and registration
* Vehicle selection and booking UI
* Clean and simple UI flow

---

## ⚙️ Tech Stack

* **Backend:** ASP.NET Core Web API (.NET 10)
* **Frontend:** ASP.NET MVC (Razor) (.NET 10)
* **Database:** SQL Server
* **Database Tool:** SQL Server Management Studio (Latest)
* **ORM:** Entity Framework Core
* **IDE:** Visual Studio 2026 (Latest)
* **Version Control:** Git & GitHub

---

## ✨ Features

* 🔐 JWT Authentication (Secure Token-Based Login)
* 🚘 Browse Available Vehicles
* 📅 Vehicle Reservation System
* 👤 Role-based access (Admin/User)
* 🧾 Reservation tracking
* 🔌 API-driven architecture

---

## 🔐 Authentication

This project uses **JSON Web Token (JWT) authentication** to secure API endpoints.

* Token generated on successful login
* Stored and sent via Authorization header
* Protects sensitive routes
* Enables stateless authentication

---

## 🚀 Getting Started

### Prerequisites

* Visual Studio 2026 or later
* .NET 10 SDK
* SQL Server / LocalDB
* SQL Server Management Studio (Latest)
* Git installed

---

### 🔧 Setup Instructions

1. **Clone the repository**

   ```bash
   git clone https://github.com/codePR-26/DriveSync.git
   ```

2. **Open solution in Visual Studio**

   * Open `DriveSync.sln`

3. **Configure database**

   * Update connection string in `appsettings.json`
   * Run migrations (if applicable)

4. **Run Backend API**

   * Set `DriveSync` as startup project
   * Note the API base URL

5. **Run MVC Frontend**

   * Set `DriveSyncMVC` as startup project
   * Ensure API base URL matches backend port

---

## 📚 Learning Objectives

This project demonstrates:

* Full-stack .NET development
* API + MVC integration
* GitHub project structuring
* Real-world CRUD operations
* Clean layered architecture

---

## 👨‍💻 Author

**Prithwish Bhowmik**

---

## 📄 License

This project is for educational and portfolio purposes.
