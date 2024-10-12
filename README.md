# Airbnb Clone Project

![Airbnb Clone Logo](https://www.digital.ink/wp-content/uploads/airbnb_logo_detail.jpg)

## Overview

This is a full-stack Airbnb clone web application that allows users to list properties, book stays, and manage bookings. The project integrates with Stripe for secure payment processing and offers essential features like user authentication, property management, and reviews.

## Table of Contents

- [Tech Stack](#tech-stack)
- [Features](#features)
- [API Endpoints](#api-endpoints)
- [Installation](#installation)
- [Environment Variables](#environment-variables)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Tech Stack

- **Backend**: ASP.NET Core, Entity Framework Core, SQL Server
- **Payment Gateway**: Stripe
- **Database**: SQL Server
- **Authentication**: ASP.NET Core Identity
- **Cloud**: Deployed on [Monsterasp.net]

## Features

- **User Authentication**: Register, login, email confirmation, password reset, etc.
- **Property Listings**: Create, update, and delete properties.
- **Bookings**: Users can create, view, update, and cancel bookings.
- **Payments**: Integrated secure payments via Stripe.
- **Reviews**: Add, update, and view reviews for properties.
- **Admin Panel**: Manage users, properties, bookings, and reviews.

## API Endpoints

### Account

- `POST /api/Account/Login`
- `POST /api/Account/Register`
- `POST /api/Account/EmailConfirmation`
- `GET /api/Account/ForgetPassword`
- `PUT /api/Account/ResetPassword`

### Booking

- `POST /api/Booking/CreateBooking`
- `PUT /api/Booking/CancelBooking`
- `DELETE /api/Booking/DeleteBooking`
- `GET /api/Booking/GetBooking`
- `GET /api/Booking/GetBookingsByUser`
- `PUT /api/Booking/UpdateBookingById`

### Payment

- `POST /api/Payment/create-payment-intent`
- `GET /api/Payment/success`
- `GET /api/Payment/cancel`
- `POST /api/Payment/create-checkout-session`
- `POST /api/Payment/refund-payment`

### Property

- `GET /api/Property/GetProperties`
- `GET /api/Property/GetProperty`
- `POST /api/Property/CreateProperty`
- `DELETE /api/Property/DeleteProperty`
- `PUT /api/Property/UpdateProperty`

### Review

- `GET /api/Review/GetReview/{id}`
- `POST /api/Review/AddReview`
- `PUT /api/Review/UpdateReview/{id}`
- `DELETE /api/Review/DeleteReview/{id}`
- `GET /api/Review/Property/{propertyId}`

### Users

- `GET /api/Users/GetAllUsers`
- `GET /api/Users/GetUserById/{Id}`
- `DELETE /api/Users/RemoveUser/{Id}`
- `POST /api/Users/CreateUser`
- `PUT /api/Users/UpdateUser`

## Installation

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

### Backend Setup

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/airbnb-clone.git
    cd airbnb-clone
    ```

2. Navigate to the backend directory and install dependencies:

    ```bash
    cd backend
    dotnet restore
    ```

3. Update the connection string in `appsettings.json` to point to your SQL Server database.

4. Apply migrations to the database:

    ```bash
    dotnet ef database update
    ```

5. Run the backend:

    ```bash
    dotnet run
    ```


## Environment Variables

Create an `.env` file in the backend directory and configure the following:

```bash
STRIPE_SECRET_KEY=your_stripe_secret_key
STRIPE_PUBLISHABLE_KEY=your_stripe_publishable_key
