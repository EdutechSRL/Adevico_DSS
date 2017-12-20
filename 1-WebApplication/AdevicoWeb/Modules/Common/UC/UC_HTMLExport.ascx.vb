Imports Telerik.Web.UI
Imports Telerik.QuickStart
Imports Telerik.Web.UI.Editor.Import
Imports System.IO
Imports System.Drawing.Text
Imports System.Drawing

Public Class UC_HTMLExport
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'BindConfiguratorValues()
            'SetConfiguratorDefaultValues()
            'ResetErrorLogs()

            'If docxUpload.UploadedFiles.Count = 0 Then
            '    LoadInitialSampleContent()
            '    ' Disable import button if there are no uploaded files
            '    ImportDocx.Enabled = False
            'End If
        End If
    End Sub



    Public Property HTML As String
        Get
            Return Me.UCRADeditor.Content
        End Get
        Set(value As String)
            Me.UCRADeditor.Content = value
        End Set
    End Property

    Public ReadOnly Property GetText As String
        Get
            Return Me.UCRADeditor.Text
        End Get
    End Property

    Private Sub LKBPDF_Click(sender As Object, e As EventArgs) Handles LKBPDF.Click

        Try
            Me.UCRADeditor.ExportToPdf()
        Catch ex As Exception
            LBerror.Text = "Exception: " + ex.Message
        End Try

    End Sub

    Private Sub LKBDOCX_Click(sender As Object, e As EventArgs) Handles LKBDOCX.Click
        'ConfigureExportSettingsDoc()

        Try
            UCRADeditor.ExportToDocx()
        Catch ex As Exception
            LBerror.Text = "Exception: " + ex.Message
        End Try
    End Sub

    Private Sub LKBRTF_Click(sender As Object, e As EventArgs) Handles LKBRTF.Click
        'ConfigureExportSettingsRTF()

        Try
            UCRADeditor.ExportToRtf()
        Catch ex As Exception
            LBerror.Text = "Exception: " + ex.Message
        End Try
    End Sub

    'Private Sub ConfigureExportSettingsDoc()
    '    Dim exportSettings As EditorDocxSettings = UCRADeditor.ExportSettings.Docx

    '    exportSettings.DefaultFontName = "Arial" 'ListOfFonts.SelectedItem.Text
    '    exportSettings.DefaultFontSizeInPoints = CDec(12)
    '    exportSettings.HeaderFontSizeInPoints = CDec(14)
    '    exportSettings.PageHeader = "Test"
    'End Sub

    'Private Sub ConfigureExportSettingsPDF()
    '    Dim exportSettings As GridPdfSettings = UCRADeditor.ExportSettings.Pdf

    '    exportSettings.DefaultFontName = "Arial" 'ListOfFonts.SelectedItem.Text
    '    exportSettings.DefaultFontSizeInPoints = CDec(12)
    '    exportSettings.HeaderFontSizeInPoints = CDec(14)
    '    exportSettings.PageHeader = "Test"
    'End Sub

    'Private Sub ConfigureExportSettingsRTF()
    '    Dim exportSettings As EditorRtfSettings = UCRADeditor.ExportSettings.Rtf

    '    exportSettings.DefaultFontName = "Arial"
    '    exportSettings.DefaultFontSizeInPoints = CDec(12)
    '    exportSettings.HeaderFontSizeInPoints = CDec(14)
    '    exportSettings.PageHeader = "Test"
    'End Sub

    Private Sub LKBshowhide_Click(sender As Object, e As EventArgs) Handles LKBshowhide.Click
        Dim visible As Boolean = Not Me.UCRADeditor.Visible

        Me.UCRADeditor.Visible = visible


        If visible Then
            LKBshowhide.Text = "Hide"
        Else
            LKBshowhide.Text = "Show"
        End If

    End Sub

    Public Sub ShowButtons(ByVal ShowPdf As Boolean, ByVal ShowRtf As Boolean, ByVal ShowDocx As Boolean, ByVal ShowHide As Boolean)

        LKBPDF.Visible = ShowPdf
        LKBPDF.Enabled = ShowPdf

        LKBRTF.Visible = ShowRtf
        LKBRTF.Enabled = ShowRtf

        LKBDOCX.Visible = ShowDocx
        LKBDOCX.Enabled = ShowDocx

        LKBshowhide.Visible = ShowHide
        LKBshowhide.Enabled = ShowHide
    End Sub

    Public Property ShowEditor As Boolean
        Get
            Return Me.UCRADeditor.Visible
        End Get
        Set(value As Boolean)
            Me.UCRADeditor.Visible = value
        End Set
    End Property

    Public Property ShowPdf As Boolean
        Get
            Return Me.LKBPDF.Visible
        End Get
        Set(value As Boolean)
            Me.LKBPDF.Visible = value
        End Set
    End Property

    Public Property ShowRtf As Boolean
        Get
            Return LKBRTF.Visible
        End Get
        Set(value As Boolean)
            LKBRTF.Visible = value
        End Set
    End Property

    Public Property ShowDocx As Boolean
        Get
            Return LKBDOCX.Visible
        End Get
        Set(value As Boolean)
            LKBDOCX.Visible = value
        End Set
    End Property

    Public Property ShowHideBtn As Boolean
        Get
            Return LKBshowhide.Visible
        End Get
        Set(value As Boolean)
            LKBshowhide.Visible = value
        End Set
    End Property

    Public Sub SetToolFile(ByVal File As String)

        Me.UCRADeditor.ToolsFile = File

    End Sub



    'Private Sub BindConfiguratorValues()
    '    Dim fontsCollection As New InstalledFontCollection()
    '    Dim fontFamilies As FontFamily() = fontsCollection.Families
    '    Dim fonts As New List(Of String)()
    '    For Each font As FontFamily In fontFamilies
    '        fonts.Add(font.Name)
    '    Next
    '    ListOfFonts.DataSource = fonts
    '    ListOfFonts.DataBind()

    '    rcbDocumentLevel.DataSource = System.[Enum].GetValues(GetType(DocumentLevel))
    '    rcbDocumentLevel.DataBind()
    '    rcbDocumentLevel.SelectedIndex = 1

    '    rcbImagesMode.DataSource = System.[Enum].GetValues(GetType(ImagesMode))
    '    rcbImagesMode.DataBind()

    '    rcbStylesMode.DataSource = System.[Enum].GetValues(GetType(StylesMode))
    '    rcbStylesMode.DataBind()
    'End Sub

    'Private Sub SetConfiguratorDefaultValues()
    '    Dim exportSettings As EditorDocxSettings = RadEditor1.ExportSettings.Docx
    '    Dim importSettings As ImportDocxSettings = RadEditor1.ImportSettings.Docx

    '    Dim fontToSelect As RadComboBoxItem = ListOfFonts.FindItemByText(exportSettings.DefaultFontName)
    '    fontToSelect.Selected = True

    '    rntbDefaultFontSize.Value = CDbl(exportSettings.DefaultFontSizeInPoints)
    '    rntbHeaderFontSize.Value = CDbl(exportSettings.HeaderFontSizeInPoints)
    '    rtbPageHeader.Text = exportSettings.PageHeader

    '    Dim docLevelToSelect As RadComboBoxItem = rcbDocumentLevel.FindItemByText(importSettings.DocumentLevel.ToString())
    '    docLevelToSelect.Selected = True

    '    Dim imagesModeToSelect As RadComboBoxItem = rcbImagesMode.FindItemByText(importSettings.ImagesMode.ToString())
    '    imagesModeToSelect.Selected = True

    '    Dim stylesModeToSelect As RadComboBoxItem = rcbStylesMode.FindItemByText(importSettings.StylesMode.ToString())
    '    stylesModeToSelect.Selected = True
    'End Sub


    Public Property RTFText As String
        Get
            Return LKBRTF.Text
        End Get
        Set(value As String)
            LKBRTF.Text = value
        End Set
    End Property

    Public Property DOCXTex As String
        Get
            Return LKBDOCX.Text
        End Get
        Set(value As String)
            LKBDOCX.Text = value
        End Set
    End Property

    Public Property PDFTex As String
        Get
            Return LKBPDF.Text
        End Get
        Set(value As String)
            LKBPDF.Text = value
        End Set
    End Property

    Public Property ShowHideText As String
        Get
            Return LKBshowhide.Text
        End Get
        Set(value As String)
            LKBshowhide.Text = value
        End Set
    End Property

End Class