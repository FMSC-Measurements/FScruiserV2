;created using example @ http://codebetter.com/petervanooijen/2006/08/03/deploying-mobile-apps-the-easy-way-using-inno-setup/
; #defines require the ISPP add-on: http://sourceforge.net/projects/ispp/
#define VERSION "2014.08.12"
#define SPECIALTAG "Beta"

[Setup]
AppName=FSCruiser V2
AppVerName=FSCruiser version {#VERSION} for Windows Mobile 5 and 6 
AppPublisher=U.S. Forest Service, Forest Management Service Center
AppPublisherURL=http://www.fs.fed.us/fmsc/measure/
CreateAppDir=yes
DefaultDirName={pf}\FSCruiserV2
OutputBaseFilename=FSCruiserSetup_WM_{#VERSION}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Files]
Source: "netcf.ini"; DestDir: "{app}";
Source: "NETCFv35.wce.armv4.cab"; DestDir: "{app}";
Source: "NETCFv35.ppc.armv4.cab"; DestDir: "{app}";
Source: "NETCFv35.wm.armv4i.cab"; DestDir: "{app}";

Source: "FSCruiserV2.ini"; DestDir: "{app}";
Source: "..\FSCruiserV2WM5_CAB\Release\FSCruiserV2WM5_CAB.CAB"; DestDir: "{app}";

;[Run]
;FileName: {code:GetCEappManager}; Parameters: {code:GetIniFile|\netcf.ini} {code:GetIniFile|\FSCruiserV2.ini}; Flags: runascurrentuser;

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
  if CurStep=ssPostInstall then
  begin
    RunCmd:= GetCEappManager('');
    if RunCmd <>'' then
    begin
      Prams:= ExpandConstant('"{app}\netcf.ini" "{app}\FSCruiserV2.ini"');
      Exec(RunCmd, Prams, '', SW_SHOW, ewNoWait, ErrorCode)
    end
    else
    begin
      MsgBox('Unable to locate CEAppMgr.exe Ensure that Windows Mobile Device Center or ActiveSync is installed properly', mbInformation, MB_OK);
    end;
     
  end;   
end;


