Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita


Public Class ModificaRuoloIscritto
    Inherits System.Web.UI.Page
Private oResource As ResourceManager

#Region "Pannello Modifica"
    Protected WithEvents LBlegenda As System.Web.UI.WebControls.Label
    Protected WithEvents PNLmodifica As System.Web.UI.WebControls.Panel
    Protected WithEvents LBcomunita_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcomunita As System.Web.UI.WebControls.Label

    Protected WithEvents HDrlpc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNprsnID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNrlpc_Attivato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNrlpc_Abilitato As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDN_cmntID As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LBanagrafica_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBNomeCognome As System.Web.UI.WebControls.Label
    Protected WithEvents LBruolo_t As System.Web.UI.WebControls.Label
    Protected WithEvents DDLruolo As System.Web.UI.WebControls.DropDownList

    Protected WithEvents LBabilitato_t As System.Web.UI.WebControls.Label
    Protected WithEvents LNBabilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBdisabilita As System.Web.UI.WebControls.LinkButton
    Protected WithEvents LNBattiva As System.Web.UI.WebControls.LinkButton

    Protected WithEvents LBresponsabile_t As System.Web.UI.WebControls.Label
    Protected WithEvents CHBresponsabile As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNmodifica As System.Web.UI.WebControls.Button


#End Region

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
            Me.SetCulture(Session("LinguaCode"))
            Me.SetupInternazionalizzazione()
        End If
        If Page.IsPostBack = False Then
            If Request.QueryString("CMNT_id") <> "" And Request.QueryString("PRSN_ID") <> "" Then
                Dim CMNT_ID, PRSN_ID As Integer
                Try
                    CMNT_ID = Request.QueryString("CMNT_id")
                    PRSN_ID = Request.QueryString("PRSN_ID")

                    If CMNT_ID > 0 And PRSN_ID > 0 Then
                        Me.HDN_cmntID.Value = CMNT_ID
                        Me.Bind_TipoRuolo(CMNT_ID)
                        Me.Bind_DatiUtente(CMNT_ID, PRSN_ID)
                    Else
                        Me.Setup_OnCloseScript()
                    End If
                Catch ex As Exception
                    Me.Setup_OnCloseScript()
                End Try
            Else
                Me.Setup_OnCloseScript()
            End If
        End If
    End Sub


    Private Sub Setup_OnCloseScript()
        Dim oScript As String

        oScript = "<script language=Javascript>" & vbCrLf
        oScript = oScript & "window.close();"
        oScript = oScript & "</script>" & vbCrLf
        If (Not Me.Page.ClientScript.IsClientScriptBlockRegistered("clientScriptClose")) Then
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScriptClose", oScript)
        End If
    End Sub

