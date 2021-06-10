using CommandLine;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Base64FileSplitter
{
	class Program
	{
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

		private static void SplitFileUsingOptions(Options o)
		{
			var splitter = new FileSplitter(o.CurrentFile, o.BlockSize);
			var fileInfo = new FileInfo(o.CurrentFile);
			WriteMessageToConsole("Splitting file...");
			var splitParts = splitter.SplitFile();
			Parallel.ForEach(splitParts, async (part) =>
			{
				var fileName = Path.Combine(fileInfo.Directory.FullName, fileInfo.Name.Replace(".", $"{part.Number}"));
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
