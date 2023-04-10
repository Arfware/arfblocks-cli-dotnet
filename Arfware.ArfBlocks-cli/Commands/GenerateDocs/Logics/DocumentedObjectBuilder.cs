using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

namespace Arfware.ArfBlocksCli.Commands.GenerateDocs;

internal class DocumentedObjectBuilder
{
	public static void DumpDocumentedObjects(List<DocumentedObject> documentedObjects)
	{
		// documentedObjects.ForEach((documentedObject) =>
		// {
		// 	System.Console.WriteLine("\n*************************************************************************\n");
		// 	System.Console.WriteLine($"## {documentedObject.ObjectName}");

		// 	documentedObject.DocumentedEndpoints.ForEach((documentedEndpoint) =>
		// 	{
		// 		System.Console.WriteLine("\n-------------------------------------------------------------------------\n");
		// 		System.Console.WriteLine($":: {documentedEndpoint.ActionName}");
		// 		System.Console.WriteLine($"{documentedEndpoint.RequestModelAsTypescriptInterface}");
		// 		System.Console.WriteLine($"{documentedEndpoint.ResponseModelAsTypescriptInterface}");
		// 	});
		// });

		System.Console.WriteLine("_____________________________ CLASSES __________________________________________");

		// foreach (var kv in TypeExtractor.classTypeList)
		// {
		// 	System.Console.WriteLine($"\n## {kv.Key.Name}");
		// 	System.Console.WriteLine($"\n{kv.Value}");
		// }

		System.Console.WriteLine("______________________________ ENUMS _________________________________________");

		// foreach (var kv in TypeExtractor.enumTypeList)
		// {
		// 	System.Console.WriteLine($"\n## {kv.Key.Name}");
		// 	System.Console.WriteLine($"\n{kv.Value}");
		// }
	}

	//************************************************************************************\\
	// Build Documented Object
	//************************************************************************************\\

	public static List<DocumentedObject> BuildDocumentedObjects(List<EndpointModel> endpoints)
	{
		var documentedObjects = new List<DocumentedObject>();

		endpoints.ForEach((endpoint) =>
		{
			if (endpoint.IsInternal)
				return;

			var documentedObject = documentedObjects.FirstOrDefault(d => d.ObjectName == endpoint.ObjectName);
			if (documentedObject == null)
			{
				documentedObject = new DocumentedObject()
				{
					ObjectName = endpoint.ObjectName,
					DocumentedEndpoints = new List<DocumentedEndpoint>(),
				};
				documentedObjects.Add(documentedObject);
			}

			var documentedEndoint = new DocumentedEndpoint()
			{
				HttpVerb = endpoint.EndpointType == EndpointModel.EndpointTypes.Command ? HttpVerbs.POST : HttpVerbs.GET,
				ActionName = endpoint.ActionName,
				RequestPath = $"/{endpoint.ObjectName}/{endpoint.ActionName}",
				IsRequireAuthorization = endpoint.IsAuthorize,
				IsResponsePayloadArray = endpoint.IsResponseModelArray,
			};
			documentedObject.DocumentedEndpoints.Add(documentedEndoint);

			if (endpoint.RequestModel != null)
			{
				var response = TypeExtractor.CreateTypescriptClass(endpoint.ObjectName, endpoint.ActionName, endpoint.RequestModel, EndpointModelType.RequestModel);
				documentedEndoint.RequestModelAsTypescriptInterface = response.classAsTypescript;
				documentedEndoint.RequestClassTypeList = response.types;
				documentedEndoint.HasRequestModel = true;
			}

			if (endpoint.ResponseModel != null)
			{
				var response = TypeExtractor.CreateTypescriptClass(endpoint.ObjectName, endpoint.ActionName, endpoint.ResponseModel, EndpointModelType.ResponseModel);
				documentedEndoint.ResponseModelAsTypescriptInterface = response.classAsTypescript;
				documentedEndoint.ResponseClassTypeList = response.types;
				documentedEndoint.HasResponseModel = true;
			}
		});

		return documentedObjects;
	}
}
