# dotnet-openapi-sdk-generator

<a href="https://github.com/felipespinelli/dotnet-openapi-sdk-generator/actions/workflows/github-actions.yaml" alt="Github Action CI/CD"><img src="https://github.com/felipespinelli/dotnet-openapi-sdk-generator/actions/workflows/github-actions.yaml/badge.svg" alt="Github Action CI/CD"/></a>

## Introduction
This package aims to make SDK providing easier. 
You must dedicate all efforts to make an amazing API. Generate its SDK is up to **OpenApiSdkGenerator**.


## Usage

### Installation
```shell
dotnet add package OpenApiSdkGenerator --version 0.1.18
```

### Configuration
#### OpenAPI-based json file
Once you'd installed the **OpenApiSdkGenerator** package, you need to create a file, inside your SDK project. This file name MUST ends with `openapi.json`.
This file must contains your OpenAPI-base definition (Swagger file). Something like the example below:
```json
{
  "openapi": "3.0.0",
  "info": {
    "version": "1.0.0",
    "title": "Swagger Petstore",
    "description": "A sample API that uses a petstore as an example to demonstrate features in the OpenAPI 3.0 specification",
    "termsOfService": "http://swagger.io/terms/",
    "contact": {
      "name": "Swagger API Team",
      "email": "apiteam@swagger.io",
      "url": "http://swagger.io"
    },
    "license": {
      "name": "Apache 2.0",
      "url": "https://www.apache.org/licenses/LICENSE-2.0.html"
    }
  },
  "servers": [
    {
      "url": "http://petstore.swagger.io/api"
    }
  ],
  "paths": {
    "/pets": {
      "get": {
        "description": "Returns all pets from the system that the user has access to\nNam sed condimentum est. Maecenas tempor sagittis sapien, nec rhoncus sem sagittis sit amet. Aenean at gravida augue, ac iaculis sem. Curabitur odio lorem, ornare eget elementum nec, cursus id lectus. Duis mi turpis, pulvinar ac eros ac, tincidunt varius justo. In hac habitasse platea dictumst. Integer at adipiscing ante, a sagittis ligula. Aenean pharetra tempor ante molestie imperdiet. Vivamus id aliquam diam. Cras quis velit non tortor eleifend sagittis. Praesent at enim pharetra urna volutpat venenatis eget eget mauris. In eleifend fermentum facilisis. Praesent enim enim, gravida ac sodales sed, placerat id erat. Suspendisse lacus dolor, consectetur non augue vel, vehicula interdum libero. Morbi euismod sagittis libero sed lacinia.\n\nSed tempus felis lobortis leo pulvinar rutrum. Nam mattis velit nisl, eu condimentum ligula luctus nec. Phasellus semper velit eget aliquet faucibus. In a mattis elit. Phasellus vel urna viverra, condimentum lorem id, rhoncus nibh. Ut pellentesque posuere elementum. Sed a varius odio. Morbi rhoncus ligula libero, vel eleifend nunc tristique vitae. Fusce et sem dui. Aenean nec scelerisque tortor. Fusce malesuada accumsan magna vel tempus. Quisque mollis felis eu dolor tristique, sit amet auctor felis gravida. Sed libero lorem, molestie sed nisl in, accumsan tempor nisi. Fusce sollicitudin massa ut lacinia mattis. Sed vel eleifend lorem. Pellentesque vitae felis pretium, pulvinar elit eu, euismod sapien.\n",
        "operationId": "findPets",
        "parameters": [
          {
            "name": "tags",
            "in": "query",
            "description": "tags to filter by",
            "required": false,
            "style": "form",
            "schema": {
              "type": "array",
              "items": {
                "type": "string"
              }
            }
          },
          {
            "name": "limit",
            "in": "query",
            "description": "maximum number of results to return",
            "required": false,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "pet response",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/Pet"
                  }
                }
              }
            }
          },
          "default": {
            "description": "unexpected error",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Error"
                }
              }
            }
          }
        }
      },
      "post": {
        "description": "Creates a new pet in the store. Duplicates are allowed",
        "operationId": "addPet",
        "requestBody": {
          "description": "Pet to add to the store",
          "required": true,
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/NewPet"
              }
            }
          }
        },
        "responses": {
          "200": {
            "description": "pet response",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Pet"
                }
              }
            }
          },
          "default": {
            "description": "unexpected error",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Error"
                }
              }
            }
          }
        }
      }
    },
    "/pets/{id}": {
      "get": {
        "description": "Returns a user based on a single ID, if the user does not have access to the pet",
        "operationId": "find pet by id",
        "parameters": [
          {
            "name": "id",
            "in": "path",
            "description": "ID of pet to fetch",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int64"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "pet response",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Pet"
                }
              }
            }
          },
          "default": {
            "description": "unexpected error",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Error"
                }
              }
            }
          }
        }
      },
      "delete": {
        "description": "deletes a single pet based on the ID supplied",
        "operationId": "deletePet",
        "parameters": [
          {
            "name": "id",
            "in": "path",
            "description": "ID of pet to delete",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int64"
            }
          }
        ],
        "responses": {
          "204": {
            "description": "pet deleted"
          },
          "default": {
            "description": "unexpected error",
            "content": {
              "application/json": {
                "schema": {
                  "$ref": "#/components/schemas/Error"
                }
              }
            }
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "Pet": {
        "allOf": [
          {
            "$ref": "#/components/schemas/NewPet"
          },
          {
            "type": "object",
            "required": [
              "id"
            ],
            "properties": {
              "id": {
                "type": "integer",
                "format": "int64"
              },
              "height": {
                "type": "number",
                "format": "decimal"
              },
              "vaccinated": {
                "type": "boolean"
              }
            }
          }
        ]
      },
      "NewPet": {
        "type": "object",
        "required": [
          "name"
        ],
        "properties": {
          "name": {
            "type": "string"
          },
          "tag": {
            "type": "string"
          }
        }
      },
      "Error": {
        "type": "object",
        "required": [
          "code",
          "message"
        ],
        "properties": {
          "code": {
            "type": "integer",
            "format": "int32"
          },
          "message": {
            "type": "string"
          }
        }
      }
    }
  }
}
```

