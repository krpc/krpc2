#!/bin/bash

# Install steam
# Download KSP2
# Configure the game to run in Steam:
#   In compatibility settings set it to run with proton experimental
#   Use these launch options:
#     PROTON_USE_WINED3D=1 bash -c 'exec "${@/%"PDLauncher/LauncherPatcher.exe"/KSP2_x64.exe}"' -- %command% -popupwindow
#
# Download and install SpaceWarp in the KSP2 folder
#
# Configure BepInEx to run (https://docs.bepinex.dev/articles/advanced/proton_wine.html)
# Install protontricks:
#   sudo apt install flatpak
#   flatpak install com.github.Matoking.protontricks
#   flatpak remote-add --if-not-exists flathub https://flathub.org/repo/flathub.flatpakrepo
#   echo "alias protontricks='flatpak run com.github.Matoking.protontricks'" >> ~/.bashrc
#   echo "alias protontricks-launch='flatpak run --command=protontricks-launch com.github.Matoking.protontricks'" >> ~/.bashrc
# Run winecfg:
#   protontricks 954850 winecfg
# In the wine config dialog, click on the libraries tab. Where it says New override for library enter winhttp then click the Add button.
# At the bottom of list you should now see winhttp (native, builtin), click Apply/OK to save and close dialog.
#
# Then run this script to launch KSP2 and output the BepInEx logs

set -ev

tools/install.sh

echo "" > lib/ksp2/BepInEx/LogOutput.log
steam steam://rungameid/954850 &
# tail -f lib/ksp2/PDLauncher/Ksp2.log
tail -f lib/ksp2/BepInEx/LogOutput.log
