namespace Arfware.ArfBlocksCli;

internal class CommandFile
{
	public string Project { get; set; }
	public string Company { get; set; }
	public string MinCliVersion { get; set; }
	public List<CommandDefinition> Commands { get; set; }
}