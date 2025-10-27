# Accounting

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