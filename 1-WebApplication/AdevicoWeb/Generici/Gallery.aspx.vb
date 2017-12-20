Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports lm.Comol.Core.File

Public Class Gallery
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager

#Region "Pannello Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label

#Region "Filtro"
    Protected WithEvents TBLfiltroNew As System.Web.UI.WebControls.Table
    Protected WithEvents TBRchiudiFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBchiudiFiltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRapriFiltro As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBapriFiltro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBRfiltri As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBtipoRuolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBnumRecord_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoRicerca_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBvalore_t As System.Web.UI.WebControls.Label

    Protected WithEvents RBLabilitazione As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents CBXwithFoto As System.Web.UI.WebControls.CheckBox
    Protected WithEvents LKBtutti As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBaltro As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBa As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBb As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBc As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBd As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBe As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBf As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBg As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBh As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBi As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBj As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBk As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBl As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBm As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBn As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBo As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBp As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBq As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBr As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBs As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBt As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBu As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBv As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBw As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBx As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBy As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LKBz As System.Web.UI.WebControls.LinkButton
    Protected WithEvents DDLNumeroRecord As System.Web.UI.WebControls.DropDownList
    Protected WithEvents BTNCerca As System.Web.UI.WebControls.Button
    Protected WithEvents DDLTipoRuolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents DDLTipoRicerca As System.Web.UI.WebControls.DropDownList
    Protected WithEvents TXBValore As System.Web.UI.WebControls.TextBox

#End Region

    Protected WithEvents TBLgallery As System.Web.UI.WebControls.Table
    Protected WithEvents DGgallery As System.Web.UI.WebControls.DataGrid

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
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona
        Dim oServizioGallery As New Services_Gallery
        Dim PermessiAssociati As String

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        Try
            oPersona = Session("objPersona")
            If oPersona.Id <> 0 Then

            End If
        Catch ex As Exception
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            oPersona = Nothing
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Response.End()
            Exit Sub
        End Try
        Try
            If Session("idComunita") <= 0 Then
                Me.ExitToLimbo()
                Exit Sub
            End If
        Catch ex As Exception
            Me.ExitToLimbo()
            Exit Sub
        End Try

        Try
            If Not Page.IsPostBack Then
                'Session("azione") = "load"
                Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioGallery.Codex)
                oServizioGallery.PermessiAssociati = Me.ViewState("PermessiAssociati")
            Else
                If Me.ViewState("PermessiAssociati") = "" Then
                    Me.ViewState("PermessiAssociati") = Me.GetPermessiForPage(oServizioGallery.Codex)
                End If
                oServizioGallery.PermessiAssociati = Me.ViewState("PermessiAssociati")
            End If
        Catch ex As Exception
            oServizioGallery.PermessiAssociati = "00000000000000000000000000000000"
        End Try
        If Not Page.IsPostBack Then
            Try
                Me.SetupInternazionalizzazione()

                Me.ViewState("intCurPage") = 0
                Me.ViewState("intAnagrafica") = CType(Main.FiltroComunita.tutti, Main.FiltroComunita)
                Me.LKBtutti.CssClass = "lettera_Selezionata"

                Me.Bind_TipoRuoloFiltro()

                Dim isSimpleUser As Boolean = False
                isSimpleUser = Not (oServizioGallery.Admin Or oServizioGallery.Management)
                If oServizioGallery.Admin Or oServizioGallery.Management Then
                    Me.RBLabilitazione.Visible = True
                    Me.CBXwithFoto.Visible = True
                    Me.PNLcontenuto.Visible = True
                    Me.Bind_Griglia(True)
                ElseIf oServizioGallery.List Then
                    Me.RBLabilitazione.Visible = False
                    Me.CBXwithFoto.Visible = True
                    Me.PNLcontenuto.Visible = True
                    Me.Bind_Griglia(True)
                Else
                    Me.PNLpermessi.Visible = True
                    Me.PNLcontenuto.Visible = False
                End If

                Session("azione") = "load"
            Catch ex As Exception
                Me.PNLpermessi.Visible = True
                Me.PNLcontenuto.Visible = False
            End Try
        End If

    End Sub


    Private Sub ExitToLimbo()
        Session("Limbo") = True
        Session("ORGN_id") = 0
        Session("IdRuolo") = ""
        Session("ArrPermessi") = ""
        Session("RLPC_ID") = ""

        Session("AdminForChange") = False
        Session("CMNT_path_forAdmin") = ""
        Session("idComunita_forAdmin") = ""
        Session("TPCM_ID") = ""
        Me.Response.Expires = 0
        Me.Response.Redirect("./../Comunita/EntrataComunita.aspx", True)
    End Sub

    Private Function GetPermessiForPage(ByVal Codex As String) As String
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita
        Dim CMNT_id As Integer

        Dim PermessiAssociati As String

        Try
            oPersona = Session("objPersona")
            PermessiAssociati = Permessi(Codex, Me.Page)

            If (PermessiAssociati = "") Then
                PermessiAssociati = "00000000000000000000000000000000"
            End If
        Catch ex As Exception
            PermessiAssociati = "00000000000000000000000000000000"
        End Try

        If Request.QueryString("CMNT_id") Is Nothing Then
            Try
                CMNT_id = Session("IdComunita")
                PermessiAssociati = Permessi(Codex, Me.Page)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
        Else
            Dim oComunita As New COL_Comunita
            CMNT_id = Request.QueryString("CMNT_id")
            oComunita.Id = CMNT_id
            Try
                PermessiAssociati = oComunita.GetPermessiForServizioByPersona(oPersona.Id, Request.QueryString("CMNT_id"), Codex)
                If (PermessiAssociati = "") Then
                    PermessiAssociati = "00000000000000000000000000000000"
                End If
            Catch ex As Exception
                PermessiAssociati = "00000000000000000000000000000000"
            End Try
        End If

        Return PermessiAssociati
    End Function

