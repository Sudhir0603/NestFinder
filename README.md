# 🏠 NestFinder - ASP.NET MVC PG/Hostel Finder Web Application
 A web-based PG/Hostel listing platform built with ASP.NET MVC and Entity Framework. Users can post, search, and filter properties. Admins verify listings, manage users, and reply to queries. Features include login, favorites, chat, ratings, and email integration.



---

## 📸 Demo Screenshot

![NestFinder Dashboard](assets/Dashboard.png)

---

## 📌 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
- [Admin Credentials](#admin-credentials)
- [Database Design](#database-design)
- [Project Structure](#project-structure)
- [Challenges Faced](#challenges-faced)
- [Future Scope](#future-scope)
- [Developer Info](#developer-info)

---

## ✨ Features

### 🧑‍💼 User Module

- User Registration & Login (ASP.NET Identity)
- Post new PG properties with full details
- Add rooms to properties
- View, search, and filter PGs
- Favorite properties for quick access
- Comment and rate properties
- Basic user-to-user chat
- Manage personal profile and properties

### 👮‍♂️ Admin Module

- Admin Dashboard with total stats
- Approve or reject user-posted PGs
- Promote/block/delete users
- Cascade delete: remove all user content on deletion
- View reports and top contributors
- Reply to user contact queries using SMTP

---

## 🧰 Tech Stack

| Technology         | Use                      |
|--------------------|---------------------------|
| ASP.NET MVC        | Web Application Framework |
| Entity Framework   | ORM for DB operations     |
| SQL Server         | Backend Database          |
| ASP.NET Identity   | User Authentication & Roles |
| Bootstrap          | UI Styling                |
| jQuery + AJAX      | Dynamic UI Interactions   |
| Gmail SMTP         | Contact Form + Notification Emails |

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/Sudhir0603/NestFinder.git
cd NestFinder

```
2. Set up SQL Server
Create a database in SSMS named NestFinder25252

Use the connection string in Web.config:
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=localhost\\MSSQLSERVER07;Database=NestFinder25252;Trusted_Connection=True;MultipleActiveResultSets=true" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
##   🔑 Admin Credentials
The admin account is auto-created during application startup in Startup.cs.

Email:    nestfinder2026@gmail.com
Password: AdminHa@123

# You can modify this logic in:
App_Start/Startup.cs
→ CreateAdminUserAndRole()



## 🗃️ Database Design (Key Tables)
 AspNetUsers – User credentials + profile info
Properties – PG property data
Rooms – Rooms under each property
Comments – User comments per property
Ratings – Ratings per property
Chats – Messages between users
ContactQueries – Contact form submissions



### 📂 Project Structure

Models/ – Entity classes
Controllers/ – Logic for each module (PropertyController, AdminController, RoomController, etc.)
Views/ – Razor Pages for all features
App_Start/ – Startup configuration (Startup.cs)
Content/ – CSS, images, and static assets
Scripts/ – jQuery, AJAX, Bootstrap JS


### ❗ Challenges Faced

Role-based access control with ASP.NET Identity
Secure password and email handling
Cascade deletion to maintain database integrity
Managing file/image uploads for properties
Complex filter combinations in property search

### 🔭 Future Scope
🌍 Google Maps integration to display property location
💬 Real-time chat using SignalR
📅 PG booking and visit scheduling system
💳 Payment Gateway for deposit/booking
📱 Cross-platform mobile app using .NET MAUI

---

## 📸 Project Screenshots

All screenshots are stored in the `/assets` folder and arranged below by feature.

---

### 🔐 1. Login & Registration Screens
- **Login Screen**
  
  ![Login Screen](assets/Picture1.png)

- **User Registration**

  ![Registration Screen](assets/Picture2.png)

---

### 👤 2. Profile View & Edit
- **View Profile**
  
  ![Profile View](assets/Picture3.png)

- **Edit Profile**
  
  ![Edit Profile](assets/Picture4.png)

---

### 📊 3. Dashboard, Post Property & Add Rooms
- **User Dashboard + Property Posting**

  ![User Dashboard](assets/Picture5.png)

---

### 🏡 4. Property Details View

- **Property Details**

  ![Property Details](assets/Picture6.png)

---

### 📝 5. My Uploaded Properties

- **My Properties Page**

  ![My Uploaded Properties](assets/Picture7.png)

---

### 🛠️ 6. Edit Property

- **Edit Property Form**

  ![Edit Property](assets/Picture8.png)

---

### ❤️ 7. Property List & Favorites

- **Available Properties + Add to Favorites**

  ![Available Properties](assets/Picture9.png)

- **Favorites List + Remove from Favorites**

  ![Favorites Page](assets/Picture10.png)

---

### 🔍 8. Property Search

- **Search with Filters**

  ![Search Filters](assets/Picture11.png)

---

### 🧑‍💼 9. Admin Panel, Reports & Management

- **Admin Dashboard**

  ![Admin Dashboard](assets/Picture14.png)

- **Admin Reports, Total Users, Manage Users, Pending Approvals**

  ![Admin Reports](assets/Picture15.png)

---

### 📬 10. Contact Form & Queries (Admin View)

- **User Contact Form**

  ![Contact Us](assets/Picture16.png)

- **Admin - View User Queries**

  ![View Queries](assets/Picture17.png)

---

### 📈 11. User Management (Admin)

- **Total Users, Manage Users, Pending Property Approvals**

  ![Manage Users](assets/Picture18.png)

---

### 🔒 12. Privacy Policy Page

- **Privacy Policy**

  ![Privacy Policy](assets/Picture21.png)

---





### 👨‍💻 Developer Info
Created by: Sudhir Ashok Kamble
📧 Email: sudhir.kamble0603@gmail.com
