namespace Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

internal class EndpointDefinitionModel
{
	public string ObjectName { get; set; }

	public List<PropertyDefinitionModel> EntityProperties { get; set; }
}
