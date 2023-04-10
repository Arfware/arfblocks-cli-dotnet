namespace Arfware.ArfBlocksCli.Utils;

internal class FileUtils
{
	//************************************************************************************\\
	// File Utils
	//************************************************************************************\\

	public static string GetAbsoluteOutputPath(string outputPath)
	{
		return Path.Combine(Directory.GetCurrentDirectory(), outputPath);
	}

	public static List<string> BuildAssemblyPath(string targetProject)
	{
		var targetProjectDirectory = FileUtils.GetTargetProjectDirectory(targetProject);
		var targetProjectName = FileUtils.GetProjectNameWithoutExtention(targetProject);
		var targetProjectDllName = $"{targetProjectName}.dll";
		var dllDirectory = Path.Combine(Directory.GetCurrentDirectory(), targetProjectDirectory, "bin", "Release", "net6.0", "publish");
		// var dllDirectory = Path.Combine(Directory.GetCurrentDirectory(), targetProjectDirectory, "bin", "Release", "net6.0", targetProjectDllName);
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
