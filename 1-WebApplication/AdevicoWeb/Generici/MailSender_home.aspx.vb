Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2ServiziBase.ContattiMail
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Mail
Imports Comunita_OnLine.ModuloGenerale
Imports lm.ActionDataContract
Imports lm.Comol.Core.File


Public Class MailSender_home
    Inherits System.Web.UI.Page
    Private _Resource As ResourceManager
    Private _PageUtility As PresentationLayer.OLDpageUtility

    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
    Private ReadOnly Property oResource() As ResourceManager
        Get
            If IsNothing(_Resource) Then
                _Resource = New ResourceManager
                _Resource.UserLanguages = Me.PageUtility.LinguaCode
                _Resource.ResourcesName = "pg_MailSender"
                _Resource.Folder_Level1 = "Generici"
                _Resource.Folder_Level2 = ""
                _Resource.setCulture()
            End If
            Return _Resource
        End Get
    End Property

    Private Enum Avviso
        zeroByte = 0
        Esiste = 1
        Uploaded = 2
        OverByte = 3
    End Enum

    'Private Delegate Sub oGestore(ByVal Param As String)

    'Impedisce l'invio delle mail quando è a True!
    Private ReadOnly Property DebugMode() As Boolean
        Get
            Return False
        End Get
    End Property

#Region "Oggetti WEB"
    Protected WithEvents CTRLsorgenteComunita As Comunita_OnLine.UC_FiltroComunitaByServizio_NEW

    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
    Protected WithEvents TBLContenuto As System.Web.UI.WebControls.Table

    Protected WithEvents LBcomunita_t As System.Web.UI.WebControls.Label

    Protected WithEvents TBRfrom As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBfrom As System.Web.UI.WebControls.Label
    Protected WithEvents TXBFrom As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRa As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBa As System.Web.UI.WebControls.Label

    Protected WithEvents TXBa As System.Web.UI.WebControls.TextBox
    Protected WithEvents TXBa_n As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TXBa_n_rlpc As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents LNBMostraCC As System.Web.UI.WebControls.LinkButton

    Protected WithEvents TBRcc As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBcc As System.Web.UI.WebControls.Label
    Protected WithEvents TXBcc As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRccn As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBccn As System.Web.UI.WebControls.Label

    Protected WithEvents IMBrubricaCCN As System.Web.UI.WebControls.ImageButton
    Protected WithEvents IMBrubrica As System.Web.UI.WebControls.ImageButton

    Protected WithEvents TXBccn As System.Web.UI.WebControls.TextBox
    Protected WithEvents TBRrubrica As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBaRubrica As System.Web.UI.WebControls.Label

    Protected WithEvents PNLrubrica As System.Web.UI.WebControls.Panel
    Protected WithEvents CTRLrubrica As Comunita_OnLine.UC_RubricaMail_NEW
    Protected WithEvents BTNcloseRubrica As System.Web.UI.WebControls.Button
    Protected WithEvents HDNtotaleDestinatari As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBRobj As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBobj As System.Web.UI.WebControls.Label
    Protected WithEvents TXBObj As System.Web.UI.WebControls.TextBox
    'Protected WithEvents RFVBody As System.Web.UI.WebControls.RequiredFieldValidator

    Protected WithEvents LNBMostraAttach As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TBRattach1 As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBattach As System.Web.UI.WebControls.Label
    Protected WithEvents fileAllega As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents BTallega As System.Web.UI.WebControls.Button
    Protected WithEvents BTN_DelAllAttach As System.Web.UI.WebControls.Button

    Protected WithEvents TBRattach2 As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBFrom_att As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TBLattach As System.Web.UI.WebControls.Table
    Protected WithEvents TXBnascosto As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBRbody As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBbody As System.Web.UI.WebControls.Label
    Protected WithEvents TXBbody As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRinvia As System.Web.UI.WebControls.TableRow
    Protected WithEvents CBXcopiamittente As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBXricezione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNinvia As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents PNLinviato As System.Web.UI.WebControls.Panel
    Protected WithEvents LBinviata As System.Web.UI.WebControls.Label
    Protected WithEvents BTNok As System.Web.UI.WebControls.Button

    Protected WithEvents PNLerrore As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNokerrore As System.Web.UI.WebControls.Button

    Protected WithEvents LBLInfoA As System.Web.UI.WebControls.Label
    Protected WithEvents LBLInfoJava As System.Web.UI.WebControls.Label

    Protected WithEvents LBLErroreAttach As System.Web.UI.WebControls.Label
    Protected WithEvents LBerrore As System.Web.UI.WebControls.Label

    Protected WithEvents LbErroreInvio As System.Web.UI.WebControls.Label

    Protected WithEvents LBInfoCom As System.Web.UI.WebControls.Label
    Protected WithEvents LBLInfoComJava As System.Web.UI.WebControls.Label

    Protected WithEvents REVvalid As System.Web.UI.WebControls.RegularExpressionValidator

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

        'If Not Page.IsPostBack Then
        '    Me.SetupInternazionalizzazione()
        'End If

        If Me.SessioneScaduta() Then
            Exit Sub
        End If

        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")

        Catch ex As Exception

        End Try

        If Page.IsPostBack = False Then
            'Dim oDelegate As New oGestore(AddressOf Conta)
            'If Session("DelegateTest") = "" Then    'impedisco che partano più thread...
            '    Session("DelegateTest") = "Started"
            '    'invoco il metodo
            '    oDelegate.BeginInvoke("Parametro", New AsyncCallback(AddressOf Me.FunzioneCallback), oDelegate)
            'End If

            SetMailGUID()

            ' REGISTRAZIONE EVENTO
            Session("azione") = "loaded"

            Me.SetupInternazionalizzazione()
            Dim oServizio As New UCServices.Services_Mail
            Me.CTRLsorgenteComunita.ServizioCode = UCServices.Services_Mail.Codex
            Me.CTRLsorgenteComunita.SetupControl()
            Me.CTRLsorgenteComunita.Visible = True
            Dim PermessiAssociati As String

            Try
                'If Me.CTRLsorgenteComunita.HasComunita Then
                If Me.CTRLsorgenteComunita.ComunitaID > 0 Then
                    Dim i_link As String
                    i_link = "./Attachments.aspx?CMNT_ID=" & Me.CTRLsorgenteComunita.ComunitaID
                    Me.BTallega.Enabled = True
                    Me.IMBrubrica.Visible = True
                    Me.IMBrubrica.Enabled = True

                    Me.IMBrubricaCCN.Visible = True
                    Me.IMBrubricaCCN.Visible = True

                    Me.BTNinvia.Visible = True
                Else
                    Me.IMBrubrica.Visible = False
                    Me.IMBrubrica.Enabled = False
                    Me.IMBrubricaCCN.Visible = False
                    Me.IMBrubricaCCN.Enabled = False

                    Me.BTallega.Enabled = True

                    Me.BTNinvia.Visible = False
                End If

                TXBFrom.Text = oPersona.Nome & " " & oPersona.Cognome
                Me.PNLpermessi.Visible = False
                'Else
                '	Me.PNLcontenuto.Visible = False

                '	Me.PNLinviato.Visible = False
                '	Me.PNLerrore.Visible = False
                '	Me.PNLpermessi.Visible = True
                'End If
            Catch ex As Exception
                Me.PNLcontenuto.Visible = False
                Me.PNLinviato.Visible = False
                Me.PNLerrore.Visible = False
                Me.PNLpermessi.Visible = True
            End Try
            Me.PageUtility.AddAction(IIf(Me.CTRLsorgenteComunita.ComunitaID > 0, Me.CTRLsorgenteComunita.ComunitaID, 0), IIf(Me.PNLcontenuto.Visible, Services_Mail.ActionType.EnterService, Services_Mail.ActionType.NoPermission), Nothing, InteractionType.UserWithUser)
        End If

        Dim stringa As String
        stringa = TXBnascosto.Value
        If stringa <> "" Then
            EliminaAllegato(stringa)
            TXBnascosto.Value = ""
        End If
        If Me.PNLcontenuto.Visible Or Me.PNLrubrica.Visible = True Then
            Me.LoadAttachments(, True, )
        End If

        If Me.CTRLsorgenteComunita.ComunitaID > -1 Then
            Me.LBInfoCom.Visible = False
        Else
            Me.LBInfoCom.Visible = True
        End If

        'setfocus(Me.TXBbody)
    End Sub

    Private Function SessioneScaduta() As Boolean
        Dim oPersona As COL_Persona
        Dim isScaduta As Boolean = True
        Try
            oPersona = Session("objPersona")
            If oPersona.ID > 0 Then
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
            Return True
        Else
            Return False
        End If
    End Function

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        _Resource = New ResourceManager
        _Resource.UserLanguages = Code
        _Resource.ResourcesName = "pg_MailSender_Home"
        _Resource.Folder_Level1 = "Generici"
        _Resource.Folder_Level2 = ""
        _Resource.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()

        If IsNothing(Me.oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        '      onmouseover=ChangeState(event,'layer1','visible')
        'onmouseout=ChangeState(event,'layer1','hidden')>Info</a>

        With oResource
            .setLinkButton(Me.LNBMostraAttach, True, True, False, False)

            '.setLabel(LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(LBNopermessi)

            .setLabel(Me.LBcomunita_t)

            .setLabel(LBfrom)

            .setLabel(LBa)

            .setLinkButton(LNBMostraCC, True, True, False, False)

            .setLabel(LBcc)

            .setLabel(LBccn)
            '.setButton(BTNrubrica, True, False, False, True)
            '.setButton(BTNrubricaCCN, True, False, False, True)

            .setLabel(LBaRubrica)

            .setButton(BTNcloseRubrica, True, False, False, True)


            .setLabel(LBobj)
            .setLinkButton(LNBMostraAttach, True, True, False, False)
            .setLabel(LBattach)
            'fileAllega As System.Web.UI.HtmlControls.HtmlInputFile
            .setButton(BTallega, True, False, False, True)

            .setLabel(LBbody)
            '.setRequiredFieldValidator(RFVBody, True, False)

            .setCheckBox(CBXcopiamittente)
            .setCheckBox(CBXricezione)
            '.setButton(Me.BTNinvia, True, False, False, True)

            .setLabel(LBinviata)
            .setButton(BTNok)

            '.setLabel(LBerrore)
            .setButton(BTNokerrore)

            .setLabel(LBLInfoA)
            .setLabel(LBLInfoJava)
            .setLabel(LBLInfoComJava)
            .setLabel(LBInfoCom)

            .setImageButton(IMBrubricaCCN, False, True, True, False)
            .setImageButton(IMBrubrica, False, True, True, False)

            .setRegularExpressionValidator(Me.REVvalid)

            .setButton(Me.BTN_DelAllAttach, True, False, True, True)

        End With

        Me.LBLInfoA.Attributes.Add("onmouseover", "ChangeState(event,'layer1','visible', -20)")
        Me.LBLInfoA.Attributes.Add("onmouseout", "ChangeState(event,'layer1','hidden', -20)")
        Me.LBInfoCom.Attributes.Add("onmouseover", "ChangeState(event,'layer2','visible', -160)")
        Me.LBInfoCom.Attributes.Add("onmouseout", "ChangeState(event,'layer2','hidden',-160)")
    End Sub
#End Region

#Region "SendMail"

    Sub SendMail(ByVal obj As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            If Session("azione") <> "inviato" Then
                Session("azione") = "inviato"

                If (TXBa.Text <> "" Or TXBcc.Text <> "" Or TXBccn.Text <> "") Then 'And TXBbody.Text <> ""
                    Me.LBerrore.Visible = False
                    Try
                        Dim oPersona As New COL_Persona
                        Dim oUtility As New OLDpageUtility(Me.Context)
                        Dim oMail As New COL_E_Mail(oUtility.LocalizedMail)

                        Dim CMNT_ID, numFile As Integer
                        Dim arrListaFiles As Array
                        oPersona = Session("objPersona")


                        oMail.Mittente = New MailAddress(oPersona.Mail, oPersona.Anagrafica)

                        Dim ElencoA, ElencoCCN, ElencoAForCCn As COL_CollectionContatti
                        ElencoA = Me.CTRLrubrica.Contatti_TO
                        ElencoCCN = Me.CTRLrubrica.Contatti_CCN

                        Dim i, len As Integer
                        If Me.TXBcc.Text <> "" Then
                            If InStr(Me.TXBcc.Text, ";") > 0 Then
                                Dim ElencoContattiManuali As String = Me.TXBcc.Text
                                Dim ContattoManuale As String = ""
                                ElencoContattiManuali = ElencoContattiManuali.Replace(";", ",")
                                For Each ContattoManuale In ElencoContattiManuali
                                    oMail.IndirizziCC.Add(New MailAddress(ContattoManuale))
                                Next

                            Else
                                oMail.IndirizziCC.Add(New MailAddress(Me.TXBcc.Text))
                            End If
                        End If

                        If ElencoA.Count = 1 Then
                            oMail.IndirizziTO.Add(ElencoA.Item(0).Mail)
                        ElseIf ElencoA.Count > 1 Then
                            oMail.IndirizziTO.Add(ElencoA.Item(0).Mail)
                            For i = 1 To ElencoA.Count - 1
                                oMail.IndirizziCCN.Add(ElencoA.Item(i).Mail)
                            Next
                        End If
                        oMail.IndirizziCCN = ElencoCCN.GetEmailAddresses


                        oMail.Oggetto = TXBObj.Text
                        oMail.Body = TXBbody.Text

                        'Carico attachments
                        'Dim ofile As New COL_File
                        CMNT_ID = Me.CTRLsorgenteComunita.ComunitaID 'Session("IdComunita")
                        ContentOf.Directory(MailAttachmentPath & "\", numFile, arrListaFiles, True)
                        Dim ArrListFilesTemp As New ArrayList
                        If Not IsNothing(arrListaFiles) Then
                            For Each fileName As String In arrListaFiles
                                ArrListFilesTemp.Add(fileName)
                            Next
                        End If
                       
                        oMail.Attachment = ArrListFilesTemp
                        oMail.NotificaRicezione = Me.CBXricezione.Checked
                        oMail.HasCopiaMittente = Me.CBXcopiamittente.Checked

                        If Not Me.DebugMode Then
                            oMail.SendMailWithRecipientsLimit(Me.PageUtility.SystemSettings.Presenter.DefaultSplitMailRecipients)
                        End If

                        Try
                            Dim Astring, CCstring As String
                            If oMail.IndirizziTO.Count > 0 Then : Astring = "A"
                            End If
                            If oMail.IndirizziCC.Count > 0 And oMail.IndirizziCCN.Count > 0 <> "" Then : CCstring = "CC-CCN"
                            ElseIf oMail.IndirizziCC.Count > 0 Then : CCstring = "CC"
                            ElseIf oMail.IndirizziCCN.Count > 0 Then : CCstring = "CCN"
                            End If

                            Me.PageUtility.AddAction(Services_Mail.ActionType.SendMail, Me.CTRLsorgenteComunita.ComunitaID, Nothing, InteractionType.UserWithUser)
                        Catch ex As Exception
                        Finally

                        End Try


                        If oMail.Errore = Errori_Db.System Then
                            Me.PNLcontenuto.Visible = False
                            Me.PNLerrore.Visible = True
                            oResource.setLabel_To_Value(Me.LbErroreInvio, "InfoErrore2.text")
                            Session("azione") = "loaded"
                        End If

                        
                        If oMail.Errore = Errori_Db.System Then
                            Exit Sub
                        End If

                        EliminaTuttiAllegati()
                        Me.SetMailGUID()


                        Me.PNLcontenuto.Visible = False
                        PNLinviato.Visible = True


                    Catch ex As Exception
                        Session("azione") = "loaded"
                    End Try
                Else
                    Session("azione") = "loaded"
                    Me.LBerrore.Visible = True
                    Me.LBerrore.Text = Me.oResource.getValue("Error.NoDest") & "<br>"
                End If
            End If
        End If
    End Sub

#End Region

#Region "Gestione rubrica"

    Private Sub IMBrubrica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBrubrica.Click
        Me.LBerrore.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.PNLrubrica.Visible = True

        Me.CTRLrubrica.ComunitaID = Me.CTRLsorgenteComunita.ComunitaID 'Session("IdComunita")
        Me.CTRLrubrica.ComunitaPercorso = Me.CTRLsorgenteComunita.ComunitaPath
        Me.CTRLrubrica.setA_Address = True
        Me.CTRLrubrica.setCC_Address = False
        Me.CTRLrubrica.setCCN_Address = False

        If Me.TXBa.Text = "" And Me.TXBccn.Text = "" Then
            Me.CTRLrubrica.Bind_Dati()
            Me.CTRLrubrica.ResetForm()
        ElseIf Me.TXBa.Text = "" Then
            Me.CTRLrubrica.ResetForm()
        Else
            Me.CTRLrubrica.UpdateForm()
        End If
    End Sub

    Private Sub IMBrubricaCCN_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBrubricaCCN.Click
        Me.LBerrore.Visible = False
        Me.PNLcontenuto.Visible = False
        Me.PNLrubrica.Visible = True

        Me.CTRLrubrica.ComunitaID = Me.CTRLsorgenteComunita.ComunitaID 'Session("IdComunita")
        Me.CTRLrubrica.ComunitaPercorso = Me.CTRLsorgenteComunita.ComunitaPath
        Me.CTRLrubrica.setA_Address = False
        Me.CTRLrubrica.setCC_Address = False
        Me.CTRLrubrica.setCCN_Address = True

        If Me.TXBa.Text = "" And Me.TXBccn.Text = "" Then
            Me.CTRLrubrica.Bind_Dati()
            Me.CTRLrubrica.ResetForm()
        ElseIf Me.TXBccn.Text = "" Then
            Me.CTRLrubrica.ResetForm()
        Else
            Me.CTRLrubrica.UpdateForm()
        End If
    End Sub
    Private Sub BTNcloseRubrica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcloseRubrica.Click
        Me.PNLcontenuto.Visible = True
        Me.PNLrubrica.Visible = False

        If Me.CTRLrubrica.setA_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
            Me.TXBa.Text = ""
            Try
                Me.TXBa.Text = Me.CTRLrubrica.GetDestinatariMail_A
            Catch ex As Exception

            End Try
            If Me.TXBa.Text.IndexOf(";") = Me.TXBa.Text.LastIndexOf(";") Then
                'Me.LBLInfoA.Text = ""
                Me.LBLInfoA.Visible = False
            Else
                'Me.LBLInfoA.Text = "!"
                Me.LBLInfoA.Visible = True
            End If

        ElseIf Me.CTRLrubrica.setCC_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
            Me.TXBccn.Text = ""
            Try
                Me.TXBcc.Text = Me.CTRLrubrica.GetDestinatariMail_CC
            Catch ex As Exception

            End Try
        ElseIf Me.CTRLrubrica.setCCN_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
            Me.TXBccn.Text = ""
            Try
                Me.TXBccn.Text = Me.CTRLrubrica.GetDestinatariMail_CCN
            Catch ex As Exception

            End Try
        End If
    End Sub
#End Region

#Region "Show CC ed Attach"
    Private Sub LNBMostraCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBMostraCC.Click
        Me.MostraCC()
    End Sub
    Private Sub LNBMostraAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LNBMostraAttach.Click
        Me.MostraAttach()
    End Sub

    Private Sub MostraAttach()
        Me.TBRattach1.Visible = True
        Me.TBRattach2.Visible = True
        Me.LNBMostraAttach.Visible = False
    End Sub
    Private Sub MostraCC()
        Me.TBRcc.Visible = True
        Me.TBRccn.Visible = True
        Me.LNBMostraCC.Visible = False
    End Sub
#End Region

#Region "Gestione Allegati"
    Private Sub LoadAttachments(Optional ByVal mostraErrore As Boolean = True, Optional ByVal Bind As Boolean = False, Optional ByVal StrErrore As String = "")
        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")
            If oPersona.ID <> 0 Then

            End If
        Catch ex As Exception
            If IsNothing(oResource) Then
                SetCulture(Session("LinguaCode"))
            End If
            Dim alertMSG As String
            alertMSG = oResource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
            End If
            Response.Write("<script language='javascript'>alert('" & alertMSG & "');</script>")
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
        End Try

        Try
            Dim CMNT_ID As Integer = Me.CTRLsorgenteComunita.ComunitaID 'Session("IdComunita")
            Dim arrListaFiles As Array
            ' Dim oFile As New COL_File
            Dim nomeFile As String
            Dim numFile, dimFile, i As Integer
            Dim fMSG As FileMessage
            fMSG = ContentOf.Directory(MailAttachmentPath & "\", numFile, arrListaFiles, False)

            dimFile = ContentOf.Directory_Size((MailAttachmentPath & "\"), False)
            Dim HasFile As Boolean = False

            If fMSG = FileMessage.Read Then
                If dimFile > 6291456 Then '6MB in byte
                    Me.BTallega.Enabled = False
                    If mostraErrore = True Then
                        Me.LBLErroreAttach.Text = Me.oResource.getValue("LBAvviso." & Me.Avviso.OverByte)
                    End If

                    Me.LBLErroreAttach.Text = Replace(Me.LBLErroreAttach.Text, "#%%#", dimFile)
                    BTNinvia.Disabled = True

                Else
                    Me.BTallega.Enabled = True
                    BTNinvia.Disabled = False
                    Me.LBLErroreAttach.Text = StrErrore
                End If

                i = 1
                Dim row As New TableRow
                For Each nomeFile In arrListaFiles
                    HasFile = True
                    Dim oButton As New System.Web.UI.WebControls.ImageButton
                    If i Mod 2 <> 0 Then
                        row = New TableRow
                    End If
                    Dim cella As New TableCell
                    Dim cellaB As New TableCell
                    cellaB.Width = New System.Web.UI.WebControls.Unit(15)
                    cella.Width = New System.Web.UI.WebControls.Unit(320)

                    oButton.CssClass = ""
                    oButton.CommandName = Me.oResource.getValue("BTN.elimina")
                    oButton.AlternateText = Me.oResource.getValue("BTN.X")
                    oButton.CommandArgument = nomeFile
                    oButton.ImageUrl = ".\..\images\cestino.gif"
                    oButton.Attributes.Add("onclick", "document.forms[0]." & Replace(Me.TXBnascosto.UniqueID, ":", "_") & ".value='" & nomeFile.Replace("'", "\'") & "';document.forms[0].submit();return false;")
                    cellaB.Controls.Add(oButton)
                    row.Cells.Add(cellaB)

                    cella.Controls.Add(New LiteralControl(nomeFile))
                    row.Cells.Add(cella)

                    If i Mod 2 = 0 Then
                        TBLattach.Rows.Add(row)
                    ElseIf i Mod 2 <> 0 And i = numFile Then
                        row.Cells.Add(New TableCell)
                        row.Cells.Add(New TableCell)
                        TBLattach.Rows.Add(row)
                    End If
                    i = i + 1
                Next
                If arrListaFiles.Length > 0 Then
                    Me.MostraAttach()
                End If
            End If
            If Not HasFile Then
                Me.BTN_DelAllAttach.Visible = False
                Me.TBLattach.Rows.Clear()
                Me.TBLattach.Visible = False
            Else
                Me.TBLattach.Visible = True
                Me.BTN_DelAllAttach.Visible = True
            End If
            If Bind Then
                Me.TBLattach.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub EliminaAllegato(ByVal nomeFile As String)
        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")
            Dim IdComunita As Integer = Me.CTRLsorgenteComunita.ComunitaID 'Session("IdComunita")
            Dim oFile As New COL_File
            If oFile.Errore = Errore_File.fileNotFound Then
                Response.Write("Non si trova il file")
            Else
                Environment.CurrentDirectory = Server.MapPath("./../")
                Delete.File_FM(MailAttachmentPath & "\" & nomeFile)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub EliminaTuttiAllegati()
        Delete.Directory(MailAttachmentPath & "\", True)
    End Sub

    Private Sub BTallega_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTallega.Click

        If (fileAllega.PostedFile.FileName.ToString <> "") Then

            Dim ofile As New COL_File



            'prima di fare l'upload devo controllare che la dimensione totale attuale non superi quella massima

            ofile.Upload(fileAllega, MailAttachmentUploadPath)

            Dim strErrore As String
            Select Case ofile.Errore
                Case Errore_File.zeroByte
                    strErrore = Me.oResource.getValue("LBAvviso." & Me.Avviso.zeroByte)
                Case Errore_File.exsist
                    strErrore = Me.oResource.getValue("LBAvviso." & Me.Avviso.Esiste)
                Case Errore_File.none
                    strErrore = Me.oResource.getValue("LBAvviso." & Me.Avviso.Uploaded)
            End Select

            Me.TBLattach.Rows.Clear()
            Me.LoadAttachments(, True, strErrore)
        End If
    End Sub
#End Region

    Sub ritornadaerrore(ByVal sender As Object, ByVal e As System.EventArgs)
        Session("azione") = "loaded"
        Me.PNLcontenuto.Visible = True
        PNLinviato.Visible = False
        PNLerrore.Visible = False
        LoadAttachments(False)
    End Sub

#Region "Altri bottoni"
    Private Sub BTNokerrore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNokerrore.Click
        Session("azione") = "loaded"
        Me.PNLcontenuto.Visible = True
        PNLinviato.Visible = False
        PNLerrore.Visible = False
        LoadAttachments(False)
    End Sub
    Private Sub BTNok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNok.Click
        Session("azione") = "loaded"
        Me.PNLcontenuto.Visible = True
        Me.PNLinviato.Visible = False
        Me.CTRLrubrica.ResetForm()
        Me.TXBa.Text = ""
        Me.TXBa_n.Value = ""
        Me.TXBa_n_rlpc.Value = ""
        Me.TXBbody.Text = ""
        Me.TXBcc.Text = ""
        Me.TXBccn.Text = ""
        Me.CBXcopiamittente.Checked = False
        Me.CBXricezione.Checked = False
        Me.TXBObj.Text = ""

        Me.LNBMostraAttach.Visible = True
        Me.TBLattach.Visible = False

        Me.LNBMostraCC.Visible = True
        Me.TBRcc.Visible = False
        Me.TBRccn.Visible = False

        Me.TBRattach1.Visible = False
        Me.TBRattach2.Visible = False

        Me.LNBMostraAttach.Visible = True
        Me.LBLInfoA.Visible = False

        Me.LoadAttachments()
    End Sub
#End Region

    Private Sub CTRLsorgenteComunita_AggiornaDati(ByVal sender As Object, ByVal e As System.EventArgs) Handles CTRLsorgenteComunita.AggiornaDati
        If Me.CTRLsorgenteComunita.ComunitaID > 0 Then
            Dim i_link As String
            i_link = "./Attachments.aspx?CMNT_ID=" & Me.CTRLsorgenteComunita.ComunitaID
            Me.IMBrubricaCCN.Visible = True
            Me.IMBrubricaCCN.Enabled = True

            Me.IMBrubrica.Visible = True
            Me.IMBrubrica.Enabled = True

            Me.LNBMostraAttach.Visible = True

            Me.BTallega.Enabled = True
            Me.BTNinvia.Visible = True
            If Me.CTRLrubrica.ComunitaID <> Me.CTRLsorgenteComunita.ComunitaID Then
                Me.TXBa.Text = ""
                Me.TXBccn.Text = ""
            End If
        Else
            Me.IMBrubricaCCN.Visible = False
            Me.IMBrubricaCCN.Enabled = False
            Me.IMBrubrica.Visible = False
            Me.IMBrubrica.Enabled = False

            Me.LNBMostraAttach.Visible = False

            Me.BTallega.Enabled = False
            Me.BTNinvia.Visible = False

            Me.TXBa.Text = ""
            Me.TXBccn.Text = ""
        End If
    End Sub

#Region "Funzioni delegate per asincronia"
    'Public Sub FunzioneCallback(ByVal AsResult As IAsyncResult)
    '    'termino la chiamata asincrona
    '    Dim d As oGestore = CType(AsResult.AsyncState, oGestore)
    '    d.EndInvoke(AsResult)
    '    Session("DelegateTest") &= "Ended"

    'End Sub

    'Public Sub Conta(ByVal Param As String)
    '    Dim Cont As Integer = 0
    '    Dim Uscita As Boolean = False
    '    'Cont = 0
    '    Try
    '        Do Until Uscita
    '            Cont += 1
    '            Session("DelegateTest") &= Cont.ToString & "-"
    '            'fermo per 1 secondi prima di continuare a contare
    '            System.Threading.Thread.Sleep(1000)

    '            If Cont > 150 Then 'quando supero quota 30 esco dal loop
    '                Uscita = True
    '            End If
    '        Loop
    '    Catch ex As Exception
    '    End Try
    'End Sub
#End Region

    Private Sub BTN_DelAllAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTN_DelAllAttach.Click
        Me.EliminaTuttiAllegati()
        Me.LoadAttachments()
    End Sub

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        PageUtility.CurrentModule = PageUtility.GetModule(Services_Mail.Codex)
    End Sub

    Private Sub SetMailGUID()
        Me.ViewState("MailGuid") = System.Guid.NewGuid()
    End Sub

    Private ReadOnly Property MailAttachmentUploadPath As String
        Get
            Environment.CurrentDirectory = Server.MapPath("./../")

            Dim oPersona As New COL_Persona
            Dim IdComunita As Integer

            IdComunita = Me.CTRLsorgenteComunita.ComunitaID

            oPersona = Session("objPersona")

            Return Environment.CurrentDirectory & "\Mail\" & oPersona.ID & "\" & IdComunita & "\" & Me.ViewState("MailGuid").ToString() & "\"
        End Get
    End Property

    Private ReadOnly Property MailAttachmentPath As String
        Get
            Environment.CurrentDirectory = Server.MapPath("./../")

            Dim oPersona As New COL_Persona
            Dim IdComunita As Integer

            IdComunita = Me.CTRLsorgenteComunita.ComunitaID

            oPersona = Session("objPersona")

            Return ".\Mail\" & oPersona.ID & "\" & IdComunita & "\" & Me.ViewState("MailGuid").ToString()
            '".\Mail\" & IdComunita & "\" & oPersona.Id & "\"
        End Get
    End Property

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AjaxPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AjaxPortal)
        End Get
    End Property
End Class