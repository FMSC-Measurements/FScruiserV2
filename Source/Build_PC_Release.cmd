SET msbuildPath=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe
SET solutionDir=%cd%\

%msbuildPath%  .\FSCruiserV2\FScruiserPC.csproj /target:Rebuild /property:Configuration=Release;Platform=AnyCPU;SolutionDir=%solutionDir%

