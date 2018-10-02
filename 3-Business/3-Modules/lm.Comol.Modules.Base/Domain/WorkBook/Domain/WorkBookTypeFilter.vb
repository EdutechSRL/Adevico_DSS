Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True), Flags()> Public Enum WorkBookTypeFilter As Integer
        None = 0
        PersonalWorkBook = 1
        AssignedWorkBook = 2
        ManageWorkBook = 4
    End Enum
End Namespace