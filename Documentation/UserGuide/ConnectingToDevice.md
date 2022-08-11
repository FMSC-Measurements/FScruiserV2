# Connecting To Device

## Windows Mobile Device Center (WMDC)

To connect your device to a PC, first make sure you have Windows Mobile Device Center (WMDC) installed. If you have used WMDC before, it may automatically start when you connect your device to your PC using USB, otherwise, start WMDC from the start menu.

If using a Forest Service computer you should be able to download and install WMDC using the software center. 
Installing using the software center should also apply any nessicary patches to allow you to use WMDC with Windows 10.

Otherwise if you need to download and install WMDC see download link below. 

[Download WMDC](https://junipersys.com/data/support/drvupdate-amd64.exe)


### Disable Advanced Network Functionality
Your computers firewall settings may be preventing WMDC from connecting to your device if it has Advanced Network Functionality enabled. 
To disable Advanced Network Functionality

1. On the device, go to the connections tab in the settings menu
2. Open USB to PC, and disable "Enable advanced network functionality".

### Windows 10
Some Windows 10 users may experience issues using WMDC. These issues may due to the version of Windows 10 update installed. 

There two are tools available that attempt to fix compatibility issues in Windows 10. You will only need to install one, but they will not conflict with eachother. 

 - [wmdc-fixes-for-win10.msi (recommended)](https://junipersys.com/data/support/WMDC-fixes-for-Win10.msi)
 - [WMDCRegistryUpdate.exe](https://trl.trimble.com/dscgi/ds.py/Get/File-848877/WMDCRegistryUpdate.exe)

 **After running either tool you will need to reboot your computer for the changes to take effect.**

If you are using Internet Explorer, you may need to save the file and run it outside of Internet Explorer, rather than using the "Run" button from inside your browser. 

[Additional information on connecting using Windows 10](http://www.junipersys.com/Juniper-Systems-Rugged-Handheld-Computers/support/Knowledge-Base/Support-Knowledge-Base-Topics/Desktop-Connection-ActiveSync-or-Windows-Mobile-Device-Center/WMDC-in-Windows-10)

## Use Device as External Storage Drive (Allegro Only)
This method appears to be the most reliable and easiest for users using Juniper Allegro 2 devices, since it doesn't require any 
software or configuring on the host computer. Note this method requires your device have a SD card installed and 
not all files on the device will be accessible. Only files located on the SD card can be accessed, and the SD card 
can not be accessed from the device while connected to the PC. 


1. On the device, go to the connections tab in the settings menu
2. Select the option that says "SD Card - Use as external drive"
3. Plug the device into a usb cable connected to the pc. 
4. A new drive labed `USB Drive` should appear under `This PC`



## Thumbdrive 
If you are unable to connect to your device using WMDC, you might still be able to transfer files between your device and PC using a thumb drive or SD card. 

Due to Microsoft ending support for the Windows Mobile operating system 2015. It is becoming increasingly hard to work with Windows Mobile devices. If you experience new issues with connecting to your device please contact us to let us know.  







 
