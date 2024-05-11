using System.Reflection;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocksCli.Commands.GenerateCode.Logics;
using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;
using Arfware.ArfBlocksCli.Utils;

namespace Arfware.ArfBlocksCli.Commands.GenerateCode;

internal class GenerateCodeCommand
{
	private string _targetEndpointTypes;
	private string _outputPath;
	public GenerateCodeCommand(List<CommandParameter> commandParameters)
	{
		ParseParams(commandParameters);
	}

	public void ParseParams(List<CommandParameter> parameters)
	{
		var targetEndpointTypesParameterKey = "targetEndpointTypes";
		var targetProjectParemeter = parameters.FirstOrDefault(p => p.Key == targetEndpointTypesParameterKey);
		if (targetProjectParemeter == null)
			throw new Exception($"Parameter is null: {targetEndpointTypesParameterKey}");
		_targetEndpointTypes = targetProjectParemeter.Value;

		var outputPathParameterKey = "outputPath";
		var outputPathParemeter = parameters.FirstOrDefault(p => p.Key == outputPathParameterKey);
		if (targetProjectParemeter == null)
			throw new Exception($"Parameter is null: {outputPathParameterKey}");
		_outputPath = targetProjectParemeter.Value;
	}

	public List<DocumentedObject> allDocumentedObjects;

	public async Task Generate()
	{
		var endpointDefiniton = new EndpointDefinitionModel()
		{
			ObjectName = "User",
			EntityProperties = new List<PropertyDefinitionModel>()
			{
				new PropertyDefinitionModel(){IsKey=true, IsPrimitive=true,Name="Id", PropertyTypeAsString="Guid"},
				new PropertyDefinitionModel(){IsKey=false,IsPrimitive=true, Name="FirstName", PropertyTypeAsString="string"},
				new PropertyDefinitionModel(){IsKey=false,IsPrimitive=true, Name="LastName", PropertyTypeAsString="string"},
			}
		};

		var codeGenerator = new CodeGenerator(endpointDefiniton, _outputPath);
		var isCreated = await codeGenerator.Generate(_targetEndpointTypes.ToCharArray());

		System.Console.WriteLine("-------------------------------------------------------------------------");

		if (isCreated)
			System.Console.WriteLine("Succedded 'Create' Command ");
		else
			System.Console.WriteLine("Failed 'Create' Command ");
	}
}
