# todo
# package /nuget and push to github?
# publish app as exe file

rm -Force -Recurse .\src\autojump\bin
rm -Force -Recurse .\src\autojump\obj

dotnet build .\src\autojump -c Release

# publish as single file
if (Test-Path .\publish) {
    rm -Force -Recurse .\publish
}
dotnet publish .\src\autojump -r win-x64 -c Release --self-contained true -o .\publish
Compress-archive -Path .\publish -DestinationPath .\publish.zip -Force