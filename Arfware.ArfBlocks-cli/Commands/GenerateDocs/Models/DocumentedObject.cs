
namespace Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

internal class DocumentedObject
{
	public string ObjectName { get; set; }
	public List<DocumentedEndpoint> DocumentedEndpoints { get; set; }
}
