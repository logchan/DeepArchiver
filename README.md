# Deep Archiver

Backup files to AWS S3 tier Deep Archive.

## What is this

A tool for using S3 deep archive to mirroring bulk data that rarely changes (movies, music, photos, etc.).

Compared to local solutions (RAID1, RAID5, ZFS mirror, etc.), S3 is:

+ Cheap ($1/mo per TB)
+ Convenient (no more installing, monitoring, and moving stacks of HDDs)

The downside is data retrival takes 48 hours and about $30/TB. But, that's when all your local backups has failed.

## Features

+ Select local directories for backup
+ Incrementally upload new or modified files
+ Limit single upload size, so your ISP don't see 1TB upload in a day
+ Remote files list stored in local database, make sure you backup them

## Early in development

The features should work but the code is messy.

## License

[DBAD license](https://dbad-license.org/)