namespace Base64FileSplitter
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class FileSplitter
	{
        public string Source { get; }
        public int ChunkSize { get; }

        public FileSplitter(string source, int chunkSize)
		{
            Source = source;
            ChunkSize = chunkSize;		
        }

		public IEnumerable<FilePart> SplitFile()
		{
            var rdr = new BinaryReader(new FileStream(Source, FileMode.Open, FileAccess.Read));
            var counter = 1;
            while (rdr.BaseStream.Position < rdr.BaseStream.Length)
            {
                byte[] b = new byte[ChunkSize];

                long remaining = rdr.BaseStream.Length - rdr.BaseStream.Position;
                if (remaining >= ChunkSize)
                {
                    rdr.Read(b, 0, ChunkSize);
                }
                else
                {
                    rdr.Read(b, 0, (int)remaining);
                }
                string chunkString = Convert.ToBase64String(b);
                yield return new FilePart(counter++, chunkString);
            }
        }
    }
}
