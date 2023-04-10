using System.Reflection;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;
using Arfware.ArfBlocksCli.Utils;

namespace Arfware.ArfBlocksCli.Commands.GenerateDocs;

internal class GenerateDocsCommand
{
	private string _targetProject;
	private string _outputPath;
	public GenerateDocsCommand(string targetProject, string outputPath)
	{
		_targetProject = targetProject;
		_outputPath = outputPath;
	}

	public List<DocumentedObject> allDocumentedObjects;

	public void Generate()
	{
		// Generate DLL File By Build Project
		// var _targetProjectInfrastructure = _targetProject.Replace(".Application", ".Infrastructure");
		// var _targetProjectDomain = _targetProject.Replace(".Application", ".Domain");

		// CommandUtils.BuildProject(_targetProjectDomain, "Domain");
		// CommandUtils.BuildProject(_targetProjectInfrastructure, "Infrastructure");
		CommandUtils.BuildProject(_targetProject);

		// Build Assembly Path
		var dllFilesInDirectory = FileUtils.BuildAssemblyPath(_targetProject);

		// Load Assembly and Register Endpoints
		Assembly assembly = null;
		dllFilesInDirectory.ForEach((dllFile) =>
		{
			var tempAssembly = Assembly.LoadFile(dllFile);

			if (dllFile.Contains("Application.dll"))
				assembly = tempAssembly;
		});

		if (assembly == null)
			throw new Exception("Could not find Application DLL in DLL Directory");

		// Register ArfBlocks Commands and Queries
		CommandQueryRegister.RegisterAssembly(assembly);

		// Load Endpoints
		var allEndpoints = CommandQueryRegister.GetAllEndpoints();

		// Build Documented Objects
		this.allDocumentedObjects = DocumentedObjectBuilder.BuildDocumentedObjects(allEndpoints);
		// DocumentedObjectBuilder.DumpDocumentedObjects(this.allDocumentedObjects);

		// Generate Docs
		var outputPath = FileUtils.GetAbsoluteOutputPath(this._outputPath);
		DocWriter.Write(TypeExtractor.enumTypeList, this.allDocumentedObjects, outputPath);

		System.Console.WriteLine("\nGenerateDocs Command Succedded");
	}
}
