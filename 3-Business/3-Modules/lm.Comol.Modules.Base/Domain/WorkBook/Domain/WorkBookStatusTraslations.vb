Imports lm.Comol.Core.DomainModel

Namespace lm.Comol.Modules.Base.DomainModel
    <Serializable(), CLSCompliant(True)> Public Class WorkBookStatusTraslations
        Inherits lm.Comol.Core.DomainModel.DomainObject(Of Integer)

#Region "Private"
        Private _Status As WorkBookStatus
        Private _Language As Language
        Private _Translation As String
#End Region

        Public Overridable Property Translation() As String
            Get
                Return _Translation
            End Get
            Set(ByVal value As String)
                _Translation = value
            End Set
        End Property
        Public Overridable Property Status() As WorkBookStatus
            Get
                Return _Status
            End Get
            Set(ByVal value As WorkBookStatus)
                _Status = value
            End Set
        End Property
        Public Overridable Property SelectedLanguage() As Language
            Get
                Return _Language
            End Get
            Set(ByVal value As Language)
                _Language = value
            End Set
        End Property
        Sub New()
        End Sub
    End Class
End Namespace