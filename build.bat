@echo off
setlocal
chcp 65001 >nul
cd /d "%~dp0"

rem ===== Chemin de Inno Setup (modifiez si besoin) =====
set "ISCC=%ProgramFiles(x86)%\Inno Setup 7\ISCC.exe"
if not exist "%ISCC%" set "ISCC=%ProgramFiles%\Inno Setup 7\ISCC.exe"
if not exist "%ISCC%" set "ISCC=%LocalAppData%\Programs\Inno Setup 7\ISCC.exe"

echo ============================================
echo  1/3  Restauration et compilation (Release)
echo ============================================
dotnet build ForthEditor.csproj -c Release --nologo
if errorlevel 1 goto :erreur

echo.
echo ============================================
echo  2/3  Publication (profil FolderProfile)
echo ============================================
dotnet publish ForthEditor.csproj -c Release -p:PublishProfile=FolderProfile --nologo
if errorlevel 1 goto :erreur

echo.
echo ============================================
echo  3/3  Creation de l'installateur (Inno Setup)
echo ============================================
if not exist "%ISCC%" (
    echo Inno Setup introuvable. Editez build.bat et corrigez la variable ISCC.
    goto :erreur
)
"%ISCC%" "ESP32ForthEditor.iss"
if errorlevel 1 goto :erreur

echo.
echo ===== TERMINE : installateur dans "%~dp0Output" =====
if /i not "%~1"=="nopause" pause
exit /b 0

:erreur
echo.
echo ===== ECHEC (code %errorlevel%) =====
if /i not "%~1"=="nopause" pause
exit /b 1
