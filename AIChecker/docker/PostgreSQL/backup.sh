#!/bin/bash
gitRemoteUrl=ssh://git@gitlab.hl-dev.de:39566/aichecker
gitRepositoryName="de.devcocdemonkey.aichecker.dbbackup"
backupName="$(date +"%Y%m%d_%H%M%S")_AiCheckerDB_backup"
gitBranchName="backup/$backupName"

cd /tmp
git clone $gitRemoteUrl/$gitRepositoryName.git
cd $gitRepositoryName
git checkout -b $gitBranchName
# create backup and override the last backup file
docker exec -it aichecker-db-1 pg_dump -U AiChecker -h localhost -p 5432 AiCheckerDB > /tmp/$gitRepositoryName/$backupName.sql
git add $backupName.sql
git commit -m "database backup $backupName"
git push origin $gitBranchName
cd ..
rm -rf $gitRepositoryName



