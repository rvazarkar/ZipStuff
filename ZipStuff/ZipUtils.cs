using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ZipStuff
{
    internal class ZipUtils
    {
        internal static void ZipFile(string filename, ZipOutputStream stream, int offset)
        {
            try
            {
                using (var sReader = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    var info = new FileInfo(filename);
                    var entryName = ZipEntry.CleanName(filename.Substring(offset));
                    var zEntry = new ZipEntry(entryName)
                    {
                        DateTime = info.LastWriteTime,
                        Size = info.Length
                    };

                    stream.PutNextEntry(zEntry);
                    var buffer = new byte[4096];
                    StreamUtils.Copy(sReader, stream, buffer);

                    stream.CloseEntry();
                }
            }
            catch
            {
                //Access denied probably
            }
        }

        internal static void RecurseDir(string path, ZipOutputStream stream, int offset, string outpath)
        {
            //Console.WriteLine($"Processing directory: {path}");

            string[] files;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch
            {
                return;
            }

            foreach (var f in files)
            {
                if (f.Equals(outpath))
                {
                    continue;
                }

                ZipFile(f, stream, offset);
            }

            foreach (var f in Directory.GetDirectories(path))
            {
                RecurseDir(f, stream, offset, outpath);
            }
        }
    }
}
