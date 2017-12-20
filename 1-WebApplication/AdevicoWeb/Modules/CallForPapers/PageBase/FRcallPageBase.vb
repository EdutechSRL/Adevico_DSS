Public MustInherit Class FRcallPageBase
    Inherits PageBase

#Region "Implements"
    Protected Property PageIdentifier As Guid
        Get
            Return ViewStateOrDefault("PageIdentifier", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("PageIdentifier") = value
        End Set
    End Property
    Protected Property OperationTicket As Long
        Get
            Return ViewStateOrDefault("OperationTicket", 0)
        End Get
        Set(value As Long)
            ViewState("OperationTicket") = value
        End Set
    End Property
    Protected ReadOnly Property CurrentOperations As Dictionary(Of Guid, Long)
        Get
            Dim operations As New Dictionary(Of Guid, Long)
            If IsNothing(Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations)) OrElse Not TypeOf (Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations)) Is Dictionary(Of Guid, Long) Then
                Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations) = operations
            Else
                operations = Session(lm.Comol.Core.FileRepository.Domain.SessionConstants.PageOperations)
            End If
            Return operations
        End Get
    End Property
    Protected Sub AddCurrentTicket(idPage As Guid, idOperationTicket As Long)
        If CurrentOperations.ContainsKey(idPage) Then
            CurrentOperations.Item(idPage) = idOperationTicket
        Else
            CurrentOperations.Add(idPage, idOperationTicket)
        End If
    End Sub
    Protected Function GetCurrentOperationTicket(idPage As Guid) As Long
        Dim operations As Dictionary(Of Guid, Long) = CurrentOperations
        If operations.ContainsKey(idPage) Then
            Return operations(idPage)
        Else
            Return 0
        End If
    End Function
    Protected Function isValidOperation() As Boolean
        Dim isValid As Boolean = False
        Dim lastTicket As Long = GetCurrentOperationTicket(PageIdentifier)
        Dim thisTicket As Long = OperationTicket
        If thisTicket > lastTicket OrElse (lastTicket = thisTicket AndAlso thisTicket = 0) Then
            AddCurrentTicket(PageIdentifier, thisTicket)
            Return True
        Else
            Return False
        End If
    End Function
    Protected Sub TrackRefreshState()
        Dim lastTicket As Long = GetCurrentOperationTicket(PageIdentifier) + 1
        AddCurrentTicket(PageIdentifier, lastTicket)
    End Sub
#End Region

    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        SaveRefreshState()
    End Sub
    Private Sub SaveRefreshState()
        Dim ticket As Long = GetCurrentOperationTicket(PageIdentifier) + 1
        OperationTicket = ticket
    End Sub
End Class