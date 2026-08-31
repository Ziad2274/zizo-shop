# Zizo Shop — E-Commerce Platform

A full-stack e-commerce application: an ASP.NET Core Web API backend built with Clean Architecture, paired with an Angular frontend.

## Stack

- **Backend:** ASP.NET Core (.NET 9), Entity Framework Core, SQL Server
- **Architecture:** Clean Architecture (API / Application / Domain / Infrastructure), CQRS with MediatR
- **Auth:** ASP.NET Identity, JWT bearer authentication, role-based authorization
- **Background jobs:** Hangfire (SQL Server storage)
- **Docs:** Swagger / OpenAPI
- **Containerization:** Docker

## Features

- User registration, login, and role-based access via ASP.NET Identity + JWT
- Product catalog with brands, categories, and product images
- Cart and wishlist management
- Coupons and checkout flow
- Order and payment processing
- Product reviews
- Address management
- Admin dashboard endpoints
- Background job processing via Hangfire (scheduled cleanup tasks)

## Architecture

```
/API             → controllers, middleware, composition root (Program.cs)
/Application     → CQRS commands/queries (MediatR), DTOs, validation
/Domain          → entities (Product, Order, Cart, Coupon, Review, etc.), enums
/Infrastructure  → EF Core persistence, Identity, Hangfire jobs, repositories
```

Each feature (Auth, Cart, Products, Orders, Checkout, Coupons, Reviews, Wishlist, Payments, Addresses, Dashboard, Brands, Categories) follows the same CQRS structure under `Application/Features`.

## Running locally

```bash
# Restore and run migrations
dotnet restore
dotnet ef database update

# Run the API
dotnet run --project API
```

Update the connection string in `appsettings.json` (or `appsettings.Development.json`) to point at your local SQL Server instance before running migrations.



## Live demo

http://zizoshop.runasp.net/index.html

