## OAuth2 and Bearer Middleware
1. Clone the BankServiceMvc repositories
2. Create Azure account and register your application
```
https://docs.microsoft.com/en-us/azure/active-directory/azuread-dev/v1-protocols-oauth-code
```
3. Specify the redirect url as
```
http://loaclhost:3504/v1.0/invoke/method/display
```
4. Open oauth2.yaml located in file under BankService/bin/Debug/netcoreapp3.1/components
5. Replace the tenant ID, client ID and client secret
6. Open bearer.yaml located in file under BankService/bin/Debug/netcoreapp3.1/components
7. Replace the tenant ID in the issuer url
8. Navigate to bin/Debug/netcoreapp3.1
9. Run the app
```
sudo dapr run --app-id serviceA --app-port 5000 --port 3504 --components-path ./components --config pipeline.yaml dotnet BankServiceMvc.dll
```
10. Access http://localhost:3504/v1.0/invoke/serviceA/method/display in the browser
11. On redirection, enter the credentials used to create your azure account
12. If the credentials are right, you will be redirected to the redirect url http://localhost:3504/v1.0/invoke/serviceA/method/display to print "Hello World -from display"
13. App log in the terminal should have printed all the headers including Authorization header with bearer token
14. If the browser is displaying "unauthorized", replace the clientID in the bearer.yaml with the aud claim in the token.
15. Token can be decoded into its claims using https://jwt.io/

## Custom Component
1. Complete the OAuth2 and Bearer setup as mentioned above
2. Clone StateStoreService1
3. Follow the **Prerequisites** and **Clone dapr and component contrib** steps as mentioned in https://github.com/dapr/components-contrib/blob/master/docs/developing-component.md
4. Create a validation folder under go/src/github.com/dapr/components-contrib/meiddleware/http
5. Download the validation_middleware.go file under the validation folder
6. Follow steps 1 and 2 of **validating dapr core** mentioned in https://github.com/dapr/components-contrib/blob/master/docs/developing-component.md 
7. Go to  /home/ubuntu/go/src/github.com/dapr/dapr/cmd/daprd/main.go
8. In the main.go file, import the following under middleware section
```
github.com/dapr/components-contrib/middleware/http/validation
```
9. Add this under runtime.WithHTTPMiddleware
```
http_middleware_loader.New("validation", func(metadata middleware.Metadata) http_middleware.Middleware {
				handler, _ := validation.NewValidationMiddleware(log).GetHandler(metadata)
				return handler
			}),
```
10. Follow steps 5 and 6 of **validating dapr core** mentioned in https://github.com/dapr/components-contrib/blob/master/docs/developing-component.md
11. Navigate to /bin/Debug/netcoreapp3.1 to run BankserviceMvc app
```
sudo dapr run --app-id serviceA --app-port 5000 --port 3504 --components-path ./components --config pipeline.yaml dotnet BankServiceMvc.dll
```
12. Run StateStoreService1
```
sudo dapr run --app-id serviceB --app-port 8000 --port 3502 --components-path ./components --config pipeline.yaml python3 app.py
```
13. Access the url http://localhost:3504/v1.0/invoke/serviceA/method/component
14. If token is of valid syntax, the browser should display "Hello world from python app"
15. The app logs of both apps in terminal should print the Authorization header.
16. the dapr log of python app should display the various claims parsed from the token.
15. If the token is not of valid syntax, the browser will display "unauthorized".

