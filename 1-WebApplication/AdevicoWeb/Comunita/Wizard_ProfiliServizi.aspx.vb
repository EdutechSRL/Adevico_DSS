Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi



Public Class Wizard_ProfiliServizi
    Inherits System.Web.UI.Page

    Private oResource As ResourceManager

    Public Enum IndiceFasi
        Fase1_Dati = 0
        Fase2_SceltaRuoli = 1
        Fase3_SceltaServizi = 2
        Fase4_DefinizionePermessi = 3
        Fase5_ModificaDati = 4
    End Enum


    Protected WithEvents HDN_profiloID As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents HDNazione As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents TBRmenu As System.Web.UI.WebControls.TableRow
    Protected WithEvents LNBindietro As System.Web.UI.WebControls.LinkButton

    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel

    Protected WithEvents TBLdati As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLdati As Comunita_OnLine.UC_DatiProfilo
    Protected WithEvents TBLassociazioneRuoli As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLruoli As Comunita_OnLine.UC_ProfiloRuoli
    Protected WithEvents TBLservizi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLservizi As Comunita_OnLine.UC_ProfiloServizi
    Protected WithEvents TBLruoliPermessi As System.Web.UI.WebControls.Table
    Protected WithEvents CTRLruoliPermessi As Comunita_OnLine.UC_ProfiloPermessi
    Protected WithEvents PNLnavigazione As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalva As System.Web.UI.WebControls.Button
    Protected WithEvents PNLnavigazione2 As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNelenco2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNindietro2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNavanti2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNconferma2 As System.Web.UI.WebControls.Button
    Protected WithEvents BTNsalva2 As System.Web.UI.WebControls.Button

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
            If oPersona.TipoPersona.id <> Main.TipoPersonaStandard.Guest Then
                Me.PNLcontenuto.Visible = True
                Me.TBRmenu.Visible = False
                Me.PNLpermessi.Visible = False
                Me.Bind_dati()
            Else
                Me.TBRmenu.Visible = True
                Me.PNLcontenuto.Visible = False
                Me.PNLpermessi.Visible = True
                Me.PNLnavigazione2.Visible = False
                Me.PNLnavigazione.Visible = False
            End If
        End If

        Me.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultButton = Me.BTNavanti.UniqueID
        Me.Master.Page.Form.DefaultFocus = Me.BTNavanti.UniqueID
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
        oResource.ResourcesName = "pg_Wizard_ProfiliServizi"
        oResource.Folder_Level1 = "Comunita"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(Me.LBNopermessi)
            .setButton(Me.BTNavanti, True, , , True)
            .setButton(Me.BTNconferma, True, , , True)
            .setButton(Me.BTNsalva, True, , , True)
            .setButton(Me.BTNavanti2, True, , , True)
            .setButton(Me.BTNconferma2, True, , , True)
            .setButton(Me.BTNsalva2, True, , , True)
            .setLinkButton(Me.LNBindietro, True, True)
            If Session("Azione_from") = "creaComunita" Then
                .setButtonByValue(Me.BTNelenco, "fromComunita", True, , , True)
                .setButtonByValue(Me.BTNindietro, "fromComunita", True, , , True)

                .setButtonByValue(Me.BTNelenco2, "fromComunita", True, , , True)
                .setButtonByValue(Me.BTNindietro2, "fromComunita", True, , , True)
            Else
                .setButton(Me.BTNelenco, True, , , True)
                .setButton(Me.BTNindietro, True, , , True)
                .setButton(Me.BTNelenco2, True, , , True)
                .setButton(Me.BTNindietro2, True, , , True)
            End If
        End With
    End Sub
#End Region

