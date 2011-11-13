function Create-Database($dbInstance, $dbName){
    [System.Reflection.Assembly]::LoadWithPartialName("Microsoft.SqlServer.Smo")
    $dbServer = new-object Microsoft.SqlServer.Management.Smo.Server ($dbInstance)
    $db = new-object Microsoft.SqlServer.Management.Smo.Database
    # Loop thru the db list to find the one we need. If found, set the local
    # vars to avoid errors when trying to delete the db from within the loop.
    $found = "false"
    foreach ($_ in $dbServer.Databases)
    {
        if ($_.Name -eq $dbName)
        {
            $db = $_
            $found = "true"
        }
    }
    # Now that we're out of the loop we can kill the db
    if ($found -eq "false")
    {
        "Creating database $dbName..."
        $db = new-object Microsoft.SqlServer.Management.Smo.Database ($dbServer, $dbName)
        $db.Create()
    }
    else
    {
        "Database $dbName already exists"
    }
}