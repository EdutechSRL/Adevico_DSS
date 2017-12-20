Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi



Public Class AdminG_WizardModificaServizio
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Private _Utility As OLDpageUtility
    Private ReadOnly Property PageUtility As OLDpageUtility
        Get
            If IsNothing(_Utility) Then
                _Utility = New OLDpageUtility(Me.Context)
            End If
            Return _Utility
        End Get
    End Property
    Public Enum IndiceFasi
        Fase1_Dati = 0
        Fase2_SceltaPermessi = 1
        Fase3_SceltaTipiComunita = 2
        Fase4_DefinizionePermessi = 3
        Fase1_ModificaDati = 4
        Fase2_TranslatePermessi = 5
        Fase6_ConfermaDati = 6
    End Enum

    Protected WithEvents HDN_servizioID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLdati As Comunita_OnLine.UC_DatiServizio
    Protected WithEvents TBLtipoComunita As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLtipoComunita As Comunita_OnLine.UC_ServizioDefinisciTipoComunita
    'Protected WithEvents TBLruoliPermessi As System.Web.UI.WebControls.Table
    'Protected WithEvents CTRLruoliPermessi As Comunita_OnLine.UC_AdminServizioModificaDefinizionePermessi
    Protected WithEvents TBLconfermaPrincipali As System.Web.UI.WebControls.Table
    Protected WithEvents LBinfoConferma As System.Web.UI.WebControls.Label
    Protected WithEvents TBLsceltaPermessi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLsceltaPermessi As Comunita_OnLine.UC_SceltaPermessi
    Protected WithEvents TBLtraduzionePermessi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLtraduzionePermessi As Comunita_OnLine.UC_AdminServizioTraduzionePermessi


#Region "Navigazione"
    Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdefault As System.Web.UI.WebControls.Button
    Protected WithEvents BTNreplicaSuTutti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalvaDati As System.Web.UI.WebControls.Button


    Protected WithEvents PNLnavigazione2 As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNdefault2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNreplicaSuTutti2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalvaDati2 As System.Web.UI.WebControls.Button
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
        End If
        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            Dim oPersona As COL_Persona
            oPersona = Session("objPersona")

            Me.SetupInternazionalizzazione()
			If oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.AdminSecondario Then
				Me.PNLcontenuto.Visible = True
				Me.TBRmenu.Visible = False
				Me.PNLpermessi.Visible = False
				Me.PNLnavigazione.Visible = True
				Me.PNLnavigazione2.Visible = True
				Me.Bind_dati()
			Else
				Me.TBRmenu.Visible = True
				Me.PNLcontenuto.Visible = False
				Me.PNLnavigazione.Visible = False
				Me.PNLnavigazione2.Visible = False
				Me.PNLpermessi.Visible = True
			End If
        End If
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.Id > 0 Then
                isScaduta = False
                Return False
            End If
        Catch ex As Exception

        End Try
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            Response.End()
            Return True
        End If
    End Function

