param($currentVersion)

$currentVersion = [System.Version]"$currentBuildNumber"
$majorVersion = $currentVersion.Major
$minorVersion = $currentVersion.Minor
$currentVersionBuild = $currentVersion.Build

$newBuildNumber = "$majorVersion.$minorVersion.$currentVersionBuild"
Write-Host "Setting the TeamCity build number to $newBuildNumber"
Write-Host "##teamcity[buildNumber '$newBuildNumber']"

function Update-SourceVersion
{
  Param ([int]$major, [int]$minor, [int]$build, [int]$revision)
  $NewVersion = 'AssemblyVersion("' + "$major.0.0.0" + '")';
  $NewFileVersion = 'AssemblyFileVersion("' + "$major.$minor.$build.0" + '")';
  $NewInformationalVersion = 'AssemblyInformationalVersion("' + "$major.$minor.$build" + '")';

	Write-Host "Setting the assembly version to $NewVersion"
	Write-Host "Setting the assembly file version to $NewFileVersion"
	Write-Host "Setting the assembly informational version to $NewInformationalVersion"


  foreach ($o in $input) 
  {
    Write-output $o.FullName
    $TmpFile = $o.FullName + ".tmp"

     get-content $o.FullName | 
        %{$_ -replace 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $NewVersion } |
        %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $NewFileVersion }  | 
		%{$_ -replace 'AssemblyInformationalVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $NewInformationalVersion } > $TmpFile

     move-item $TmpFile $o.FullName -force
  }
}


foreach ($file in "AssemblyInfo.cs") 
{
get-childitem -recurse |? {$_.Name -eq $file} | Update-SourceVersion $majorVersion $minorVersion $currentVersionBuild $currentVersionRevision
}
