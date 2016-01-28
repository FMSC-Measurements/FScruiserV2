;created using example @ http://codebetter.com/petervanooijen/2006/08/03/deploying-mobile-apps-the-easy-way-using-inno-setup/
; #defines require the ISPP add-on: http://sourceforge.net/projects/ispp/
#define APP "FSCruiserV2"

#define APP_VERSION "2016.01.15"
#define SETUPVERSION "20160115"
#define SPECIALTAG "Production"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define ORGANIZATION "U.S. Forest Service, Forest Management Service Center"


#define DOTNET_INI "netcf20.ini"
#define FSCRUISER_INI "FSCruiserV2_CF20.ini"

[Setup]
AppName=FSCruiser V2
AppVersion={#APP_VERSION}
AppVerName=FSCruiser version {#SETUPVERSION} for Windows CE, and Windows Mobile devices
AppPublisher={#ORGANIZATION}
AppPublisherURL={#BASEURL}
AppSupportURL={#BASEURL}/support.shtml
AppUpdatesURL={#BASEURL}/cruising/index.shtml

DefaultDirName={pf32}\FMSC\{#APP}
DefaultGroupName=FMSC\{#APP}
UsePreviousAppDir=no

CreateAppDir=yes
OutputBaseFilename=FScruiserV2_FDR_{#SETUPVERSION}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=lowest
ShowLanguageDialog=no

InfoBeforeFile=..\Documentation\ConnectingToDevice.md

[Files]
;Compact framework files
Source: "netcf20.ini"; DestDir: "{app}"; Flags: ignoreversion; 
Source: "wce400\armv4\*.CAB"; DestDir: "{app}\wce400\armv4"; 
Source: "wce400\mipsii\*.CAB"; DestDir: "{app}\wce400\mipsii"; 
Source: "wce400\mipsiv\*.CAB"; DestDir: "{app}\wce400\mipsiv"; 
Source: "wce400\sh4\*.CAB"; DestDir: "{app}\wce400\sh4"; 
Source: "wce400\x86\*.CAB"; DestDir: "{app}\wce400\x86"; 
Source: "wce500\armv4i\*.CAB"; DestDir: "{app}\wce500\armv4i"; 
Source: "wce500\mipsii\*.CAB"; DestDir: "{app}\wce500\mipsii"; 
Source: "wce500\mipsiv\*.CAB"; DestDir: "{app}\wce500\mipsiv"; 
Source: "wce500\sh4\*.CAB"; DestDir: "{app}\wce500\sh4"; 
Source: "wce500\x86\*.CAB"; DestDir: "{app}\wce500\x86"; 

;FScruiser mobile files
Source: {#FSCRUISER_INI}; DestDir: "{app}"; Flags: ignoreversion; 
Source: "..\FSCruiserV2CECF20_CAB\Release\FSCruiserV2.CAB"; DestDir: "{app}"; Flags: ignoreversion; 


;Documentation
Source: "..\Documentation\FScruiserV2UserGuide.docx"; DestName:"FScruiserV2UserGuide.docx"; DestDir: "{app}"; Flags: ignoreversion;

[Icons]
Name: {group}\FScruiser V2 User Guide.docx; Filename: {app}\FScruiser V2 User Guide.docx

;[Run]
;FileName: {code:GetCEappManager}; Parameters: {code:GetIniFile|\netcf20.ini} {code:GetIniFile|\FSCruiserV2_CF20.ini}; Flags: runascurrentuser;

;[Tasks]
;Name: mobileInstall; Description: "Install FScruiser on mobile devices"; GroupDescription: "Install Options";


;function InitializeSetup() : Boolean;
;var
;ResultCode: Integer;
;begin
;  if IsAdminLoggedOn() or IsPowerUserLoggedOn() then
;  begin
;    Exec('runas', ExpandConstant('/trustlevel:0x20000 "{srcexe} ') + GetCmdTail() +'"', '', SW_HIDE, ewNoWait, ResultCode)
;    result := False
;  end
;  else
;    result := True
;end;  


[Code]



function GetCEappManager(Param : string) : string;
var Path: String;
begin
  Path:= '';
  RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CEAPPMGR.EXE','', Path)
    if Path = '' then
      if FileExists('C:\Windows\WindowsMobile\ceappmgr.exe') then
        Path:= 'C:\Windows\WindowsMobile\ceappmgr.exe'

      
  result:=  Path
end;

procedure CurStepChanged(CurStep: TSetupStep);
var 
RunCmd : String;
ErrorCode : Integer;
Prams : String;
begin
  if ((CurStep=ssPostInstall)) then
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