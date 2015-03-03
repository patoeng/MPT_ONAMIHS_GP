@echo OFF
echo Installing Services
serviceEx.exe install loggerUDP
serviceEx.exe install loggerMonitor
serviceEx.exe install loggerChecker             
pause
