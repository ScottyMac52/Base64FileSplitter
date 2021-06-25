using CommandLine;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Base64FileSplitter
{
	class Program
	{
		static string FilePattern { get; set; }

		static int Main(string[] args)
		{
			try
			{
				Parser.Default.ParseArguments<Options>(args)
			.WithParsed(o =>
			{
				if (!File.Exists(o.CurrentFile))
				{
					WriteErrorToConsole($"Unable to find file {o.CurrentFile}.");
				}
				else
				{
					FilePattern = GetFilePattern(o.CurrentFile);
					SplitFileUsingOptions(o);
				}
			});

			}
			catch (Exception ex)
			{
				WriteErrorToConsole(ex.ToString());
			}
            return 0;
         }

		private static string GetFilePattern(string currentFile)
		{
			var fileInfo = new FileInfo(currentFile);
			var extensionReplacement = fileInfo.Name.Replace($"{fileInfo.Extension}", $"-.part<<INDEX>>");
			return Path.Combine(fileInfo.Directory.FullName, extensionReplacement);
		}

		private static void SplitFileUsingOptions(Options o)
		{
			var splitter = new FileSplitter(o.CurrentFile, o.BlockSize);
			WriteMessageToConsole("Splitting file...");
			var splitParts = splitter.SplitFile();
			Parallel.ForEach(splitParts, async (part) =>
			{
				var fileName = FilePattern.Replace("<<INDEX>>", part.Number.ToString());
				await File.WriteAllTextAsync(fileName, part.Base64Fragment);
			});
			WriteMessageToConsole($"File was split into {splitParts.Count()} parts");
		}

		static void WriteMessageToConsole(string message)
		{
            var color = Console.ForegroundColor;
            var backColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = color;
        }

        static void WriteErrorToConsole(string message)
		{
            var color = Console.ForegroundColor;
            var backColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.BackgroundColor = backColor;
            Console.ForegroundColor = color;
        }
    }
}
