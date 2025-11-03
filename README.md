Employee Portal 🏢

This project is a complete employee management system designed as a centralized and secure portal for Human Resources (HR). Built on a robust .NET backend, it provides administrators with the tools to manage the entire employee lifecycle within the organization.

Core Features

🔐 Secure Authentication: Built with ASP.NET Identity to ensure that only authorized HR personnel can access and manage sensitive employee data.

👥 Centralized Employee Directory: Provides a clean, searchable, and paginated view of all employees in the database.

✏️ Full CRUD Functionality: Enables administrators to easily perform all necessary actions: Add new hires, View/Edit employee profiles (like department, salary, or contact info), and Delete records.

🚀 Scalable Foundation: The application is designed to be a reliable and scalable foundation for a larger internal HR tool.

Key Technical Features & Architecture

🧱 ASP.NET MVC Core: Built on a modern, fast, and cross-platform .NET framework for a clean separation of concerns (Model-View-Controller).

🗃️ Entity Framework Core: Uses a code-first approach with EF Core to model the database, manage data, and handle complex queries.

📦 Repository Pattern: Implements the repository pattern to decouple the business logic from the data access layer. This makes the application easier to test, maintain, and scale.

💉 Dependency Injection: Leverages built-in .NET dependency injection to manage services (like the Employee Repository), promoting a clean and loosely-coupled architecture.

Tech Stack

🖥️ Backend: .NET 6 (MVC)
🗄️ Database: Entity Framework Core 6, Microsoft SQL Server
🎨 Frontend: Razor Views, Bootstrap 5
🏗️ Architecture: Repository Pattern, Dependency Injection
