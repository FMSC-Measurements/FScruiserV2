;created using example @ http://codebetter.com/petervanooijen/2006/08/03/deploying-mobile-apps-the-easy-way-using-inno-setup/
; #defines require the ISPP add-on: http://sourceforge.net/projects/ispp/
#define APP "FSCruiserV2"

#define SETUPVERSION "20150310"
#define SPECIALTAG "Production"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define ORGANIZATION "U.S. Forest Service, Forest Management Service Center"


#define DOTNET_INI "netcf20.ini"
#define FSCRUISER_INI "FSCruiserV2_CF20.ini"

[Setup]
AppName=FSCruiser V2
AppVerName=FSCruiser version {#SETUPVERSION} for Windows CE, and Windows Mobile devices
AppPublisher={#ORGANIZATION}
AppPublisherURL={#BASEURL}
AppSupportURL={#BASEURL}/support.shtml
AppUpdatesURL={#BASEURL}/cruising/index.shtml

DefaultDirName={pf32}\FMSC\{#APP}
DefaultGroupName=FMSC\{#APP}
UsePreviousAppDir=no

CreateAppDir=yes
OutputBaseFilename=FScruiserV2_Setup_{#SETUPVERSION}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
ShowLanguageDialog=no

[Files]
;Compact framework files
Source: "netcf20.ini"; DestDir: "{app}"; Flags: ignoreversion; Tasks: mobileInstall;
Source: "wce400\armv4\*.CAB"; DestDir: "{app}\wce400\armv4"; Tasks: mobileInstall;
Source: "wce400\mipsii\*.CAB"; DestDir: "{app}\wce400\mipsii"; Tasks: mobileInstall;
Source: "wce400\mipsiv\*.CAB"; DestDir: "{app}\wce400\mipsiv"; Tasks: mobileInstall;
Source: "wce400\sh4\*.CAB"; DestDir: "{app}\wce400\sh4"; Tasks: mobileInstall;
Source: "wce400\x86\*.CAB"; DestDir: "{app}\wce400\x86"; Tasks: mobileInstall;
Source: "wce500\armv4i\*.CAB"; DestDir: "{app}\wce500\armv4i"; Tasks: mobileInstall;
Source: "wce500\mipsii\*.CAB"; DestDir: "{app}\wce500\mipsii"; Tasks: mobileInstall;
Source: "wce500\mipsiv\*.CAB"; DestDir: "{app}\wce500\mipsiv"; Tasks: mobileInstall;
Source: "wce500\sh4\*.CAB"; DestDir: "{app}\wce500\sh4"; Tasks: mobileInstall;
Source: "wce500\x86\*.CAB"; DestDir: "{app}\wce500\x86"; Tasks: mobileInstall;

;FScruiser mobile files
Source: {#FSCRUISER_INI}; DestDir: "{app}"; Flags: ignoreversion; Tasks: mobileInstall;
Source: "..\FSCruiserV2CECF20_CAB\Release\FSCruiserV2.CAB"; DestDir: "{app}"; Flags: ignoreversion; Tasks: mobileInstall;

;FScruiser desktop files
Source: "..\FSCruiserV2\bin\x86\Release\FScruiserPC.exe"; DestDir: "{app}"; Flags: ignoreversion; Tasks: desktopInstall;
Source: "..\FSCruiserV2\bin\x86\Release\CruiseDAL.dll"; DestDir: "{app}"; Flags: ignoreversion; Tasks: desktopInstall;
Source: "..\FSCruiserV2\bin\x86\Release\FMSC.Utility.dll"; DestDir: "{app}"; Flags: ignoreversion; Tasks: desktopInstall;
Source: "..\FSCruiserV2\bin\x86\Release\FMSCUI_FF35.dll"; DestDir: "{app}"; Flags: ignoreversion; Tasks: desktopInstall;
Source: "..\FSCruiserV2\bin\x86\Release\FMSC.Sampling_FF35.dll"; DestDir: "{app}"; Flags: ignoreversion; Tasks: desktopInstall;
Source: "..\FSCruiserV2\bin\x86\Release\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion; Tasks: desktopInstall;

;Documentation
Source: "..\Documentation\FScruiserV2UserGuide.docx"; DestName:"FScruiserV2UserGuide_{#SETUPVERSION}.docx"; DestDir: "{app}"; Flags: ignoreversion;

[Icons]
Name: {userdesktop}\FScruiserV2; Filename: {app}\FScruiserPC.exe;
Name: {group}\FScruiser V2 User Guide.docx; Filename: {app}\FScruiser V2 User Guide.docx

;[Run]
;FileName: {code:GetCEappManager}; Parameters: {code:GetIniFile|\netcf20.ini} {code:GetIniFile|\FSCruiserV2_CF20.ini}; Flags: runascurrentuser;

[Tasks]
Name: mobileInstall; Description: "Install FScruiser on mobile devices"; GroupDescription: "Install Options";
Name: desktopInstall; Description: "Install FScruiser on this PC (alpha version)"; GroupDescription: "Install Options"; Flags: unchecked;


[Code]
function GetCEappManager(Param : string) : string;
var Path: String;
begin
  Path:= '';
  RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CEAPPMGR.EXE','', Path)
    if Path = '' then
      if FileExists('C:\Windows\WindowsMobile\ceappmgr.exe') then
        Path:= 'C:\Windows\WindowsMobile\ceappmgr.exe'

      
  result:= Path
end;

procedure CurStepChanged(CurStep: TSetupStep);
var 
RunCmd : String;
ErrorCode : Integer;
Prams : String;
begin
  if ((CurStep=ssPostInstall) and IsTaskSelected('mobileInstall')) then
  begin
    RunCmd:= GetCEappManager('');
    if RunCmd <>'' then
    begin
      Prams:= ExpandConstant('"{app}\{#DOTNET_INI}" "{app}\{#FSCRUISER_INI}"');
      Exec(RunCmd, Prams, '', SW_SHOW, ewNoWait, ErrorCode)
    end
    else
    begin
      MsgBox('Unable to locate CEAppMgr.exe Ensure that Windows Mobile Device Center or ActiveSync is installed properly', mbInformation, MB_OK);
    end;
     
  end;   
end;