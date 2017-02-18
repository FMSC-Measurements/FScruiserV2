SET msbuildPath=C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe
SET solutionDir=%cd%\

%msbuildPath%  .\FSCruiserV2\FScruiserV2CE_CF20.csproj /target:Rebuild /property:Configuration=Release;Platform=AnyCPU;SolutionDir=%solutionDir%

