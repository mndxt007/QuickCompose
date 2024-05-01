Publish Command:

```
 dotnet publish -f net8.0-windows10.0.19041.0 -c Release
 dotnet publish -r win-x64 -f net8.0-windows10.0.19041.0 -p:PublishSingleFile=true --self-contained true -o ./bin/publish/singleexe -c Release
 dotnet publish -r win-x64 -f net8.0-windows10.0.19041.0 -p:PublishSingleFile=true -p:WindowsAppSDKSelfContained true -o ./bin/publish/singleexe -c Release
 ```

 https://www.youtube.com/watch?v=bluImEvcJNw&t=3s

