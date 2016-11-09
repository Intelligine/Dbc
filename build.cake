#tool nuget:?package=xunit.runner.console
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var srcBinGlob = "./src/**/bin/"+configuration;
var tstBinGlob = "./tst/**/bin/"+configuration;

var binDir = Directory("./bin") ;
var artifactsDir = Directory("./artifacts");

var solutionFile = "./Intelligine.DesignByContract.sln";

CreateDirectory(artifactsDir);

var nugetFilePaths = GetFiles("./src/*/*.csproj");//.Concat(GetFiles("./**/*.nuspec"));

var nuGetPackSettings = new NuGetPackSettings
{   
  BasePath = binDir + Directory(configuration),
  OutputDirectory = artifactsDir,
  ArgumentCustomization = args => args.Append("-Prop Configuration=" + configuration)
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories(srcBinGlob);
    CleanDirectories(tstBinGlob);
    CleanDirectories(artifactsDir);
});

Task("Restore-NuGet")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Restore").IsDependentOn("Restore-NuGet");

Task("Build")
    .IsDependentOn("Restore-NuGet")
    .Does(() =>
{
    // Use MSBuild
    MSBuild(solutionFile, settings => 
	{
		settings.SetConfiguration(configuration);
		settings.SetVerbosity(Verbosity.Minimal);
		settings.SetMaxCpuCount(0);
	});
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
	
	var testAssemblyes =  GetFiles("./tst/*.Tests/*.csproj").Select(project => project​.GetDirectory().FullPath + "/bin/"+ configuration + "/"+ ParseProject(project​.FullPath).AssemblyName+".dll");

    XUnit2(testAssemblyes, new XUnit2Settings { Parallelism = ParallelismOption.All, ShadowCopy = false});

});

Task("Package")
.IsDependentOn("Build")
.Does(() => NuGetPack(nugetFilePaths, nuGetPackSettings));

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);