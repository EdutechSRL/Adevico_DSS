<Serializable()> _
Public Enum InvitedUsersImportStep
    None = 0
    SourceCSV = 1
    FieldsMatcher = 2
    ItemsSelctor = 4
    Summary = 8
    Errors = 16
    Completed = 32
    ImportWithErrors = 64
End Enum