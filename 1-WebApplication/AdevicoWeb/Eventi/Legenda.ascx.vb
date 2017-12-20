Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2

Public Class Legenda
    Inherits System.Web.UI.UserControl

    Protected WithEvents DGLegenda As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DTLlegenda As System.Web.UI.WebControls.DataList
    Protected oResource As ResourceManager

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oDataset As New DataSet
        Dim oTipoEvento As New COL_Tipo_Evento
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If


        Try
            oDataset = oTipoEvento.Elenca(CInt(Session("LinguaID")))
            If oDataset.Tables(0).Rows.Count > 0 Then
                Dim oRow As DataRow
                oRow = oDataset.Tables(0).NewRow
                oRow.Item("TPEV_id") = -1
                oRow.Item("TPEV_icon") = "gainsboro"
                oRow.Item("TPEV_nome") = oResource.getValue("legenda.piuEventi")
                oDataset.Tables(0).Rows.Add(oRow)
                Me.DTLlegenda.DataSource = oDataset.Tables(0).DefaultView
                Me.DTLlegenda.DataBind()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_CalendarioEventi"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub
End Class
