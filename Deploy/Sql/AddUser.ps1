function Add-User($dbInstance, $dbName, $userName, $role){

    "Name: " + $dbName | out-host
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

    $login = new-object ('Microsoft.SqlServer.Management.Smo.Login') $dbServer, $userName
    $login.LoginType = [Microsoft.SqlServer.Management.SMO.LoginType]::WindowsUser;
    try
    {
        $login.Create()
    }
    catch{}

    try{
        $user = new-object ('Microsoft.SqlServer.Management.Smo.User') $db, $userName
	$user.Login = $userName
        $user.Create() 
    }catch{}

    try{
        if ($user.IsMember($role) -ne $True) {		
            # Not a member, so add that role
	    $cn = new-object system.data.SqlClient.SqlConnection("Data Source=$inst;Integrated Security=true;Initial Catalog=$dbname");
	    $cn.Open()
	    $q = "EXEC sp_addrolemember @rolename = N'$role', @membername = N'$userName'"
	    $cmd = new-object "System.Data.SqlClient.SqlCommand" ($q, $cn)
	    $cmd.ExecuteNonQuery() | out-null
 	    $cn.Close()	
        }
    }catch{}

}

