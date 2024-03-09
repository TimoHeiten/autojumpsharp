# todo
# package /nuget and push to github?
# publish app as exe file

rm -Force -Recurse .\src\autojump\bin
rm -Force -Recurse .\src\autojump\obj

dotnet build .\src\autojump -c Release

if (Test-Path .\publish) {
    rm -Force -Recurse .\publish
}
if (Test-Path .\autojump-app.zip) {
    rm -Force .\autojump-app.zip
}

# publish as single file
if (Test-Path .\publish) {
    rm -Force -Recurse .\publish
}
dotnet publish .\src\autojump -r win-x64 -c Release --self-contained true -o .\publish
mv .\publish .\autojump
cp .\src\autojump\config.json .\autojump\config.json
gci .\autojump -File -Recurse | Compress-archive -DestinationPath .\autojump.zip -Force

if (Test-Path .\autojump) {
    rm -Force -Recurse .\autojump
}