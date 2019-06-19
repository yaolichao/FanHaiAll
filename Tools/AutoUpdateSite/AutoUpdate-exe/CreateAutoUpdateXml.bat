
REM 设置为当前目录
for %%i in (%0) do set aa=%%~dpi 
cd /d %aa%

for /r . %%i in (*.pdb) do del /s/a/f/q "%%i" 
for /r . %%i in (*.db) do del /s/a/f/q "%%i" 
for /r . %%i in (*.db) do del /s/aHS/f/q "%%i" 
AutoUpdate /o
exit 0