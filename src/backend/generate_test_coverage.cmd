@echo off
echo Cleaning old coverage data...

rmdir /s /q TestResults 2>nul
rmdir /s /q coverage-report 2>nul

echo Running tests with coverage...

dotnet test --settings coverlet.runsettings --collect:"XPlat Code Coverage" --results-directory TestResults
if errorlevel 1 (
    echo Tests failed. Stopping.
    exit /b 1
)

echo Generating coverage report...

reportgenerator -reports:"**\coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
if errorlevel 1 (
    echo Report generation failed.
    exit /b 1
)

echo Done! Open coverage-report\index.html
pause