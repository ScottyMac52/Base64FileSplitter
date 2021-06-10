
namespace Base64FileSplitter
{
	using CommandLine;
	public class Options
	{
		[Option('f', "file", Required = true, HelpText = "Full path to the file you want to split")]
		public string CurrentFile { get; set; }

		[Option('b', "blockSize", Required = false, Default = 16000, HelpText = "The block size to use for the file split")]
		public int BlockSize { get; set; } = 16000;
	}
}
