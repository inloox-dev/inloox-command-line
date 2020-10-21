# InLoox CommandLine

A simple commandline interface to interact with the inloox api.

## Examples 

### query inloox projects
query all projects 
```
inlooxcmd.exe list --columns:Name,Note --entity:project
```

output:
```
Project
 -------------------------------------------------------------------------------
 | Name                                           | Note                       |
 -------------------------------------------------------------------------------
 | ProjectA                                       | a first note               |
 -------------------------------------------------------------------------------
 | ProjectB                                       |                            |
 -------------------------------------------------------------------------------
 | ProjectC                                       |                            |
 -------------------------------------------------------------------------------
 | ProjectD                                       | second note                |
 -------------------------------------------------------------------------------
```

### import csv file to inloox tasks
```
inlooxcmd.exe import-csv -f C:\path\to\csvfile.csv
```

example csv file

```
Name;StartDateTime;EndeDateTime
Test1;31.10.2019;01.11.2019
```

The columns name represents the workpackageview columns,
see Model in https://app.inlooxnow.de/api/help/index#!/Task/getWorkPackageView

additional entities
```
inlooxcmd.exe list task
```

```
inlooxcmd.exe list timetracking
```


Issues:
- import-csv works for project and tasks
