@CALL dotnet restore
@CALL dotnet build -c "%~1"
@CALL dotnet pack --no-build