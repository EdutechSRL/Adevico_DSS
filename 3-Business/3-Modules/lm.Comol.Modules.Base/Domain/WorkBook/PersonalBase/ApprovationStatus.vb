Imports System.ComponentModel


Namespace lm.Comol.Modules.Base.DomainModel

    <Serializable(), CLSCompliant(True), DefaultValue(ApprovationStatus.Ignore)> Public Enum ApprovationStatus
        NotDefined = 0
        Approved = 1
        NotApproved = 2
        Waiting = 3
        Ignore = 4
        Under = 5
    End Enum
End Namespace