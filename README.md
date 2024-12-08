Financial Chatt APP

These are the instructions for the app, mostly we have to run a couple of commands as I'm using the local DB from SQL server.

# EF Core Commands
dotnet ef migrations add <MigrationName>         # Create a new migration
dotnet ef database update                        # Apply migrations to the database
dotnet ef dbcontext info                         # Inspect the database context info
dotnet ef migrations list                        # View applied migrations
dotnet ef migrations remove                      # Remove the last migration

# only after running the commands
# Run Tests
dotnet add package xunit                        # Install xUnit for unit testing
dotnet add package Moq                          # Install Moq for mocking dependencies
dotnet add package Microsoft.NET.Test.Sdk       # Install test SDK
dotnet test                                     # Run all unit tests
# this is not important, as it's on code, but you can do it if you want to test it
# RabbitMQ (Using RabbitMQ.Client in code)
await _channel.BasicPublishAsync(               # Send message to RabbitMQ
    exchange: "", 
    routingKey: "ChatQueue", 
    mandatory: false, 
    body: body
);

await _channel.BasicConsumeAsync(               # Consume message from RabbitMQ
    queue: "ChatQueue", 
    autoAck: true, 
    consumer: consumer
);
# these sql commands are really important to create chat rooms
# SQL Commands for SQLite (via SQL client or in Visual Studio)
INSERT INTO Room (Name) VALUES ('General');     # Insert a default room
SELECT * FROM Room;                             # Query all rooms

# General .NET Commands
dotnet build                                    # Build the project
dotnet run                                      # Run the project
dotnet add package <PackageName>                # Install a specific package


# Instructions on how to use SQL Server Object Explorer
# Visual Studio - SQL Server Object Explorer
# Use View > SQL Server Object Explorer to connect to a database
# Tools > Connect to Database to connect to SQLite or SQL Server
