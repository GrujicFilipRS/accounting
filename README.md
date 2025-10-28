# Accounting

## General info

This is a very simple console application, that will communicate with a database.
There will be essentially only two tables:
- the user table
- the account table

For now I'm speculating on using NeonDB for the database hosting, but we'll see where the future takes us

When starting the program, the user can choose between logging in and registering
When logged in, he can either create one of two types of  accounts:
- Checkings account
- Savings account

Both of these will be implemented with classes in C#, and both of those classes will inherit from a higher abstract class `Account`

Then he will be able to use both of these accounts like a regular bank account

## How to build and run

You're going to need dotnet (this one is configured for version 9)
You just clone the repository, and do `dotnet build` and then `dotnet run` to run the project
