#!/bin/bash
# create backup and override the last backup file
docker exec -it 'MyMS_SQLServer' /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P '123456789!_Asdf' -Q "BACKUP DATABASE [AIChecker] TO DISK = N'/var/opt/mssql/data/AIChecker.bak' WITH INIT, NAME = N'AIChecker-Full Database Backup';"
backupName="$(date +"%Y%m%d_%H%M%S")_backup"
# backup with git
git checkout main
git checkout -b $backupName
# create folder if not exist
mkdir -p ./backup
# copy from container to host
docker cp 'MyMS_SQLServer':/var/opt/mssql/data/AIChecker.bak ./backup/
# backup with git
git add .
git commit -m "database backup $backupName"
git push origin $backupName
git checkout main
cat <<'EOF'
                 __,__
        .--.  .-"     "-.  .--.
      / .. \/  .-. .-.  \/ .. \
      | |  '|  /   Y   \  |'  | |
      | \   \  \ 0 | 0 /  /   / |
      \ '- ,\.-"""""""-./, -' /
        `'-' /_   ^ ^   _\ '-'`
            |  \._   _./  |
            \   \ `~` /   /
            '._ '-=-' _.'
               '~---~'
        |------------------|
        | devcodemonkey.de |
        |------------------|
EOF