#Region "Bind_Dati"
    Public Sub Bind_TipoRuolo(ByVal CMNT_ID As Integer)
        Me.DDLruolo.Items.Clear()
        Try
            Dim oDataset As DataSet
            Dim i, Totale As Integer
            Dim oComunita As New COL_Comunita

            oComunita.Id = CMNT_ID
            If CMNT_ID = Session("IdComunita") Then
                oDataset = oComunita.RuoliAssociabiliByPersona(Session("objPersona").id, Main.FiltroRuoli.ForTipoComunita_NoGuest)
            Else
                oDataset = oComunita.RuoliAssociati(Session("LinguaID"), Main.FiltroRuoli.ForTipoComunita_NoGuest)
            End If

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
                    Me.DDLruolo.Items.Add(oListItem)
                Next
                Me.BTNmodifica.Enabled = True
            Else
                Me.DDLruolo.Items.Add(New ListItem("< nessun ruolo >", -1))
                Me.BTNmodifica.Enabled = False
            End If
        Catch ex As Exception
            Me.DDLruolo.Items.Add(New ListItem("< nessun ruolo >", -1))
            Me.BTNmodifica.Enabled = False
        End Try
        oResource.setDropDownList(Me.DDLruolo, -1)
    End Sub
    Private Sub Bind_DatiUtente(ByVal CMNT_ID As Integer, ByVal PRSN_ID As Integer)
        Dim oPersona As New COL_Persona
        Dim oRuoloComunita As New COL_RuoloPersonaComunita

        Try
            Me.LNBabilita.Visible = False
            Me.LNBattiva.Visible = False
            Me.LNBdisabilita.Visible = False

            oRuoloComunita.Estrai(CMNT_ID, PRSN_ID)
            If oRuoloComunita.Errore = Errori_Db.None Then
                If oRuoloComunita.Abilitato And oRuoloComunita.Attivato Then
                    Me.LNBdisabilita.Visible = True
                ElseIf Not oRuoloComunita.Abilitato And oRuoloComunita.Attivato Then
                    Me.LNBabilita.Visible = True
                ElseIf Not oRuoloComunita.Attivato Then
                    Me.LNBattiva.Visible = True
                End If
                Try
                    Me.DDLruolo.SelectedValue = oRuoloComunita.TipoRuolo.Id
                Catch ex As Exception

                End Try
                Me.CHBresponsabile.Checked = oRuoloComunita.isResponsabile
                Me.CHBresponsabile.Enabled = Not Me.CHBresponsabile.Checked


                Me.HDrlpc.Value = oRuoloComunita.Id
                Me.HDNprsnID.Value = PRSN_ID
                Me.HDNrlpc_Attivato.Value = oRuoloComunita.Attivato
                Me.HDNrlpc_Abilitato.Value = oRuoloComunita.Abilitato
            Else
                Me.BTNmodifica.Enabled = False
            End If

            oPersona.Id = PRSN_ID
            oPersona.Estrai(Session("LinguaID"))
            If oPersona.Errore = Errori_Db.None Then
                Me.LBNomeCognome.Text = oPersona.Cognome & " " & oPersona.Nome
                Me.BTNmodifica.Enabled = True
                Me.LNBabilita.Enabled = True
                Me.LNBattiva.Enabled = True
                Me.LNBdisabilita.Enabled = True
            Else
                Me.BTNmodifica.Enabled = False
                Me.LNBabilita.Enabled = False
                Me.LNBattiva.Enabled = False
                Me.LNBdisabilita.Enabled = False
            End If
            Me.LBcomunita.Text = COL_Comunita.EstraiNomeBylingua(CMNT_ID, Session("LinguaID"))
        Catch ex As Exception
            Me.BTNmodifica.Enabled = False
        End Try
    End Sub
