@ECHO OFF
REM The following directory is for .NET4.0
set DOTNETFX=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319;
set PATH=%PATH%;%DOTNETFX%

echo ���ڰ�װ����
echo ---------------------------------------------------

REM InstallUtil /i ManzSortService.exe
InstallUtil /i IVTestService.exe

echo ---------------------------------------------------

echo Done.
pause
exit