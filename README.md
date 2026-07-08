# 🏢 Enterprise Salary Slip Management System

![Project Status](https://img.shields.io/badge/Status-Active-success?style=for-the-badge)
![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![.NET Core](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/EF_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)

A full-stack, enterprise-grade web application built to manage employee records, organizational structures (Roles & Departments), and eventually salary slips. This system features a robust **.NET 8** backend with Entity Framework Core, connected to a modern, highly responsive **Angular** frontend styled with **Tailwind CSS**.

---

## 🌟 Key Features

### 🔐 Security & Authentication
- **Role-Based Access Control (RBAC):** Strict separation between `Admin` and `Employee` portals.
- **JWT Authentication:** Secure token-based API protection.
- **Route Guards:** Angular frontend guards prevent unauthorized access to Admin dashboards.

### 👨‍💼 Admin Portal
- **Real-Time Dashboard:** View Key Performance Indicators (KPIs) like total employees, department counts, and estimated monthly payroll.
- **Master Data Management:** Instantly add, edit, and remove System Roles and Departments using Optimistic UI updates for zero-latency interactions.
- **Employee Onboarding:** Securely "Hire" new employees into specific departments and roles.
- **Full CRUD Operations:** Manage the entire lifecycle of enterprise personnel.

### 🧑‍💻 Employee Portal
- **Self-Service Dashboard:** Employees can securely log in to view their personal details, assigned role, and department.
- **Profile Management:** Employees can update their contact information (like Phone Number) seamlessly.

---

## 🛠️ Technology Stack

### Backend (SalarySlipManagementApi)
*   **Framework:** .NET 8 Web API
*   **Language:** C#
*   **ORM:** Entity Framework (EF) Core
*   **Database:** Microsoft SQL Server
*   **Architecture:** Repository Pattern & Unit of Work for highly testable and scalable data access.
*   **Security:** JWT (JSON Web Tokens) Bearer Authentication.

### Frontend (SalarySlipManagementClient)
*   **Framework:** Angular 18 (Using Modern Standalone Components)
*   **Styling:** Tailwind CSS (Custom Dark Mode & Glassmorphism UI)
*   **State Management:** RxJS & Signals
*   **Performance:** Angular Universal (Server-Side Rendering / SSR) for blazing fast initial loads.

---

## 🚀 Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### Prerequisites
Before you begin, ensure you have the following installed:
*   [Node.js](https://nodejs.org/) (v18 or higher)
*   [Angular CLI](https://angular.io/cli) (`npm install -g @angular/cli`)
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer edition)
*   An IDE like **Visual Studio 2022** or **VS Code**.

### 1. Database Setup & Backend Configuration
1. Clone the repository.
2. Open the `SalarySlipManagementApi` folder in your terminal or Visual Studio.
3. Open `appsettings.json` and ensure the `DefaultConnection` string points to your local SQL Server instance.
4. Open the **Package Manager Console** in Visual Studio and run the Entity Framework migrations to build your database:
   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```
5. Run the backend API by clicking the green **Run (https)** button at the top of Visual Studio. The Swagger API documentation will automatically open in your browser.

### 2. Frontend Configuration
1. Open a new terminal and navigate to the `SalarySlipManagementClient` folder.
2. Install the necessary NPM packages:
   ```bash
   npm install
   ```
3. Start the Angular development server:
   ```bash
   npx ng serve
   ```
4. Open your browser and navigate to `http://localhost:4200`.

---

## 🏗️ Architectural Approach

This project strictly adheres to enterprise best practices:
1.  **Separation of Concerns:** The backend API acts strictly as a data provider, completely decoupled from the UI.
2.  **Repository Pattern:** Database logic is abstracted away from Controllers, ensuring the API remains lightweight and easily testable.
3.  **Optimistic UI:** The frontend leverages local array mutations (`.push()`, `.filter()`) paired with Angular's `ChangeDetectorRef.markForCheck()` to provide instant user feedback without waiting for HTTP round-trips.
4.  **Modern UI/UX:** The interface rejects generic bootstrap templates in favor of a custom, highly polished Tailwind CSS dark theme with micro-animations.

---

## 🤝 Contributing
Contributions, issues, and feature requests are welcome! Feel free to check the issues page.

## 📝 License
This project is open-source and available under the [MIT License](LICENSE).
