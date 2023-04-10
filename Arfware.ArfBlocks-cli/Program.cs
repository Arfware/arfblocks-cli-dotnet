using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;
using Arfware.ArfBlocksCli.Commands.GenerateCode;
using Arfware.ArfBlocksCli.Commands.GenerateDocs;
using Arfware.ArfBlocksCli.Constants;

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

		var generateDocsCommand = new Command("generate-docs")
		{
			new Option<string>(
				"--target-project",
				description: "The project that contains ArfBlocks handlers"),
			new Option<string>(
				"--output",
				description: "The output to extract generated files"),

		};

		var generateCodeCommand = new Command("generate-code")
		{
			new Option<string>(
				"--app-definition-file",
				description: "The application definition file(application-definitions.arfblocks.json) path"),
			new Option<string>(
				"--target-endpoint-types",
				description: "Which type of endpoint(s) you want to generate (c=Create, r=Detail, u=Update, d=Delete, l=All)"),
			new Option<string>(
				"--output-path",
				description: "Output path for generated code"),
		};

		generateDocsCommand.Handler = CommandHandler.Create<string, string>((targetProject, output) =>
		{
			Console.WriteLine("Generate Docs");
			Console.WriteLine($"Project: {targetProject}");
			Console.WriteLine($"Output: {output}");

			System.Console.WriteLine("-------------------------------------------------------------------------");

			var gdc = new GenerateDocsCommand(targetProject, output);
			gdc.Generate();
		});

		// NOTE: (IMPORTANT) method parameters name must be the same with parameters in command
		//   "--app-definition-file"  must be mapped to "appDefinitionFile"
		generateCodeCommand.Handler = CommandHandler.Create<string, string, string>((
			appDefinitionFile, targetEndpointTypes, outputPath) =>
		{
			System.Console.WriteLine(appDefinitionFile == null);
			Console.WriteLine("Generate Code");
			Console.WriteLine($"Application Definition File Path: {appDefinitionFile}");
			Console.WriteLine($"Target Endpoint Types           : {targetEndpointTypes}");
			Console.WriteLine($"Output Path                     : {outputPath}");

			System.Console.WriteLine("-------------------------------------------------------------------------");

			var gcc = new GenerateCodeCommand(appDefinitionFile, targetEndpointTypes, outputPath);
			gcc.Generate().GetAwaiter().GetResult();
		});

		root.AddCommand(generateDocsCommand);
		root.AddCommand(generateCodeCommand);

		await root.InvokeAsync(args);

	}
}
