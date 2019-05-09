# lets-shop
A simple hosted API that stores items in a cart

# Getting Started

### Requirements

- An IDE supporting development of .NET Core 
- [.NET Core 2.2+ SDK](https://dotnet.microsoft.com/download/visual-studio-sdks?utm_source=getdotnetsdk)
- [Make](http://gnuwin32.sourceforge.net/packages/make.htm) (optional)

### Building

The MakeFile has a few simple commands to build, test and run.   

To compile the solution, run the command   
```$ make build```

### Running the API

From the root, run the command  
``` $ make build-test-run ```  
to:  
- build the solution, 
- run the unit tests,
- run the integration tests,
- launch the API

Once the build completes, you should see from the command logs that the API is listening on https://localhost:44317 

Navigate to https://localhost:44317/api/ready and you should see the date and time - the API is up and running! 

Alternatively: Good old F5

# The API
Take a look at [Swagger] (https://localhost:44317/swagger/index.html) to see the available API operations.  

If you prefer postman, import the [Postman Collection](https://github.com/michaelBrakspear/lets-shop/blob/master/Cart.postman_collection.json)

### Note
Currently there is only a cart controller - therefore the consumer can decide the product id, price and quantity. 

# The Client 
A simple library to use to interact with the API, see the LetsShop.Demo project for a guide.

The easiest way to run the Demo is to run the command ```$ make run``` to launch the API. Set the LetsShop.Demo as the startup project and F5. 

# Next Steps
- Implement a product controller with a corresponding product repository to keep track of inventory, stock levels and pricing.
- Logging - Use [Serilog](https://serilog.net/)

