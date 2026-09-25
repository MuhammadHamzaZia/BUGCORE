@echo off
echo ========================================================
echo   MantisNetMvc Automatic Restore, Build & Run Helper
echo ========================================================
echo.
echo Restoring NuGet packages...
dotnet restore MantisNetMvc.sln
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] NuGet restore failed.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Building MantisNetMvc solution...
dotnet build MantisNetMvc.sln -c Debug
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Build failed.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Launching MantisNetMvc Web Server...
cd src\MantisNetMvc.Web
dotnet run --launch-profile http
pause
