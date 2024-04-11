using System;

namespace Arfware.ArfBlocksCli.Utils;

internal class CommandUtils
{
	public static bool BuildProject(string targetProject, string framework)
	{
		if (RuntimeSettings.IsVerboseEnabled)
			System.Console.WriteLine($"\nStarted building target project");

		var command = "dotnet";
		var arguments = $"publish {targetProject} -c Release -f {framework} --self-contained=false";
		if (RuntimeSettings.IsVerboseEnabled)
			System.Console.WriteLine($"Running: {command} {arguments}");

		CommandUtils.ExecuteCommand(command, arguments);

		if (RuntimeSettings.IsVerboseEnabled)
			System.Console.WriteLine($"Finished building target project\n");

		return true;
	}

	private static string ExecuteCommand(string command, string parameters)
	{
		try
		{
			// create the ProcessStartInfo using "cmd" as the program to be run,
			// and "/c " as the parameters.
			// Incidentally, /c tells cmd that we want it to execute the command that follows,
			// and then exit.
			System.Diagnostics.ProcessStartInfo procStartInfo =
				new System.Diagnostics.ProcessStartInfo(command, parameters);

			// The following commands are needed to redirect the standard output.
			// This means that it will be redirected to the Process.StandardOutput StreamReader.
			procStartInfo.RedirectStandardOutput = true;
			procStartInfo.UseShellExecute = false;
			// Do not create the black window.
			procStartInfo.CreateNoWindow = true;
			// Now we create a process, assign its ProcessStartInfo and start it
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.StartInfo = procStartInfo;
			proc.Start();
			// Get the output into a string
			string result = proc.StandardOutput.ReadToEnd();
			// Display the command output.

			// New Line '\n' Correction:
			if (result.EndsWith("\n"))
				result = result.Substring(0, result.Length - 1);

			return result;
		}
		catch (Exception objException)
		{
			// Log the exception
			System.Console.WriteLine(objException);
			return "0000000000000000000000000000000000000000";
		}
	}
}
