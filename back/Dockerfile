from mcr.microsoft.com/dotnet/sdk:8.0
run dotnet new webapi -n gifapi
workdir gifapi
copy Program.cs .
run dotnet build
cmd dotnet run --urls "http://0.0.0.0:80"