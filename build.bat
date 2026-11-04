@echo off
setlocal enableextensions
chcp 65001 >nul
cd /d "%~dp0"

rem ===== Chemin de Inno Setup (modifiez si besoin) =====
set "ISCC=%ProgramFiles(x86)%\Inno Setup 7\ISCC.exe"
if not exist "%ISCC%" set "ISCC=%ProgramFiles%\Inno Setup 7\ISCC.exe"
if not exist "%ISCC%" set "ISCC=%LocalAppData%\Programs\Inno Setup 7\ISCC.exe"

set "PUB=bin\Release\net10.0-windows\publish\win-x64"

echo ============================================
echo  1/5  Restauration et compilation (Release)
echo ============================================
dotnet build ForthEditor.csproj -c Release --nologo
if errorlevel 1 goto :erreur

echo.
echo ============================================
echo  2/5  Publication (profil FolderProfile)
echo ============================================
dotnet publish ForthEditor.csproj -c Release -p:PublishProfile=FolderProfile --nologo
if errorlevel 1 goto :erreur

rem --- Version lue dans l'executable publie (source unique : balise Version de ForthEditor.csproj)
rem     2.4.0.0 -> 2.4   2.4.1.0 -> 2.4.1
set "VER="
for /f "usebackq delims=" %%v in (`powershell -NoProfile -Command "(Get-Item '%PUB%\ForthEditor.exe').VersionInfo.FileVersion -replace '\.0$','' -replace '\.0$',''"`) do set "VER=%%v"
if not defined VER (
    echo Version introuvable dans %PUB%\ForthEditor.exe
    goto :erreur
)
set "VERF=%VER:.=_%"
echo.
echo Version : %VER%

echo.
echo ============================================
echo  3/5  Archive zip portable (GitHub / GitLab)
echo ============================================
set "ZIPNAME=ESP32ForthEditerm_%VERF%_portable.zip"
set "STAGE=%TEMP%\ESP32ForthEditerm_stage"
if exist "%STAGE%" rmdir /s /q "%STAGE%"
robocopy "%PUB%" "%STAGE%\ESP32ForthEditerm" /E /XF *.pdb *.xml /NFL /NDL /NJH /NJS /NP >nul
if errorlevel 8 goto :erreur
copy /y LICENSE "%STAGE%\ESP32ForthEditerm\" >nul
copy /y README.md "%STAGE%\ESP32ForthEditerm\" >nul
if not exist Output mkdir Output
if exist "Output\%ZIPNAME%" del /q "Output\%ZIPNAME%"
tar -a -c -f "Output\%ZIPNAME%" -C "%STAGE%" ESP32ForthEditerm
if errorlevel 1 goto :erreur
rmdir /s /q "%STAGE%"
echo Cree : Output\%ZIPNAME%

echo.
echo ============================================
echo  4/5  Creation de l'installateur (Inno Setup)
echo ============================================
if not exist "%ISCC%" (
    echo Inno Setup introuvable. Editez build.bat et corrigez la variable ISCC.
    goto :erreur
)
"%ISCC%" "ESP32ForthEditor.iss"
if errorlevel 1 goto :erreur

echo.
echo ============================================
echo  5/5  Sommes de controle SHA-256
echo ============================================
powershell -NoProfile -Command "Get-ChildItem 'Output' | Where-Object { $_.Name -like 'ESP32ForthEditerm_%VERF%*' -and ($_.Extension -eq '.exe' -or $_.Extension -eq '.zip') } | ForEach-Object { (Get-FileHash $_.FullName -Algorithm SHA256).Hash.ToLower() + '  ' + $_.Name } | Set-Content -Encoding ascii 'Output\SHA256SUMS_%VERF%.txt'"
if errorlevel 1 goto :erreur

echo.
echo ===== TERMINE : version %VER% =====
dir /b "Output\*%VERF%*"
if /i not "%~1"=="nopause" pause
exit /b 0

:erreur
echo.
echo ===== ECHEC (code %errorlevel%) =====
if /i not "%~1"=="nopause" pause
exit /b 1
