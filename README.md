# SmartFileCopy
Copies new files from source to destination on a configured interval.  After the file copy you can also configure an HTTP POST.

### Requirements
- .Net 6.0

### Configuration

- .config
```

source=[Source directory]
destination=[Destination directory like an SMB share]
interval=[interval in seconds to run]
afterCopyPostUrl=[HTTP address to POST after finished] --optional
afterCopyPostBody=[HTTP POST Body] --optional or required if afterCopyPostUrl has a value
```

- .lastrun

This file will be updated after each run so that we don't copy the same file twice.  If the Last Modified Date of the file is greater than the last run time, then the file will be copied.

