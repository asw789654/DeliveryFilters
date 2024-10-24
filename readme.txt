## How to run

1. Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (>=8.0.3)
2. Install and run [PostgreSql(https://www.postgresql.org/) 
3. (Recommended) Install pgAdmin(https://www.pgadmin.org/download/)
4. Insert **connection string** to database to **appsettings.json** files of project.(data that you insert for your postgreSql) Example: 
```"DefaultConnection": "Server=.;database=Todos;Integrated Security=False;User Id=sa;Password=sqlServerPassword;MultipleActiveResultSets=True;Trust Server Certificate=true;"```
5. Use Dump file "DeliveryDbDump" to configurate DataBase(or you can use migrations too)
6. Open target API project in terminal and run command ```dotnet run```(or just run through IDE)