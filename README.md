# Device CRUD API

A RESTful Minimal API built with **.NET 9**. This project uses **PostgreSQL** for persistence, **Entity Framework Core** for ORM, and is containerized using **Docker**.

## Tech Stack

* **Framework:** [.NET 9 (Minimal API)](https://dotnet.microsoft.com/)
* **Database:** [PostgreSQL](https://www.postgresql.org/)
* **ORM:** [Entity Framework Core 9](https://learn.microsoft.com/en-us/ef/core/)
* **Containerization:** [Docker](https://www.docker.com/) & Docker Compose
* **Documentation:** Swagger UI (Swashbuckle)

## Code Structure
- The main project is located on DeviceApi/
- The test project is located on DeviceApi.Tests
- The main project entry point is DeviceApi/Program.cs
- The endpoints are configured at DeviceApi/Handlers/DeviceHandlers.cs
- Docker files are located on DeviceApi/


## Prerequisites

Before running the project, ensure you have the following installed:

* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (Optional, for local development without Docker)


## How to Run (Docker)


1.  **Start the services:**
   On the DeviceApi folder
    ```bash
    /DeviceApi/docker-compose up -d --build
    ```

2.  **Access the API:**
    * **Swagger UI:** [http://localhost:8080/swagger](http://localhost:8080/swagger)

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

# DeviceApi
## Version: 1.0

### /devices

#### GET
##### Summary:

Query devices

##### Description:

Returns a list of devices filtered by name, brand, or state.
Query parameters:
- **name**: string — Filter by device name
- **brand**: string — Filter by brand
- **state**: string — Filter by state (available, in-use, inactive)


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| name | query |  | No | string |
| brand | query |  | No | string |
| state | query |  | No | string |

Examples:

GET http://localhost:8080/devices?brand=Moto

GET http://localhost:8080/devices?brand=Moto&name=Edge

GET http://localhost:8080/devices?name=Edge&state=available

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

#### POST
##### Summary:

Create device

##### Description:

Creates a new device and returns it.

Examples:

POST http://localhost:8080/devices

Content-Type: application/json

{
    "name": "Edge",
    "brand": "Moto",
    "state": "available"
}

##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | Created |
| 400 | Bad Request |

### /devices/{id}

#### GET
##### Summary:

Get device by ID

##### Description:

Returns a single device by its ID.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |
| 404 | Not Found |

#### PUT
##### Summary:

Update device

##### Description:

Updates fields name, brand and state of an existing device. Can receive name, brand and state.

Examples:

PUT http://localhost:8080/devices/5
Content-Type: application/json

{
    "brand": "Samsung"
}

PUT http://localhost:8080/devices/1
Content-Type: application/json

{
    "name": "S22"
}

PUT http://localhost:8080/devices/10
Content-Type: application/json

{
    "name": "galaxy s20",
    "brand": "Samsung",
    "state": "available"
}


##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |
| 400 | Bad Request |
| 404 | Not Found |
| 409 | Conflict |

#### DELETE
##### Summary:

Delete device

##### Description:

Deletes a device unless it is in use.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | No Content |
| 409 | Conflict |

## To run Tests

On the root of the project, execute:
    ```bash
    /dotnet test
    ```

## Future improvements
- On Device Model, we can have a Deleted boolean to enable not losing data on deletion.
- On Device Model, we can also add a ModifiedAt DateTime and update it on PUT requests.
