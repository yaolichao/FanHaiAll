@ECHO OFF

REM The following directory is for .NET3.5
set DOTNETFX=%SystemRoot%\Microsoft.NET\Framework\v2.0.50727;%SystemRoot%\Microsoft.NET\Framework\v3.0;%SystemRoot%\Microsoft.NET\Framework\v3.5;
set PATH=%PATH%;%DOTNETFX%

echo ���ڰ�װ  CHINT MES  ��������Ӧ�ó���
echo ---------------------------------------------------

InstallUtil /i FanHai.Hemera.Services.WipJobAutoTrack.exe

echo ---------------------------------------------------

echo Done.
pause
exit