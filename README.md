# BookTracker for Personal Library Management

This project was developed as a practical component of my thesis "The Role and Significance of Refactoring in Test-Driven Software Development" at the Faculty of Electrical Engineering in East Sarajevo. Tests for this project were conducted using the XUnit testing framework, achieving test coverage of over 85%.

## Introduction

BookTracker is a full-stack web application designed for managing your personal library. It allows you to keep track of your book collection, including details about each book, such as title, author, publication date, and more. Additionally, it provides features for searching, categorizing, and organizing your books efficiently.

## Technologies Used

- **Front-end**: The front-end is built using **React 18 with TypeScript** for a robust and responsive user interface.
- **Back-end**: The back-end of the application is developed with **ASP.NET Core** (.NET 7.0) to handle the core logic and interactions with the database.
- **Database**: BookTracker uses an **SQLite** database to store and manage book-related data.
- **Testing Environment**: The testing environment for this project was established using **XUnit** to ensure robust and comprehensive test coverage.

## Getting Started

To run the BookTracker project on your local machine, please follow these steps:

### Prerequisites

Make sure you have the following tools and dependencies installed:

- Node.js and npm
- Visual Studio 2022 or a later version
- .NET SDK

### Backend Setup

1. Clone this repository to your local machine.
2. Open the solution file **BookTracker.sln** in Visual Studio.
3. Restore the NuGet packages.
4. Run the backend application.

### Frontend Setup

1. Open a terminal in the `client-app` directory.
2. Install the required npm packages by running: `npm install`.
3. Start the front-end application using: `npm start`.

### Usage

- Open a web browser and navigate to **http://localhost:3000** to access the BookTracker application.
- You can register a new account or log in with an existing one.
- Explore the features of the application, which include adding, editing, deleting, and categorizing your books, and more.

## Technical Specification

- **Clean architecture**: The project follows clean architecture principles, as advocated by Robert C. Martin, to maintain a clear separation of concerns between different layers such as the API, application, domain, persistence, and infrastructure.
- **RESTful API**: BookTracker exposes a RESTful API that provides links to related resources and actions for seamless communication between the front-end and back-end components.
- **JWT authentication**: The application employs JSON Web Tokens (JWT) for secure user authentication and authorization, adhering to the OAuth 2.0 standard.
- **MobX**: The front-end utilizes MobX as the state management library to ensure smooth data flow and reactivity of the user interface components.
- **Semantic UI**: The user interface is designed using Semantic UI React to provide a consistent and user-friendly look and feel.

## License

This project is licensed under the MIT License - please refer to the [LICENSE](^1^) file for detailed information.
