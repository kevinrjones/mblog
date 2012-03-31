# for production
# call like .\deploy.ps1 -properties @{Configuration="test"; DatabaseConfiguration="test"}

properties {
    $BaseDir = Resolve-Path "..\"
    $SolutionFile = "$BaseDir\MBlog.sln"
    $ProjectFile = "$BaseDir\MBlog\MBlog.csproj"
    $OutputDir = "$BaseDir\Deploy\Package\"
    $OutputWebDir = "$OutputDir" + "_PublishedWebsites\MBlog"
    $MsDeploy="C:\Program Files\IIS\Microsoft Web Deploy V2\msdeploy.exe"
    $INetPubRoot = "c:\inetpub\wwwroot"
    $Configuration="Staging"    
    $DatabaseConfiguration="staging"    
    $Server = "."
    $DatabaseUser = "IIS APPPool\ASP.Net v4.0"
    $DatabaseRole = "db_owner"
}

$DatabaseDir="MBlogModel\Database"
$ProjectName = "MBlog"

$db = $ProjectName + "_" + $DatabaseConfiguration

. .\Sql\CreateDatabase.ps1
. .\Sql\DeleteDatabase.ps1
. .\Sql\AddUser.ps1

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

task CreateDatabase {
     $db = $ProjectName + "_" + $DatabaseConfiguration
     Create-Database $Server $db
}

task DeleteDatabase {
     $db = $ProjectName + "_" + $DatabaseConfiguration
     Delete-Database $Server $db
}

task RecreateDatabase -depends DeleteDatabase, AddDatabaseUser {
}

task AddDataBaseUser -depends CreateDatabase {
     $db = $ProjectName + "_" + $DatabaseConfiguration
     exec {Add-User $Server $db $DatabaseUser $DatabaseRole }
}

task DeployDatabase -depends Init {
    pushd
    cd ..\$DatabaseDir
    exec {rake RAILS_ENV=$DatabaseConfiguration }
    popd
} 

task Deploy -depends Build, CreateDatabase, AddDatabaseUser, DeployDatabase {
    $TargetDir = $INetPubRoot + "\" +  $Configuration
    out-host -InputObject $TargetDir
    exec { &$MsDeploy "-verb:sync" "-source:contentPath=$OutputWebDir" "-dest:contentPath=$TargetDir" }
}

task DeployIntegrationTest -depends RecreateDatabase, Deploy {
    $TargetDir = $INetPubRoot + "\" +  $Configuration
    out-host -InputObject $TargetDir
    exec { &$MsDeploy "-verb:sync" "-source:contentPath=$OutputWebDir" "-dest:contentPath=$TargetDir" }
}