#Region "Filtraggio Dati"
    Private Sub Bind_TipoRuoloFiltro()

        Me.DDLTipoRuolo.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita
            If Request.QueryString("CMNT_id") Is Nothing Then
                Dim IdComunita As Integer = Session("IdComunita")
                oComunita.Id = IdComunita
            Else
                oComunita.Id = Request.QueryString("CMNT_id")
            End If

            '  oComunita.Id = Session("IdComunita")
            oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForUtenti_NoGuest)

            Totale = oDataset.Tables(0).Rows.Count()
            If Totale > 0 Then
                Totale = Totale - 1
                For i = 0 To Totale
					Dim oListItem As New ListItem
                    If IsDBNull(oDataset.Tables(0).Rows(i).Item("TPRL_nome")) Then
                        oListItem.Text = "--"
                    Else
                        oListItem.Text = oDataset.Tables(0).Rows(i).Item("TPRL_nome")
                    End If
                    oListItem.Value = oDataset.Tables(0).Rows(i).Item("TPRL_ID")
                    Me.DDLTipoRuolo.Items.Add(oListItem)
                Next
                DDLTipoRuolo.Items.Insert(0, New ListItem(oResource.getValue("DDLTipoRuolo.-1"), -1))
            Else
                Me.DDLTipoRuolo.Items.Add(New ListItem(oResource.getValue("DDLTipoRuolo.-1"), -1))
            End If
        Catch ex As Exception
            Me.DDLTipoRuolo.Items.Add(New ListItem(oResource.getValue("DDLTipoRuolo.-1"), -1))
        End Try
    End Sub
    Private Sub BTNcerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNCerca.Click
        Me.Bind_Griglia(True)
    End Sub
    Public Sub FiltroLink_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        Bind_Griglia(True)
    End Sub
    Private Sub DeselezionaLink(ByVal Lettera As String)
        Dim oFiltro As Main.FiltroAnagrafica
        Lettera = CType(CInt(Lettera), Main.FiltroAnagrafica).ToString

        Dim oLink As System.Web.UI.WebControls.LinkButton
        oLink = Me.FindControlRecursive(Me.Master, "LKB" & Lettera)
        If IsNothing(oLink) = False Then
            oLink.CssClass = "lettera"
        End If
    End Sub
    Public Sub FiltroLinkLettere_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LKBaltro.Click
        If sender.commandArgument <> "" Then
            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
            Me.ViewState("intAnagrafica") = sender.commandArgument
            sender.CssClass = "lettera_Selezionata"
        Else
            Me.ViewState("intAnagrafica") = -1
            Me.LKBtutti.CssClass = "lettera_Selezionata"
        End If
        Bind_Griglia(True)
    End Sub
    Private Sub DDLTipoRuolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLTipoRuolo.SelectedIndexChanged
        Bind_Griglia(True)
    End Sub

    Private Sub RBLabilitazione_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLabilitazione.SelectedIndexChanged
        Bind_Griglia(True)
    End Sub
    Private Sub CBXwithFoto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CBXwithFoto.CheckedChanged
        Me.ViewState("intCurPage") = 0
        Bind_Griglia(True)
    End Sub
    Private Sub DDLNumeroRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLNumeroRecord.SelectedIndexChanged
        Me.DGgallery.PageSize = Me.DDLNumeroRecord.Items(DDLNumeroRecord.SelectedIndex).Value
        Me.ViewState("intCurPage") = 0
        Bind_Griglia(True)
    End Sub



    Private Sub LNBapriFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBapriFiltro.Click
        Me.TBRfiltri.Visible = True
        Me.TBRchiudiFiltro.Visible = True
        Me.TBRapriFiltro.Visible = False
        Me.Bind_Griglia()
    End Sub
    Private Sub LNBchiudiFiltro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBchiudiFiltro.Click
        Me.TBRfiltri.Visible = False
        Me.TBRchiudiFiltro.Visible = False
        Me.TBRapriFiltro.Visible = True
        Me.Bind_Griglia()
    End Sub
