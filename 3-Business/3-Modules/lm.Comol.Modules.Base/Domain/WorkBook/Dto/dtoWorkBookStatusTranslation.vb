Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class dtoWorkBookStatusTranslation
        Public UniqueID As Integer
        Public LanguageID As Integer
        Public LanguageName As String
        Public Translation As String
        Public StatusId As Integer
    End Class
End Namespace