#Region "Bind_Dati"
    Private Sub Bind_dati()
        If Session("Azione") = "inserisci" Then
            Session("Azione_selezione") = ""
            Me.HDN_profiloID.Value = 0
        Else
            If InStr(Session("Azione"), "modifica_") > 0 Then
                Try
                    Me.HDN_profiloID.Value = Session("Azione")
                    Me.HDN_profiloID.Value = Me.HDN_profiloID.Value.Remove(0, 9)
                    If Me.HDN_profiloID.Value < 0 Then
                        Me.HDN_profiloID.Value = 0
                        Session("Azione") = "inserisci"
                        Session("Azione_selezione") = ""
                    Else
                        Dim oProfilo As New COL_ProfiloServizio
                        oProfilo.Id = Me.HDN_profiloID.Value
                        oProfilo.Estrai()
                        If oProfilo.Errore <> Errori_Db.None Then
                            Me.HDN_profiloID.Value = 0
                            Session("Azione") = "inserisci"
                            Session("Azione_selezione") = ""
                        End If
                    End If
                Catch ex As Exception
                    Me.HDN_profiloID.Value = 0
                    Session("Azione") = "inserisci"
                    Session("Azione_selezione") = ""
                End Try
            Else
                Session("Azione_selezione") = ""
                Me.Response.Redirect("./Management_ProfiliServizi.aspx")
            End If
        End If
        If Session("Azione_selezione") = "" Then
            Me.resetForm_ToInit()
        ElseIf Session("Azione_selezione") = "AssociaRuoli" Then
            Me.resetForm_ToAssociaRuoli()
        ElseIf Session("Azione_selezione") = "AssociaServizi" Then
            Me.resetForm_ToAssociaServizi()
        ElseIf Session("Azione_selezione") = "DefinisciPermessi" Then
            Me.resetForm_ToDefinizionePermessi()
        Else
            Me.resetForm_ToInit()
        End If

    End Sub
    Private Sub resetForm_HideAll()
        Me.TBLassociazioneRuoli.Visible = False
        Me.TBLdati.Visible = False
        Me.TBLruoliPermessi.Visible = False
        Me.TBLservizi.Visible = False
    End Sub
    Private Sub resetForm_ToInit()
        Me.resetForm_HideAll()
        Me.TBLdati.Visible = True

        Me.BTNindietro.Visible = False
        Me.BTNconferma.Visible = False
        Me.BTNavanti.Visible = False
        Me.BTNelenco.Visible = True
        Me.BTNsalva.Visible = False

        Me.BTNindietro2.Visible = False
        Me.BTNconferma2.Visible = False
        Me.BTNavanti2.Visible = False
        Me.BTNelenco2.Visible = True
        Me.BTNsalva2.Visible = False

        If Me.HDN_profiloID.Value = 0 Then
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase1_Dati, IndiceFasi))
            Me.BTNconferma.Visible = True
            Me.BTNconferma2.Visible = True
        Else
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase5_ModificaDati, IndiceFasi))
            Me.BTNavanti.Visible = True
            Me.BTNsalva.Visible = True
            Me.BTNavanti2.Visible = True
            Me.BTNsalva2.Visible = True
        End If
        Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
        Me.CTRLdati.Setup_Controllo(Me.HDN_profiloID.Value)
    End Sub
    Private Sub resetForm_ToAssociaRuoli()
        Try
            If Me.HDN_profiloID.Value > 0 Then
                Me.CTRLdati.Setup_Controllo(Me.HDN_profiloID.Value)

                Me.resetForm_HideAll()
                Me.BTNconferma.Visible = False
                Me.BTNconferma2.Visible = False
                Me.BTNindietro.Visible = True
                Me.BTNsalva.Visible = True
                Me.BTNelenco.Visible = True
                Me.BTNavanti.Visible = True
                Me.BTNindietro2.Visible = True
                Me.BTNsalva2.Visible = True
                Me.BTNelenco2.Visible = True
                Me.BTNavanti2.Visible = True


                Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaRuoli, IndiceFasi))
                Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
                Me.TBLassociazioneRuoli.Visible = True
                Me.CTRLruoli.Setup_Controllo(Me.HDN_profiloID.Value)
                Me.BTNavanti.Visible = Me.CTRLruoli.HasRuoliSelezionati And Me.CTRLruoli.isDefinito
                Me.BTNavanti2.Visible = Me.BTNavanti.Visible
            Else
                Me.resetForm_ToInit()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToAssociaServizi()
        Try
            If Me.HDN_profiloID.Value > 0 Then
                Me.CTRLdati.Setup_Controllo(Me.HDN_profiloID.Value)

                Me.resetForm_HideAll()
                Me.BTNconferma.Visible = False
                Me.BTNconferma2.Visible = False
                Me.BTNindietro.Visible = True
                Me.BTNsalva.Visible = True
                Me.BTNelenco.Visible = True
                Me.BTNindietro2.Visible = True
                Me.BTNsalva2.Visible = True
                Me.BTNelenco2.Visible = True


                Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase3_SceltaServizi, IndiceFasi))
                Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
                Me.TBLservizi.Visible = True
                Me.CTRLservizi.Setup_Controllo(Me.HDN_profiloID.Value)
                Me.BTNavanti.Visible = Me.CTRLservizi.isDefinito And Me.CTRLservizi.hasAssociati
                Me.BTNavanti2.Visible = Me.BTNavanti.Visible
            Else
                Me.resetForm_ToInit()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub resetForm_ToDefinizionePermessi()
        Try
            If Me.HDN_profiloID.Value > 0 Then
                Me.CTRLdati.Setup_Controllo(Me.HDN_profiloID.Value)

                Me.resetForm_HideAll()
                Me.BTNconferma.Visible = False
                Me.BTNconferma2.Visible = False
                Me.BTNindietro.Visible = True
                Me.BTNsalva.Visible = True
                Me.BTNelenco.Visible = True
                Me.BTNavanti.Visible = False
                Me.BTNindietro2.Visible = True
                Me.BTNsalva2.Visible = True
                Me.BTNelenco2.Visible = True
                Me.BTNavanti2.Visible = False

                Me.TBLruoliPermessi.Visible = True
                Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase4_DefinizionePermessi, IndiceFasi))
                Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
                Me.CTRLruoliPermessi.Setup_Controllo(Me.HDN_profiloID.Value)
            Else
                Me.resetForm_ToInit()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

    Private Sub ReturnToPreviousPage()
        Session("Azione") = ""
        Session("Azione_selezione") = ""
        If Session("Azione_from") = "creaComunita" Then
            '' Redirect sulla pagina di creazione comunità
        Else
            Me.Response.Redirect("./Management_ProfiliServizi.aspx")
        End If
    End Sub

    Private Sub LNBindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBindietro.Click
        Me.ReturnToPreviousPage()
    End Sub

    Private Sub BTNconferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNconferma.Click, BTNconferma2.Click
        Dim alertMSG As String = ""
        Dim iResponse As WizardProfilo_Message

        If Session("azione") <> "inserted" Then
            iResponse = Me.CTRLdati.Salva_Dati()
            Me.HDN_profiloID.Value = Me.CTRLdati.ProfiloID

            alertMSG = Me.oResource.getValue("WizardProfilo_Message." & CType(iResponse, WizardProfilo_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = WizardProfilo_Message.Creato Or iResponse = WizardProfilo_Message.Modificato Then
                Me.BTNconferma.Visible = False
                Me.BTNconferma2.Visible = False
                Me.BTNindietro.Visible = True
                Me.BTNavanti.Visible = True
                Me.BTNindietro2.Visible = True
                Me.BTNavanti2.Visible = True
                Me.BTNsalva.Visible = True
                Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaRuoli, IndiceFasi))
                Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
                Me.TBLdati.Visible = False
                Me.TBLassociazioneRuoli.Visible = True

                iResponse = Me.CTRLruoli.Setup_Controllo(Me.HDN_profiloID.Value)

                If iResponse = ModuloEnum.WizardProfilo_Message.ProfiloNonTrovato Then
                    Me.BTNsalva.Enabled = False
                Else
                    Me.BTNsalva.Enabled = True
                End If
                Me.BTNavanti.Enabled = Me.CTRLruoli.HasRuoliSelezionati And Me.CTRLruoli.isDefinito

                Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
                Me.BTNsalva2.Visible = Me.BTNsalva.Visible
                Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
                Session("azione") = "inserted"
            End If
            End If
    End Sub

    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click, BTNindietro2.Click
        Dim iResponse As ModuloEnum.WizardProfilo_Message
        Me.BTNavanti.Visible = True
        Me.BTNconferma.Visible = False
        Me.BTNelenco.Visible = True
        Me.BTNsalva.Visible = True
        Me.BTNsalva.Enabled = True

        If Me.TBLassociazioneRuoli.Visible = True Then
            Me.resetForm_HideAll()
            Me.BTNindietro.Visible = False
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase5_ModificaDati, IndiceFasi))
            Me.TBLdati.Visible = True
            Me.BTNavanti.Enabled = True
            Me.CTRLdati.Setup_Controllo(Me.HDN_profiloID.Value)
        ElseIf Me.TBLservizi.Visible Then
            Me.resetForm_HideAll()
            Me.BTNindietro.Visible = True
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaRuoli, IndiceFasi))
            Me.TBLassociazioneRuoli.Visible = True
            iResponse = Me.CTRLruoli.Setup_Controllo(Me.HDN_profiloID.Value)
            If iResponse = ModuloEnum.WizardProfilo_Message.OperazioneConclusa Then
                Me.BTNavanti.Visible = True
                Me.BTNavanti.Enabled = Me.CTRLruoli.HasRuoliSelezionati And Me.CTRLruoli.isDefinito
            Else
                Me.BTNavanti.Visible = False
            End If
        ElseIf Me.TBLruoliPermessi.Visible Then
            Me.resetForm_HideAll()
            Me.BTNindietro.Visible = True
            Me.BTNavanti.Visible = True
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase3_SceltaServizi, IndiceFasi))
            Me.TBLservizi.Visible = True
            iResponse = Me.CTRLservizi.Setup_Controllo(Me.HDN_profiloID.Value)
            If iResponse = ModuloEnum.WizardProfilo_Message.OperazioneConclusa Then
                Me.BTNavanti.Visible = True
                Me.BTNavanti.Enabled = Me.CTRLservizi.isDefinito And Me.CTRLservizi.hasAssociati
            Else
                Me.BTNavanti.Visible = False
            End If
        End If
        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
        Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
    End Sub

    Private Sub BTNavanti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNavanti.Click, BTNavanti2.Click
        Dim iResponse As ModuloEnum.WizardProfilo_Message
        Dim alertMSG As String = ""

        Me.BTNavanti.Visible = True
        Me.BTNconferma.Visible = False
        Me.BTNelenco.Visible = True
        Me.BTNsalva.Visible = False
        Me.BTNsalva.Enabled = True

        If Me.TBLdati.Visible = True Then
            If Me.HDN_profiloID.Value <> "0" Then
                Me.resetForm_HideAll()
                Me.BTNindietro.Visible = True
                Me.BTNsalva.Visible = True
                Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase2_SceltaRuoli, IndiceFasi))
                Me.TBLassociazioneRuoli.Visible = True
                iResponse = Me.CTRLruoli.Setup_Controllo(Me.HDN_profiloID.Value)
                If iResponse = ModuloEnum.WizardProfilo_Message.OperazioneConclusa Then
                    Me.BTNsalva.Visible = True
                    Me.BTNsalva.Enabled = True
                    Me.BTNavanti.Visible = True
                    Me.BTNavanti.Enabled = Me.CTRLruoli.HasRuoliSelezionati And Me.CTRLruoli.isDefinito
                Else
                    Me.BTNsalva.Enabled = False
                    Me.BTNavanti.Visible = False
                End If
            End If
        ElseIf Me.TBLassociazioneRuoli.Visible = True Then
            Me.resetForm_HideAll()
            Me.BTNindietro.Visible = True

            Me.BTNsalva.Visible = True
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase3_SceltaServizi, IndiceFasi))
            Me.TBLservizi.Visible = True
            iResponse = Me.CTRLservizi.Setup_Controllo(Me.HDN_profiloID.Value)
            If iResponse = ModuloEnum.WizardProfilo_Message.OperazioneConclusa Then
                Me.BTNavanti.Visible = True
                Me.BTNavanti.Enabled = Me.CTRLservizi.hasAssociati And Me.CTRLservizi.isDefinito
            Else
                Me.BTNavanti.Visible = False
            End If
        ElseIf Me.TBLservizi.Visible Then
            Me.resetForm_HideAll()
            Me.BTNindietro.Visible = True
            Me.BTNavanti.Visible = False
            Me.BTNsalva.Visible = True
            Me.oResource.setLabel_To_Value(Me.LBTitolo, "IndiceFasi." & CType(Me.IndiceFasi.Fase4_DefinizionePermessi, IndiceFasi))
            Me.TBLruoliPermessi.Visible = True
            iResponse = Me.CTRLruoliPermessi.Setup_Controllo(Me.HDN_profiloID.Value)
            If iResponse = ModuloEnum.WizardProfilo_Message.OperazioneConclusa Then
                Me.BTNsalva.Enabled = True
            Else
                Me.BTNsalva.Enabled = False
            End If
        End If

        Me.BTNindietro2.Visible = Me.BTNindietro.Visible
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
        Me.BTNconferma2.Visible = Me.BTNconferma.Visible
        Me.BTNelenco2.Visible = Me.BTNelenco.Visible
        Me.BTNsalva2.Visible = Me.BTNsalva.Visible
        Me.BTNsalva2.Enabled = Me.BTNsalva.Enabled
        Me.LBTitolo.Text = Replace(Me.LBTitolo.Text, "##", Me.CTRLdati.GetNomeProfilo)
    End Sub

    Private Sub BTNsalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNsalva.Click, BTNsalva2.Click
        Dim iResponse As ModuloEnum.WizardProfilo_Message
        Dim alertMSG As String = ""

        If Me.TBLdati.Visible Then
            iResponse = Me.CTRLdati.Salva_Dati()

            alertMSG = Me.oResource.getValue("WizardProfilo_Message." & CType(iResponse, WizardProfilo_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = ModuloEnum.WizardProfilo_Message.Creato Or iResponse.Creato Then
                Me.BTNavanti.Visible = True
                Me.BTNavanti.Enabled = True
            End If
        ElseIf Me.TBLassociazioneRuoli.Visible Then
            iResponse = Me.CTRLruoli.Salva_Dati()

            alertMSG = Me.oResource.getValue("WizardProfilo_Message." & CType(iResponse, WizardProfilo_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = ModuloEnum.WizardProfilo_Message.RuoliAssociati Then
                Me.BTNavanti.Enabled = True
            Else
                Me.BTNavanti.Enabled = Me.CTRLruoli.HasRuoliSelezionati And Me.CTRLruoli.isDefinito
            End If
        ElseIf Me.TBLservizi.Visible Then
            iResponse = Me.CTRLservizi.Salva_Dati()
            alertMSG = Me.oResource.getValue("WizardProfilo_Message." & CType(iResponse, WizardProfilo_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
            If iResponse = ModuloEnum.WizardProfilo_Message.ServiziAssociati Then
                Me.BTNavanti.Enabled = True
            Else
                Me.BTNavanti.Enabled = Me.CTRLservizi.hasAssociati And Me.CTRLservizi.isDefinito
            End If
        ElseIf Me.TBLruoliPermessi.Visible Then
            iResponse = Me.CTRLruoliPermessi.SalvaDati(True)
            alertMSG = Me.oResource.getValue("WizardProfilo_Message." & CType(iResponse, WizardProfilo_Message))
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
                Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            End If
        End If
        Me.BTNavanti2.Visible = Me.BTNavanti.Visible
        Me.BTNavanti2.Enabled = Me.BTNavanti.Enabled
    End Sub

    Private Sub BTNelenco_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNelenco.Click, BTNelenco2.Click
        Me.ReturnToPreviousPage()
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class