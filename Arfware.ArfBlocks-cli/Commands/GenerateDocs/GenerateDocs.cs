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
	private string _outputImports;
	private string _outputTypes;
	private string _outputSettings;
	private string _requestUrl;
	public GenerateDocsCommand(List<CommandParameter> commandParameters)
	{
		ParseParams(commandParameters);
	}

	public void ParseParams(List<CommandParameter> parameters)
	{
		var targetProjectParameterKey = "targetProject";
		var targetProjectParameter = parameters.FirstOrDefault(p => p.Key == targetProjectParameterKey);
		if (targetProjectParameter == null)
			throw new Exception($"Parameter is null: {targetProjectParameterKey}");
		_targetProject = targetProjectParameter.Value;

		var outputParameterKey = "output";
		var outputParameter = parameters.FirstOrDefault(p => p.Key == outputParameterKey);
		if (outputParameter == null)
			throw new Exception($"Parameter is null: {outputParameterKey}");
		_outputPath = outputParameter.Value;

		var outputImportsParameterKey = "outputImports";
		var outputImportsParameter = parameters.FirstOrDefault(p => p.Key == outputImportsParameterKey);
		if (outputImportsParameter == null)
			throw new Exception($"Parameter is null: {outputImportsParameterKey}");
		_outputImports = outputImportsParameter.Value;

		var outputTypesParameterKey = "outputTypes";
		var outputTypesParameter = parameters.FirstOrDefault(p => p.Key == outputTypesParameterKey);
		if (outputTypesParameter == null)
			throw new Exception($"Parameter is null: {outputTypesParameterKey}");
		_outputTypes = outputTypesParameter.Value;

		var outputSettingsParameterKey = "outputSettings";
		var outputSeetingsParameter = parameters.FirstOrDefault(p => p.Key == outputSettingsParameterKey);
		if (outputSeetingsParameter == null)
			throw new Exception($"Parameter is null: {outputSettingsParameterKey}");
		_outputSettings = outputSeetingsParameter.Value;

		var requestUrlParameterKey = "requestUrl";
		var requestUrlParameter = parameters.FirstOrDefault(p => p.Key == requestUrlParameterKey);
		if (requestUrlParameter == null)
			throw new Exception($"Parameter is null: {requestUrlParameterKey}");
		_requestUrl = requestUrlParameter.Value;
	}

	public List<DocumentedObject> allDocumentedObjects;

	public void Generate()
	{
		// Generate DLL File By Build Project
		// var _targetProjectInfrastructure = _targetProject.Replace(".Application", ".Infrastructure");
		// var _targetProjectDomain = _targetProject.Replace(".Application", ".Domain");

		// CommandUtils.BuildProject(_targetProjectDomain, "Domain");
		// CommandUtils.BuildProject(_targetProjectInfrastructure, "Infrastructure");
		var framework = FrameworkUtils.GetTargetFramework(_targetProject);
		if (framework == null || string.IsNullOrEmpty(framework))
		{
			System.Console.WriteLine($"Target project's framework could not determined. Check project: {_targetProject}");
			return;
		}

		if (RuntimeSettings.IsVerboseEnabled)
			System.Console.WriteLine($"Determined framework: {framework}");
		CommandUtils.BuildProject(_targetProject, framework);

		// Build Assembly Path
		var dllFilesInDirectory = FrameworkUtils.BuildAssemblyPath(_targetProject, framework);

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
		var outputPath = FrameworkUtils.GetAbsoluteOutputPath(this._outputPath);
		DocWriter.Write(TypeExtractor.enumTypeList, this.allDocumentedObjects, outputPath, _outputImports, _outputTypes, _outputSettings, _requestUrl);

		System.Console.WriteLine("\nGenerateDocs Command Succedded");
	}
}
