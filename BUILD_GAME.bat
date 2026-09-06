@echo off
setlocal
cd /d "%~dp0"
py -3 HxHGameBuilder\build_game.py %*
if errorlevel 1 (
  echo Build interrompu. Consultez BUILD_REPORT_SAFE.md
  pause
  exit /b 1
)
echo Jeu pret dans Builds\Windows
pause

