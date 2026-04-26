@echo off
setlocal

rem Always run from the repository root where this script lives.
cd /d "%~dp0"

rem Keep dotnet first-run artifacts inside the repo so builds work in restricted environments.
if not exist ".dotnet" mkdir ".dotnet"
set "DOTNET_CLI_HOME=%CD%\.dotnet"
set "DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1"
set "DOTNET_ADD_GLOBAL_TOOLS_TO_PATH=0"
set "DOTNET_MULTILEVEL_LOOKUP=0"
set "DOTNET_NOLOGO=1"
set "MSBuildEnableWorkloadResolver=false"

echo [1/2] Building solution...
dotnet build ".\PredatorSense.sln" -c Release
if errorlevel 1 goto :error

echo [2/2] Building installer...
pushd ".\installer"
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" ".\setup.iss"
set "EXITCODE=%ERRORLEVEL%"
popd
if not "%EXITCODE%"=="0" goto :error

echo Build completed successfully.
exit /b 0

:error
echo Build failed with exit code %ERRORLEVEL%.
exit /b %ERRORLEVEL%