#### openapi-sdk-generator.yaml
In addition to the file with the OpenAPI specification, we need a file that defines some guidelines for creating the SDK. This file should be named `openapi-sdk-generator.yaml`.
This file should have a structure similar to the example below:
```yaml
apiName: PetApi
usings:
  - "System"
defaultOperationAttributes:
operations:
  - name: ApiHealth
    ignore: true
  - name: Ping
    ignore: true
types:
  - name: NewPet
    replacementName: CreatePetRequest
    ignore: false
```
Let's explain it detailed the above structure:
- `apiName`: This value will be used to creates the Client. In the example an interfaced called `IPetApi` will be generated. **Default is `MyApi`**
- `usings`: This value represents an array of custom usings which must be added to generated files.
- `defaultOperationAttributes`: This value represents an array of custom attributes which must be added to **every** generated operation. For each configured attribute it's respective namespace must be declared in the `usings` array.
- `operations`: Each operation present in the OpenAPI specification file will generate a method to call it in the generate API Client. However, some of those not necessarily must be generated, or futhermore, some needs to be decorated with a specific attribute. For these cases you can use this section. This parameter is an array of an object with the follow properties:
  - `name`: The name of operation. It works like a selector.
  - `ignore`: Indicates whether the operation must be generated, or not.
  - `attributes`: This value represents an array of custom attributes which must be added only to the operation. For each configured attribute it's respective namespace must be declared in the `usings` array.
- `types`: Each schema present in the OpenAPI specification will generate a type. You replace they usage and/or ignore it. For example: You have an enum type, but want to provide a string property instead. So, you just need to specify it's name, the type to be used instead and, if you dont want to generate the enum, set the property `ignore` as `false`.
  - `name`: The name of type. *In the above example scenario **`MyEnum`**.*
  - `replacementName`: the name of type to be used instead. *In the above example scenario **`string`**.*
  - `ignore`: Indicates whether the type must not be generated. *In the above example scenario **`false`**.*

#### Making these files visible to OpenApiSdkGenerator
After configure the two files, you need to make them visible to **OpenApiSdkGenerator**. You just simply need to add the above section into your `.csproj` file:
```xml
<ItemGroup>
  <AdditionalFiles Include="openapi.json" />
  <AdditionalFiles Include="openapi-sdk-generator.yaml" />
</ItemGroup>
```
*Its assuming your OpenApi specification file is named as `openapi.json`*

### Conclusion
Now every time you update your OpenAPI specification file and rebuild your solution a new version of your ApiClient will be generated.
