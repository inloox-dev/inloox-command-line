# InLoox Commandline

A simple commandline interface to interact with the inloox api.

## Examples 

### import csv file to inloox tasks
```
inlooxcmd.exe import-csv -f C:\path\to\csvfile.csv
```

the file looks like:

```
Name;StartDateTime;EndeDateTime
Test1;31.10.2019;01.11.2019
```

The columns name represents the workpackageview columns,
see Model in https://app.inlooxnow.de/api/help/index#!/Task/getWorkPackageView

### query inloox via api
queries the first 
```
inlooxcmd.exe list project
```

output:
```
Project
 -------------------------------------------------------------------------------
 | Name                                           | StartDateTime              |
 -------------------------------------------------------------------------------
 | ProjectA                                       | 07.11.2019 15:00:00 +00:00 |
 -------------------------------------------------------------------------------
 | ProjectB                                       |                            |
 -------------------------------------------------------------------------------
 | ProjectC                                       |                            |
 -------------------------------------------------------------------------------
 | ProjectD                                       | 06.11.2019 08:00:00 +00:00 |
 -------------------------------------------------------------------------------
```

additional entities
```
inlooxcmd.exe list task
```

```
inlooxcmd.exe list timetracking
```


Issues:
- only works for inloox now!
- import-csv only works for tasks