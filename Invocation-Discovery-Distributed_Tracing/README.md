Install MongoDB, start mongodb, create a database called "admin" and a collection called "bankdetails"
```
Download & Install - https://docs.mongodb.com/manual/tutorial/install-mongodb-on-ubuntu/
Start Mongo - mongo
Create db - use admin
Create collection - db.createCollection("bankdetails")
```
1. Open a new terminal and run the following command to set up Zipkin
```
docker run -d -p 9411:9411 openzipkin/zipkin
```
If you go to localhost:9411, you should now be able to see the Zipkin UI.

2. Clone the Invocation-Discovery-Distributed_Tracing folder
``` 
git clone http://github/tejaswini1/DAPR/Invocation-Discovery-Distributed_Tracing
```
3. Navigate to BankServiceMvc folder in the terminal
4. Set environment variable to use app port 5000
```
export ASPNETCORE_URLS="http://localhost:5000"
```
4. Build the dotnet application
```
sudo dotnet build
```
5. Navigate to bin/Debug/netcoreapp3.1 folder within the BankServiceMvc folder
```
cd ./bin/Debug/netcoreapp3.1
```
6. Run the application at port 5000 and start Dapr at port 3504
```
sudo dapr run --app-id serviceA --app-port 5000 --port 3504 --components-path ./components --config ./tracing.yaml dotnet BankServiceMvc.dll
```
7. Open a new terminal and navigate to the StateStoreService1 application under Invocation-Discovery-Distributed_Tracing folder
8. Install required packages 
```
pip3 install wheel
pip3 install python-dotenv
```
9. Set environment variable to use app port 8000 
```
export FLASK_RUN_PORT=8000
```
10. Set environment variable to use file app.py
```
export FLASK_APP=app.py
```
11. Run the application at port 8000 and start Dapr at port 3502
```
sudo dapr run --app-id serviceB --app-port 8000 --port 3502 --components-path ./components --config ./tracing.yaml flask run
```
*OR*
```
sudo dapr run --app-id serviceB --app-port 8000 --port 3502 --components-path ./components --config ./tracing.yaml python3 app.py
```
12. Go to the postman app/browser and access 
```
http://localhost:5000/getall (OR) http://localhost:3504/v1.0/invoke/serviceA/method/getall
http://localhost:5000/getstate<any name> (OR) http://localhost:3504/v1.0/invoke/serviceA/method/getstate/<any name>
http://localhost:5000/addstate (OR) http://localhost:3504/v1.0/invoke/serviceA/method/addstate [will not work in browser, is a POST method]
```
13. To view traces, go to localhost:9411. Click on the search(magnifying glass) icon on the top right hand side.

**Reference to set up distributed tracing using Zipkin :** https://github.com/dapr/docs/blob/v0.7.0/howto/diagnose-with-tracing/zipkin.md
**Reference to for service invocation :** https://github.com/dapr/samples/tree/master/3.distributed-calculator

