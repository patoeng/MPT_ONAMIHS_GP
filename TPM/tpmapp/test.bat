@echo off
START "" "IEXPLORE.EXE"
IF EXIST "C:\MCDAILYPROMPT\TPM Daily Prompt.exe"  START "" "C:\MCDAILYPROMPT\TPM Daily Prompt.exe" %1 %2

