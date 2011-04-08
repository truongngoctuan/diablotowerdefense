
@echo off

rem  ------------------------------------------------------------------
echo.
echo MS Age of Empires Modpack Installation Script
echo Copyright (C) 1997 Stoyan Ratchev. All rights reserved.
echo.
echo Run this script from your Age of Empires installation folder.
echo As a single argument to it specify the name of the modpack
echo you want to install - that is the name of the folder under your "modpack" 
echo folder where the modpack files are located.
echo.
echo To uninstall any modpacks and restore the original AoE resource
echo files, run this script without arguments.
echo.

rem  ------------------------------------------------------------------

echo Checking for being run in the AoE installation folder ...
if not exist data\*.drs goto missing_drs

rem  ------------------------------------------------------------------

echo Checking for existing backups of the AoE resource files ...
if exist data\backup\*.drs goto restore_drs

rem  ------------------------------------------------------------------

echo Existing backups of the AoE resource files were not found
echo in the data\backup folder, creating them ...
mkdir data\backup
xcopy data\*.drs data\backup /v

rem  ------------------------------------------------------------------

:restore_drs
echo Restoring the original AoE resource files ...
xcopy data\backup\*.drs data /v /y

rem  ------------------------------------------------------------------

echo Checking usage ...
if "%1" == "" goto no_arguments

rem  ------------------------------------------------------------------

:install
echo Installing %1 ...

:sounds
if not exist modpack\%1\*.wav goto graphics
drsbuild /r data\*.drs modpack\%1\*.wav /s
if errorlevel 1 goto failed

:graphics
if not exist modpack\%1\*.slp goto complete
drsbuild /r data\*.drs modpack\%1\*.slp /s
if errorlevel 1 goto failed

rem  ------------------------------------------------------------------

:complete
echo Installation completed succeessfully.
goto end

rem  ------------------------------------------------------------------

:no_arguments
echo Started without arguments, nothing to install.
goto end

rem  ------------------------------------------------------------------

:failed
echo Installation failed: DRSBUILD error.
echo Contact the modpack author if you see this message.
goto end

rem  ------------------------------------------------------------------

:missing_drs
echo Installation failed: missing data\*.drs.
echo Make sure you are running this script from your AoE installation 
echo folder.
goto end

rem  ------------------------------------------------------------------

:end
echo.



