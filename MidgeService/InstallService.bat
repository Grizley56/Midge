@echo off

net stop Midge

cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
installutil.exe -u %~dp0\MidgeService.exe
installutil.exe %~dp0\MidgeService.exe

net start Midge

if ERRORLEVEL 1 goto error
exit
:error
echo There was a problem
pause