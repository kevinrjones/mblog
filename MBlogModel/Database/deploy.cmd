
:PRESTAGING
SET DATABASE=mblog_staging
sqlcmd -i db\scripts\insert.sql
rem GOTO :SQLCMD

:PRODUCTION
SET DATABASE=mblog
sqlcmd -i db\scripts\insert.sql
rem GOTO :SQLCMD

:SQLCMD
rem sqlcmd -i db\scripts\insert.sql
