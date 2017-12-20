
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi



Public Class WizardCreaServizio
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
        Fase2_TranslatePermessi = 5
        Fase3_SceltaTipiComunita = 2
        Fase4_DefinizionePermessi = 3
        Fase5_ModificaDati = 4
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

    Protected WithEvents TBLsceltaPermessi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLsceltaPermessi As Comunita_OnLine.UC_SceltaPermessi
    Protected WithEvents TBLtraduzionePermessi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLtraduzionePermessi As Comunita_OnLine.UC_AdminServizioTraduzionePermessi

    Protected WithEvents TBLtipoComunita As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLtipoComunita As Comunita_OnLine.UC_ServizioDefinisciTipoComunita
    ' Protected WithEvents TBLruoliPermessi As System.Web.UI.WebControls.Table
    'Protected WithEvents CTRLruoliPermessi As Comunita_OnLine.UC_AdminServizioDefinizionePermessiGenerici

    Protected WithEvents TBLfinale As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLfinale As Comunita_OnLine.UC_AdminServizioConfermaFinale

    Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNcrea As System.Web.UI.WebControls.Button
    Protected WithEvents PNLnavigazione2 As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNcrea2 As System.Web.UI.WebControls.Button


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
				Me.Bind_dati()
			Else
				Me.TBRmenu.Visible = True
				Me.PNLcontenuto.Visible = False
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
            .setButton(Me.BTNavanti, True, , , True)
            .setButton(Me.BTNcrea, True, , , True)
            .setButton(Me.BTNelenco, True, , , True)
            .setButton(Me.BTNindietro, True, , , True)
            .setLinkButton(Me.LNBindietro, True, True)
            .setButton(Me.BTNavanti2, True, , , True)
            .setButton(Me.BTNcrea2, True, , , True)
            .setButton(Me.BTNelenco2, True, , , True)
            .setButton(Me.BTNindietro2, True, , , True)
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_dati()
        Session("Azione_selezione") = ""
        Me.HDN_servizioID.Value = 0
        Me.resetForm_ToInit(True)

        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNcrea2.Visible = Me.BTNcrea.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
    End Sub
    Private Sub resetForm_HideAll()
        Me.TBLsceltaPermessi.Visible = False
        Me.TBLtraduzionePermessi.Visible = False
        Me.TBLdati.Visible = False
        'Me.TBLruoliPermessi.Visible = False
        Me.TBLtipoComunita.Visible = False
        Me.TBLfinale.Visible = False
    End Sub
    Private Sub resetForm_ToInit(ByVal Setup As Boolean)
        Me.resetForm_HideAll()
        Me.TBLdati.Visible = True

        Me.BTNindietro.Visible = False
        Me.BTNcrea.Visible = False
        Me.BTNavanti.Visible = True
        Me.BTNelenco.Visible = True
        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi))
        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        If Setup Then
            Me.CTRLdati.Setup_Controllo(Me.HDN_servizioID.Value)
        End If
    End Sub
    Private Sub resetForm_ToSelezionePermessi()
        Try
            Me.resetForm_HideAll()
            Me.BTNcrea.Visible = False
            Me.BTNindietro.Visible = True
            Me.BTNelenco.Visible = True
            Me.BTNavanti.Visible = True
            If Not Me.CTRLsceltaPermessi.isInizializzato Then
                Me.CTRLsceltaPermessi.Setup_Controllo(Me.HDN_servizioID.Value)
            End If
            Me.TBLsceltaPermessi.Visible = True
            Me.BTNavanti.Visible = Me.CTRLsceltaPermessi.HasPermessi
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaPermessi, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaPermessi, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToTraduzionePermessi()
        Try
            Me.resetForm_HideAll()
            Me.BTNcrea.Visible = False
            Me.BTNindietro.Visible = True
            Me.BTNelenco.Visible = True
            Me.BTNavanti.Visible = True
            If Me.CTRLtraduzionePermessi.isInizializzato Then
                Me.CTRLtraduzionePermessi.Update_Controllo(Me.HDN_servizioID.Value, Me.CTRLsceltaPermessi.PermessiSelezionati)
            Else
                Me.CTRLtraduzionePermessi.Setup_Controllo(Me.HDN_servizioID.Value, Me.CTRLsceltaPermessi.PermessiSelezionati)
            End If
            Me.TBLtraduzionePermessi.Visible = True
            Me.BTNavanti.Visible = Me.CTRLtraduzionePermessi.HasPermessi
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_TranslatePermessi, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase2_TranslatePermessi, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToSelezioneTipocomunita()

        Try
            Me.resetForm_HideAll()

            Me.BTNcrea.Visible = False
            Me.BTNindietro.Visible = True
            Me.BTNavanti.Visible = True

            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase3_SceltaTipiComunita, IndiceFasi))
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase3_SceltaTipiComunita, IndiceFasi))

            Me.TBLtipoComunita.Visible = True
            If Me.CTRLtipoComunita.isInizializzato = False Then
                Me.CTRLtipoComunita.Setup_Controllo(Me.HDN_servizioID.Value)
            End If
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub resetForm_ToDefinizionePermessiTipoComunita()
    '    Try
    '        Me.resetForm_HideAll()
    '        Me.BTNcrea.Visible = False
    '        Me.BTNindietro.Visible = True
    '        Me.BTNelenco.Visible = True
    '        Me.BTNavanti.Visible = True
    '        If Not Me.CTRLruoliPermessi.isInizializzato Then
    '            If Not Me.CTRLtipoComunita.isInizializzato Then
    '                Me.CTRLtipoComunita.Setup_Controllo(Me.HDN_servizioID.Value)
    '            End If
    '            Me.CTRLruoliPermessi.Setup_Controllo(Me.HDN_servizioID.Value, Me.CTRLtipoComunita.ListaTipiComunitaSelezionati, Me.CTRLsceltaPermessi.PermessiSelezionati, Me.CTRLtraduzionePermessi.TraduzionePermessiDefault)
    '        Else
    '            Me.CTRLruoliPermessi.Update_Controllo(Me.HDN_servizioID.Value, Me.CTRLtipoComunita.ListaTipiComunitaSelezionati, Me.CTRLsceltaPermessi.PermessiSelezionati, Me.CTRLtraduzionePermessi.TraduzionePermessiDefault)
    '        End If

    '        Me.TBLruoliPermessi.Visible = True
    '        'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase4_DefinizionePermessi, IndiceFasi))
    '        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
    '        Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase4_DefinizionePermessi, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub resetForm_ToConfermaFinale()
        Try
            Me.resetForm_HideAll()
            Me.BTNavanti.Visible = False
            Me.BTNcrea.Visible = True
            Me.BTNindietro.Visible = True
            Me.BTNelenco.Visible = True
            Me.TBLfinale.Visible = True
            'Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase6_ConfermaDati, IndiceFasi))
            'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
            Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase6_ConfermaDati, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Navigazione Standard"
    Private Sub BTNelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNelenco.Click, LNBindietro.Click, BTNelenco2.Click
        Me.Response.Redirect("./AdminG_ManagementServizi.aspx")
    End Sub
    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click, BTNindietro2.Click
        If Me.TBLsceltaPermessi.Visible = True Then
            Me.Page.Validate()
            If Me.Page.IsValid Then
                Me.resetForm_ToInit(False)
            End If
        ElseIf Me.TBLtraduzionePermessi.Visible = True Then
            Me.resetForm_ToSelezionePermessi()
        ElseIf Me.TBLtipoComunita.Visible = True Then
            Me.resetForm_ToTraduzionePermessi()
            'ElseIf Me.TBLruoliPermessi.Visible = True Then
            '    Me.resetForm_ToSelezioneTipocomunita()
        ElseIf Me.TBLfinale.Visible Then
            Me.resetForm_ToSelezioneTipocomunita()
        End If
        Me.BTNcrea2.Visible = Me.BTNcrea.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
        '  Me.Master.ServiceTitle = oResource.getValue("LBTitolo.text").Replace("##", Me.CTRLdati.GetNomeServizio)

        'Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)
    End Sub
    Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click, BTNavanti2.Click
        Dim iResponse As ModuloEnum.WizardServizio_Message
        Dim alertMSG As String = ""

        If Me.TBLdati.Visible = True Then
            Me.Page.Validate()
            If Me.Page.IsValid Then
                Me.resetForm_ToSelezionePermessi()
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
            End If
        ElseIf Me.TBLtipoComunita.Visible Then
            If Me.CTRLtipoComunita.isDefinito Then
                resetForm_ToConfermaFinale()
            Else
                alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(WizardServizio_Message.SelezionaTipoComunita, WizardServizio_Message))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                End If
            End If
            'ElseIf Me.TBLruoliPermessi.Visible Then
            '    resetForm_ToConfermaFinale(rr)
        End If
        'Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
        'Me.Master.ServiceTitle = oResource.getValue("IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi)).Replace("##", Me.CTRLdati.GetNomeServizio)
        'Me.Master.ServiceTitle = oResource.getValue("LBTitolo.text").Replace("##", Me.CTRLdati.GetNomeServizio)

        Me.BTNcrea2.Visible = Me.BTNcrea.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
    End Sub
    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcrea.Click, BTNcrea2.Click
        Dim alertMSG As String = ""
        Dim iResponse As WizardServizio_Message

        If Session("azione") <> "inserted" Then
            iResponse = Me.CTRLdati.Salva_Dati()
            If iResponse = ModuloEnum.WizardServizio_Message.Creato Then
                Me.HDN_servizioID.Value = Me.CTRLdati.ServizioID

                Dim Messaggio As String = ""
                Dim iResponseAssociaPermessi As WizardServizio_Message
                Dim iResponseTraduzioni As WizardServizio_Message
                Dim iResponseTipoComunita As WizardServizio_Message
                '  Dim iResponsePermessi As WizardServizio_Message

                Messaggio = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardComunita_Message))


                iResponseAssociaPermessi = Me.CTRLsceltaPermessi.Salva_Dati(Me.CTRLdati.ServizioID)
                iResponseTraduzioni = Me.CTRLtraduzionePermessi.Salva_Dati(Me.CTRLdati.ServizioID)
                iResponseTipoComunita = Me.CTRLtipoComunita.SalvaDati(Me.CTRLdati.ServizioID)
                'iResponsePermessi = Me.CTRLruoliPermessi.Salva_Dati(Me.CTRLdati.ServizioID)

                Dim oServizio As New COL_Servizio
                oServizio.GeneraProfiloDefault(Me.HDN_servizioID.Value)
                If iResponseAssociaPermessi <> ModuloEnum.WizardServizio_Message.OperazioneConclusa Then
                    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseAssociaPermessi, WizardServizio_Message))
                End If
                If iResponseTraduzioni <> ModuloEnum.WizardServizio_Message.OperazioneConclusa Then
                    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseTraduzioni, WizardServizio_Message))
                End If
                If iResponseTipoComunita <> ModuloEnum.WizardServizio_Message.TipiComunitaAssociati Then
                    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseTipoComunita, WizardServizio_Message))
                End If
                'If iResponsePermessi <> ModuloEnum.WizardServizio_Message.PermessiRuoliAssociati Then
                '    Messaggio &= "\n" & Me.oResource.getValue("WizardComunita_Message." & CType(iResponseTipoComunita, WizardServizio_Message))
                'End If

                'Dim RichiediSSL As Boolean = False
                'Dim RichiediLoginSSL As Boolean = False
                'Dim Link As String
                'Try
                '    Dim oResourceConfig As New ResourceManager
                '    oResourceConfig = GetResourceConfig(Session("LinguaCode"))

                '    RichiediSSL = oResourceConfig.getValue("RichiediSSL")
                '    '  RichiediLoginSSL = oResourceConfig.getValue("RichiediLoginSSL")
                'Catch ex As Exception

                'End Try
                
                If Messaggio <> "" Then
                    Dim link As String = PageUtility.ApplicationUrlBase() & "/Admin_Globale/AdminG_ManagementServizi.aspx"
                    Messaggio = Messaggio.Replace("'", "\'")
                    Response.Write("<script language='javascript'>function AlertGoToPage(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertGoToPage('" & Messaggio & "','" & Link & "');</script>")
                Else
                    PageUtility.RedirectToUrl("Admin_Globale/PortalPermissionManagement.aspx?IdModule=" & Me.CTRLdati.ServizioID)
                End If
                Response.Expires = -1
                Response.ExpiresAbsolute = Now


            Else
                alertMSG = Me.oResource.getValue("WizardServizio_Message." & CType(iResponse, WizardServizio_Message))
                If alertMSG <> "" Then
                    alertMSG = alertMSG.Replace("'", "\'")
                    Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
                End If
            End If

            If iResponse = WizardServizio_Message.Creato Or iResponse = WizardServizio_Message.Modificato Or iResponse = WizardServizio_Message.ErroreAssociazioneLingue Then
                Session("azione") = "inserted"

                '        Me.BTNcrea.Visible = False
                '        Me.BTNindietro.Visible = True
                '        Me.BTNavanti.Visible = True
                '        Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaPermessi, IndiceFasi))
                '        Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeServizio)
                '        Me.TBLdati.Visible = False
                '        Me.TBLassociazionePermessi.Visible = True
                '        Me.CTRLassociazionePermessi.Setup_Controllo(Me.HDN_servizioID.Value, UC_PermessiServizio.Azione.AssociaPermessi)
            End If
        End If
    End Sub
#End Region

    Private Sub CTRLtipoComunita_AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean) Handles CTRLtipoComunita.AggiornaMenuServizi
        Me.BTNavanti.Enabled = Abilitato
        Me.BTNavanti2.Enabled = Abilitato
    End Sub

    'Private Sub CTRLruoliPermessi_AggiornaMenuServizi(ByVal AttivaDefault As Boolean, ByVal Abilitato As Boolean) Handles CTRLruoliPermessi.AggiornaMenuServizi
    '    Me.BTNavanti.Enabled = Abilitato
    '    Me.BTNavanti2.Enabled = Abilitato
    'End Sub

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