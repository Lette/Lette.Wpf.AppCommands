dotnet restore
dotnet build --no-restore --no-incremental -c Release /p:ContinuousIntegrationBuild=true
dotnet pack .\Lette.Wpf.AppCommands --no-build -c Release
mv .\Lette.Wpf.AppCommands\bin\Release\*.nupkg .
