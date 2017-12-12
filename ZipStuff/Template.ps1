
function Invoke-ZipStuff{
    <#
    .SYNOPSIS

        Zips stuff up

    .DESCRIPTION

        Using reflection and assembly.load, load the compiled ZipStuff C# binary into memory
        and run it without touching disk. Parameters are converted to the equivalent CLI arguments
        for the ZipStuff executable and passed in via reflection. The appropriate function
        calls are made in order to ensure that assembly dependencies are loaded properly.

    .PARAMETER Method

        Which Compression Method to use (Zip, TarGz)
        Defaults to Zip

    .PARAMETER Files

        Which files to compress (comma seperate list)
        Defaults to "." or current directory

    .PARAMETER OutFileName

        Filename for compressed file. 
        Defaults to "compressed.<filextension>"

    .PARAMETER OutFileDir

        Directory to place compressed file
        Defaults to "." or current directory

    .PARAMETER CompressionLevel

        Compression level from 0-9.
        Defaults to 7

    .PARAMETER ZipPassword

        Password to protect the zip file
        
    .EXAMPLE

        PS C:\> Invoke-ZipStuff

        Compresses the current directory to compressed.zip

    .EXAMPLE
        
        PS C:\> Invoke-ZipStuff -Files "C:\Users\rvazarkar,."

        Compresses the folder c:\users\rvazarkar and the current directory to compressed.zip

    .EXAMPLE
        
        PS C:\> Invoke-ZipStuff -OutFileDir c:\users\rvazarkar -OutFileName abc.huh
    
        Compresses current folder to c:\users\rvazarkar\abc.huh
    #>

    param(
        [String]
        $Method,

        [String]
        [Parameter(Position=0)]
        $Files = ".",

        [String]
        $OutFileName,

        [String]
        $OutFileDir = $(Get-Location),

        [Int]
        [ValidateRange(0,9)]
        $CompressionLevel,

        [String]
        $ZipPassword
    )

    $vars = New-Object System.Collections.Generic.List[System.Object]

    if ($Method){
        $vars.Add("--Method")
        $vars.Add($Method)
    }
    
    if ($Files){
        $vars.Add("--Files")
        $vars.Add($Files)
    }

    if ($OutFileName){
        $vars.Add("--OutFileName")
        $vars.Add($OutFileName)
    }

    if ($OutFileDir){
        $vars.Add("--OutFileDir")
        $vars.Add($OutFileDir)
    }
    
    if ($CompressionLevel){
        $vars.Add("--CompressionLevel")
        $vars.Add($CompressionLevel)
    }

    if ($ZipPassword){
        $vars.Add("--ZipPassword")
        $vars.Add($ZipPassword)
    }

    $cwd = Get-Location

    #This is to override the directory set by Assembly.Load
    $vars.Add("--CwdOverride")
    $vars.Add($cwd)

    $passed = [string[]]$vars.ToArray()

    #ENCODEDCONTENTHERE
}
