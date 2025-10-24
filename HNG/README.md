# String Analyzer Service

A RESTful API service built with ASP.NET Core that analyzes strings, computes their properties, and stores them for querying. The service supports standard filtering and natural language queries, and is fully containerized with Docker.

**GitHub Repository:** [https://github.com/phurhard/HNG](https://github.com/phurhard/HNG)

## Features

-   **String Analysis**: Computes length, palindrome status, unique characters, word count, SHA-256 hash, and character frequency for any given string.
-   **RESTful Endpoints**: Standard CRUD-like operations for managing analyzed strings.
-   **Advanced Filtering**: Filter stored strings by properties like length, word count, and palindrome status.
-   **Natural Language Queries**: A powerful endpoint to filter strings using plain English queries (e.g., "all single word palindromic strings").
-   **Rate Limiting**: Protects the API from excessive requests (100 requests per minute).
-   **CORS Enabled**: Configured to allow requests from any origin.
-   **Containerized**: Fully containerized with Docker for easy deployment and consistent environments.

## Technologies Used

-   [.NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
-   [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
-   [Docker](https://www.docker.com/)

---

## Setup and Running

You can run this project either locally using the .NET SDK or as a Docker container.

### Prerequisites

-   **For Local Development:** [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
-   **For Containerization:** [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Option 1: Running Locally

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/phurhard/HNG.git
    cd HNG
    ```

2.  **Restore Dependencies:**
    The .NET CLI will automatically restore packages when you build or run the project. You can also do it manually:
    ```sh
    dotnet restore
    ```

3.  **Run the application:**
    ```sh
    dotnet run
    ```

The API will be available at `http://localhost:5086`. You can access the Swagger UI for interactive documentation at `http://localhost:5086/swagger`.

### Option 2: Running with Docker

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/phurhard/HNG.git
    cd HNG
    ```

2.  **Build the Docker image:**
    From the root of the project directory (where the `Dockerfile` is located), run the following command:
    ```sh
    docker build -t hng-string-analyzer .
    ```

3.  **Run the Docker container:**
    This command starts the container and maps port `8080` on your local machine to port `8080` inside the container.
    ```sh
    docker run -p 8080:8080 hng-string-analyzer
    ```

The API will be available at `http://localhost:8080`. You can access the Swagger UI at `http://localhost:8080/swagger`.

---

## API Endpoints

### 1. Create/Analyze String

-   **Endpoint:** `POST /StringAnalysis/strings`
-   **Description:** Analyzes a new string and stores its properties.
-   **Request Body:**
    ```json
    {
      "value": "A man a plan a canal Panama"
    }
    ```
-   **Success Response (201 Created):**
    ```json
    {
      "id": "b2d354199964a1d48a78345720ac7583e2ec78216430334a416995b08994f069",
      "value": "A man a plan a canal Panama",
      "properties": {
        "length": 28,
        "is_palindrome": true,
        "unique_characters": 10,
        "word_count": 7,
        "character_frequency_map": { /* ... */ },
        "sha256_hash": "b2d354199964a1d48a78345720ac7583e2ec78216430334a416995b08994f069"
      },
      "created_at": "2024-08-27T10:00:00Z"
    }
    ```
-   **Error Responses:**
    -   `409 Conflict`: If the string has already been analyzed.
    -   `400 Bad Request`: If the request body is invalid or the `value` field is missing/empty.

### 2. Get Specific String

-   **Endpoint:** `GET /StringAnalysis/strings/{stringValue}`
-   **Description:** Retrieves the analysis properties for a specific string.
-   **Success Response (200 OK):** Returns the analysis object as shown above.
-   **Error Response:** `404 Not Found` if the string does not exist.

### 3. Get All Strings with Filtering

-   **Endpoint:** `GET /StringAnalysis/strings`
-   **Description:** Retrieves a list of all analyzed strings, with optional filtering.
-   **Query Parameters:**
    -   `is_palindrome` (boolean)
    -   `min_length` (integer)
    -   `max_length` (integer)
    -   `word_count` (integer)
    -   `contains_character` (char)
-   **Example:** `GET /StringAnalysis/strings?is_palindrome=true&min_length=5`
-   **Success Response (200 OK):**
    ```json
    {
      "data": [ /* array of analysis objects */ ],
      "count": 5,
      "filters_applied": {
        "is_palindrome": true,
        "min_length": 5
      }
    }
    ```

### 4. Natural Language Filtering

-   **Endpoint:** `GET /StringAnalysis/strings/filter-by-natural-language`
-   **Description:** Filters strings based on a natural language query.
-   **Query Parameter:** `query` (string)
-   **Example:** `GET /StringAnalysis/strings/filter-by-natural-language?query=all%20single%20word%20palindromic%20strings`
-   **Success Response (200 OK):**
    ```json
    {
      "data": [ /* array of matching analysis objects */ ],
      "count": 3,
      "interpreted_query": {
        "original": "all single word palindromic strings",
        "parsed_filters": {
          "is_palindrome": true,
          "word_count": 1
        }
      }
    }
    ```

### 5. Delete String

-   **Endpoint:** `DELETE /StringAnalysis/strings/{stringValue}`
-   **Description:** Deletes a string and its analysis from the system.
-   **Success Response:** `204 No Content`
-   **Error Response:** `404 Not Found` if the string does not exist.