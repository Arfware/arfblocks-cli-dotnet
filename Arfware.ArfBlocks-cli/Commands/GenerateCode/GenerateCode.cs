using System.Reflection;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocksCli.Commands.GenerateCode.Logics;
using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;
using Arfware.ArfBlocksCli.Utils;

namespace Arfware.ArfBlocksCli.Commands.GenerateCode;

internal class GenerateCodeCommand
{
	private string _appDefinitionFile;
	private string _targetEndpointTypes;
	private string _outputPath;
	public GenerateCodeCommand(string appDefinitionFile, string targetEndpointTypes, string outputPath)
	{
		_appDefinitionFile = appDefinitionFile;
		_targetEndpointTypes = targetEndpointTypes;
		_outputPath = outputPath;
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
