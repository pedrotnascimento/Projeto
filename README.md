this solution is divided by 4 applications

ChatFrontEnd - frontend in Angular 13 (latest)

Application - API RestFul responsible for the login and receiveing messages

StockBotFunction - Function for receive the command /stock=stock_code

StockSocket - isolated Websocket for consuming messages from a rabbitMQ queue and send to clients.

I decided to keep all of them in the solution for keeping simple where the projects are.

As couldn't create a proper deployment for those applications, for watch the magic happens you must open 4 visual studio instances and Set as a default project each one of the above projects.

You also need to run the migrations in Repositories project:
> Update-Database

- if you don't have dotnet-ef, run this command:
> dotnet tool install --global dotnet-ef

