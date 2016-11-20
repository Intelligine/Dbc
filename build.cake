#tool nuget:?package=xunit.runner.console
#tool nuget:?package=GitVersion.CommandLine
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var verbosity = (Cake.Core.Diagnostics.Verbosity)Enum.Parse(typeof(Cake.Core.Diagnostics.Verbosity),Argument("verbosity", "Minimal"));

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define variables.
var srcBinGlob = "./src/**/bin/"+configuration;
var tstBinGlob = "./tst/**/bin/"+configuration;

var artifactsDir = Directory("./artifacts");
var libraryArtifactsDir = Directory("./artifacts/library");

var solutionName = "Intelligine.DesignByContract";
var solutionFile = string.Format("./{0}.sln",solutionName);

var nugetFilePaths = GetFiles("./src/*/*.csproj",fileSystemInfo => !fileSystemInfo.Path.FullPath.EndsWith("Tests", StringComparison.OrdinalIgnoreCase));

var versionInfo = (GitVersion)null;

var author ="Lars Kjær Thomsen";
var authors = new[] {author};
var company ="Intelligine";
var owners = new[] {author};
var projectUrl = new Uri("https://github.com/Intelligine/DesignByContract/");
var licenseUrl = new Uri("http://opensource.org/licenses/MIT");
var copyright = string.Format("© {0} 2016 - {1}", author,DateTime.Now.Year);
var product = solutionName;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories(srcBinGlob);
    CleanDirectories(tstBinGlob);

    CleanDirectories(artifactsDir);

	CreateDirectory(artifactsDir);
	CreateDirectory(libraryArtifactsDir);
});


Task("Restore").Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Version")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Use MSBuild
    MSBuild(solutionFile, settings => 
	{
		settings.SetConfiguration(configuration);
		settings.SetVerbosity(verbosity);
		settings.SetMaxCpuCount(0);
	});
}).Finally(() =>
{  
	RunTarget("RestoreMetadata");
});

Task("Test")
    .IsDependentOn("Build")
	.Does(() =>
	{
	
		var testAssemblyes =  GetFiles("./src/*.Tests/*.csproj").Select(project => project​.GetDirectory().FullPath + "/bin/"+ configuration + "/"+ ParseProject(project​.FullPath).AssemblyName+".dll");

		XUnit2(testAssemblyes, new XUnit2Settings { Parallelism = ParallelismOption.All, ShadowCopy = false});

	});


Task("RestoreMetadata").Does(() =>
{
	string file = "GlobalAssemblyInfo.cs";

	string temp = "GlobalAssemblyInfo.cs.temp";

	CopyFile(temp, file);

	DeleteFile(temp);
});

Task("Version")
    .Does(() =>
{
	string file = "GlobalAssemblyInfo.cs";

	string temp = "GlobalAssemblyInfo.cs.temp";
	
	CopyFile(file, temp);
	
	CreateAssemblyInfo(file, new AssemblyInfoSettings {
		Product = product,
		Copyright = copyright,
		Company = company,
		Configuration = configuration
	});

    versionInfo = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
		UpdateAssemblyInfoFilePath = file
    });

	if(AppVeyor.IsRunningOnAppVeyor){
		AppVeyor.UpdateBuildVersion(string.Format("{0}.build.{1}",versionInfo.FullSemVer,AppVeyor.Environment.Build.Number));
	}
});

Task("Package")
.IsDependentOn("Build")
.Does(() => { 

	foreach(var project in nugetFilePaths){

		var parsedProject = ParseProject(project​.FullPath);
		var dir = project.GetDirectory();

		var document = new System.Xml.XmlDocument();
		document.Load(string.Format("{0}/packages.config",dir));

		var dependencies =document.SelectNodes("/packages/package").Cast<System.Xml.XmlNode>().Select(package=> new NuSpecDependency {Id = package.Attributes["id"].Value, Version = package.Attributes["version"].Value}).ToArray();

		var nuGetPackSettings   = new NuGetPackSettings {
                            Id                      = parsedProject.AssemblyName,
                            Version                 = versionInfo.NuGetVersion,
                            Title                   = parsedProject.AssemblyName.Replace(".", " "),
                            Authors                 = authors,
                            Owners                  = owners,
                            Description             = @"Provides support for Design By Contract
													as described by Bertrand Meyer in his seminal book,
													Object-Oriented Software Construction (2nd Ed) Prentice Hall 1997
													(See chapters 11 and 12).
													See also Building Bug-free O-O Software: An Introduction to Design by Contract
													http://www.eiffel.com/doc/manuals/technology/contract/
													",
                            ProjectUrl              = projectUrl,
                            LicenseUrl              = licenseUrl,
                            Copyright               = copyright,
                            Tags                    = new [] {company,"DesignByContract", "DbC"},
                            NoPackageAnalysis       = true,
							BasePath = string.Format("{0}/bin/{1}",dir.FullPath,configuration),
							OutputDirectory = libraryArtifactsDir,
                            Files                   = new [] {
                                                                 new NuSpecContent {Source = parsedProject.AssemblyName+".dll", Target = "lib/net"+parsedProject.TargetFrameworkVersion.Replace("v","").Replace(".","")},
                                                             },
							Dependencies			= dependencies
                        };
		
		NuGetPack(nuGetPackSettings);
	}
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("CI")
	.IsDependentOn("Build")
    .IsDependentOn("Test")
	.IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);