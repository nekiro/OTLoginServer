@echo off
echo Publish self-container? [true/false]
echo self contained app will have noticeably bigger size, but wont require other depedencies e.g .net core runtime
set /p selfcontained=

cls
echo Publishing files...
echo.

dotnet publish OTLoginServer/OTLoginServer.csproj -p:PublishSingleFile=true --self-contained %selfcontained% -r linux-x64 -o out -v q

echo Done... Binaries stored in out directory.
echo.

PAUSE
