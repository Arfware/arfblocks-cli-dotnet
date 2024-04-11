using Microsoft.Build.Construction;

namespace Arfware.ArfBlocksCli.Utils;

internal class FrameworkUtils
{
	//************************************************************************************\\
	// File Utils
	//************************************************************************************\\

	public static string GetTargetFramework(string targetProject)
	{
		var determinedFramework = string.Empty;
		var projectRootElement = ProjectRootElement.Open(targetProject);
		return projectRootElement.Properties.Where(p => p.Name == "TargetFramework").Select(p => p.Value).FirstOrDefault();
	}

	public static string GetAbsoluteOutputPath(string outputPath)
	{
		return Path.Combine(Directory.GetCurrentDirectory(), outputPath);
	}

	public static List<string> BuildAssemblyPath(string targetProject, string framework)
	{
		var targetProjectDirectory = FrameworkUtils.GetTargetProjectDirectory(targetProject);
		var targetProjectName = FrameworkUtils.GetProjectNameWithoutExtention(targetProject);
		var targetProjectDllName = $"{targetProjectName}.dll";
		var dllDirectory = Path.Combine(Directory.GetCurrentDirectory(), targetProjectDirectory, "bin", "Release", framework, "publish");
		// var dllDirectory = Path.Combine(Directory.GetCurrentDirectory(), targetProjectDirectory, "bin", "Release", framework, targetProjectDllName);
		var allFilesInDllDirectory = Directory.GetFiles(dllDirectory);
		var dllsFilesInDllDirectory = allFilesInDllDirectory?
										.Where(d => Path.GetExtension(d) == ".dll"
												&& !d.Contains("Microsoft.")
												&& !d.Contains("System."))
										.ToList() ?? new List<string>();

		if (RuntimeSettings.IsVerboseEnabled)
		{
			System.Console.WriteLine($"Listing DLLs in: {dllDirectory}");
			dllsFilesInDllDirectory.ForEach(dllFile =>
			{
				System.Console.WriteLine($"Will Load: {dllFile}");
			});
		}

		return dllsFilesInDllDirectory;
	}

	private static string GetTargetProjectDirectory(string targetProject)
	{
		var parts = targetProject.Split('/');
		return targetProject.Replace(parts[parts.Length - 1], "");
	}

	private static string GetProjectNameWithoutExtention(string targetProject)
	{
		var parts = targetProject.Split("/");
		return parts[parts.Length - 1].Replace(".csproj", "");
	}
}
