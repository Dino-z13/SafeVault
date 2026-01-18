# SafeVault – Secure REST API

SafeVault is a secure REST API built with ASP.NET Core as part of the Microsoft Back-End Development course.  
The project focuses on implementing secure coding practices, authentication, authorization, and vulnerability mitigation using Microsoft Copilot.

---

## 📌 Project Overview

SafeVault is designed to manage sensitive user data securely.  
It demonstrates how to protect a backend application against common security threats such as:

- SQL Injection
- Cross-Site Scripting (XSS)
- Unauthorized access to protected resources

The application follows REST API principles and does **not** include a graphical user interface.

---

## 🔐 Security Features

### Input Validation & XSS Prevention
- User input is sanitized before processing.
- Malicious scripts and unsafe characters are detected and rejected.

### SQL Injection Prevention
- All database interactions use **parameterized queries**.
- Unsafe string concatenation is avoided.

### Authentication
- User passwords are securely hashed before storage.
- Login verifies credentials against hashed values.

### Authorization (RBAC)
- Role-Based Access Control is implemented.
- Admin-only endpoints are protected using role checks.

---

## 🧪 Testing

The project includes automated tests that:
- Simulate SQL injection attempts
- Test XSS attack vectors
- Verify authentication and authorization behavior

Tests are written using **NUnit** and are located in the `SafeVault.Tests` project.

---

## 🛠 Technologies Used

- ASP.NET Core (Minimal API)
- SQLite
- BCrypt for password hashing
- NUnit for testing
- Microsoft Copilot for code generation and debugging

---

## ▶️ Running the Application

```bash
cd Course_5/SafeVault
dotnet run
