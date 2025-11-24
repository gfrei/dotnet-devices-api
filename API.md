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

Updates fields name, brand and state of an existing device.

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
