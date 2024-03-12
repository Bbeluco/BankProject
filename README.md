# BankProject


## What is available in this project
This project has the goal to demonstrate commons requirements in API's nowadays.
I'm current developing the features so this project is not finished yet, but it will have in the future:

Front-end
Back-end

### Stacks
1. ASP.NET
2. Entity Framework
3. Moq

4. Angular (In future)

## Project structure

### Backend
The backend part is simple, I wrote the program in C# using ASP.NET Core, so the way API's where developed was following the industry pattern organization. We have the following structure so far:

Controllers -> Here are all the controllers in the application

DTOs -> Folder that has all the objects that the APIs use to change information between client and server

Migrations -> Common folder created by Entity Framework, in that folder you can check all the DB changes that occurs along the develop stage

Models -> Folder that has all the database tables.

Repositories -> In this folder, I put all the repository (and interfaces correlated to repos). Repository in a abstraction layer to communicate with the database. In this project I'm using Entity Framework

Services -> This folder has all the services (and interfaces correlated). Services are classes that has the responsability to contains all the API logic and business rules.

# Prepare your enviroment
The following stacks are required to be installed in your computer to run the project

1. dotnet 8
2. NPM 16

# Dependencies + DB Setup
To prepare the enviroment is preatty simple, you must run the following commands:

> dotnet restore
> dotnet ef database update

# Run project
To run the project simply run the following commands:

1. dotnet run
2. npm start

