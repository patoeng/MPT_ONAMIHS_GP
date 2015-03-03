@echo off
sqlcmd -S 127.0.0.1\ -U sa -P passwordsa -i backup.sql
del "C:\logger\Backup\db_backup.zip" 
7z a -tzip "C:\logger\Backup\db_backup.zip"  "C:\logger\Backup\db_backup_*.bak"
del  "C:\logger\Backup\db_backup_*".bak
del R:\db_backup_old.zip
ren R:\db_backup.zip  db_backup_old.zip
copy "C:\logger\Backup\db_backup.zip"  R:\

