@echo off
setlocal
set "_LOCKDOWN_DEMO_DIR=%~dp0"
echo Lockdown Breakout - Verify and join both downloaded parts
powershell.exe -NoLogo -NoProfile -Command "$ErrorActionPreference='Stop'; $env:PSModulePath=(Join-Path $PSHOME 'Modules')+[IO.Path]::PathSeparator+$env:PSModulePath; $root=$env:_LOCKDOWN_DEMO_DIR; $zip=Join-Path $root 'LockdownBreakOut-Windows-x64-v0.1.0.zip'; $completeHash='bd915f1f0fa78f7e8e96330af3525ce4ce78f2c2990fcedc0cc640fef2db289f'; if((Test-Path -LiteralPath $zip -PathType Leaf) -and (Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash -eq $completeHash){Write-Host 'Existing ZIP verified. Use Windows Extract All.'; exit 0}; $parts=@(@('LockdownBreakOut-Windows-x64-v0.1.0.zip.001','4c64fd7c92a0b21a5b1a9df0cfdd50f32114afc54bc55eefdf9bcac3480579e1'),@('LockdownBreakOut-Windows-x64-v0.1.0.zip.002','a975212617ef79e6e936d3899f8c1bc62e501439441e5b6949c8a687d00de7e3')); foreach($part in $parts){$path=Join-Path $root $part[0]; if(-not(Test-Path -LiteralPath $path -PathType Leaf)){throw ('Missing part: '+$part[0])}; Write-Host ('Checking '+$part[0]); if((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash -ne $part[1]){throw ('Checksum mismatch. Download this part again: '+$part[0])}}; if(Test-Path -LiteralPath $zip){Write-Host 'Existing ZIP is incomplete or damaged. Rebuilding from verified parts.'}; $pending=$zip+'.pending'; $destination=[IO.File]::Open($pending,[IO.FileMode]::Create,[IO.FileAccess]::Write); try{foreach($part in $parts){$inputFile=[IO.File]::OpenRead((Join-Path $root $part[0])); try{$inputFile.CopyTo($destination)}finally{$inputFile.Dispose()}}}finally{$destination.Dispose()}; if((Get-FileHash -LiteralPath $pending -Algorithm SHA256).Hash -ne $completeHash){throw 'Combined ZIP checksum failed. The previous ZIP has not been replaced.'}; Move-Item -LiteralPath $pending -Destination $zip -Force; Write-Host 'Complete ZIP verified. Right-click it and choose Extract All.'; Write-Host 'Then enter Quest Link and run LockdownBreakOut.exe from the extracted folder.'"
if errorlevel 1 (
  echo Joining failed. Read the error above and keep both parts together.
  pause
  exit /b 1
)
pause
exit /b 0
