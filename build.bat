dotnet restore
dotnet build
cd tests/web/ui/ng-alain
npm install
ng build --prod --aot --output-path ../../Liuliu.Demo.Web/wwwroot
cd ../../Liuliu.Demo.Web
dotnet build -c Release
dotnet publish -c Release