@ECHO OFF
REM The following directory is for .NET4.0
set DOTNETFX=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319;%SystemRoot%\Microsoft.NET\Framework\v2.0.50727;
set PATH=%PATH%;%DOTNETFX%

echo ����ж�ط���
echo ---------------------------------------------------

InstallUtil /U IVTestService.exe

echo ---------------------------------------------------

echo Done.
pause
exit