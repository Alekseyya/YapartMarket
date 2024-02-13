rd publish /s /q
dotnet tool uninstall dotnet-clear --global
dotnet tool install -v q --global dotnet-clear

dotnet-clear -c