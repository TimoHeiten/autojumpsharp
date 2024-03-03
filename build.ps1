
# build sln
dotnet build 
checkExitCode "Build failed"

# run tests
dotnet test .\tests\autojump\autojump.csproj
checkExitCode "Tests failed"


function checkExitCode([string] $message) {
    $exitCode = $LastExitCode
    if ($exitCode -ne 0) {
        Write-Host "Error: $message"
        exit $exitCode
    }
}
