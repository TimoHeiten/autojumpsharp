# copy zip to user folder
if (!(Test-Path ~\autojump)) {
    mkdir ~\autojump
}
# unzip app folder
Expand-Archive -Path .\autojump.zip -DestinationPath ~\autojump -Force
# set env variable
cp .\src\autojump\config.json ~\autojump\config.json
# run app with init command to create the db 
start ~\autojump\autojump.exe init

# modify PATH variable
# Get the current PATH
$currentPath = [Environment]::GetEnvironmentVariable("Path", "User")
# Define the new path you want to add
$newPath = "C:\users\timoh\autojump"
# Combine the current PATH and the new path
$updatedPath = $currentPath + ";" + $newPath
# Set the updated PATH for the current user
[Environment]::SetEnvironmentVariable("Path", $updatedPath, "User")

#modify the $PROFILE file for ps1