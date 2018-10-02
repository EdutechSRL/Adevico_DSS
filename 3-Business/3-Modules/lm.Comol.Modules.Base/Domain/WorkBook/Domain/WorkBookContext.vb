Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Common
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.Modules.Base.BusinessLogic

Namespace lm.Comol.Modules.Base.DomainModel
    <CLSCompliant(True), Serializable()> Public Class WorkBookContext
        Public PageIndex As Integer
        Public View As WorkBookTypeFilter
        Public PageSize As Integer
        Public CommunityFilter As WorkBookCommunityFilter
        Public Order As WorkBookOrder
    End Class
End Namespace