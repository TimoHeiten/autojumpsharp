.\build.ps1 -c "Release"

cd .\src\autojump\bin\Release\net6.0\
.\autojump.exe cd "kamel"
write-host "Should read kamel"

.\autojump.exe cd "repos"
# even though repos has multiple lines in the db, it should still read repos since kamel was visited
write-host "Should also read kamel"

cd ..\..\..\..\..
