using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;
using System.Text.Json;
using Arfware.ArfBlocksCli.Commands.GenerateCode;
using Arfware.ArfBlocksCli.Commands.GenerateDocs;
using Arfware.ArfBlocksCli.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Arfware.ArfBlocksCli;

internal class Program
{
	private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		// System.Console.WriteLine($"Want to resolve: {args.Name}");
		// ((AppDomain)sender).GetAssemblies().ToList().ForEach((e) =>
		// {
		// 	System.Console.WriteLine(e.FullName);
		// });
		var assembly = ((AppDomain)sender).GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
		// System.Console.WriteLine($"### Loaded: {assembly.FullName}");
		return assembly;
	}

	public static async Task Main(string[] args)
	{
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

		// Global Runtime Settings
		RuntimeSettings.IsVerboseEnabled = true;

		var root = new RootCommand
		{
			// new Option<bool>("v", description:"Verbose mode")
		};

		var commandsViaFileCommand = new Command("exec")
		{
			new Option<string>(
				"--file",
				description: "The file that contains ArfBlocks-cli commands as json format"),
		};

		commandsViaFileCommand.Handler = CommandHandler.Create<string>((file) =>
		{
			System.Console.WriteLine(file == null);
			Console.WriteLine("Command Via File");
			Console.WriteLine($"File Path: {file}");

			if (!File.Exists(file))
			{
				throw new Exception($"File not exist: '{file}'");
			}

			System.Console.WriteLine("-------------------------------------------------------------------------");

			var fileContent = File.ReadAllText(file);
			var commandFile = JsonSerializer.Deserialize<CommandFile>(fileContent, new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			});

			foreach (var command in commandFile.Commands)
			{
				System.Console.WriteLine($"Looking for command: {command.Name}");
				System.Console.WriteLine($"Parameter Count: {command.Parameters.Count}");
				switch (command.Name)
				{
					case "generate-docs":
						System.Console.WriteLine($"Detected Command: 'generate-docs'");
						var generateDocsCmd = new GenerateDocsCommand(command.Parameters);
						generateDocsCmd.Generate();
						break;

					case "generate-code":
						System.Console.WriteLine($"Detected Command: 'generate-code'");
						var generateCodeCmd = new GenerateCodeCommand(command.Parameters);
						generateCodeCmd.Generate().GetAwaiter().GetResult();
						break;

					default:
						throw new Exception($"Command not recognized: {command.Name}");
				}
			}
		});

		root.AddCommand(commandsViaFileCommand);

		await root.InvokeAsync(args);
	}
}
