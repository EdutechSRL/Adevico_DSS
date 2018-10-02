Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True), Flags()> Public Enum EditingPermission As Integer
        None = 0
        Responsible = 1
        Owner = 2
        Authors = 4
        ModuleManager = 8
    End Enum
End Namespace