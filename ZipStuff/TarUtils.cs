using System;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;

namespace ZipStuff
{
    internal class TarUtils
    {
        internal static void TarFile(string filename, TarOutputStream stream, int offset)
        {
            try
            {
                using (var sReader = File.Open(filename, FileMode.Open, FileAccess.Read))
                {
                    var tName = filename.Substring(offset);
                    var fSize = sReader.Length;

                    var tEntry = TarEntry.CreateTarEntry(tName);
                    tEntry.Size = fSize;

                    stream.PutNextEntry(tEntry);

                    var localBuffer = new byte[32 * 1024];
                    while (true)
                    {
                        var numRead = sReader.Read(localBuffer, 0, localBuffer.Length);
                        if (numRead <= 0)
                        {
                            break;
                        }
                        stream.Write(localBuffer, 0, numRead);
                    }

                    stream.CloseEntry();
                }
            }
            catch
            {
                Console.WriteLine($"Access denied on {filename}");
            }
        }

        internal static void RecurseDir(string path, TarOutputStream stream, int offset, string outputpath)
        {
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
                if (Path.GetFullPath(f).Equals(outputpath))
                {
                    continue;
                }

                var temp = f.Replace("\\", "/");

                TarFile(temp, stream, offset);
            }

            foreach (var f in Directory.GetDirectories(path))
            {
                RecurseDir(f, stream, offset, outputpath);
            }
        }
    }
}
