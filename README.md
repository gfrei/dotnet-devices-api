# Device Management API

A RESTful Minimal API built with **.NET 9**. This project uses **PostgreSQL** for persistence, **Entity Framework Core** for ORM, and is containerized using **Docker**.

## Tech Stack

* **Framework:** [.NET 9 (Minimal API)](https://dotnet.microsoft.com/)
* **Database:** [PostgreSQL](https://www.postgresql.org/)
* **ORM:** [Entity Framework Core 9](https://learn.microsoft.com/en-us/ef/core/)
* **Containerization:** [Docker](https://www.docker.com/) & Docker Compose
* **Documentation:** Swagger UI (Swashbuckle)

---

## Prerequisites

Before running the project, ensure you have the following installed:

* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (Optional, for local development without Docker)

---

## How to Run (Docker)


1.  **Start the services:**
    ```bash
    docker-compose up -d --build
    ```

2.  **Access the API:**
    * **Swagger UI:** [http://localhost:8080/swagger](http://localhost:8080/swagger)

---
## Examples of requests

```
POST http://localhost:8080/devices
Content-Type: application/json

{
    "name": "Z",
    "brand": "Moto",
    "state": "available"
}


PUT http://localhost:8080/devices/1
Content-Type: application/json

{
    "brand": "Samsung"
}


GET http://localhost:8080/devices/1


GET http://localhost:8080/devices/


DELETE http://localhost:8080/devices/1


GET http://localhost:8080/devices?brand=Moto&name=Edge
```
More examples on DeviceApi/DeviceApi.http


## Future improvements
- On Device Model, we can have a Deleted boolean to enable not losing data on deletion.
- On Device Model, we can also add a ModifiedAt DateTime and update it on PUT requests.
