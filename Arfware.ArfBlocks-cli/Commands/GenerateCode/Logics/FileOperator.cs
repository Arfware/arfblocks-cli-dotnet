using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

namespace Arfware.ArfBlocksCli.Commands.GenerateCode.Logics;

internal class FileOperator
{
	public static async Task<string> Read(string path)
	{
		if (!File.Exists(path))
			throw new Exception($"File not exist: {path}");

		var streamReader = new StreamReader(path);
		var templateContent = await streamReader.ReadToEndAsync();
		streamReader.Close();

		System.Console.WriteLine("\n\n\n\n");
		System.Console.WriteLine(templateContent);

		return templateContent;
	}

	public static async Task<bool> Write(string path, string template)
	{
		try
		{
			var streamWriter = new StreamWriter(path);
			await streamWriter.WriteAsync(template);
			streamWriter.Close();

			return true;
		}
		catch (System.Exception e)
		{
			System.Console.WriteLine(e.Message);
			return false;
		}
	}
}