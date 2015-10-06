param(
		$semVer = '1.0.0',
        $configuration = 'Release'
    )
    

$msbuild = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
$buildScriptDir = "$PsScriptRoot"
$solutionDir = "$buildScriptDir\.." | Resolve-Path
$buildDir = "$solutionDir\BuildOutput"
$logDir = "$buildDir\log"
$packagesDir = "$buildDir\packages"
$nugetPath = Join-Path $buildDir "nuget.exe"
$solution = (Get-ChildItem -path $solutionDir -filter '*.sln' | select -first 1).FullName
$nuspecs = (Get-ChildItem -path $solutionDir -recurse -filter '*.nuspec')

if (!(Test-Path $logDir)) {
	mkdir $logDir
}
if (!(Test-Path $packagesDir)) {
	mkdir $packagesDir
}
if (!(Test-Path $nugetPath)) {
	Invoke-WebRequest 'https://nuget.org/nuget.exe' -OutFile $nugetPath
}

. $buildScriptDir\set-semver.ps1 $semVer

. $nugetPath restore "$solution" -Verbosity detailed


. $msbuild "$solution" /m /nr:false /t:Build `
	/p:SolutionDir=$solutionDir\ `
	/p:Configuration=$configuration `
	/flp1:verbosity=normal`;LogFile=$logDir\msbuild.normal.log `
	/flp2:WarningsOnly`;LogFile=$logDir\msbuild.warnings.log `
	/flp3:PerformanceSummary`;NoSummary`;verbosity=quiet`;LogFile=$logDir\msbuild.performance.log

foreach($nuspec in $nuspecs) {
	. $nugetPath pack $nuspec.FullName -BasePath $buildDir\$configuration -Verbosity detailed -version $semVer -properties "releaseNotes=''" -OutputDirectory $packagesDir
}