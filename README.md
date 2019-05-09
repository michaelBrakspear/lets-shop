# lets-shop
A simple hosted API that 

# Getting Started

### Requirements

- An IDE supporting development of .NET Core 
- [.NET Core 2.1+ SDK]https://dotnet.microsoft.com/download/dotnet-core/2.1
- [Make](http://gnuwin32.sourceforge.net/packages/make.htm)

### Building

The MakeFile has a few simple commands to build, test and run. 

Run the command:
``` $ make build-test-run ```
to 
- build the project, 
- run the unit tests,
- run the integration tests,
- launch the API

Once the command completes, you should see from the command logs that the API is listening on https://localhost:44317 

Navigate to https://localhost:44317/api/ready and you should see the date and time - the API is up and running! 

Take a look at [Swagger] (https://localhost:44317/swagger/index.html) to see the available API operations. 



logs
add product controller and repo	