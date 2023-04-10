namespace Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

internal class PropertyDefinitionModel
{
	public bool IsKey { get; set; }
	public bool IsPrimitive { get; set; }

	public string Name { get; set; }
	public string PropertyTypeAsString { get; set; }
}
