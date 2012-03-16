
pushd .
cd ..\MBlogModel\Database
rake RAILS_ENV=acceptance
popd

$myPath = Get-Location
Write-Output $myPath
sqlcmd -v Database=mblog_acceptancetest -v Directory="`"$myPath/Images`"" -i insert.sql