#End Region

    Private Sub LNBabilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBabilita.Click
        Try
            Dim oTreeComunita As New COL_TreeComunita
            Dim oComunita As New COL_Comunita

            oComunita.Id = Me.HDN_cmntID.Value
            oComunita.AbilitaIscritti("," & Me.HDNprsnID.Value & ",")

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & Me.HDNprsnID.Value & "\"
            oTreeComunita.Nome = Me.HDNprsnID.Value & ".xml"
            oTreeComunita.CambiaAbilitazione(Me.HDN_cmntID.Value, True)
            Me.LNBdisabilita.Visible = True
            Me.LNBabilita.Visible = False
        Catch ex As Exception

        End Try
    End Sub
    Private Sub LNBdisabilita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBdisabilita.Click
        Try
            Dim oTreeComunita As New COL_TreeComunita
            Dim oComunita As New COL_Comunita

            oComunita.Id = Me.HDN_cmntID.Value
            oComunita.DisabilitaIscritti("," & Me.HDNprsnID.Value & ",")

            oTreeComunita.Directory = Server.MapPath(".\..\profili\") & Me.HDNprsnID.Value & "\"
            oTreeComunita.Nome = Me.HDNprsnID.Value & ".xml"
            oTreeComunita.CambiaAbilitazione(Me.HDN_cmntID.Value, False)

            Me.LNBdisabilita.Visible = False
            Me.LNBabilita.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LNBattiva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBattiva.Click
        Try
            Dim oTreeComunita As New COL_TreeComunita
            Dim oComunita As New COL_Comunita
            Dim oPersona As New COL_Persona
            Dim CMNT_ID, PRSN_ID As Integer

            CMNT_ID = Me.HDN_cmntID.Value
            PRSN_ID = Me.HDNprsnID.Value

            oComunita.Id = CMNT_ID
            oComunita.AttivaIscritto(PRSN_ID)

            If oComunita.Errore = Errori_Db.None Then
                Dim oResourceUtente As ResourceManager = New ResourceManager
                Me.LNBdisabilita.Visible = True
                Me.LNBattiva.Visible = False
                Me.LNBabilita.Visible = False

                oResourceUtente = Me.oResource
                Try
                    oPersona.Id = PRSN_ID
                    oPersona.Estrai(Session("LinguaID"))
                    If oPersona.Errore = Errori_Db.None Then
                       Dim oUtility As New OLDpageUtility(Me.Context)
						oComunita.MailAccettazione(oPersona, oUtility.LocalizedMail)

                        oResourceUtente.UserLanguages = oPersona.Lingua.Codice
                        oResourceUtente.ResourcesName = "pg_ModificaRuoloIscritto"
                        oResourceUtente.Folder_Level1 = "Comunita"
                        oResourceUtente.setCulture()
                    End If
                Catch ex As Exception

                End Try
                oTreeComunita.Directory = Server.MapPath(".\..\profili\") & PRSN_ID & "\"
                oTreeComunita.Nome = PRSN_ID & ".xml"
                oTreeComunita.CambiaAttivazione(CMNT_ID, True, oResourceUtente)
            End If


        Catch ex As Exception

        End Try
    End Sub


    Private Sub BTNmodifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNmodifica.Click
        Try
            Dim PRSN_ID, TPRL_ID As Integer
            Dim isAbilitato, isAttivato, isResponsabile As Boolean
            Dim oRuoloPersonaComunita As New COL_RuoloPersonaComunita

            TPRL_ID = Me.DDLruolo.SelectedItem.Value
            oRuoloPersonaComunita.Id = Me.HDrlpc.Value
            oRuoloPersonaComunita.TipoRuolo.Id = TPRL_ID
            oRuoloPersonaComunita.isResponsabile = CHBresponsabile.Checked
            oRuoloPersonaComunita.CambiaRuolo(Me.HDNprsnID.Value)

            If CBool(Me.HDNrlpc_Attivato.Value) Then
                isAttivato = CBool(Me.HDNrlpc_Attivato.Value)
            Else
                isAttivato = False
            End If
            If CBool(Me.HDNrlpc_Abilitato.Value) Then
                isAbilitato = CBool(Me.HDNrlpc_Abilitato.Value)
            Else
                isAbilitato = False
            End If
            If Me.CHBresponsabile.Checked = True Then
                isResponsabile = True
            Else
                isResponsabile = False
            End If
            PRSN_ID = Me.HDNprsnID.Value

        Catch ex As Exception

        End Try
        Me.Setup_OnCloseScript()
    End Sub

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = code
        oResource.ResourcesName = "pg_ModificaRuoloIscritto"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
        
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBabilitato_t)
            .setLabel(Me.LBanagrafica_t)
            .setLabel(Me.LBresponsabile_t)
            .setLabel(Me.LBruolo_t)
            .setLabel(Me.LBcomunita_t)
            .setLabel(Me.LBlegenda)
            .setButton(Me.BTNmodifica)
            .setLinkButton(Me.LNBabilita, False, False)
            .setLinkButton(Me.LNBattiva, False, False)
            .setLinkButton(Me.LNBdisabilita, False, False)
            .setCheckBox(Me.CHBresponsabile)
        End With
    End Sub
#End Region

End Class
