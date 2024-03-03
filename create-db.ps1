rm .\tests\autojump.db

sqlite3.exe .\tests\autojump.db "CREATE TABLE locations (id INTEGER PRIMARY KEY, path TEXT NOT NULL, count REAL NOT NULL, last_accessed DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP)"
sqlite3.exe .\tests\autojump.db "Insert into locations (path, count) values ('G:\repos', 1)"
sqlite3.exe .\tests\autojump.db "Insert into locations (path, count) values ('G:\repos\heitech\lol', 1)"
sqlite3.exe .\tests\autojump.db "Insert into locations (path, count) values ('G:\repos\heitech\lol', 1)"
sqlite3.exe .\tests\autojump.db "Insert into locations (path, count) values ('G:\repos\kunden\v1\kamel', 1)"

# todo more data later