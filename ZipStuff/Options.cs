using CommandLine;

namespace ZipStuff
{
    internal class Options
    {
        [Option(DefaultValue = Methods.Zip)]
        public Methods Method { get; set; }

        [Option(DefaultValue = ".")]
        public string Files { get; set; }

        [Option(DefaultValue = null)]
        public string OutFileName { get; set; }

        [Option(DefaultValue = ".")]
        public string OutFileDir { get; set; }

        [Option(DefaultValue = 7)]
        public int CompressionLevel { get; set; }

        [Option(DefaultValue = null)]
        public string ZipPassword { get; set; }

        //DO NOT USE THIS OPTION
        [Option(DefaultValue =  null)]
        public string CwdOverride { get; set; }
    }
}
