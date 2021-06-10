namespace Base64FileSplitter
{
	public class FilePart
	{
		public FilePart(int number, string base64Fragment)
		{
			Number = number;
			Base64Fragment = base64Fragment;
		}

		public int Number { get; set; }
		public string Base64Fragment { get; set; }
	}
}