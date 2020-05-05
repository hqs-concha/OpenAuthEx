@set nuget_source=http://nuget.hqs.pub/
@set nuget_key=oy2knaskpnmhvcbhgx7q3bmgc72hqhuexp7kphkpyhzmii

@set /p package_version=Enter the global nuget package version(like:6.6.6):
@echo bulid 

dotnet build
dotnet pack -p:PackageVersion=%package_version% -c release

@echo push
dotnet nuget push .\bin\Release\Mvc.Core.%package_version%.nupkg -k %nuget_key% -s %nuget_source% --timeout 360

@pause