Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_permessi



Public Class StampaIscritti
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager


    Protected WithEvents DGiscritti As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DGiscrittiBloccati As System.Web.UI.WebControls.DataGrid
    Protected WithEvents DGiscrittiInAttesa As System.Web.UI.WebControls.DataGrid
    Protected WithEvents BTNOk As System.Web.UI.WebControls.Button
    Protected WithEvents BTNstampa As System.Web.UI.WebControls.Button
    Protected WithEvents BTNexcel As System.Web.UI.WebControls.Button
    Protected WithEvents LBnoIscritti As System.Web.UI.WebControls.Label
    Protected WithEvents LBnoIscrittiBloccati As System.Web.UI.WebControls.Label
    Protected WithEvents LBnoIscrittiInAttesa As System.Web.UI.WebControls.Label
    Protected WithEvents LBinAttesa As System.Web.UI.WebControls.Label
    Protected WithEvents LButentiBloccati As System.Web.UI.WebControls.Label
    Protected WithEvents LBabilitati As System.Web.UI.WebControls.Label
    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRiscritti As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRiscrittiSeparatore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRbloccati As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRbloccatiSeparatore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRbloccati1 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRbloccati2 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBLDettagli As System.Web.UI.WebControls.Table
    Protected WithEvents TBRinAttesa As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRinAttesaSeparatore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRinAttesa1 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRinAttesa2 As System.Web.UI.WebControls.TableRow



#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTA: la seguente dichiarazione è richiesta da Progettazione Web Form.
    'Non spostarla o rimuoverla.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If


        If Not Page.IsPostBack Then
            SetupInternazionalizzazione()

            Dim oServizio As New UCServices.Services_GestioneIscritti
            Dim PermessiAssociati As String

            Dim oPersona As New COL_Persona
            oPersona = Session("objPersona")
            'oPersona.GetPermessiForServizio(Request.QueryString("CMNT_ID"), oServizio.Codex)
            Try

                If Request.QueryString("CMNT_ID") Is Nothing Then
                    PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
                Else
                    PermessiAssociati = oPersona.GetPermessiForServizio(Request.QueryString("CMNT_ID"), oServizio.Codex)
                End If

                If Not (PermessiAssociati = "") Then
                    oServizio.PermessiAssociati = PermessiAssociati
                Else
                    oServizio.PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                oServizio.PermessiAssociati = "00000000000000000000000000000000"
            End Try

            Me.BTNOk.Attributes.Add("onclick", "ChiudiMi();return false;")
            Me.BTNstampa.Attributes.Add("onclick", "stampa();return false;")

            Bind_Griglia_Abilitati(Main.FiltroAbilitazione.AttivatoAbilitato)
            If oServizio.Admin Or oServizio.Management Then
                Bind_Griglia_InAttesa(Main.FiltroAbilitazione.NonAttivato)
                Bind_Griglia_Bloccati(Main.FiltroAbilitazione.NonAbilitatoAttivato)
            Else
                Me.TBRbloccati.Visible = False
                Me.TBRbloccatiSeparatore.Visible = False
                Me.TBRbloccati1.Visible = False
                Me.TBRbloccati2.Visible = False

                Me.TBRinAttesa.Visible = False
                Me.TBRinAttesa1.Visible = False
                Me.TBRinAttesa2.Visible = False
                Me.TBRinAttesaSeparatore.Visible = False
            End If


            Dim TPRL_id As Integer
            If Request.QueryString("TPRL_id") Is Nothing Then
                TPRL_id = -1
            Else
                Try
                    TPRL_id = Request.QueryString("TPRL_id")
                Catch ex As Exception
                    TPRL_id = -1
                End Try
            End If
            If TPRL_id = -1 Then
                Me.LBtitolo.Text = oResource.getValue("LBtitolo.standard")
            Else
                Try
                    Dim oTipoRuolo As New COL_TipoRuolo
                    oTipoRuolo.Id = TPRL_id
                    oTipoRuolo.Estrai(Session("LinguaID"))
                    If oTipoRuolo.Errore = Errori_Db.None Then
                        Me.LBtitolo.Text = oResource.getValue("LBtitolo.altri")
                        Me.LBtitolo.Text = Replace(Me.LBtitolo.Text, "#ruolo#", oTipoRuolo.Nome)
                    Else
                        Me.LBtitolo.Text = oResource.getValue("LBtitolo.standard")
                    End If
                Catch ex As Exception
                    Me.LBtitolo.Text = oResource.getValue("LBtitolo.standard")
                End Try
            End If
        End If


    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = code
        oResource.ResourcesName = "pg_StampaIscritti"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()

    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setButton(Me.BTNOk, True)
            .setButton(Me.BTNstampa, True)
            .setButton(Me.BTNexcel, True)
            .setHeaderDatagrid(Me.DGiscritti, 1, "matricola", True)
            .setHeaderDatagrid(Me.DGiscritti, 2, "nome", True)
            .setHeaderDatagrid(Me.DGiscritti, 3, "cognome", True)
            .setHeaderDatagrid(Me.DGiscritti, 4, "Mail", True)
            .setHeaderDatagrid(Me.DGiscritti, 5, "PRSN_login", True)
            .setHeaderDatagrid(Me.DGiscritti, 6, "TPRL_nome", True)
            .setHeaderDatagrid(Me.DGiscritti, 7, "RLPC_IscrittoIl", True)
            Me.DGiscrittiBloccati.Columns(1).HeaderText = Me.DGiscritti.Columns(1).HeaderText
            Me.DGiscrittiInAttesa.Columns(1).HeaderText = Me.DGiscritti.Columns(1).HeaderText

            Me.DGiscrittiBloccati.Columns(2).HeaderText = Me.DGiscritti.Columns(2).HeaderText
            Me.DGiscrittiInAttesa.Columns(2).HeaderText = Me.DGiscritti.Columns(2).HeaderText
            Me.DGiscrittiBloccati.Columns(3).HeaderText = Me.DGiscritti.Columns(3).HeaderText
            Me.DGiscrittiInAttesa.Columns(3).HeaderText = Me.DGiscritti.Columns(3).HeaderText
            Me.DGiscrittiBloccati.Columns(4).HeaderText = Me.DGiscritti.Columns(4).HeaderText
            Me.DGiscrittiInAttesa.Columns(4).HeaderText = Me.DGiscritti.Columns(4).HeaderText
            Me.DGiscrittiBloccati.Columns(5).HeaderText = Me.DGiscritti.Columns(5).HeaderText
            Me.DGiscrittiInAttesa.Columns(5).HeaderText = Me.DGiscritti.Columns(5).HeaderText
            Me.DGiscrittiBloccati.Columns(6).HeaderText = Me.DGiscritti.Columns(6).HeaderText
            Me.DGiscrittiInAttesa.Columns(6).HeaderText = Me.DGiscritti.Columns(6).HeaderText
            Me.DGiscrittiBloccati.Columns(7).HeaderText = Me.DGiscritti.Columns(7).HeaderText
            Me.DGiscrittiInAttesa.Columns(7).HeaderText = Me.DGiscritti.Columns(7).HeaderText

            .setLabel(Me.LBnoIscrittiInAttesa)
            .setLabel(Me.LBinAttesa)
            .setLabel(Me.LBnoIscrittiBloccati)
            .setLabel(Me.LButentiBloccati)
            .setLabel(Me.LBnoIscritti)
            .setLabel(Me.LBabilitati)
        End With
    End Sub
