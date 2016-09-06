;created using example @ http://codebetter.com/petervanooijen/2006/08/03/deploying-mobile-apps-the-easy-way-using-inno-setup/
; #defines require the ISPP add-on: http://sourceforge.net/projects/ispp/
#define APP "FSCruiserV2"

#define APP_VERSION "2016.09.06"
#define SETUPVERSION "20160906"
#define SPECIALTAG "Production"
#define BASEURL "http://www.fs.fed.us/fmsc/measure"
#define ORGANIZATION "U.S. Forest Service, Forest Management Service Center"


#define DOTNET_INI "netcf20.ini"
#define FSCRUISER_INI "FSCruiserV2_CF20.ini"

[Setup]
AppName         =FSCruiser V2
AppVersion      ={#APP_VERSION}
AppVerName      =FSCruiser version {#SETUPVERSION} for Windows CE, and Windows Mobile devices
AppPublisher    ={#ORGANIZATION}
AppPublisherURL ={#BASEURL}
AppSupportURL   ={#BASEURL}/support.shtml
AppUpdatesURL   ={#BASEURL}/cruising/index.shtml

;this is not a tyical installer and doesn't install anything on the users computer
Uninstallable   = no 
CreateAppDir    = no

ShowLanguageDialog      = no
DisableProgramGroupPage = yes
DefaultGroupName        =FMSC\{#APP}


OutputBaseFilename=FScruiserV2_FDR_{#SETUPVERSION}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=lowest


InfoBeforeFile=..\Documentation\ConnectingToDevice.md

[Files]
;Compact framework files
Source: "netcf20.ini"; DestDir: "{localappdata}\{#APP}\FDR_Install\"; Flags: ignoreversion; 
Source: "wce400\armv4\*.CAB"; DestDir: "{localappdata}\{#APP}\FDR_Install\\wce400\armv4";  
Source: "wce500\armv4i\*.CAB"; DestDir: "{localappdata}\{#APP}\FDR_Install\\wce500\armv4i"; 


;FScruiser mobile files
Source: {#FSCRUISER_INI}; DestDir: "{localappdata}\{#APP}\FDR_Install"; Flags: ignoreversion; 
Source: "..\FSCruiserV2CECF20_CAB\Release\FSCruiserV2.CAB"; DestDir: "{localappdata}\{#APP}\FDR_Install"; Flags: ignoreversion; 

;Documentation
Source: "..\Documentation\FScruiserV2UserGuide.docx"; DestName:"FScruiserV2UserGuide.docx"; DestDir: "{userappdata}\{#APP}"; Flags: ignoreversion;

[Icons]
Name: {group}\FScruiser V2 User Guide.docx; Filename: {localappdata}\{#APP}\FScruiser V2 User Guide.docx


[Tasks]
Name: netcf; Description: "Install Compact Framework 2.0  (Required for: Allegro CX)"; Flags: unchecked

[Code]
function GetCEappManager(Param : string) : string;
var Path: String;
begin
  Path:= '';
  RegQueryStringValue(HKEY_LOCAL_MACHINE, 'SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CEAPPMGR.EXE','', Path)
    if Path = '' then
      if FileExists('C:\Windows\WindowsMobile\ceappmgr.exe') then
        Path:= 'C:\Windows\WindowsMobile\ceappmgr.exe'

  if Path = '' then
  begin
    MsgBox('Unable to locate CEAppMgr.exe Ensure that Windows Mobile Device Center or ActiveSync is installed', mbInformation, MB_OK);
  end    
  result:=  Path
end;

procedure CurStepChanged(CurStep: TSetupStep);
var 
CEAppMgrPath : String;
ErrorCode : Integer;
Prams : String;
begin
  if ((CurStep=ssPostInstall)) then
  begin
    CEAppMgrPath:= GetCEappManager('');
    if CEAppMgrPath <>'' then
    begin
          

      Prams:= ExpandConstant(' "{localappdata}\{#APP}\FDR_Install\{#FSCRUISER_INI}"');
      ExecAsOriginalUser(CEAppMgrPath , Prams, '', SW_SHOW, ewNoWait, ErrorCode); 
        
    end

    if IsTaskSelected('netcf') then
    begin
        ExecAsOriginalUser(CEAppMgrPath 
      , ExpandConstant(' "{localappdata}\{#APP}\FDR_Install\{#DOTNET_INI}"')
      , '', SW_SHOW, ewNoWait, ErrorCode);
    end
  end;   
end;