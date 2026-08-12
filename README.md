# LegacyRecords.CaseWareFileUsers

A console application for generating a spreadsheet of organization-recognizable staff assigned to batches of CaseWare files. It will accept UNC file paths and database identifiers, retrieve each file’s assigned users, exclude members of configured support Active Directory groups, map remaining users to staff identities, and group the results by file.

The application is designed for large batches and fault isolation: a failure to retrieve one file or resolve one user will be recorded in the output spreadsheet without preventing processing of other files. Future processing will use parallel execution where appropriate while preserving clear per-file and per-user error reporting.

This particular repository is meant for testing out various AI-enhanced development operations and SDLC experimentation, and is not meant to be used in a production environment.

## Contributing

To contribute to this repository, please see the [contribution guidelines](CONTRIBUTING.md).

<!-- reference urls -->
