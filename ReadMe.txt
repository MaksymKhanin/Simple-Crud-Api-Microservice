1. Go to Appsettings.Dev and set Auth:Authority - URL of Authentication server
2. In Program.cs setup cors (URL of apim, bff or front deployed to DEV)
3. For dev remove [Authorize] attribute in controller and add [AllowAnonymous].
4. When launch application, add /swagger to url.
5. If you have in Program.cs AddAuthentication(), UseAuthentication() or app.MapControllers().RequireAuthorization(), 
   you need to [AllowAnonymous] for controller. Otherwise it is enough just to remove [Authorize] attribute.
6. Your controller will return 404 (NotFound) if you don`t add app.MapContrllers();