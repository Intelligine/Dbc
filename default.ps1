#globals
$GIT_VERSION = Join-Path $TOOLS_DIR GitVersion.CommandLine\tools\GitVersion.exe

$TEMP_DIR = "$BuildRoot\temp"
$ARTIFACTS_DIR = "$BuildRoot\artifacts"

Set-Alias GitVersion ($GIT_VERSION)

#tasks
task Restore -If {$SkipPackageRestore -eq $false} {
    exec { & dotnet restore }
}

task Version {
    $versionJson = exec { GitVersion } | Out-String
    $script:Version = ConvertFrom-Json $versionJson

    $buildNumber = $script:Version.FullSemVer + ".build."  + $env:APPVEYOR_BUILD_NUMBER

    $script:buildNumber = $buildNumber
    $env:branchName =  $script:Version.BranchName

}, PublishVersion


task Clean {
    exec { & dotnet clean }

    rm $TEMP_DIR -Recurse -Force -ErrorAction 0
    rm $ARTIFACTS_DIR -Recurse -Force -ErrorAction 0

    mkdir $TEMP_DIR -Force | Out-Null
    mkdir $ARTIFACTS_DIR -Force | Out-Null
}

task Build Version, Restore, Clean, {
    $AssemblySemFileVer = $script:Version.AssemblySemFileVer
    $AssemblySemVer = $script:Version.AssemblySemVer
    $InformationalVersion = $script:Version.InformationalVersion
    exec { & dotnet build -v $Verbosity -c $Configuration --no-restore /p:AssemblyVersion=$AssemblySemVer /p:FileVersion=$AssemblySemFileVer /p:InformationalVersion=$InformationalVersion}
}

task Pack Build, {

    $packageVersion = $script:Version.FullSemVer
    $script:packageVersion = $packageVersion

    exec { & dotnet pack src\Dbc --no-build --no-restore --output "$ARTIFACTS_DIR\lib" /p:PackageVersion=$script:packageVersion /p:NoPackageAnalysis=true -c $Configuration}
}


task Push -If $env:APPVEYOR { 

    if($env:branchName  -like "master") {

        Get-Item "$ARTIFACTS_DIR\lib\*.nupkg" | % {
            $path = $_.FullName
            exec { Nuget push $_.FullName $env:Nuget_Api_Key -Source $env:Nuget_Feed_Push }
        }
    }
}

task Release Push -If $env:APPVEYOR

task CI Pack, Release

task default Build

task PublishVersion -If $env:APPVEYOR {
    Update-AppveyorBuild -Version $script:buildNumber
}
