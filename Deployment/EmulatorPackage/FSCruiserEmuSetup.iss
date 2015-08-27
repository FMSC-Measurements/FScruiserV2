#define FILEVERSION 20141219

[Setup]
AppName=FSCruiser + Emulator 
AppVersion= 0.1
CreateAppDir=no
DisableDirPage=yes
CreateUninstallRegKey = no
Uninstallable=no
DefaultDirName={pf32}\FMSC\FSCruiser
InfoBeforeFile=infoBefore.txt
DefaultGroupName=FMSC\FSCruiser
AllowNoIcons=yes
;OutputDir="\Output"
OutputBaseFilename=FSCruiserEmuSetup_{#FILEVERSION}
OutputManifestFile=Setup-Manifest.txt
;SetupIconFile=C:\Development\Installation\Setup.ico
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Icons]
Name: "{commondesktop}\Lanunch FSCruiser Emu"; Filename: "{pf32}\Microsoft Device Emulator\1.0\DeviceEmulator.exe" ; Parameters: "/MemSize 128 /s ""%appdata%\Microsoft\Device Emulator\{{E4FC2BC5-3AC4-452C-A893-AD4F273C3A7C}.dess"" /sharedfolder ""%userprofile%/My Documents"""; Tasks: desktopicon;
Name: "{group}\FSCruiser Mobile Emulator"; Filename: "{pf32}\Microsoft Device Emulator\1.0\DeviceEmulator.exe" ; Parameters: "/MemSize 128 /s ""%appdata%\Microsoft\Device Emulator\{{E4FC2BC5-3AC4-452C-A893-AD4F273C3A7C}.dess"" /sharedfolder ""%userprofile%/My Documents""";

[Files]
Source: "Windows Mobile 6 Professional Images (USA).msi"; Flags: dontcopy nocompression;
Source: "{E4FC2BC5-3AC4-452C-A893-AD4F273C3A7C}.dess"; DestDir:  "{userappdata}\Microsoft\Device Emulator";

[Types]
Name: "full"; Description: "Installs Windows Mobile Emulator, and device image"; 
Name: "update"; Description: "Copies over new device image (for users with the emulator alreday installed)"

[Components]
Name: Emulator; Description: "Windows Mobile 6 Emulator Install"; Types: full; Flags: exclusive;
Name: Image; Description: "Device State Image"; Types: full update; Flags: fixed;

[Tasks]
Name: desktopicon; Description: "Create desktop icon"; Components: Image; Flags:


[code]
#define MinJRE "1.6"
procedure CurStepChanged(CurStep: TsetupStep);
var
  ErrorCode: Integer;
  WebJRE: string;
begin
  if CurStep=ssPostInstall then
  begin
    if IsComponentSelected('Emulator') then
    begin
      ExtractTemporaryFile('Windows Mobile 6 Professional Images (USA).msi')
      WebJRE:= '"' + Expandconstant('{tmp}\Windows Mobile 6 Professional Images (USA).msi')+'"'
      ShellExec('',WebJRE, '', '', SW_SHOW,ewWaituntilterminated, Errorcode)
    end;
  end;
end;