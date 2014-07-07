using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace LoggingConfigUpgrader
{
	internal class Program
	{
		static string _appenderFragment;

		static void Main(string[] args)
		{
			string path = args[0];
			string apiKey = args[1];

			if (args.Length != 2)
			{
				Console.Error.Write("Expected 2 arguments:  LoggingConfigUpgrade.exe <path> <apiKey>");
				Environment.Exit(1);
			}

			CreateFragmentForDataDogAppender(apiKey);

			string[] matchingFiles = Directory.GetFiles(path, "logging.config", SearchOption.AllDirectories);

			foreach (string file in matchingFiles)
			{
				string binPath = Path.Combine(Path.GetDirectoryName(file), "bin");
				BackupFile(file);
				CopyAppenderToBinFolder(binPath);
				AddLoggerConfigToFile(file);
			}

			Console.WriteLine("\r\n All done! Press <enter> to exit");
			Console.ReadLine();
		}

		static void CopyAppenderToBinFolder(string binPath)
		{
			if (!Directory.Exists(binPath)) return;
			var currentDirectory = GetExecutingFolder();
			const string dllFileName = "DevDefined.Datadog.log4net.dll";
			string appenderDllFile = Path.Combine(currentDirectory, dllFileName);
			string target = Path.Combine(binPath, dllFileName);
			File.Copy(appenderDllFile, target, true);
			Console.WriteLine("Copied file: " + appenderDllFile + " to path: " + target);
		}

		static string GetExecutingFolder()
		{
			var currentDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
			return currentDirectory;
		}

		static void CreateFragmentForDataDogAppender(string apiKey)
		{
			_appenderFragment = @"<appender name=""datadog"" type=""DevDefined.Datadog.log4net.DatadogAppender,DevDefined.Datadog.log4net"">
      <param name=""ApiKey"" value=""__key__"" />
      <layout type=""log4net.Layout.PatternLayout,log4net"">
        <param name=""ConversionPattern"" value=""%m%n%nLogger: %c%nThread: %t%nLocation: %location%n%n"" />
      </layout>
    </appender>".Replace("__key__", apiKey);
		}

		static void AddLoggerConfigToFile(string file)
		{
			XDocument doc = XDocument.Parse(File.ReadAllText(file));
			if (doc.Root.Elements("appender").Any(x => x.Attribute("name").Value == "datadog")) return;
			doc.Root.Add(XElement.Parse(_appenderFragment));
			doc.Root.Elements("root").First().Add(XElement.Parse(@"<appender-ref ref=""datadog"" />"));
			doc.Save(file);
			Console.WriteLine("Added datadoghq appender to logging.config file: " + file);
		}

		static void BackupFile(string file)
		{
			var backupPath = Path.Combine(Path.GetDirectoryName(file), "backup_" + Path.GetFileName(file));
			File.Copy(file, backupPath,true);
			Console.WriteLine("Backing up file: " + file + " to: "+ backupPath);
		}
	}
}