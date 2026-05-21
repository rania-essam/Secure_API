# AuthAPI

A simple API for user authentication and role-based authorization.

---

## What it does

* User registration
* User login
* Role-based access (User, Admin)
* Protected routes using JWT

---

## Roles

* User → basic access
* Admin → full access

---

## Basic Flow

1. Register user
2. Login user
3. Get token (JWT)
4. Use token to access protected routes

---

## Endpoints

* POST /auth/register → create account
* POST /auth/login → login
