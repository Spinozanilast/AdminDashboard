<div align="center">

# Admin Dashboard

<img src="https://img.shields.io/badge/version-0.0.1-blue.svg?cacheSeconds=2592000" />

</div>

## Description

This is a simple full-stack admin dashboard app.

## Installation

Just compose the app with docker-compose:

1. Clone the repository
2. Navigate to the project directory:
```bash
cd <path-to-cloned-repository>
```

4. Run the following command to start the app:
```bash
docker compose up
```

You can access the backend API doc at http://localhost:5000/scalar and the frontend at http://localhost:5173 (default email and password are *admin@mirra.dev* and *admin123* by default). 

## Screenshots

### Login Page
![Login](https://raw.githubusercontent.com/Spinozanilast/AdminDashboard/refs/heads/master/screenshots/login-page.png)
### Dashboard Page
![Dashboard](https://raw.githubusercontent.com/Spinozanilast/AdminDashboard/refs/heads/master/screenshots/dashboard-page.png)

### Endpoints screen from Scalar page
![Endpoints](https://raw.githubusercontent.com/Spinozanilast/AdminDashboard/refs/heads/master/screenshots/endpoints.png)


## Envs are open cause of it's a demo and connection to database is not embedded in the compose but you should not do it in production