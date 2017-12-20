Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.Repository.Presentation


Public Class UC_GenericCSVuploader
    Inherits BaseControl
    Implements IViewGenericCSVuploader

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As GenericCSVuploaderPresenter
    Private ReadOnly Property CurrentPresenter() As GenericCSVuploaderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GenericCSVuploaderPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private WriteOnly Property AllowPreview As Boolean Implements IViewGenericCSVuploader.AllowPreview
        Set(value As Boolean)
            Me.SPNpreview.Visible = value
        End Set
    End Property
    Private Property CurrentFile As lm.Comol.Core.BaseModules.Repository.dtoCSVfile Implements IViewGenericCSVuploader.CurrentFile
        Get
            Return ViewStateOrDefault("CurrentFile", New lm.Comol.Core.BaseModules.Repository.dtoCSVfile)
        End Get
        Set(value As lm.Comol.Core.BaseModules.Repository.dtoCSVfile)
            ViewState("CurrentFile") = value
        End Set
    End Property
    Private Property CurrentSettings As dtoCsvSettings Implements IViewGenericCSVuploader.CurrentSettings
        Get
            Dim dto As New dtoCsvSettings
            dto.RowsToSkip = Me.DDLrowsToSkip.SelectedValue
            dto.FirstRowColumnNames = Me.CBXfirstRowColumnNames.Checked
            dto.ColumnDelimeter = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TextDelimiter).GetByString(Me.DDLcolumnDelimiter.SelectedValue, TextDelimiter.Comma)
            dto.RowDelimeter = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of TextDelimiter).GetByString(Me.DDLrowDelimiter.SelectedValue, TextDelimiter.CrLf)

            Return dto
        End Get
        Set(value As dtoCsvSettings)
            Me.DDLrowsToSkip.SelectedValue = value.RowsToSkip
            Me.CBXfirstRowColumnNames.Checked = value.FirstRowColumnNames
            Me.DDLrowDelimiter.SelectedValue = value.RowDelimeter.ToString
            Me.DDLcolumnDelimiter.SelectedValue = value.ColumnDelimeter.ToString
        End Set
    End Property
    Public Property isInitialized As Boolean Implements IViewGenericCSVuploader.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public Property ClearPreviousFiles As Boolean Implements IViewGenericCSVuploader.ClearPreviousFiles
        Get
            Return ViewStateOrDefault("ClearPreviousFiles", False)
        End Get
        Set(value As Boolean)
            ViewState("ClearPreviousFiles") = value
        End Set
    End Property
    Public Property AvailableColumns As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewGenericCSVuploader.AvailableColumns
        Get
            Return ViewStateOrDefault("AvailableColumns", New List(Of ExternalColumnComparer(Of String, Integer)))
        End Get
        Set(value As List(Of ExternalColumnComparer(Of String, Integer)))
            ViewState("AvailableColumns") = value
        End Set
    End Property
    Public Property isValid As Boolean Implements IViewGenericCSVuploader.isValid
        Get
            Return ViewStateOrDefault("isValid", False)
        End Get
        Set(value As Boolean)
            ViewState("isValid") = value
        End Set
    End Property
    Public Property CSVfolder As String Implements IViewGenericCSVuploader.DefaultCSVfolder
        Get
            Return ViewStateOrDefault("CSVfolder", "\CSVfolder\")
        End Get
        Set(value As String)
            ViewState("CSVfolder") = value
        End Set
    End Property
    Public Property DefaultDelimiter As TextDelimiter Implements IViewGenericCSVuploader.DefaultDelimiter
        Get
            Return ViewStateOrDefault("DefaultDelimiter", TextDelimiter.Semicolon)
        End Get
        Set(value As TextDelimiter)
            ViewState("DefaultDelimiter") = value
        End Set
    End Property
    Private ReadOnly Property CurrentCSVPath As String
        Get
            Dim CommunityPath As String = ""
            If Me.SystemSettings.File.Materiale.DrivePath = "" Then
                CommunityPath = Server.MapPath(BaseUrl & Me.SystemSettings.File.Materiale.VirtualPath) & CSVfolder
            Else
                CommunityPath = Me.SystemSettings.File.Materiale.DrivePath & CSVfolder
            End If
            Return Replace(CommunityPath, "\\", "\")
        End Get
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
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CSVgenericImport", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBemptyMessage)
            .setLabel(LBsourceFile_t)
            .setButton(BTNupload, True)

            .setLabel(LBuploadedFile_t)
            .setLabel(LBuploadedFile)
            .setButton(BTNremoveFile, True)

            .setLabel(LBfirstRowColumnNames_t)
            .setCheckBox(CBXfirstRowColumnNames)
            .setLabel(LBcolumnDelimiter_t)
            .setLabel(LBrowDelimiter_t)
            .setLabel(LBrowToSkip_t)

            For Each item As ListItem In Me.DDLcolumnDelimiter.Items
                item.Text = Me.Resource.getValue("TextDelimiter." & item.Value)
            Next
            For Each item As ListItem In Me.DDLrowDelimiter.Items
                item.Text = Me.Resource.getValue("TextDelimiter." & item.Value)
            Next
            .setButton(BTNpreviewCSV, True)

            For i As Integer = 0 To 5
                Me.DDLrowsToSkip.Items.Add(i.ToString)
            Next
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl() Implements IViewGenericCSVuploader.InitializeControl
        isValid = False
        isInitialized = False
        Me.CurrentPresenter.InitView(CurrentCSVPath)
    End Sub
    Public Sub InitializeControl(settings As dtoCsvSettings, file As lm.Comol.Core.BaseModules.Repository.dtoCSVfile) Implements IViewGenericCSVuploader.InitializeControl
        Me.CurrentSettings = settings
        Me.isInitialized = True
        Dim uploaded As Boolean = Not IsNothing(file)

        Me.SPNpreview.Visible = uploaded
        Me.SPNselectFile.Visible = Not uploaded
        Me.SPNdisplayFile.Visible = uploaded

        If uploaded Then
            Me.Resource.setLabel(Me.LBuploadedFile)
            Me.LBuploadedFile.Text = String.Format(Me.LBuploadedFile.Text, file.Name, FormatDateTime(file.UploadedOn, DateFormat.ShortDate), FormatDateTime(file.UploadedOn, DateFormat.ShortTime))
            Me.RPTpreview.Visible = False
            SPNpreviewTable.Visible = False
            CurrentFile = file
        End If
        Me.MLVcontrolData.SetActiveView(VIWselectCSV)
    End Sub
    'Public Sub InitalizeControl(settings As dtoCSVsettings, file As dtoCSVfile) Implements IViewGenericCSVuploader.InitalizeControl

    'End Sub
    Public Sub DisplayUploadError() Implements IViewGenericCSVuploader.DisplayUploadError

    End Sub

    Public Sub LoadUploadedFile(file As lm.Comol.Core.BaseModules.Repository.dtoCSVfile) Implements IViewGenericCSVuploader.LoadUploadedFile
        Dim uploaded As Boolean = Not IsNothing(file)

        Me.SPNpreview.Visible = uploaded
        Me.SPNselectFile.Visible = Not uploaded
        Me.SPNdisplayFile.Visible = uploaded

        If uploaded Then
            Me.Resource.setLabel(Me.LBuploadedFile)
            Me.LBuploadedFile.Text = String.Format(Me.LBuploadedFile.Text, file.Name, FormatDateTime(file.UploadedOn, DateFormat.ShortDate), FormatDateTime(file.UploadedOn, DateFormat.ShortTime))
            Me.RPTpreview.Visible = False
            SPNpreviewTable.Visible = False
        End If
        CurrentFile = file
    End Sub
    Public Sub PreviewRows(file As CsvFile) Implements IViewGenericCSVuploader.PreviewRows
        If Not IsNothing(file) Then
            Dim list As New List(Of CsvFile)
            list.Add(file)
            Me.RPTpreview.DataSource = list
            Me.RPTpreview.DataBind()
            Me.RPTpreview.Visible = True
            SPNpreviewTable.Visible = True
        Else
            Me.RPTpreview.Visible = False
            SPNpreviewTable.Visible = False
        End If
    End Sub

    Public Sub DisplayFileToUpload() Implements IViewGenericCSVuploader.DisplayFileToUpload
        Me.SPNpreview.Visible = False
        Me.SPNselectFile.Visible = True
        Me.SPNdisplayFile.Visible = False
    End Sub
    Public Sub DisplaySessionTimeout() Implements IViewGenericCSVuploader.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
    Public Function RetrieveFile() As CsvFile Implements IViewGenericCSVuploader.RetrieveFile

    End Function

    Public Function GetAvailableColumns() As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewGenericCSVuploader.GetAvailableColumns
        Return Me.CurrentPresenter.GetAvailableColumns(Me.CurrentSettings, CurrentFile)
    End Function

    Public Function GetFileContent(columns As List(Of ExternalColumnComparer(Of String, Integer))) As ExternalResource Implements IViewGenericCSVuploader.GetFileContent
        Return Me.CurrentPresenter.GetFileContent(columns, Me.CurrentSettings, CurrentFile)
    End Function
    'Public Function GetFileContent(columns As List(Of ColumnComparer(Of String, OffertaTesi))) As ProfileExternalResource Implements IViewGenericCSVuploader.GetFileContent
    '   
    'End Function
    'Public Function GetAvailableColumns() As List(Of ProfileColumnComparer(Of String)) Implements IViewGenericCSVuploader.GetAvailableColumns
    '  
    'End Function
#End Region

    Private Sub BTNpreviewCSV_Click(sender As Object, e As System.EventArgs) Handles BTNpreviewCSV.Click
        Me.CurrentPresenter.Preview(Me.CurrentSettings, CurrentFile)
    End Sub

    Private Sub BTNremoveFile_Click(sender As Object, e As System.EventArgs) Handles BTNremoveFile.Click
        Me.CurrentPresenter.RemoveFile(CurrentFile)
    End Sub

    Private Sub BTNupload_Click(sender As Object, e As System.EventArgs) Handles BTNupload.Click
        If Not IsNothing(Me.INFcsv.PostedFile) Then
            Me.CurrentPresenter.UploadCSV(Me.INFcsv.PostedFile, Me.CurrentCSVPath)
        End If
    End Sub

    Protected Sub RPTcsvHeader_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim column As TextColumn = DirectCast(e.Item.DataItem, TextColumn)
            Dim oLiteral As Literal = e.Item.FindControl("LTcolumn")

            If column.Empty Then
                oLiteral.Text = String.Format(Me.Resource.getValue("Column"), column.Number.ToString)
            Else
                oLiteral.Text = column.Value
            End If
        End If
    End Sub
    Protected Sub RPTitems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As String = DirectCast(e.Item.DataItem, String)
            Dim oLiteral As Literal = e.Item.FindControl("LTitem")

            oLiteral.Text = IIf(String.IsNullOrEmpty(item), " ", item)
        End If
    End Sub
End Class