using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;

namespace ZipStuff
{
    public class ZipStuff
    {
        public static void Main(string[] args)
        {
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.CompressionLevel > 9)
                {
                    Console.WriteLine("Setting compression level to 9");
                }else if (options.CompressionLevel < 0)
                {
                    Console.WriteLine("Setting compression level to 0");
                }

                if (options.CwdOverride != null)
                {
                    Environment.CurrentDirectory = Path.GetFullPath(options.CwdOverride);
                }

                if (options.OutFileName == null)
                {
                    options.OutFileName = options.Method.Equals(Methods.Zip) ? "compressed.zip" : "compressed.tar.gz";
                }

                var outpath = Path.GetFullPath(Path.Combine(options.OutFileDir, options.OutFileName));

                var tocomp = new List<string>();
                foreach (var p in options.Files.Split(','))
                {
                    tocomp.Add(Path.GetFullPath(p).TrimEnd('\\'));
                }

                if (options.Method.Equals(Methods.Zip))
                {
                    var fsOut = File.Create(outpath);
                    var zipstream = new ZipOutputStream(fsOut);
                    zipstream.SetLevel(options.CompressionLevel);
                    zipstream.Password = options.ZipPassword;

                    foreach (var fpath in tocomp)
                    {
                        if (IsDirectory(fpath))
                        {
                            var offset = fpath.Length - Path.GetFileName(fpath).Length;
                            ZipUtils.RecurseDir(fpath, zipstream, offset, outpath);
                        }
                        else
                        {
                            if (fpath.Equals(outpath))
                                continue;

                            ZipUtils.ZipFile(fpath, zipstream, 0);
                        }
                    }

                    zipstream.IsStreamOwner = true;
                    zipstream.Close();
                    fsOut.Close();

                }
                else if (options.Method.Equals(Methods.TarGz))
                {
                    var outStream = File.Create(outpath);
                    var gzStream = new GZipOutputStream(outStream);
                    gzStream.SetLevel(options.CompressionLevel);
                    gzStream.Password = options.ZipPassword;

                    var stream = new TarOutputStream(gzStream);

                    foreach (var fpath in tocomp)
                    {
                        var tpath = fpath.Replace("\\", "/");
                        var offset = fpath.Length - Path.GetFileName(fpath).Length;
                        if (IsDirectory(tpath))
                        {
                            TarUtils.RecurseDir(tpath, stream, offset, outpath);
                        }
                        else
                        {
                            if (fpath.Equals(outpath))
                                continue;

                            TarUtils.TarFile(tpath, stream, offset);
                        }
                    }

                    stream.Close();
                }
            }
            else
            {
                Console.WriteLine("Invalid Options");
            }
        }

        

        private static bool IsDirectory(string path)
        {
            var attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) != 0;
        }

        public static void InvokeZipStuff(string[] args)
        {
            Main(args);
        }
    }

}