#End Region

#Region "Bind_Dati"

    Private Function FiltraggioDati(Optional ByVal ricalcola As Boolean = False) As DataSet
        Dim oDataset As New DataSet
        Try
            Dim Valore As String
            Dim oComunita As New COL_Comunita
            If Request.QueryString("CMNT_id") Is Nothing Then
                Dim CMNT_ID As Integer = Session("IdComunita")
                oComunita.Id = CMNT_ID
            Else
                oComunita.Id = Request.QueryString("CMNT_id")
            End If

            Dim TPRL_id As Integer
            TPRL_id = Me.DDLTipoRuolo.SelectedValue

            Dim oFiltroCampoOrdine As COL_Comunita.FiltroCampoOrdine
            Dim oFiltroOrdinamento As Main.FiltroOrdinamento

            Dim oFiltroAnagrafica As Main.FiltroAnagrafica
            Dim oFiltroRicerca As Main.FiltroRicercaAnagrafica = Main.FiltroRicercaAnagrafica.tutti

            If Me.TXBValore.Text <> "" Then
                Valore = Trim(Me.TXBValore.Text)
            End If
            Try
                If IsNothing(Me.ViewState("intAnagrafica")) Then
                    oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Else
                    oFiltroAnagrafica = CType(Me.ViewState("intAnagrafica"), Main.FiltroAnagrafica)
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
                Me.ViewState("intAnagrafica") = -1
            End Try


            Try
                If Valore <> "" Then
                    Select Case Me.DDLTipoRicerca.SelectedItem.Value
                        Case Main.FiltroRicercaAnagrafica.nome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nome
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                        Case Main.FiltroRicercaAnagrafica.cognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.cognome

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.nomeCognome
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.nomeCognome
                        Case Main.FiltroRicercaAnagrafica.matricola
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.matricola

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Main.FiltroRicercaAnagrafica.dataNascita
                            Try
                                If IsDate(Valore) Then
                                    Valore = Main.DateToString(Valore, False)
                                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.dataNascita
                                End If
                            Catch ex As Exception

                            End Try
                        Case Main.FiltroRicercaAnagrafica.login
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.login

                            oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                            Me.LKBtutti.CssClass = "lettera_Selezionata"
                            Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                            Me.ViewState("intAnagrafica") = -1
                        Case Else
                            oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                    End Select
                Else
                    oFiltroRicerca = Main.FiltroRicercaAnagrafica.tutti
                End If
            Catch ex As Exception
                Me.DeselezionaLink(Me.ViewState("intAnagrafica"))
                oFiltroAnagrafica = Main.FiltroAnagrafica.tutti
                Me.LKBtutti.CssClass = "lettera_Selezionata"
            End Try
            Dim oFiltroAbilitazione As Main.FiltroAbilitazione

            Select Case Me.RBLabilitazione.SelectedValue
                Case Main.FiltroAbilitazione.Tutti
                    oFiltroAbilitazione = Main.FiltroAbilitazione.Tutti
                Case Main.FiltroAbilitazione.AttivatoAbilitato
                    oFiltroAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
                Case Main.FiltroAbilitazione.NonAbilitatoAttivato
                    oFiltroAbilitazione = Main.FiltroAbilitazione.NonAbilitatoAttivato
                Case Main.FiltroAbilitazione.NonAttivato
                    oFiltroAbilitazione = Main.FiltroAbilitazione.NonAttivato
                Case Else
                    oFiltroAbilitazione = Main.FiltroAbilitazione.AttivatoAbilitato
            End Select

            oFiltroCampoOrdine = COL_Comunita.FiltroCampoOrdine.anagrafica
            oFiltroOrdinamento = Main.FiltroOrdinamento.Crescente

            Dim totale As Decimal
            If ricalcola Then
                Me.ViewState("intCurPage") = 0
                Me.DGgallery.CurrentPageIndex = 0
            End If
            'come persona id passo 0 così includo anche io me stesso
            Return oComunita.ElencaIscrittiNoMittente(Session("LinguaID"), 0, oFiltroAbilitazione, Main.FiltroUtenti.NoPassantiNoCreatori, TPRL_id, Me.DGgallery.PageSize, Me.ViewState("intCurPage"), Valore, oFiltroAnagrafica, oFiltroOrdinamento, oFiltroCampoOrdine, Me.CBXwithFoto.Checked, oFiltroRicerca)

        Catch ex As Exception
            Return oDataset
        End Try
    End Function

    Public Sub Bind_Griglia(Optional ByVal ricalcola As Boolean = False)
        Dim oPersona As New COL_Persona
        Dim oDataSet As New DataSet
        Try
            oPersona = Session("objPersona")
            oDataSet = FiltraggioDati(ricalcola)

            Dim i, totale As Integer

            totale = oDataSet.Tables(0).Rows.Count
            If totale = 0 Then
                Me.DGgallery.Visible = False
                Me.DGgallery.VirtualItemCount = 0
            Else
                Me.DGgallery.VirtualItemCount = oDataSet.Tables(0).Rows(0).Item("Totale")
                oDataSet.Tables(0).Columns.Add(New DataColumn("oIscrittoIl"))
                'For i = 0 To totale - 1
                '    Dim oRow As DataRow

                '    oRow = oDataSet.Tables(0).Rows(i)
                '    If IsDBNull(oRow.Item("RLPC_IscrittoIl")) = False Then
                '        If Equals(oRow.Item("RLPC_IscrittoIl"), New Date) Then
                '            oRow.Item("oIscrittoIl") = "--"
                '        Else
                '            oRow.Item("oIscrittoIl") = FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortDate) & " " & FormatDateTime(oRow.Item("RLPC_IscrittoIl"), DateFormat.ShortTime)
                '        End If
                '    Else
                '        oRow.Item("oIscrittoIl") = "--"
                '    End If
                'Next

                Dim totaleElementi, NumRecordRiga As Integer
                totaleElementi = Me.DDLNumeroRecord.SelectedValue

                Dim oServizioGallery As New UCServices.Services_Gallery
                oServizioGallery.PermessiAssociati = GetPermessiForPage(oServizioGallery.Codex)

                NumRecordRiga = 6

                Dim oTableRow As New TableRow
                Me.TBLgallery.Rows.Clear()
                For i = 0 To totale - 1
                    Dim oRow As DataRow
                    oRow = oDataSet.Tables(0).Rows(i)

                    Dim oTableCell As New TableCell
                    If NumRecordRiga = 6 Then
                        oTableRow = New TableRow
                    End If

                    'creo il contenuto della cella, ossia una tabella con l'immagine ed il nome !
                    Dim oTable As New Table
                    Dim oImage As New System.Web.UI.WebControls.Image

                    Dim oTableRowImage_0 As New TableRow
                    Dim oTableCellImage_0 As New TableCell
                    Dim oTableRowImage_1 As New TableRow
                    Dim oTableCellImage_1 As New TableCell

                    oTable.Width = System.Web.UI.WebControls.Unit.Pixel(128)
                    If IsDBNull(oRow.Item("PRSN_FotoPath")) Then
                        oImage.ImageUrl = "./../images/noimage.jpg"
                    Else
                        Dim url As String = "./../profili/" & oRow.Item("PRSN_ID") & "/" & oRow.Item("PRSN_FotoPath")
                        If Exists.File(Server.MapPath(url)) Then 'file su disco trovato
                            oImage.ImageUrl = url
                        Else
                            oImage.ImageUrl = "./../images/noimage.jpg"
                        End If
                    End If
                    oImage.Height = System.Web.UI.WebControls.Unit.Pixel(125)
                    oImage.Width = System.Web.UI.WebControls.Unit.Pixel(100)

                    ' Aggiungo l'immagine !
                    oTableCellImage_0.HorizontalAlign = HorizontalAlign.Center
                    oTableCellImage_0.Controls.Add(oImage)
                    oTableCellImage_0.VerticalAlign = VerticalAlign.Top
                    oTableRowImage_0.Cells.Add(oTableCellImage_0)
                    oTable.Rows.Add(oTableRowImage_0)


                    oTableCellImage_1.HorizontalAlign = HorizontalAlign.Center
                    oTableCellImage_1.VerticalAlign = VerticalAlign.Top
                    If oServizioGallery.Management Or oServizioGallery.Admin Then
                        Dim oHyperLink As New System.Web.UI.WebControls.HyperLink
                        oHyperLink.NavigateUrl = "mailto:" & oRow.Item("PRSN_Mail")
                        oHyperLink.Text = oRow.Item("PRSN_Anagrafica")
                        oTableCellImage_1.Controls.Add(oHyperLink)
                    Else
                        Dim mostraMail As Boolean = False
                        Try
                            If oRow.Item("PRSN_mostraMail") = True Then
                                mostraMail = True
                            End If
                        Catch ex As Exception

                        End Try

                        Try
                            If Not IsDBNull(oRow.Item("RLPC_PRSN_mostraMail")) Then
                                mostraMail = CBool(oRow.Item("RLPC_PRSN_mostraMail"))
                            End If
                        Catch ex As Exception

                        End Try

                        If mostraMail Then
                            Dim oHyperLink As New System.Web.UI.WebControls.HyperLink
                            oHyperLink.NavigateUrl = "mailto:" & oRow.Item("PRSN_Mail")
                            oHyperLink.Text = oRow.Item("PRSN_Anagrafica")
                            oTableCellImage_1.Controls.Add(oHyperLink)
                        Else
                            Dim oLabel As New System.Web.UI.WebControls.Label
                            oLabel.Text = oRow.Item("PRSN_Anagrafica")
                            oTableCellImage_1.Controls.Add(oLabel)
                        End If
                    End If

                    oTableRowImage_1.Cells.Add(oTableCellImage_1)
                    oTable.Rows.Add(oTableRowImage_1)

                    oTableCell.HorizontalAlign = HorizontalAlign.Center
                    oTableCell.VerticalAlign = VerticalAlign.Top
                    oTableCell.Controls.Add(oTable)
                    oTableRow.Cells.Add(oTableCell)
                    NumRecordRiga -= 1
                    If NumRecordRiga = 0 Then
                        Me.TBLgallery.Rows.Add(oTableRow)
                        NumRecordRiga = 6
                    End If
                Next
                If NumRecordRiga > 0 Then
                    If oTableRow.Cells.Count < 6 Then
                        Dim oTableCell As New TableCell
                        oTableCell.ColumnSpan = NumRecordRiga
                        oTableCell.Text = "&nbsp;"
                        oTableRow.Cells.Add(oTableCell)
                    End If
                    Me.TBLgallery.Rows.Add(oTableRow)
                End If
                If totale > 0 Then
                    Me.DGgallery.Visible = True

                    Dim oDataview As DataView
                    oDataview = oDataSet.Tables(0).DefaultView
                    If viewstate("SortExspression") = "" Then
                        viewstate("SortExspression") = "PRSN_Anagrafica"
                        viewstate("SortDirection") = "asc"
                    End If
                    oDataview.Sort = viewstate("SortExspression") & " " & viewstate("SortDirection")

                    Me.DGgallery.DataSource = oDataview
                    Me.DGgallery.DataBind()

                    If Me.DGgallery.VirtualItemCount >= Me.DGgallery.PageSize Then
                        Me.DGgallery.Visible = True
                    Else
                        Me.DGgallery.Visible = False
                    End If
                Else
                    Me.DGgallery.Visible = False
                End If
            End If
        Catch ex As Exception
            Me.DGgallery.Visible = False
        End Try
    End Sub
