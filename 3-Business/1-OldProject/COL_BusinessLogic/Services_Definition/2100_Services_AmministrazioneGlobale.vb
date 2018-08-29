
Namespace UCServices
    Public Class Services_AmministrazioneGlobale
        Inherits Abstract.MyServices
        Private Const Codice As String = "SRVADMGLB"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Codice
            End Get
        End Property

#Region "Public Property"

#End Region

        Sub New()
            MyBase.New()
        End Sub
		Sub New(ByVal Permessi As String)
			MyBase.New()
			MyBase.PermessiAssociati = Permessi
		End Sub
    End Class
End Namespace