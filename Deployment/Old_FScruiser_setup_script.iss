; #defines require the ISPP add-on: http://sourceforge.net/projects/ispp/
#define APP "FScruiser"
#define VERSION "2011.04.27"
#define SPECIALTAG "Field Data Recorder"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define DIR "C:\Projects\FScruiser"

[Setup]
AppName={#APP} {#SPECIALTAG}
AppMutex={#APP}
AppID={#APP}
AppVerName={#APP} {#SPECIALTAG} {#VERSION} Beta
AppPublisher=USDA Forest Service, Forest Management Service Center
AppPublisherURL={#BASEURL}
AppSupportURL={#BASEURL}/support.shtml
AppUpdatesURL={#BASEURL}/cruising/fscruiser/index.php
; CurPageChanged in the [Code] section checks if C:\fsapps exists. If it does, it uses it as the default install directory.
DefaultDirName={pf}\FMSC Software\{#APP}
OutputDir={#DIR}\Installations
OutputBaseFilename={#APP}FDR_Setup
OutputManifestFile=Setup-Manifest.txt
; Use a standard setup icon for the FScruiser setup exe.
;SetupIconFile={#DIR}\Installations\Setup.ico <-- apparently not needed (gets the correct icon)
SolidCompression=yes
ChangesAssociations=yes
AllowUNCPath=no
PrivilegesRequired=admin
VersionInfoDescription={#APP} Set Up
VersionInfoCompany=USDA Forest Service, Forest Management Service Center
VersionInfoVersion={#VERSION}
ChangesEnvironment=yes
ShowLanguageDialog=no
; InfoBeforeFile={#DIR}\Installations\InfoBefore.txt
; InfoAfterFile={#DIR}\Installations\InfoAfter.txt

[Languages]
Name: english; MessagesFile: compiler:Default.isl


[Tasks]
Name: AllegroCE; Description: Juniper Allegro CE; GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
Name: AllegroCX; Description: Juniper Allegro CX; GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
;Name: AllegroMX; Description: Juniper Allegro MX; GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
;Name: DAP5320; Description: DAP 5320; GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
;Name: Ranger; Description: Trimble/TDS Ranger with WM5; GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
;Name: PPC2002; Description: Pocket PC 2002 (includes R8's Field Keypad); GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
;Name: PPCMips; Description: Pocket PC 2000 with Mips processor (includes R8's Field Keypad); GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive
;Name: Q200; Description: Itronix Q200; GroupDescription: Field Data Recorder Versions:; Flags: unchecked exclusive


[Dirs]
Name: "{app}"; Permissions: everyone-full
; try next -> 
;Name: "C:\fsapps"; Permissions: everyone-full  (causes install to hang)
; try next -> 
;Name: "{pf}"; Permissions: everyone-full   (causes install to hang)


[Files]
; Load cab files for the requested FDR version.
;Source: {#DIR}\Installations\FDR\cabs\FScruiserPPC2002.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPC2002
Source: {#DIR}\Installations\FDR\cabs\FScruiserAllegroCE.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroCE
Source: {#DIR}\Installations\FDR\cabs\FScruiserAllegroCX.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroCX
;Source: {#DIR}\Installations\FDR\cabs\FScruiserAllegroMX.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroMX
;Source: {#DIR}\Installations\FDR\cabs\FScruiserQ200.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: Q200
;Source: {#DIR}\Installations\FDR\cabs\FScruiserDAP5320.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: DAP5320
;Source: {#DIR}\Installations\FDR\cabs\FScruiserRangerWM5.arm.CAB; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: Ranger
;Source: {#DIR}\Installations\FDR\cabs\FScruiserPPC.mips.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPCMips

; Also load zip files for the requested FDR version.
;Source: {#DIR}\ArmRel\PPC2002\FScruiserPPC2002.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPC2002
Source: {#DIR}\ArmRel\AllegroCE\FScruiserAllegroCE.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroCE
Source: {#DIR}\ArmRel\AllegroCX\FScruiserAllegroCX.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroCX
;Source: {#DIR}\ArmRel\AllegroMX\FScruiserAllegroMX.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroMX
;Source: {#DIR}\ArmRel\Q200\FScruiserQ200.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: Q200
;Source: {#DIR}\ArmRel\DAP5320\FScruiserDAP5320.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: DAP5320
;Source: {#DIR}\MIPSRel\PocketPC\FScruiserPPCMips.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPCMips
;Source: {#DIR}\ArmRel\TDS_Ranger\FScruiserRangerWM5.zip; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: Ranger

; Load Joe Kennedy's Timber Keypad for PPC mips and arm
;Source: {#DIR}\MIPSRel\PocketPC\armpktkb.mips.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPCMips
;Source: {#DIR}\ARMRel\PPC2002\armpktkb10.arm.cab; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPC2002

; Load the ini files used by CEAPPMGR.EXE
;Source: {#DIR}\Installations\FDR\inis\FScruiserPPC2002AppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPC2002
Source: {#DIR}\Installations\FDR\inis\FScruiserAllegroCEAppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroCE
Source: {#DIR}\Installations\FDR\inis\FScruiserAllegroCXAppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroCX
;Source: {#DIR}\Installations\FDR\inis\FScruiserAllegroMXAppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: AllegroMX
;Source: {#DIR}\Installations\FDR\inis\FScruiserQ200AppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: Q200
;Source: {#DIR}\Installations\FDR\inis\FScruiserDAP5320AppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: DAP5320
;Source: {#DIR}\Installations\FDR\inis\FScruiserRangerWM5AppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: Ranger
;Source: {#DIR}\Installations\FDR\inis\FScruiserPPCMipsAppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPCMips
;Source: {#DIR}\Installations\FDR\inis\FieldKeypadPPCArmCEAppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPC2002
;Source: {#DIR}\Installations\FDR\inis\FieldKeypadPPCMipsCEAppMgr.ini; DestDir: {app}\FDR Versions; Flags: ignoreversion; Tasks: PPCMips

; For spash screen
Source: {#DIR}\Installations\FScruiserSplash.bmp; DestDir: {tmp}; Attribs: hidden; Flags: ignoreversion

[Registry]
; Register cruise file

[Icons]
; Start Menu

[Run]
;Filename: {reg:HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CEAPPMGR.EXE,}; Parameters: """{app}\FDR Versions\FScruiserPPC2002AppMgr.ini"""; Description: Install FScruiser on Pocket PC 2002 with ActiveSync.; Flags: skipifdoesntexist postinstall runascurrentuser; Tasks: PPC2002
Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserAllegroCEAppMgr.ini"""; Description: Install FScruiser on Allegro CE with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: AllegroCE
Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserAllegroCXAppMgr.ini"""; Description: Install FScruiser on Allegro CX with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: AllegroCX
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserAllegroMXAppMgr.ini"""; Description: Install FScruiser on Allegro MX with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: AllegroMX
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserQ200AppMgr.ini"""; Description: Install FScruiser on Q200 with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: Q200
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserDAP5320AppMgr.ini"""; Description: Install FScruiser on DAP 5320 with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: DAP5320
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserRangerWM5AppMgr.ini"""; Description: Install FScruiser on Ranger WM5 with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: Ranger
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FScruiserPPCMipsAppMgr.ini"""; Description: Install FScruiser on PPC (Mips) with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: PPCMips


; Joe Kennedy's Timber Keypad
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FieldKeypadPPCArmCEAppMgr.ini"""; Description: Install Field Keypad with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: PPC2002
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}; Parameters: """{app}\FDR Versions\FieldKeypadPPCMipsCEAppMgr.ini"""; Description: Install Field Keypad with ActiveSync.; Flags: skipifdoesntexist postinstall; Tasks: PPCMips


[UninstallRun]
; was getting annoying and not really necessary.
;Filename: {reg:HKLM\Software\Microsoft\Windows\CurrentVersion\App Paths\CEAppMgr.exe,}


[INI]
;Filename: {app}\..\FMSC.ini; Section: Version; Key: FScruiser; String: {#Version}; Flags: uninsdeleteentry


[Code]
//function ShouldSkipPage(PageID: Integer): Boolean;
//begin
//   if(PageID = wpSelectDir) then
//   begin
//       result := true;
//   end;
//end;


//////////// CurPageChanged //////////
procedure CurPageChanged(CurPage: integer);
begin
  if CurPage = wpSelectDir then
  begin
    if(DirExists('C:\fsapps')) then
    begin
      // Must be a FS computer. Change the default install directory.
      WizardForm.DirEdit.Text := 'C:\fsapps\FMSC Software\FScruiser';
    end;
  end;
end;



/////////// InitializeWizard //////////
// This throws up the splash screen
procedure InitializeWizard();
var
  SplashImage: TBitmapImage;
  SplashForm: TForm;
  SplashFileName: String;
  I : Integer;

begin
  SplashFileName := ExpandConstant('{tmp}\FScruiserSplash.bmp');
  ExtractTemporaryFile(ExtractFileName(SplashFileName));

  SplashForm := TForm.create(nil);

  with SplashForm do
  begin
    BorderStyle := bsNone;
    Position := poScreenCenter;
    // Adjust the height and width of the SplashForm to the size of your splash image
    ClientWidth := 540;
    ClientHeight := 295;
  end;

  SplashImage := TBitmapImage.Create(SplashForm);

  with SplashImage do
  begin
    Bitmap.LoadFromFile(SplashFileName);
    Stretch := true;
    Align := alClient;
    Parent := SplashForm;
  end;

  with SplashForm do
  begin
    Show;
    Repaint;
    // show for four seconds
    for I := 1 to 3 do
    begin
      Sleep(1000);
    end;
    Close;
    Free;
  end;
end;



function InitializeSetup() : boolean;
   var Path: String;
begin
Path:= '';
RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CEAPPMGR.EXE','', Path)
if Path = '' then
   begin
   MsgBox('ActiveSync not found! Cannot install directly to the data recorder.', mbError, MB_OK);
   result:= true;									            	//	<-- true instead of false here
   end
else
   result:= true;
end;
