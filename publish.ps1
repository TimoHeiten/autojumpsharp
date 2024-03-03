# todo
# package /nuget and push to github?
# publish app as exe file

# publish as single file
if (Test-Path .\publish) {
    rm -Force -Recurse .\publish
}
dotnet publish .\src\autojump -r win-x64 -c Release --self-contained true -o .\publish
