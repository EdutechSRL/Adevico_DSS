
Namespace UCServices
    Namespace Abstract
        <Serializable(), CLSCompliant(True)> Public MustInherit Class MyServices
            Private _PermissionString As String = ""
            Private n_StringaPermessi(31) As Byte
            Private Const Codice As String = "SRVABSTRACT"

            Public Property PermessiAssociati() As String
                Get
                    Dim i As Integer
                    Dim oListaPermessi(n_StringaPermessi.Length - 1) As Char

                    For i = 0 To n_StringaPermessi.Length - 1
                        oListaPermessi(i) = n_StringaPermessi(i).ToString
                    Next
                    PermessiAssociati = oListaPermessi
                End Get
                Set(ByVal Value As String)
                    Dim i As Integer
                    Dim oListaPermessi() As Char
                    oListaPermessi = Value.ToCharArray()

                    For i = 0 To oListaPermessi.Length - 1
                        n_StringaPermessi(i) = CByte(oListaPermessi(i).ToString)
                    Next
                    _PermissionString = Value
                End Set
            End Property
            Public Enum PermissionType
                None = -1
                Read = 0 '1
                Write = 1 '2
                Change = 2 '4
                Delete = 3 '8
                Moderate = 4 '16
                Grant = 5 '32
                Admin = 6 '64
                Send = 7 ' 128
                Receive = 8 '256
                Synchronize = 9 '512
                Browse = 10 '1024
                Print = 11 '2048
                ChangeOwner = 12 '4096
                Add = 13 '8192
                ChangeStatus = 14 '16384
                DownLoad = 15 '32768
            End Enum
            Sub New()
                Dim i As Integer
                For i = 0 To n_StringaPermessi.Length - 1
                    n_StringaPermessi(i) = 0
                Next
            End Sub
            Protected Overridable Function GetPermissionByPosition(ByVal oPosizione As Integer) As Boolean
                If oPosizione > (n_StringaPermessi.Length - 1) Then
                    Return False
                Else
                    Return CBool(n_StringaPermessi(oPosizione))
                End If
                Return False
            End Function
            Protected Overridable Function GetPermissionValue(ByVal oType As PermissionType) As Boolean
                Dim iPosizione As Integer
                iPosizione = CType(oType, PermissionType)
                Return GetPermissionByPosition(iPosizione)
            End Function
            Protected Overridable Function SetPermissionByPosition(ByVal oPosizione As Integer, ByVal oValue As Byte) As Boolean
                If oPosizione > (n_StringaPermessi.Length - 1) Then
                    Return False
                Else
                    n_StringaPermessi(oPosizione) = oValue
                End If
                Return False
            End Function

            Public Function ConvertToInt() As Int32
                Return Convert.ToInt32(PermessiAssociati.Reverse.ToArray, 2)
            End Function
            Public Function ConvertToLong() As Long
                If PermessiAssociati <> "" Then
                    Return Convert.ToInt64(New String(PermessiAssociati.Reverse.ToArray), 2)
                Else
                    Return 0
                End If
            End Function
        End Class
    End Namespace
End Namespace