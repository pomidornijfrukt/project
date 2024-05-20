# Project

This is a .NET project that includes a data management system using SQLite. It provides functionalities to add, delete, and update data, as well as display data within a specified time period. The project also includes unit tests to ensure the correctness of the implemented functionalities.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Running Tests](#running-tests)
- [Project Structure](#project-structure)
- [Dependencies](#dependencies)
- [Adding Packages](#adding-packages)

## Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/pomidornijfrukt/project.git
    cd project
    ```

2. Install the necessary .NET SDK. You can download it from [here](https://dotnet.microsoft.com/download/dotnet/8.0).

3. Restore the dependencies:
    ```sh
    dotnet restore
    ```

## Usage

To run the application, use the following command:
```sh
dotnet run --project Project
```

This will execute the main project located in the `Project` directory.

## Running Tests

To run the unit tests, use the following command:
```sh
dotnet test
```
This will execute all tests in the `Project.Tests` project and display the results.

## Project structure

The project consists of two main parts:

1. **Project:** Contains the main application logic and data management functionalities.
2. **Project.Tests:** Contains unit tests to verify the correctness of the functionalities implemented in the main project.

### Project

- `DataDB.cs`: Contains methods for adding, deleting, updating, and displaying data in the SQLite database.
- `UsersDB.cs`: Contains methods for managing the active user.

### Project.Tests

- `DataDBTests.cs`: Contains unit tests for the methods in `DataDB.cs`.
- `UsersDBTests.cs`: Contains unit tests for the methods in `UsersDB.cs`.

### Project.Tests
- `DataDBTests.cs`: Contains unit tests for the methods in DataDB.cs.
- `UsersDBTests.cs`: Contains unit tests for the methods in UsersDB.cs.

## Dependencies

## Project
The `Project` depends on the following NuGet packages:

- `Microsoft.Data.SQLite` (Version 8.0.4)
- `System.Data.SQLite` (Version 1.0.118)

## Project.Tests
The `Project.Tests` depends on the following NuGet packages:

- `Microsoft.NET.Test.Sdk` (Version 17.6.0)
- `xunit` (Version 2.4.1)
- `xunit.runner.visualstudio` (Version 2.4.3)
- `Moq` (Version 4.16.1)

## Adding Packages
To add the required NuGet packages, you can run the following commands in your console:

For the main project:
```sh
dotnet add Project package Microsoft.Data.SQLite --version 8.0.4
dotnet add Project package System.Data.SQLite --version 1.0.118
```

For the test in the project:
```sh
dotnet add Project.Tests package Microsoft.NET.Test.Sdk --version 17.6.0
dotnet add Project.Tests package xunit --version 2.4.1
dotnet add Project.Tests package xunit.runner.visualstudio --version 2.4.3
dotnet add Project.Tests package Moq --version 4.16.1
```