#End Region

#Region "Griglia Paginazione"
    Private Sub DGgallery_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGgallery.PageIndexChanged
        Me.DGgallery.CurrentPageIndex = e.NewPageIndex
        Me.ViewState("intCurPage") = e.NewPageIndex
        Me.Bind_Griglia(False)
    End Sub

    Private Sub DGnewsPaginazione_ItemCreated(ByVal sender As Object, ByVal e As DataGridItemEventArgs) Handles DGgallery.ItemCreated
        Dim i As Integer

        If IsNothing(oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If

        If e.Item.ItemType = ListItemType.Pager Then
            Dim oCell As TableCell
            Dim n As Integer
            oCell = CType(e.Item.Controls(0), TableCell)


            For n = 0 To oCell.Controls.Count - 1 Step 2
                Dim szLnk As String
                szLnk = "System.Web.UI.WebControls.DataGridLinkButton"
                Dim oWebControl As WebControl

                oWebControl = oCell.Controls(n)

                If (oWebControl.GetType().ToString() = szLnk) Then
                    oWebControl.CssClass = "PagerLink"
                End If
                Try
                    Dim oLabel As Label
                    oLabel = oWebControl
                    oLabel.Text = oLabel.Text
                    oLabel.CssClass = "PagerSpan"
                Catch ex As Exception
                    Dim oLinkbutton As LinkButton
                    oLinkbutton = oWebControl
                    oLinkbutton.CssClass = "PagerLink"
                    oResource.setPageDatagrid(Me.DGgallery, oLinkbutton)

                End Try
            Next
        End If
    End Sub

#End Region

#Region "Internazionalizzazione"
    ' Inizializzazione oggetto risorse: SEMPRE DA FARE
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_Gallery"
        oResource.Folder_Level1 = "Generici"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            '.setLabel(Me.LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(Me.LBNopermessi)
            .setLinkButton(Me.LKBaltro, True, True)
            .setLinkButton(Me.LKBtutti, True, True)
            Dim i As Integer
            For i = Asc("a") To Asc("z") 'status dei link button delle lettere
                Dim oLinkButton As New LinkButton
                oLinkButton = FindControlRecursive(Me.Master, "LKB" & Chr(i))
                Dim Carattere As String = Chr(i)

                If IsNothing(oLinkButton) = False Then
                    oResource.setLinkButtonLettera(oLinkButton, "#%%#", Carattere.ToUpper, True, True)
                End If
            Next

            .setLabel(Me.LBtipoRuolo_t)
            .setLabel(Me.LBnumRecord_t)
            .setLabel(Me.LBtipoRicerca_t)
            .setLabel(Me.LBvalore_t)
            .setButton(Me.BTNCerca)

            .setDropDownList(Me.DDLTipoRicerca, -2)
            .setDropDownList(Me.DDLTipoRicerca, -3)
            .setDropDownList(Me.DDLTipoRicerca, -4)

            .setRadioButtonList(Me.RBLabilitazione, 0)
            .setRadioButtonList(Me.RBLabilitazione, 1)
            .setRadioButtonList(Me.RBLabilitazione, 7)
            .setRadioButtonList(Me.RBLabilitazione, 4)

            .setCheckBox(Me.CBXwithFoto)

            .setLinkButton(Me.LNBapriFiltro, True, True)
            .setLinkButton(Me.LNBchiudiFiltro, True, True)
        End With

    End Sub
#End Region


    Private Function FindControlRecursive(ByVal Root As Control, ByVal Id As String) As Control
        If Root.ID = Id Then
            Return Root
        End If

        For Each Ctl As Control In Root.Controls
            Dim FoundCtl As Control = FindControlRecursive(Ctl, Id)
            If FoundCtl IsNot Nothing Then
                Return FoundCtl
            End If
        Next
        Return Nothing
    End Function

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property



End Class
