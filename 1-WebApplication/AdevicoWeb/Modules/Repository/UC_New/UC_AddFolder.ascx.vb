Imports lm.Comol.Core.FileRepository.Domain
Public Class UC_AddFolder
    Inherits FRbaseControl

#Region "Internal"
    Private _itemsCount As Integer
    Public Property MaxItems As Integer
        Get
            Return ViewStateOrDefault("MaxItems", 1)
        End Get
        Set(value As Integer)
            ViewState("MaxItems") = value
        End Set
    End Property

    Public ReadOnly Property ItemsCount As Int32
        Get
            Return RPTitems.Items.Count
        End Get
    End Property
    Private Property IdFolderFather As Long
        Get
            Return ViewStateOrDefault("IdFolderFather", -1)
        End Get
        Set(value As Long)
            ViewState("IdFolderFather") = value
        End Set
    End Property
    Public ReadOnly Property AddFolderDialogTitle As String
        Get
            Return Resource.getValue("AddFolderDialogTitle")
        End Get
    End Property
    Public Event AddFolders(idFather As Long, ByVal items As List(Of dtoFolderName))
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            .setHyperLink(HYPcloseAddFolderDialog, False, True)
            .setButton(BTNaddFolder, True)
            .setLabel(LBaddFolderToCommunity_t)
            .setLiteral(LTaddFolderDescription)
            .setLabel(LBhideFolders)
            .setLabel(LBcurrentPath_t)
            .setLabel(LBselectDestinationFolderPath)

            .setLabel(LBhideFolders_t)
            .setLabel(LBallowUploadIntoFolder_t)
            .setLabel(LBallowUploadIntoFolder)
        End With
    End Sub
#End Region

#Region "Internal"
    Public Sub InitializeControl(idFolder As Long, folderName As String, folderPath As String, folders As List(Of dtoNodeFolderItem))
        InitializeControl(MaxItems, idFolder, folderName, folderPath, folders)
    End Sub
    Public Sub InitializeControl(numberOfFolders As Integer, idFolder As Long, folderName As String, folderPath As String, folders As List(Of dtoNodeFolderItem))
        IdFolderFather = idFolder
        If MaxItems < 1 Then
            MaxItems = 1
        End If
        _itemsCount = MaxItems
        RPTitems.DataSource = (From e As Integer In Enumerable.Range(0, _itemsCount)).Select(Function(i) "").ToList()
        RPTitems.DataBind()
        BTNaddFolder.Visible = numberOfFolders > 0
        InternalInitializeControl(idFolder, folderName, folderPath, folders)
    End Sub

    Private Sub InternalInitializeControl(idFolder As Long, folderName As String, folderPath As String, folders As List(Of dtoNodeFolderItem))
        If String.IsNullOrWhiteSpace(folderPath) Then
            DVcurrentPath.Visible = False
        Else
            DVcurrentPath.Visible = True
            LBcurrentPath.Text = folderPath
            LBcurrentPath.ToolTip = folderName
        End If
        CTRLfolderSelector.InitializeControl(idFolder, folderName, folders)
        DVselectFolder.Visible = Not IsNothing(folders) AndAlso (folders.Where(Function(f) f.Type = NodeType.Item).Count > 1)
    End Sub

    Private Sub RPTitems_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTitems.ItemDataBound
        Dim oLabel As Label = e.Item.FindControl("LBnewFolderName_t")
        Resource.setLabel(oLabel)
    End Sub
    Public Function GetFoldersNames() As List(Of dtoFolderName)
        Dim items As New List(Of dtoFolderName)

        For Each row As RepeaterItem In RPTitems.Items
            Dim oTextBox As TextBox = row.FindControl("TXBnewFolderName")
            If Not String.IsNullOrWhiteSpace(oTextBox.Text) Then
                items.Add(New dtoFolderName() With {.Name = oTextBox.Text, .AllowUpload = CBXallowUpload.Checked, .IsVisible = Not CBXhideFolders.Checked})
            End If
        Next
        Return items
    End Function
    Private Sub ResetInputItems()
        Dim items As New List(Of dtoFolderName)

        For Each row As RepeaterItem In RPTitems.Items
            Dim oTextBox As TextBox = row.FindControl("TXBnewFolderName")
            oTextBox.Text = ""
        Next
        CBXallowUpload.Checked = False
        CBXhideFolders.Checked = False
    End Sub
    Protected Function GetCssClass(index As Integer)
        Dim cssClass As String = ""
        Select Case _itemsCount
            Case 0

            Case 1
                cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString & " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
            Case Else
                If index = 0 Then
                    cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.first.ToString
                ElseIf index = (_itemsCount - 1) Then
                    cssClass = " " & lm.Comol.Core.DomainModel.ItemDisplayOrder.last.ToString
                End If
        End Select
        Return cssClass
    End Function
    Private Sub BTNaddFolder_Click(sender As Object, e As System.EventArgs) Handles BTNaddFolder.Click
        Dim folders As List(Of dtoFolderName) = GetFoldersNames()
        If folders.Any() Then
            RaiseEvent AddFolders(CTRLfolderSelector.IdSelectedFolder, folders)
            ResetInputItems()
        End If
    End Sub
#End Region

End Class