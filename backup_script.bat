@echo off
set TIMESTAMP=%DATE:~10,4%-%DATE:~4,2%-%DATE:~7,2%_%TIME:~0,2%-%TIME:~3,2%-%TIME:~6,2%
set FILENAME=C:\Users\Даня\source\repos\MeatCountefeitDetector\Backup-%TIMESTAMP%.sql

pg_dump --username=postgres --dbname=CounterfeitKB --clean --create --file="%FILENAME%"

set FILENAME=C:\Users\Даня\source\repos\MeatCountefeitDetector\Backup-%TIMESTAMP%.sql

pg_dump --username=postgres --dbname=ResultDB --clean --create --file="%FILENAME%"
