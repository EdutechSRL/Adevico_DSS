Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoProfileFilters
        Public OrganizationID As Integer
        Public ProfileTypeID As Integer
        Public Status As ProfileStatus
        Public AuthenticationTypeID As Integer
        Public SearchFor As ProfileSearchBy
        Public Value As String
        Public StartWith As String
        Public PageSize As Integer
        Public PageIndex As Integer
        Public OrderBy As ProfileOrder
        Public Ascending As Boolean
    End Class
End Namespace