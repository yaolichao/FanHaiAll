@ECHO OFF
REM The following directory is for .NET4.0
set DOTNETFX=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319;
set PATH=%PATH%;%DOTNETFX%

echo ���ڰ�װ����
echo ---------------------------------------------------

InstallUtil /i ManzSortService.exe


echo ---------------------------------------------------

echo Done.
pause
exit