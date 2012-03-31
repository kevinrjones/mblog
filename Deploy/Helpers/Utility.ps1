param([string]$ScriptPath)
#Inject the script path


"Params" | Out-Host
$ScriptPath | Out-Host

function PressAKey() {
	"Press a Key" | out-host
	$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}

function GetProcessForFile($FilePath)
{
	if ($ScriptPath -eq $null)
	{
		return -1
	}
	
	#Copy the executable locally as we can't execute from a remote drive without warnings going off
	$ExePath = $ScriptPath + "bin\handle.exe"
	$LocalExePath = $env:TEMP + "\handle.exe"
	if ((Test-Path $LocalExePath) -ne $true)
	{
		copy $ExePath $env:TEMP
	}
	
	$ScreenOutput = & $LocalExePath "-accepteula" $FilePath
	if ($ScreenOutput -ne $null)
	{
		$ProcessIDResults = $ScreenOutput | SELECT-STRING -pattern 'pid: [\w]*'
		if ($ProcessIDResults -eq $null)
		{
			return 0
		}
		if ($ProcessIDResults.Length -gt 1)
		{
			return $ProcessIDResults[0].matches[0].ToString().SubString(5)		
		}
		return $ProcessIDResults.matches[0].ToString().SubString(5)
	}
	return 0
}

#Decrypts a secure-string back to plain text [http://www.vistax64.com/powershell/159190-read-host-assecurestring-problem.html]
function ConvertTo-PlainText( [security.securestring]$secure )
{
   $marshal = [Runtime.InteropServices.Marshal]
   $marshal::PtrToStringAuto( $marshal::SecureStringToBSTR($secure) )
}


#Used as a debugging utility. Just keeps listing the PID of a particular file handle
function GetProcessForFileRepeat($FilePath)
{
	do
	{
		GetProcessForFile($FilePath)
		Start-Sleep -Milliseconds 100
	}
	while ($true)
}

#function SetConsoleBufferWidth()      WTF!!!!!
#{
#	$Acl = Get-Acl $folder
#	$colRights = [System.Security.AccessControl.FileSystemRights]"Read, Write"
#	$rule = New-Object System.Security.AccessControl.FileSystemAccessRule($username, $colRights, "ContainerInherit, ObjectInherit", "None", "Allow")
#	$acl.AddAccessRule($rule)
#	
#	Set-Acl $folder $Acl
#}

function SetConsoleBufferWidth()
{
	$pshost = get-host
	$pswindow = $pshost.ui.rawui

	$newsize = $pswindow.buffersize
	$newsize.height = 3000
	$newsize.width = 1000
	$pswindow.buffersize = $newsize
}


function CreateDatedFolderName($baseFolder, $date, $prefix)
{
	$suffix = 1
	$dirPrefix = ""
	if (($prefix -ne "") -and ($prefix -ne $null))
	{
		$dirPrefix = $prefix + "_"
	}
	do
	{
		"Prefix: $dirPrefix" | Out-Host
		"Suffix: $suffix" | Out-Host
		$subFolder = $dirPrefix + $date.ToString("yyyyMMdd") + "_" + $suffix
		"Subfolder: $subFolder" | Out-Host
		$archiveTo = [IO.Path]::Combine($baseFolder, $subFolder)
		"Full Archive path: $archiveTo" | Out-Host
		$suffix += 1
	}
	while ((Test-Path $archiveTo) -eq $true)
	return $archiveTo
}
