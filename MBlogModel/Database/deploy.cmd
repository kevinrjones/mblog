
:DEVELOPMENT
SET DATABASE=mblog_development
sqlcmd -i db\scripts\insert.sql

:PRESTAGING
SET DATABASE=mblog_staging
rem sqlcmd -i db\scripts\insert.sql
rem GOTO :SQLCMD

:PRODUCTION
SET DATABASE=mblog
rem sqlcmd -i db\scripts\insert.sql
rem GOTO :SQLCMD

:SQLCMD
rem sqlcmd -i db\scripts\insert.sql
