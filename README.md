# ZipStuff
PowerShell Solution to ZipStuff

# Usage
ZipStuff is intended to be used via the compiled PowerShell script which can be found [here](https://github.com/rvazarkar/ZipStuff/releases/download/v1.0/ZipStuff.ps1).

ZipStuff supports the following options:

* Files - Which files to compress. This is a comma seperated list ("c:\users\admin, ..")
* Method - Which compression method to use. Can be Zip or TarGz
* OutFileName - Name of the compressed file to output
* OutFileDir - Directory in which to store the compressed file
* CompressionLevel - Level of compression to use (Lowest 0 - 9 Highest). Defaults to 7.
* ZipPassword - Optional password to encrypt the file with

# Compiling
Restore packages from NuGet and then build the solution. The PowerShell script will be automatically generated and placed in the binary folder.

# Credits
Thanks to [@mattifestation](https://www.twitter.com/mattifestation) for [Out-CompressedDLL](https://github.com/PowerShellMafia/PowerSploit/blob/master/ScriptModification/Out-CompressedDll.ps1).

Thanks to the developers of [SharpZipLib](https://github.com/icsharpcode/SharpZipLib)
