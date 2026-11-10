@echo off
setlocal
cd /d "%~dp0"
if exist "LockdownBreakOut-Windows-x64-v0.1.0.zip" (
  echo The complete ZIP already exists. Extract it with Windows Extract All.
  pause
  exit /b 0
)
if not exist "LockdownBreakOut-Windows-x64-v0.1.0.zip.001" (
  echo Missing LockdownBreakOut-Windows-x64-v0.1.0.zip.001. Download all parts into this folder.
  pause
  exit /b 1
)
if not exist "LockdownBreakOut-Windows-x64-v0.1.0.zip.002" (
  echo Missing LockdownBreakOut-Windows-x64-v0.1.0.zip.002. Download all parts into this folder.
  pause
  exit /b 1
)
echo Combining the game archive. Please wait...
copy /b "LockdownBreakOut-Windows-x64-v0.1.0.zip.001"+"LockdownBreakOut-Windows-x64-v0.1.0.zip.002" "LockdownBreakOut-Windows-x64-v0.1.0.zip"
if errorlevel 1 (
  echo Joining failed. Keep the parts and check free disk space.
  pause
  exit /b 1
)
echo Complete: LockdownBreakOut-Windows-x64-v0.1.0.zip
echo Right-click the complete ZIP and choose Extract All.
echo Then enter Quest Link and run LockdownBreakOut.exe in the extracted folder.
pause
exit /b 0
