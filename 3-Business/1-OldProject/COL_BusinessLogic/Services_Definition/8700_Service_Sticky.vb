Namespace UCServices
    ' SOLO TEMPORANEO POI ANDRA' A MORIRE QUESTO SERVICE !
    <Serializable(), CLSCompliant(True)> Public Class Service_Sticky
        Inherits Abstract.MyServices

        Const Code = "SRVStickyNote"

        Public Overloads Shared ReadOnly Property Codex() As String
            Get
                Codex = Code
            End Get
        End Property

        Sub New()
            MyBase.New()

        End Sub
        Sub New(ByVal Permessi As String)
            MyBase.New()
            MyBase.PermessiAssociati = Permessi
        End Sub
        Public Shared Function Create() As Service_Sticky
            Return New Service_Sticky("00000000000000000000000000000000")
        End Function

        
        

    End Class

End Namespace