#Region "Internazionalizzazione"
    Private Sub SetCulture(ByVal Code As String)
        Me.oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AdminG_WizardServizio"
        oResource.Folder_Level1 = "Admin_Globale"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBNopermessi)
            .setLabel(Me.LBinfoConferma)
            .setButton(Me.BTNavanti, True, , , True)
            .setButton(Me.BTNconferma, True, , , True)
            .setButton(Me.BTNdefault, True, , , True)
            .setButton(Me.BTNelenco, True, , , True)
            .setButton(Me.BTNindietro, True, , , True)
            .setButton(Me.BTNreplicaSuTutti, True, , , True)
            .setButton(Me.BTNsalvaDati, True, , , True)

            .setButton(Me.BTNavanti2, True, , , True)
            .setButton(Me.BTNconferma2, True, , , True)
            .setButton(Me.BTNdefault2, True, , , True)
            .setButton(Me.BTNelenco2, True, , , True)
            .setButton(Me.BTNindietro2, True, , , True)
            .setButton(Me.BTNreplicaSuTutti2, True, , , True)
            .setButton(Me.BTNsalvaDati2, True, , , True)
            .setLinkButton(Me.LNBindietro, True, True)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub AggiornaNavigazione2()
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNconferma2.Enabled = Me.BTNconferma.Enabled
        Me.BTNdefault2.Visible = Me.BTNdefault.Visible
        Me.BTNdefault2.Enabled = Me.BTNdefault.Enabled
        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNelenco2.Enabled = Me.BTNelenco.Enabled
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNindietro2.Enabled = Me.BTNindietro.Enabled
        Me.BTNreplicaSuTutti2.Visible = Me.BTNreplicaSuTutti.Visible
        Me.BTNreplicaSuTutti2.Enabled = Me.BTNreplicaSuTutti.Enabled
        Me.BTNsalvaDati2.Visible = Me.BTNsalvaDati.Visible
        Me.BTNsalvaDati2.Enabled = Me.BTNsalvaDati.Enabled
    End Sub
    Private Sub Bind_dati()
        If Session("Azione") = "inserisci" Then
            Session("Azione_selezione") = ""
            Me.HDN_servizioID.Value = 0
        Else
            If InStr(Session("Azione"), "modifica_") > 0 Then
                Try
                    Me.HDN_servizioID.Value = Session("Azione")
                    Me.HDN_servizioID.Value = Me.HDN_servizioID.Value.Remove(0, 9)
                    If Me.HDN_servizioID.Value < 0 Then
                        Me.HDN_servizioID.Value = 0
                        Session("Azione") = "inserisci"
                        Session("Azione_selezione") = ""
                    End If
                Catch ex As Exception
                    Me.HDN_servizioID.Value = 0
                    Session("Azione") = "inserisci"
                    Session("Azione_selezione") = ""
                End Try
            Else
                Session("Azione_selezione") = ""
                Me.Response.Redirect("./AdminG_ManagementServizi.aspx")
            End If
        End If

        Me.CTRLdati.Setup_Controllo(Me.HDN_servizioID.Value)
        If Session("Azione_selezione") = "" Then
            Me.resetForm_ToInit(False)
        ElseIf Session("Azione_selezione") = "AssociaPermessi" Then
            Me.resetForm_ToSelezionePermessi()
        ElseIf Session("Azione_selezione") = "tipocomunita" Then
            Me.resetForm_ToSelezioneTipocomunita()
        ElseIf Session("Azione_selezione") = "DefinisciPermessi" Then
            ' Me.resetForm_ToDefinizionePermessiTipoComunita(True)
            PageUtility.RedirectToUrl("Admin_Globale/PortalPermissionManagement.aspx?IdModule=" & Me.HDN_servizioID.Value)
        Else
            Me.resetForm_ToInit(False)
        End If
        Me.AggiornaNavigazione2()
    End Sub
    Private Sub resetForm_HideAll()
        Me.TBLsceltaPermessi.Visible = False
        Me.TBLtraduzionePermessi.Visible = False
        Me.TBLdati.Visible = False
        '   Me.TBLruoliPermessi.Visible = False
        Me.TBLtipoComunita.Visible = False
        Me.TBLconfermaPrincipali.Visible = False
        Me.BTNconferma.Visible = False
        Me.BTNsalvaDati.Visible = False
        Me.BTNdefault.Visible = False
        Me.BTNreplicaSuTutti.Visible = False
    End Sub

    Private Sub resetForm_ToInit(ByVal Setup As Boolean)
        Dim iResponse As WizardServizio_Message
        Me.resetForm_HideAll()
        Me.TBLdati.Visible = True

        Me.BTNindietro.Visible = False
        Me.BTNconferma.Visible = False
        Me.BTNsalvaDati.Visible = True
        Me.BTNavanti.Visible = True
        Me.BTNelenco.Visible = True

        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(IndiceFasi.Fase1_ModificaDati, IndiceFasi))
        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(IndiceFasi.Fase1_ModificaDati, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        If Setup Then
            iResponse = Me.CTRLdati.Setup_Controllo(Me.HDN_servizioID.Value)
        End If
    End Sub

    Private Sub resetForm_ToSelezionePermessi()
        Try
            Me.resetForm_HideAll()

            Me.BTNindietro.Visible = True
            Me.BTNelenco.Visible = True
            Me.BTNavanti.Visible = True
            If Not Me.CTRLsceltaPermessi.isInizializzato Then
                Me.CTRLsceltaPermessi.Setup_Controllo(Me.HDN_servizioID.Value)
            End If
            Me.TBLsceltaPermessi.Visible = True
            Me.BTNavanti.Visible = Me.CTRLsceltaPermessi.HasPermessi
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(IndiceFasi.Fase2_SceltaPermessi, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(IndiceFasi.Fase2_SceltaPermessi, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToTraduzionePermessi()
        Try
            Me.resetForm_HideAll()
            Me.BTNsalvaDati.Visible = True
            Me.BTNindietro.Visible = True
            Me.BTNelenco.Visible = True
            Me.BTNavanti.Visible = True

            If Not Me.CTRLsceltaPermessi.isInizializzato Then
                Me.CTRLsceltaPermessi.Setup_Controllo(Me.HDN_servizioID.Value)
            End If
            If Me.CTRLtraduzionePermessi.isInizializzato Then
                Me.CTRLtraduzionePermessi.Update_Controllo(Me.HDN_servizioID.Value, Me.CTRLsceltaPermessi.PermessiSelezionati)
            Else
                Me.CTRLtraduzionePermessi.Setup_Controllo(Me.HDN_servizioID.Value, Me.CTRLsceltaPermessi.PermessiSelezionati)
            End If
            Me.TBLtraduzionePermessi.Visible = True
            Me.BTNavanti.Visible = Me.CTRLtraduzionePermessi.HasPermessi
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(IndiceFasi.Fase2_TranslatePermessi, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(IndiceFasi.Fase2_TranslatePermessi, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToSelezioneTipocomunita()
        Try

            Me.resetForm_HideAll()

            Me.BTNindietro.Visible = True
            Me.BTNavanti.Visible = True
            Me.BTNreplicaSuTutti.Visible = True
            Me.BTNsalvaDati.Visible = True
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(IndiceFasi.Fase3_SceltaTipiComunita, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(IndiceFasi.Fase3_SceltaTipiComunita, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

            Me.TBLtipoComunita.Visible = True
            If Me.CTRLtipoComunita.isInizializzato = False Then
                Me.CTRLtipoComunita.Setup_Controllo(Me.HDN_servizioID.Value)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToConfermaDatiPrincipali()
        Try

            Me.resetForm_HideAll()

            Me.BTNconferma.Visible = True
            Me.BTNindietro.Visible = True
            Me.BTNavanti.Visible = True
            Me.BTNsalvaDati.Visible = False

            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(IndiceFasi.Fase6_ConfermaDati, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(IndiceFasi.Fase6_ConfermaDati, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

            Me.TBLconfermaPrincipali.Visible = True
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub resetForm_ToDefinizionePermessiTipoComunita(ByVal forInit As Boolean)
    '    Try
    '        Me.resetForm_HideAll()
    '        Me.BTNindietro.Visible = True
    '        Me.BTNelenco.Visible = True
    '        Me.BTNavanti.Visible = False

    '        Me.BTNsalvaDati.Visible = True

    '        If forInit Then
    '            If Not Me.CTRLtipoComunita.isInizializzato Then
    '                Me.CTRLtipoComunita.Setup_Controllo(Me.HDN_servizioID.Value)
    '            End If
    '        End If

    '        If Not Me.CTRLruoliPermessi.isInizializzato Then
    '            Me.CTRLruoliPermessi.Setup_Controllo(Me.HDN_servizioID.Value)
    '        Else
    '            Me.CTRLruoliPermessi.Update_Controllo(Me.HDN_servizioID.Value)

    '        End If
    '        Me.BTNdefault.Visible = False
    '        Me.BTNreplicaSuTutti.Visible = False
    '        If Me.CTRLruoliPermessi.isDefinizionegenerale Then
    '            Me.BTNreplicaSuTutti.Visible = True
    '        Else
    '            Me.BTNdefault.Visible = True
    '        End If

    '        Me.TBLruoliPermessi.Visible = True
    '        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(IndiceFasi.Fase4_DefinizionePermessi, IndiceFasi))
    '        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
    '        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(IndiceFasi.Fase4_DefinizionePermessi, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

    '    Catch ex As Exception

    '    End Try
    'End Sub
#End Region

#Region "Navigazione Standard"
    Private Sub BTNelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNelenco.Click, LNBindietro.Click, BTNelenco2.Click
        Dim Link As String = "./AdminG_ManagementServizi.aspx?pageIndex="
        Dim pageIndex As Integer = 0

        Try
            pageIndex = Me.Request.QueryString("pageIndex")
        Catch ex As Exception
            pageIndex = 0
        End Try

        Me.Response.Redirect(Link & pageIndex)
    End Sub
    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click, BTNindietro2.Click
        Me.BTNconferma.Visible = False
        Me.BTNavanti.Visible = True
        Me.BTNdefault.Visible = False
        Me.BTNreplicaSuTutti.Visible = False
        Me.BTNsalvaDati.Visible = False
        Me.BTNavanti.Enabled = True

        If Me.TBLsceltaPermessi.Visible = True Then
            Me.resetForm_ToInit(False)
        ElseIf Me.TBLtraduzionePermessi.Visible = True Then
            Me.resetForm_ToSelezionePermessi()
        ElseIf Me.TBLtipoComunita.Visible = True Then
            Me.resetForm_ToTraduzionePermessi()
        ElseIf Me.TBLconfermaPrincipali.Visible Then ' Me.TBLruoliPermessi.Visible = True Or
            Me.resetForm_ToSelezioneTipocomunita()
        End If
        Me.AggiornaNavigazione2()
        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
        Me.Master.ServiceTitle = Me.CTRLdati.GetNomeServizio ' oResource.getValue("LBTitolo.text").Replace("##",)

    End Sub
    Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click, BTNavanti2.Click
        Dim alertMSG As String = ""

        Me.BTNconferma.Visible = False
        Me.BTNindietro.Visible = True

        Me.BTNdefault.Visible = False
        Me.BTNreplicaSuTutti.Visible = False
        Me.BTNsalvaDati.Visible = False

        If Me.TBLdati.Visible = True Then
            Me.Page.Validate()
            If Me.Page.IsValid Then
                Me.resetForm_ToSelezionePermessi()
            Else
                Me.BTNsalvaDati.Visible = True
            End If
        ElseIf Me.TBLsceltaPermessi.Visible Then
            If Me.CTRLsceltaPermessi.HasPermessiSelezionati Then
                Me.resetForm_ToTraduzionePermessi()
            Else
                alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(WizardServizio_Message.SelezionaPermesso, WizardServizio_Message))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                End If
                Me.BTNsalvaDati.Visible = True
            End If
        ElseIf Me.TBLtraduzionePermessi.Visible Then
            If Me.CTRLtraduzionePermessi.isDefaultLanguageDefinito Then
                Me.resetForm_ToSelezioneTipocomunita()
            Else
                alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(WizardServizio_Message.DefinireLinguaDefault, WizardServizio_Message))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                End If
                Me.BTNsalvaDati.Visible = True
            End If
        ElseIf Me.TBLtipoComunita.Visible Then
            If Me.CTRLtipoComunita.isDefinito Then
                Me.resetForm_ToConfermaDatiPrincipali()
            Else
                alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(WizardServizio_Message.SelezionaTipoComunita, WizardServizio_Message))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                End If

                Me.BTNsalvaDati.Visible = True
                Me.BTNreplicaSuTutti.Visible = Me.CTRLtipoComunita.isDefinizionegenerale
            End If
        ElseIf Me.TBLconfermaPrincipali.Visible = True Then
            If Me.CTRLtipoComunita.isDefinito Then
                'Me.resetForm_ToDefinizionePermessiTipoComunita(False)
                PageUtility.RedirectToUrl("Admin_Globale/PortalPermissionManagement.aspx?IdModule=" & Me.HDN_servizioID.Value)
            Else
                alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(WizardServizio_Message.SelezionaTipoComunita, WizardServizio_Message))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                End If
            End If
        End If
        Me.AggiornaNavigazione2()
        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
        Me.Master.ServiceTitle = Me.CTRLdati.GetNomeServizio 'oResource.getValue("LBTitolo.text").Replace("##", Me.CTRLdati.GetNomeServizio)

    End Sub
#End Region

    Private Sub CTRLtipoComunita_AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean) Handles CTRLtipoComunita.AggiornaMenuServizi
        Me.BTNdefault.Enabled = Abilitato
        Me.BTNreplicaSuTutti.Enabled = Abilitato
        Me.BTNdefault.Visible = Not AttivaDefault
        Me.BTNreplicaSuTutti.Visible = AttivaDefault

        Me.AggiornaNavigazione2()
    End Sub

    'Private Sub CTRLruoliPermessi_AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean) Handles CTRLruoliPermessi.AggiornaMenuServizi
    '    Me.BTNdefault.Enabled = Abilitato
    '    Me.BTNreplicaSuTutti.Enabled = Abilitato
    '    Me.BTNdefault.Visible = Not AttivaDefault
    '    Me.BTNreplicaSuTutti.Visible = AttivaDefault

    '    Me.AggiornaNavigazione2()
    'End Sub

    Private Sub BTNdefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNdefault.Click, BTNdefault2.Click
        Dim alertMSG As String = ""
        Dim iResponse As WizardServizio_Message
        Dim HasErrori As Boolean = False

        If Me.TBLtipoComunita.Visible Then
            iResponse = Me.CTRLtipoComunita.ModificaDati(True, True)
            If iResponse <> ModuloEnum.WizardServizio_Message.TipiComunitaAssociati Then
                HasErrori = True
            End If
            'ElseIf Me.TBLruoliPermessi.Visible Then
            '    iResponse = Me.CTRLruoliPermessi.ImpostaDefault
            '    If iResponse <> ModuloEnum.WizardServizio_Message.PermessiRuoliAssociati Then
            '        HasErrori = True
            '    End If
        End If

        If HasErrori Then
            alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardServizio_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        End If
    End Sub

    Private Sub BTNtipocomunitaForAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNreplicaSuTutti.Click, BTNreplicaSuTutti2.Click
        Dim alertMSG As String = ""
        Dim iResponse As WizardServizio_Message
        Dim HasErrori As Boolean = False

        If Me.TBLtipoComunita.Visible Then
            iResponse = Me.CTRLtipoComunita.ModificaDati(True, True)
            If iResponse <> ModuloEnum.WizardServizio_Message.TipiComunitaAssociati Then
                HasErrori = True
            End If
            'ElseIf Me.TBLruoliPermessi.Visible Then
            '    iResponse = Me.CTRLruoliPermessi.ReplicaModificaDati
            '    If iResponse <> ModuloEnum.WizardServizio_Message.PermessiRuoliAssociati Then
            '        HasErrori = True
            '    End If
        End If

        If HasErrori Then
            alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardServizio_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        End If
    End Sub

    Private Sub BTNsalvaDati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalvaDati.Click, BTNsalvaDati2.Click
        Dim alertMSG As String = ""
        Dim HasErrori As Boolean = False
        Dim iResponse As WizardServizio_Message

        If Me.TBLdati.Visible = True Then
            iResponse = Me.CTRLdati.Salva_Dati()
            alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardServizio_Message))
            If iResponse = ModuloEnum.WizardServizio_Message.Modificato Then
                Me.Response.Redirect("./AdminG_ManagementServizi.aspx")
            Else
                HasErrori = True
            End If
        ElseIf Me.TBLtraduzionePermessi.Visible Then
            iResponse = Me.CTRLsceltaPermessi.Salva_Dati(Me.HDN_servizioID.Value)
            If iResponse = ModuloEnum.WizardServizio_Message.OperazioneConclusa Then
                iResponse = Me.CTRLtraduzionePermessi.Salva_Dati(Me.HDN_servizioID.Value)
                If iResponse = ModuloEnum.WizardServizio_Message.OperazioneConclusa Then
                    Me.Response.Redirect("./AdminG_ManagementServizi.aspx")
                Else
                    HasErrori = True
                End If
            Else
                HasErrori = True
            End If
        ElseIf Me.TBLtipoComunita.Visible Then
            iResponse = Me.CTRLtipoComunita.ModificaDati(False, Me.CTRLtipoComunita.isDefinizionegenerale)
            If iResponse <> ModuloEnum.WizardServizio_Message.TipiComunitaAssociati Then
                HasErrori = True
            End If
            'ElseIf Me.TBLruoliPermessi.Visible Then
            '    iResponse = Me.CTRLruoliPermessi.ModificaDati()
            '    If iResponse <> ModuloEnum.WizardServizio_Message.PermessiRuoliAssociati Then
            '        HasErrori = True
            '    End If
        End If
        If HasErrori Then
            alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardServizio_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        End If
    End Sub
    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconferma.Click, BTNconferma2.Click
        Dim alertMSG As String = ""
        Dim iResponse As WizardServizio_Message
        Dim hasErrore As Boolean = False


        iResponse = Me.CTRLdati.Salva_Dati
        If iResponse = ModuloEnum.WizardServizio_Message.Modificato Then
            Me.HDN_servizioID.Value = Me.CTRLdati.ServizioID

            Dim Messaggio As String = ""
            Dim iResponseAssociaPermessi As WizardServizio_Message
            Dim iResponseTraduzioni As WizardServizio_Message
            Dim iResponseTipoComunita As WizardServizio_Message

            Messaggio = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardComunita_Message))


            iResponseAssociaPermessi = Me.CTRLsceltaPermessi.Salva_Dati(Me.CTRLdati.ServizioID)
            iResponseTraduzioni = Me.CTRLtraduzionePermessi.Salva_Dati(Me.CTRLdati.ServizioID)
            iResponseTipoComunita = Me.CTRLtipoComunita.ModificaDati(False, Me.CTRLtipoComunita.isDefinizionegenerale)

            If iResponseAssociaPermessi <> ModuloEnum.WizardServizio_Message.OperazioneConclusa Then
                Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseAssociaPermessi, WizardServizio_Message))
                hasErrore = True
            End If
            If iResponseTraduzioni <> ModuloEnum.WizardServizio_Message.OperazioneConclusa Then
                Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseTraduzioni, WizardServizio_Message))
                hasErrore = True
            End If
            If iResponseTipoComunita <> ModuloEnum.WizardServizio_Message.TipiComunitaAssociati Then
                Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseTipoComunita, WizardServizio_Message))
                hasErrore = True
            End If

            If Messaggio <> "" And hasErrore = True Then
                Messaggio = Messaggio.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & Messaggio & "');</script>")
            End If
        Else
            hasErrore = True
            alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardServizio_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            End If
        End If

        If Not hasErrore Then
            Session("azione") = "inserted"
            'Me.resetForm_ToDefinizionePermessiTipoComunita(False)
            PageUtility.RedirectToUrl("Admin_Globale/PortalPermissionManagement.aspx?IdModule=" & Me.HDN_servizioID.Value)
        End If
        Me.AggiornaNavigazione2()
    End Sub

    Public ReadOnly Property BodyId As String
        Get
            Return Me.Master.BodyIdCode
        End Get
    End Property
    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property

End Class