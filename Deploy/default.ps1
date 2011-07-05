properties {
    $TargetDir = "c:\inetpub\staging\"
    $BaseDir = Resolve-Path "..\"
    $SolutionFile = "$BaseDir\MBlog.sln"
    $ProjectFile = "$BaseDir\MBlog\MBlog.csproj"
    $OutputDir = "$BaseDir\Deploy\Package\"
    $OutputWebDir = "$OutputDir" + "_PublishedWebsites\MBlog"
    $WebConfigFile = "$OutputWebDir\Web.config"
    $Debug="false"
    $MsDeploy="C:\Program Files\IIS\Microsoft Web Deploy V2\msdeploy.exe"
}

task default -depends Deploy

task Init {
    cls
}

task Clean {
    if (Test-Path $OutputDir) {
        ri $OutputDir -Recurse
    }
}

task Build {
    exec { msbuild $SolutionFile "/p:MvcBuildViews=False;OutDir=$OutputDir" }
    exec { msbuild $ProjectFile "/t:TransformWebConfig" "/p:MvcBuildViews=False;Configuration=Staging;OutDir=$OutputDir" }
}

task Deploy -depends Init,Clean,Build {
    copy $BaseDir\MBlog\obj\Staging\TransformWebConfig\transformed\Web.config $OutputWebDir
    exec { &$MsDeploy "-verb:sync" "-source:contentPath=$OutputWebDir" "-dest:contentPath=$TargetDir" }
}
