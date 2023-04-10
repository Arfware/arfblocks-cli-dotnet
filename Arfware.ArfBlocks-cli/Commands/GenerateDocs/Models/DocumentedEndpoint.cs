namespace Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

internal class DocumentedEndpoint
{
	public HttpVerbs HttpVerb { get; set; }
	public string ActionName { get; set; }
	public string RequestPath { get; set; }

	public bool HasRequestModel { get; set; }
	public bool IsResponsePayloadArray { get; set; }
	public string RequestModelAsTypescriptInterface { get; set; }
	public Dictionary<Type, string> RequestClassTypeList { get; set; }

	public bool HasResponseModel { get; set; }
	public string ResponseModelAsTypescriptInterface { get; set; }
	public Dictionary<Type, string> ResponseClassTypeList { get; set; }

	public bool IsRequireAuthorization { get; set; }
}
