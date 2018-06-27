#
# Publish.ps1
#

Param(
  [string]$BuildConfiguration = "Release",
  [string]$SolutionDirPathRelativeToWorkingFolder = "..\",
  [string]$CsProjPath = 'Adhr\Adhr.csproj',
  [string]$MsBuildVerbosity = 'quiet',
  [string]$DevEnvDir = "C:\Program Files (x86)\Microsoft Visual Studio 14.0\",
  [string]$BuildNumber = "1.0.0.0",
  [string]$PublishDir = "publish\",
  [string]$InstallUrl = "http://screenlightbeta.blob.core.windows.net/package/",
)

$workingDirectory = Get-Location
Write-Host "Working directory is: $workingDirectory"

$solutionDirAbsolutePath = [System.IO.Path]::GetFullPath((Join-Path $workingDirectory $SolutionDirPathRelativeToWorkingFolder))
Write-Host "Solution dir absolute path: $solutionDirAbsolutePath" -Debug

$csProjAbsolutePath = Join-Path $solutionDirAbsolutePath $CsProjPath

#$msBuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.EXE"
$msBuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.EXE"

[xml]$projectFile = Get-Content "$csProjAbsolutePath"
$projectFile.Project.ChildNodes.Item(1).ApplicationVersion = "$BuildNumber"
$projectFile.Save("$csProjAbsolutePath")

Write-Host "BuildNumber: $BuildNumber" -Debug

#calling msbuild to build and publish the project
& $msBuild $csProjAbsolutePath `
/p:Configuration=$BuildConfiguration `
/p:Platform=x86 /p:OutputPath=bin\$BuildConfiguration\ `
/p:SolutionDir=$solutionDirAbsolutePath `
/t:publish `
/v:$MsBuildVerbosity `
/nologo `
/p:PublishDir=$PublishDir `
/p:PublisherName="NetAcademia" `
/p:InstallUrl=$InstallUrl `
/p:DevEnvDir=$DevEnvDir 

Write-Host "Powershell script ended :)"