Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.CL_persona


Public Class activateFromMail
    Inherits System.Web.UI.Page
    Private oResource As ResourceManager

    Protected WithEvents PNLInfo As System.Web.UI.WebControls.Panel
    Protected WithEvents LBinfoAccesso As System.Web.UI.WebControls.Label
    Protected WithEvents BTNhome As System.Web.UI.WebControls.Button
    Protected WithEvents BTNchiudi As System.Web.UI.WebControls.Button

    Private Enum ErroreAccesso
        ParametriMancanti = -100
        PersonaInesistente = -101
        IscrizioneInesistente = -102
        AccessoAbilitato = -103
        ErroreAbilitazione = -104
        ErroreAttivazione = -105
        IscrizionePersonaInesistente = -106
        AccountAttivato = -107
        GiaAttivato = -108
        GiaAbilitato = -109
    End Enum

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
        Dim idPerson, idCommunity As Integer
        Dim oComunita As New COL_Comunita
        Dim oPersona As New COL_Persona

        If IsNothing(oResource) Then
            SetCulture(Session("LinguaCode"))
        End If

        If Page.IsPostBack = False Then
            Dim SessionID As String

            Me.SetupInternazionalizzazione()
			Dim oUtility As New OLDpageUtility(Me.Context)
            If Me.Request.QueryString("action") = "activate" Then
                Try
                    Dim AddCode, ExpUrl, ExpUrl2, Expfor, action As String

                    Try
                        AddCode = Me.Request.QueryString("AddCode")
                        If AddCode = "" Then
							oUtility.RedirectToDefault()
                        Else
                            idCommunity = AddCode.Remove(0, 8)
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        ExpUrl = Me.Request.QueryString("ExpUrl")
                        If ExpUrl = "" Then
							oUtility.RedirectToDefault()
                        Else
                            If ExpUrl.Chars(0) = "t" Then
                                idPerson = ExpUrl.Remove(0, 5)
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    ExpUrl2 = Me.Request.QueryString("ExpUrl2")
                    Try
                        If ExpUrl2 <> "" Then
                            If ExpUrl2.Chars(0) = "j" Then
                                SessionID = ExpUrl2.Remove(0, 6)
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    oComunita.Id = idCommunity
                    oPersona.ID = idPerson

                    oComunita.Estrai()
                    oPersona.Estrai(Session("LinguaID"))

                    If oPersona.Errore = Errori_Db.None And oComunita.Errore = Errori_Db.None Then
                        Dim oRuoloComunita As New COL_RuoloPersonaComunita

                        If oPersona.Lingua.Id <> Session("LinguaID") Then
                            SetCulture(oPersona.Lingua.Codice)
                            Me.SetupInternazionalizzazione()
                        End If

                        oRuoloComunita.EstraiByLinguaDefault(idCommunity, idPerson)
                        If oRuoloComunita.Errore = Errori_Db.None Then
                            If oRuoloComunita.Abilitato And oRuoloComunita.Attivato Then
                                ' è già iscritto !!!
                                If oComunita.Rimuovi_InAttesaConferma(SessionID, idPerson, idCommunity) Then
                                    Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.AccessoAbilitato)
                                Else
                                    Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.GiaAbilitato)
                                End If

                            Else
                                If oComunita.Rimuovi_InAttesaConferma(SessionID, idPerson, idCommunity) Then
                                    oComunita.AttivaIscritto(idPerson)
                                    If oComunita.Errore = Errori_Db.None Then
                                        lm.Comol.Core.DomainModel.Helpers.CacheHelper.PurgeCacheItems(lm.Comol.Core.BaseModules.CommunityManagement.Domain.CacheKeys.UserCommunitiesTree(idPerson))

                                        Dim oTreeComunita As New COL_TreeComunita
                                        oTreeComunita.Directory = Server.MapPath(".\profili\") & idPerson & "\"
                                        oTreeComunita.Nome = idPerson & ".xml"
                                        oTreeComunita.CambiaAttivazione(idCommunity, True, oResource)

                                        oComunita.MailAccettazione(oPersona, oUtility.LocalizedMail)
                                        Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.AccessoAbilitato)
                                    Else
                                        Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.ErroreAbilitazione)
                                    End If
                                Else
                                    Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.ErroreAbilitazione)
                                End If
                            End If
                        Else
                            Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.IscrizioneInesistente)
                        End If
                    Else
                        Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.PersonaInesistente)
                    End If
                Catch ex As Exception

                End Try
            Else
                Me.oResource.setLabel_To_Value(Me.LBinfoAccesso, "LBinfoAccesso." & Me.ErroreAccesso.ParametriMancanti)
            End If

            If Me.LBinfoAccesso.Text <> "" Then
				Me.LBinfoAccesso.Text = Replace(Me.LBinfoAccesso.Text, "%mailAdress%", oUtility.LocalizedMail.SystemSender.Address)
				Me.LBinfoAccesso.Text = Replace(Me.LBinfoAccesso.Text, "%mailSubject%", oUtility.LocalizedMail.SubjectPrefix)

                If oComunita.Id > 0 Then
                    Me.LBinfoAccesso.Text = Replace(Me.LBinfoAccesso.Text, "#nomecomunita#", oComunita.Nome)
                Else
                    Me.LBinfoAccesso.Text = Replace(Me.LBinfoAccesso.Text, "#nomecomunita#", "")
                End If

                If oPersona.Id > 0 Then
                    Me.LBinfoAccesso.Text = Replace(Me.LBinfoAccesso.Text, "#anagraficautente#", "<b>" & oPersona.Cognome & "</b>&nbsp;" & oPersona.Nome)
                Else
                    Me.LBinfoAccesso.Text = Replace(Me.LBinfoAccesso.Text, "#anagraficautente#", "")
                End If

            End If

        End If
        ' Me.SetStartupScript()
    End Sub

    Private Sub SetStartupScript()
        'Inserire qui il codice utente necessario per inizializzare la pagina
        Dim oScript As String

        oScript = "<script language=Javascript>" & vbCrLf
        oScript = oScript & "window.close();"
        oScript = oScript & "</script>" & vbCrLf
        If (Not Me.Page.ClientScript.IsClientScriptBlockRegistered("clientScript")) Then
            Me.Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "clientScript", oScript)
        End If
    End Sub

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_activateFromMail"
        oResource.Folder_Level1 = "Root"
        oResource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResource
            .setButton(Me.BTNchiudi)
            .setButton(Me.BTNhome)

            Me.BTNchiudi.Attributes.Add("onclick", "window.close();return false;")
        End With
    End Sub
#End Region


    '  Private Sub BTNhome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNhome.Click
    'Dim oUtility As New OLDpageUtility(Me.Context)
    'oUtility.RedirectToDefault()
    '  End Sub
End Class
