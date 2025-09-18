# Online Library

This project is an online library system.

## Project Overview

This project is a multi-component system featuring a desktop application, a backend server, a SOAP service, and a JAXB utility. The primary components are orchestrated for streamlined development and execution.

## Program Showcase
(Application screenshots to be added here later)

<!-- Start Page Validation Example SOAP Service Code Weather Service Code JAXB Usage CRUD Operations -->

### Core Structure

Here's a high-level overview of the project's directory structure:

* **Desktop/**: Contains the Wails (Go + Vue.js) desktop application.
    * `api/`: Go backend logic for the Wails app.
    * `build/`: Build artifacts for the desktop app.
    * `frontend/`: Vue.js frontend code.
        * `src/assets/`: **(Place your images here)**
    * `app.go`: Wails application structure.
    * `main.go`: Main entry point for the Wails desktop application.
    * `wails.json`: Wails project configuration.
* **JaxbThing/**: A Java JAXB project, likely for XML processing.
    * `src/`: Java source code.
    * `xml-resources/`: XML schemas or related resources.
    * `build.xml`: Ant build file.
* **Server/**: The main backend Go server.
    * `handlers/`: HTTP request handlers.
    * `main.go`: Entry point for the Go server.
* **SoapService/**: A .NET SOAP web service.
    * `WebsiteContactsService/`: The service implementation.

*(For a more detailed, auto-generated structure, you can refer to `project_structure.md`)*

---

*Image to be added: Disclaimer*

## Quick Start

This project uses a task runner for a complete development environment.

1.  **Install Dependencies:** Make sure you have the necessary dependencies installed.
2.  **Navigate to Project Root:** Open your terminal and `cd` into the project directory.
3.  **Run the `dev` task:**
    ```
    task dev
    ```

This single command will:

* Start the Go backend **Server**.
* Launch the Wails **Desktop** application in development mode (which includes the Vue.js frontend).
* Run the .NET **SoapService**.

All services will run concurrently, and their output will be visible in your terminal.

---

## Key Components & Visuals

### 1. Task Orchestration

The project defines how different parts are built and run. The `dev` task, for example, depends on `server`, `frontend` (Wails app), and `soap` tasks.

*Image to be added: Taskfile Configuration*

### 2. Wails Desktop Application

The entry point for the Wails desktop application configures and launches the Wails app, bridging Go functions with the Vue.js frontend.

*Image to be added: Wails main.go Configuration*

---

## Further Details

**(Optional: Add more sections as needed)**

### Prerequisites

* Go
* Node.js, npm, and bun (for the Wails frontend)
* .NET SDK
* Java JDK
* Task (Task runner)

### Individual Component Commands

If you need to run components separately:

* **Backend Server:** `cd Server && go run main.go`
* **Wails Desktop App (Dev):** `cd Desktop && wails dev`
* **SOAP Service:** `cd SoapService/WebsiteContactsService && dotnet run`

### Building for Production

* **Wails Desktop App:** `cd Desktop && wails build`

### Backend Server Technology (Go + Gin)

The backend server located in the `/Server` directory is built using Go and the Gin web framework. Gin is utilized for routing, request handling, and middleware management to provide the RESTful API services. Key aspects include:

* **Routing:** Defined in `Server/main.go` to map API endpoints to handler functions.
* **Handlers:** Located in `Server/handlers/`, these functions process incoming requests and generate responses.
* **Middleware:** Custom middleware for functionalities like JWT authentication.

---

## About

The project involves developing a backend and frontend system with a REST API and SOAP services for processing, validating, and storing XML data. It also includes an XML-RPC server for fetching weather data, a JWT-secured API, and a client application to interact with all implemented services.