#End Region

    Public Sub Bind_Griglia_Abilitati(ByVal oFiltro As Main.FiltroAbilitazione)
        Dim dsTable As New DataSet
        Try
            dsTable = FiltraggioDati(oFiltro)
            Dim i, totale As Integer
            If dsTable.Tables(0).Rows.Count = 0 Then
                Me.DGiscritti.Visible = False
                Me.TBRiscritti.Visible = False
                Me.TBRiscrittiSeparatore.Visible = False
            Else

                totale = dsTable.Tables(0).Rows.Count
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = dsTable.Tables(0).Rows(i)
                    Try
                        oRow.Item("oNumero") = i + 1 & ")"
                        If IsDBNull(oRow.Item("STDN_matricola")) = False Then
                            If oRow.Item("STDN_matricola") <> "-1" Then
                                oRow.Item("oMatricola") = oRow.Item("STDN_matricola")
                            Else
                                oRow.Item("oMatricola") = "<b>" & oResource.getValue("noMatricola") & "</b>"
                            End If
                        Else
                            oRow.Item("oMatricola") = "--"
                        End If
                        If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                            If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                                oRow.Item("oIscritto") = "&nbsp;--"
                            Else
                                oRow.Item("oIscritto") = "&nbsp;" & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                            End If
                        Else
                            oRow.Item("oIscritto") = "&nbsp;--"
                        End If
                    Catch ex As Exception

                    End Try
                Next

                If totale > 0 Then
                    Me.DGiscritti.Visible = True
                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_cognome"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")
                    Me.DGiscritti.DataSource = oDataview
                    Me.DGiscritti.DataBind()
                    LBnoIscritti.Visible = False
                    Dim TPRL_id As Integer
                    If Request.QueryString("TPRL_id") Is Nothing Then

                    Else
                        TPRL_id = Request.QueryString("TPRL_id")
                    End If
                    If TPRL_id <> -1 Then
                        Me.DGiscritti.Columns(6).Visible = False
                    End If
                Else
                    Me.DGiscritti.Visible = False
                    Me.TBRiscritti.Visible = False
                    Me.TBRiscrittiSeparatore.Visible = False
                End If
            End If
        Catch ex As Exception
            Me.DGiscritti.Visible = False
            LBnoIscritti.Visible = True
        End Try
    End Sub

    Public Sub Bind_Griglia_Bloccati(ByVal oFiltro As Main.FiltroAbilitazione)
        Dim dsTable As New DataSet
        Try
            dsTable = FiltraggioDati(oFiltro)
            Dim i, totale As Integer
            If dsTable.Tables(0).Rows.Count = 0 Then
                Me.DGiscrittiBloccati.Visible = False
                Me.TBRbloccati.Visible = False
                Me.TBRbloccatiSeparatore.Visible = False
            Else
                totale = dsTable.Tables(0).Rows.Count
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = dsTable.Tables(0).Rows(i)
                    Try
                        oRow.Item("oNumero") = i + 1 & ")"
                        If IsDBNull(oRow.Item("STDN_matricola")) = False Then
                            If oRow.Item("STDN_matricola") <> -1 Then
                                oRow.Item("oMatricola") = oRow.Item("STDN_matricola")
                            Else
                                oRow.Item("oMatricola") = "<b>" & oResource.getValue("noMatricola") & "</b>"
                            End If
                        Else
                            oRow.Item("oMatricola") = "--"
                        End If
                        If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                            If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                                oRow.Item("oIscritto") = "&nbsp;--"
                            Else
                                oRow.Item("oIscritto") = "&nbsp;" & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                            End If
                        Else
                            oRow.Item("oIscritto") = "&nbsp;--"
                        End If
                    Catch ex As Exception

                    End Try
                Next

                If totale > 0 Then
                    Me.DGiscrittiBloccati.Visible = True
                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_cognome"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")
                    Me.DGiscrittiBloccati.DataSource = oDataview
                    Me.DGiscrittiBloccati.DataBind()
                    LBnoIscritti.Visible = False
                    Dim TPRL_id As Integer
                    If Request.QueryString("TPRL_id") Is Nothing Then

                    Else
                        TPRL_id = Request.QueryString("TPRL_id")
                    End If
                    If TPRL_id <> -1 Then
                        Me.DGiscrittiBloccati.Columns(6).Visible = False
                    End If
                Else
                    Me.DGiscrittiBloccati.Visible = False
                    Me.TBRbloccati.Visible = False
                    Me.TBRbloccatiSeparatore.Visible = False
                End If
            End If
        Catch ex As Exception
            Me.DGiscrittiBloccati.Visible = False
            LBnoIscrittiBloccati.Visible = True
        End Try
    End Sub
    Public Sub Bind_Griglia_InAttesa(ByVal oFiltro As Main.FiltroAbilitazione)
        Dim dsTable As New DataSet
        Try
            dsTable = FiltraggioDati(oFiltro)
            Dim i, totale As Integer
            If dsTable.Tables(0).Rows.Count = 0 Then
                Me.DGiscrittiInAttesa.Visible = False
                Me.TBRinAttesa.Visible = False
                Me.TBRinAttesaSeparatore.Visible = False
            Else
                totale = dsTable.Tables(0).Rows.Count
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = dsTable.Tables(0).Rows(i)
                    Try
                        oRow.Item("oNumero") = i + 1 & ")"
                        If IsDBNull(oRow.Item("STDN_matricola")) = False Then
                            If oRow.Item("STDN_matricola") <> -1 Then
                                oRow.Item("oMatricola") = oRow.Item("STDN_matricola")
                            Else
                                oRow.Item("oMatricola") = "<b>" & oResource.getValue("noMatricola") & "</b>"
                            End If
                        Else
                            oRow.Item("oMatricola") = "--"
                        End If
                        If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                            If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                                oRow.Item("oIscritto") = "&nbsp;--"
                            Else
                                oRow.Item("oIscritto") = "&nbsp;" & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                            End If
                        Else
                            oRow.Item("oIscritto") = "&nbsp;--"
                        End If
                    Catch ex As Exception

                    End Try
                Next

                If totale > 0 Then
                    Me.DGiscrittiInAttesa.Visible = True
                    Dim oDataview As DataView
                    oDataview = dsTable.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_cognome"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")
                    Me.DGiscrittiInAttesa.DataSource = oDataview
                    Me.DGiscrittiInAttesa.DataBind()
                    LBnoIscrittiInAttesa.Visible = False
                    Dim TPRL_id As Integer
                    If Request.QueryString("TPRL_id") Is Nothing Then

                    Else
                        TPRL_id = Request.QueryString("TPRL_id")
                    End If
                    If TPRL_id <> -1 Then
                        Me.DGiscrittiInAttesa.Columns(6).Visible = False
                    End If
                Else
                    Me.DGiscrittiInAttesa.Visible = False
                    Me.LBnoIscrittiInAttesa.Visible = True
                    Me.TBRinAttesaSeparatore.Visible = False
                End If
            End If
        Catch ex As Exception
            Me.DGiscrittiInAttesa.Visible = False
            LBnoIscrittiInAttesa.Visible = True
        End Try
    End Sub

    Private Function FiltraggioDati(ByVal oFiltro As Main.FiltroAbilitazione) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim oPersona As New COL_Persona
            Dim Valore As String
            oPersona = Session("objPersona")

            Dim oComunita As New COL_Comunita
            'per riferimenti futuri!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            If Request.QueryString("CMNT_id") Is Nothing Then
                Dim CMNT_ID As Integer = Session("IdComunita")
                oComunita.Id = CMNT_ID
            Else
                oComunita.Id = Request.QueryString("CMNT_id")
            End If

            Dim TPRL_id As Integer
            If Request.QueryString("TPRL_id") Is Nothing Then
                ' TPRL_id = Session("IdComunita")
            Else
                TPRL_id = Request.QueryString("TPRL_id")
            End If

            oDataset = oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), oPersona.Id, oFiltro, Main.FiltroUtenti.NoPassantiNoCreatori, TPRL_id)
            If oDataset.Tables.Count > 0 Then
                oDataset.Tables(0).Columns.Add(New DataColumn("oMatricola"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oNumero"))
                oDataset.Tables(0).Columns.Add(New DataColumn("oIscritto"))
            End If

            Return oDataset
        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Private Sub BTNexcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNexcel.Click
        Dim oComunita As New COL_Comunita
        Dim IDCMNT As Integer = Session("idComunita")
        oComunita.Id = IDCMNT
        Dim nomeComunita As String = oComunita.EstraiNomeBylingua(Session("LinguaID"))
        Try
            Dim FileName As String
            If nomeComunita <> "" Then
                nomeComunita = Replace(nomeComunita, "'", "_")
                If Len(nomeComunita) > 20 Then
                    nomeComunita = Left(nomeComunita, 20)
                End If
                FileName = "IscrittiComunita_" & nomeComunita & ".xls"
            Else
                FileName = "IscrittiComunita.xls"
            End If

            Dim oStringWriter As System.IO.StringWriter = New System.IO.StringWriter
            Dim oHTMLWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)
            Dim oStringWriter2 As System.IO.StringWriter = New System.IO.StringWriter
            Dim oHTMLWriter2 As New System.Web.UI.HtmlTextWriter(oStringWriter2)
            Dim oStringWriter3 As System.IO.StringWriter = New System.IO.StringWriter
            Dim oHTMLWriter3 As New System.Web.UI.HtmlTextWriter(oStringWriter3)

            Response.Buffer = True

            Page.Response.Clear()
            Page.Response.ClearContent()
            Page.Response.ClearHeaders()

            Me.DGiscritti.RenderControl(oHTMLWriter)
            Me.DGiscrittiBloccati.RenderControl(oHTMLWriter2)
            Me.DGiscrittiInAttesa.RenderControl(oHTMLWriter3)
			Page.Response.Write("<html><head  </head><body>")


            If Me.DGiscritti.Visible = True Then
                Page.Response.Write("Utenti iscritti <br>")
                Page.Response.Write(oStringWriter)
            End If
            If Me.DGiscrittiBloccati.Visible = True Then
                Page.Response.Write("<br><br><br>Utenti Bloccati <br>")
                Page.Response.Write(oStringWriter2)
            End If
            If Me.DGiscrittiInAttesa.Visible = True Then
                Page.Response.Write("<br><br><br>Utenti In Attesa <br>")
                Page.Response.Write(oStringWriter3)
            End If


            Page.Response.Write("</body></html>")

            Dim OpenType As String = "attachment"
            Page.Response.ContentType = "application/ms-excel"
            Page.Response.AddHeader("Content-Disposition", OpenType + ";filename=" + FileName)
            Page.Response.End()
        Catch ex As Exception

        End Try
    End Sub
End Class
