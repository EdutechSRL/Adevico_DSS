Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_MoveItemsDialog
    Inherits FRbaseControl

    Private Const KeyString As String = "UC_MoveItemsDialog"

#Region "Implements"
#Region "Settings"

#End Region
#End Region

#Region "Internal"
    Public Property isInitialized As Boolean
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
  
    Private Property UniqueIdentifier As Guid
        Get
            Return ViewStateOrDefault("UniqueIdentifier", Guid.Empty)
        End Get
        Set(value As Guid)
            ViewState("UniqueIdentifier") = value
        End Set
    End Property
    Public Event CloseWindow()

    Public Event MoveToFolder(idItem As Long, idFolder As Long)
    Public Event MoveToFolders(idItems As List(Of Long), idFolder As Long)
    Public ReadOnly Property DialogIdentifier As String
        Get
            Return LTcssClassDialog.Text
        End Get
    End Property
    Private Property CurrentItems As List(Of Long)
        Get
            If Not IsNothing(Session(KeyString & UniqueIdentifier.ToString)) AndAlso TypeOf (Session(KeyString & UniqueIdentifier.ToString)) Is List(Of Long) Then
                Return DirectCast(Session(KeyString & UniqueIdentifier.ToString), List(Of Long))
            Else
                Return New List(Of Long)
            End If
        End Get
        Set(value As List(Of Long))
            Session(KeyString & UniqueIdentifier.ToString) = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBcurrentPath_t)
            .setLabel(LBselectDestinationFolderPath)
            .setButton(BTNapplyMoveTo, True)
            .setHyperLink(HYPcloseMoveConfirmDialog, False, True)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(idItemToMove As Long, idFolder As Long, folderName As String, folderPath As String, folders As List(Of dtoNodeFolderItem), fileRender As String)
        UniqueIdentifier = Guid.NewGuid
        isInitialized = True
        CurrentItems = New List(Of Long) From {idItemToMove}
        LBmoveDialogDescription.Text = String.Format(Resource.getValue("LBmoveDialogDescription.Item"), fileRender)
        InitializeControl(idFolder, folderName, folderPath, folders)
    End Sub
    Private Sub InitializeControl(idFolder As Long, folderName As String, folderPath As String, folders As List(Of dtoNodeFolderItem))
        If String.IsNullOrWhiteSpace(folderPath) Then
            DVcurrentPath.Visible = False
        Else
            DVcurrentPath.Visible = True
            LBcurrentPath.Text = folderPath
            LBcurrentPath.ToolTip = folderName
        End If
        CTRLfolderSelector.InitializeControl(idFolder, folderName, folders)
    End Sub

#End Region

#Region "Internal"
    Protected Function GetDialogTitle() As String
        Return Resource.getValue("UC_MoveItemsDialog.GetDialogTitle")
    End Function

    Private Sub BTNapplyMoveTo_Click(sender As Object, e As System.EventArgs) Handles BTNapplyMoveTo.Click
        If isInitialized Then
            Dim items As List(Of Long) = CurrentItems
            Select Case items.Count
                Case 0
                Case 1
                    RaiseEvent MoveToFolder(items.FirstOrDefault(), CTRLfolderSelector.IdSelectedFolder)
                Case Else
                    RaiseEvent MoveToFolders(items, CTRLfolderSelector.IdSelectedFolder)
            End Select
        End If
      
        ClearSession()
        isInitialized = False
        UniqueIdentifier = Guid.Empty
    End Sub
    Public Sub ClearSession()
        Session.Remove(KeyString & UniqueIdentifier.ToString)
    End Sub
#End Region


End Class