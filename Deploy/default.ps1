# for production
# call like .\deploy.ps1 -properties @{Configuration="Release"; TargetDir="c:\inetpub\wwwroot\mblog"}

properties {
    $BaseDir = Resolve-Path "..\"
    $SolutionFile = "$BaseDir\MBlog.sln"
    $ProjectFile = "$BaseDir\MBlog\MBlog.csproj"
    $OutputDir = "$BaseDir\Deploy\Package\"
    $OutputWebDir = "$OutputDir" + "_PublishedWebsites\MBlog"
    $MsDeploy="C:\Program Files\IIS\Microsoft Web Deploy V2\msdeploy.exe"
    $TargetDir = "c:\inetpub\wwwroot\staging\"
    $Configuration="Staging"    
    $DataBaseEnvironment="staging"    
    $Database="MBlogModel\Database"
}

task default -depends Deploy

task Init {
    cls
}

task Clean -depends Init {
    if (Test-Path $OutputDir) {
        ri $OutputDir -Recurse -Force
    }
}

task Build -depends Clean {
    exec { msbuild $SolutionFile "/p:MvcBuildViews=False;OutDir=$OutputDir;UseWPP_CopyWebApplication=True;PipelineDependsOnBuild=False;Configuration=$Configuration" }
}

task DeployDatabase -depends Init {
    pushd
    cd ..\$Database
    rake RAILS_ENV=$DataBaseEnvironment 
    popd
} 

task Deploy -depends Build, DeployDatabase {
    out-host -InputObject $TargetDir
    if(!(Test-Path $MSDeploy)) {
        $MSDeploy = "C:\Program Files (x86)\IIS\Microsoft Web Deploy\msdeploy.exe"
    }
    exec { &$MsDeploy "-verb:sync" "-source:contentPath=$OutputWebDir" "-dest:contentPath=$TargetDir" }
}
