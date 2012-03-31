param (
    $Properties = @{}
)
if(Get-Module -name $name){ remove-module psake}
import-module .\psake.psm1
$psake.use_exit_on_error = $true
invoke-psake -framework 4.0 -taskList DeployIntegrationTest -properties $Properties
remove-module psake