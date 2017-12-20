Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2ServiziBase.ContattiMail
Imports lm.Comol.Core.File

Imports Comunita_OnLine.ModuloGenerale

Public Class AdminG_MailSender
    Inherits System.Web.UI.Page
    'Public oLocate As COL_Localizzazione
    Private oResource As ResourceManager

    Private _PageUtility As PresentationLayer.OLDpageUtility

    Private ReadOnly Property PageUtility(Optional ByVal oContext As HttpContext = Nothing) As PresentationLayer.OLDpageUtility
        Get
            If IsNothing(_PageUtility) OrElse Not IsNothing(oContext) Then
                _PageUtility = New OLDpageUtility(HttpContext.Current)
            End If
            Return _PageUtility
        End Get
    End Property
#Region "Form Permessi"
    Protected WithEvents PNLpermessi As System.Web.UI.WebControls.Panel
    Protected WithEvents LBNopermessi As System.Web.UI.WebControls.Label
#End Region
    Protected WithEvents PNLcontenuto As System.Web.UI.WebControls.Panel
#Region "Tabella Dati"
    Protected WithEvents TBLparametri As System.Web.UI.WebControls.Table
    Protected WithEvents TBRfrom As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBfrom As System.Web.UI.WebControls.Label
    Protected WithEvents TXBFrom As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRa As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBa As System.Web.UI.WebControls.Label
    Protected WithEvents TXBa As System.Web.UI.WebControls.TextBox
    'Protected WithEvents BTNrubrica As System.Web.UI.WebControls.Button

    Protected WithEvents TBRrubrica As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBaRubrica As System.Web.UI.WebControls.Label
    Protected WithEvents PNLrubrica As System.Web.UI.WebControls.Panel

    Protected WithEvents CTRLrubrica As Comunita_OnLine.UC_RubricaMailGlobale
    Protected WithEvents BTNcloseRubrica As System.Web.UI.WebControls.Button
    Protected WithEvents HDNtotaleDestinatari As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBRcc As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBcc As System.Web.UI.WebControls.Label
    Protected WithEvents TXBcc As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRccn As System.Web.UI.WebControls.TableRow
    'Protected WithEvents BTNrubricaCCN As System.Web.UI.WebControls.Button
    Protected WithEvents IMBrubricaCCN As System.Web.UI.WebControls.ImageButton

    Protected WithEvents LBccn As System.Web.UI.WebControls.Label
    Protected WithEvents TXBccn As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRobj As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBobj As System.Web.UI.WebControls.Label
    Protected WithEvents TXBObj As System.Web.UI.WebControls.TextBox
    'Protected WithEvents BTNinviaAlto As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents TBRattach As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBattach As System.Web.UI.WebControls.Label
    'Protected WithEvents BTNattach As System.Web.UI.WebControls.Button
    Protected WithEvents TBLattach As System.Web.UI.WebControls.Table
    Protected WithEvents TXBnascosto As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents TBRbody As System.Web.UI.WebControls.TableRow
    Protected WithEvents LBLBody_t As System.Web.UI.WebControls.Label
    Protected WithEvents TXBbody As System.Web.UI.WebControls.TextBox

    Protected WithEvents TBRinvia As System.Web.UI.WebControls.TableRow
    Protected WithEvents CBXcopiamittente As System.Web.UI.WebControls.CheckBox
    Protected WithEvents CBXricezione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents BTNinvia As System.Web.UI.HtmlControls.HtmlInputButton

    Protected WithEvents BTallega As System.Web.UI.WebControls.Button
    Protected WithEvents fileAllega As System.Web.UI.HtmlControls.HtmlInputFile
#End Region

#Region "Form"
    Protected WithEvents TXBa_n As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents TXBa_n_rlpc As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents BTNok As System.Web.UI.WebControls.Button
    Protected WithEvents PNLemail As System.Web.UI.WebControls.Panel
    Protected WithEvents PNLinviato As System.Web.UI.WebControls.Panel

    Protected WithEvents PNLerrore As System.Web.UI.WebControls.Panel
    Protected WithEvents BTNokerrore As System.Web.UI.WebControls.Button
    Protected WithEvents LBerrore As System.Web.UI.WebControls.Label

    Protected WithEvents IMBrubrica As System.Web.UI.WebControls.ImageButton

#End Region

#Region "Label varie"
    'Protected WithEvents LBTitolo As System.Web.UI.WebControls.Label
    Protected WithEvents LBinviata As System.Web.UI.WebControls.Label

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
        If IsNothing(Me.oResource) Then
            Me.SetCulture(Session("LinguaCode"))
        End If
        'inizio pagina
        If Page.IsPostBack = False Then
            Me.SetupDati()

            Dim oPersona As New COL_Persona

            Try
                oPersona = Session("objPersona")
            Catch ex As Exception
                Me.PNLpermessi.Visible = True
                Me.PNLcontenuto.Visible = False
            End Try


            Try
                If oPersona.TipoPersona.id = Main.TipoPersonaStandard.Amministrativo Or oPersona.TipoPersona.id = Main.TipoPersonaStandard.SysAdmin Then
                    'Dim i_link As String

                    'i_link = "./AdminG_Attachments.aspx?PRSN_ID=oPersona.Id"
                    'Me.BTNattach.Attributes.Add("onclick", "OpenWin('" & i_link & "','400','150','no','no');return false;")
                    TXBFrom.Text = oPersona.Nome & " " & oPersona.Cognome
                    Me.PNLpermessi.Visible = False
                    'Me.BTNrubrica.Enabled = Not (oPersona.TipoPersona.id = Main.TipoPersonaStandard.Amministrativo)
                    Me.IMBrubrica.Enabled = Not (oPersona.TipoPersona.id = Main.TipoPersonaStandard.Amministrativo)
                Else
                    Me.PNLemail.Visible = False
                    Me.PNLinviato.Visible = False
                    Me.PNLerrore.Visible = False
                    Me.PNLpermessi.Visible = True
                    oResource.setLabel_To_Value(LBNopermessi, "LBNopermessi.text")

                End If
            Catch ex As Exception
                Me.PNLemail.Visible = False
                Me.PNLinviato.Visible = False
                Me.PNLerrore.Visible = False
                Me.PNLpermessi.Visible = True
                oResource.setLabel_To_Value(LBNopermessi, "LBNopermessi.text")
            End Try
            Session("azione") = "load"
        End If
        Dim stringa As String
        stringa = TXBnascosto.Value
        If stringa <> "" Then
            EliminaAllegato(stringa)
            TXBnascosto.Value = ""
        End If
        If Me.PNLemail.Visible Then
            Me.LoadAttachments()
        End If
        If Me.PNLcontenuto.Visible = True Then
            Me.PNLrubrica.Visible = False
        End If
    End Sub
#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        ' localizzazione utente
        oResource = New ResourceManager

        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_AdminG_MailSender"
        oResource.Folder_Level1 = "Admin_globale"
        oResource.Folder_Level2 = ""
        oResource.setCulture()
    End Sub

    Private Sub SetupDati()
        With oResource
            '.setLabel(LBTitolo)
            Me.Master.ServiceTitle = .getValue("LBTitolo.text")
            .setLabel(LBNopermessi)
            .setLabel(LBa)
            .setLabel(LBfrom)
            .setLabel(LBcc)
            .setLabel(LBccn)
            .setLabel(LBobj)
            .setLabel(LBattach)
            .setLabel(LBinviata)
            .setLabel(LBerrore)
            .setLabel(Me.LBLBody_t)

            .setInputButton(Me.BTNinvia)

            .setButton(Me.BTNok, True, False, False, True)
            .setButton(BTNokerrore)
            .setButton(BTNcloseRubrica, True, False, False, True)

            .setCheckBox(CBXcopiamittente)
            .setCheckBox(CBXricezione)
        End With

    End Sub
#End Region

#Region "Gestione Rubrica"
    'Private Sub BTNrubrica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNrubrica.Click

    'End Sub
    'Private Sub BTNrubricaCCN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNrubricaCCN.Click

    'End Sub
    Private Sub BTNcloseRubrica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcloseRubrica.Click
        'Me.TBRrubrica.Visible = False
        'Me.TBRattach.Visible = True
        'Me.TBRbody.Visible = True
        'Me.TBRcc.Visible = True
        'Me.TBRccn.Visible = True
        'Me.TBRinvia.Visible = True
        'Me.TBRobj.Visible = True
        'Me.TBRa.Visible = True
        ''Me.BTNrubricaCCN.Visible = True
        'Me.IMBrubricaCCN.Visible = True
        ''Me.BTNrubrica.Visible = True
        'Me.IMBrubrica.Visible = True

        Me.PNLrubrica.Visible = False
        Me.PNLcontenuto.Visible = True

        If Me.CTRLrubrica.setA_Address Then
            Me.CTRLrubrica.SalvaGruppiSelezionati()
            Me.TXBa.Text = ""
            Try
                Me.TXBa.Text = Me.CTRLrubrica.GetDestinatariMail_A
            Catch ex As Exception

            End Try
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

    Sub SendMail(ByVal obj As Object, ByVal e As EventArgs)
        If Session("azione") <> "inviato" Then
            Session("azione") = "inviato"

            If (TXBa.Text <> "" Or TXBcc.Text <> "" Or TXBccn.Text <> "") And TXBbody.Text <> "" Then
                Try
                    Dim oPersona As New COL_Persona
					Dim oUtility As New OLDpageUtility(Me.Context)
					Dim oMail As New COL_E_Mail(oUtility.LocalizedMail)

                    Dim numFile As Integer
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

                    'cCarico attachments
                    'Dim ofile As New COL_File
                    ContentOf.Directory(Server.MapPath("./../Mail/Home/") & oPersona.ID & "\", numFile, arrListaFiles, True)
                    Dim ArrListFiles As ArrayList
                    For Each fileName As String In arrListaFiles
                        ArrListFiles.Add(fileName)
                    Next
                    oMail.Attachment = ArrListFiles

                    oMail.NotificaRicezione = Me.CBXricezione.Checked
                    oMail.HasCopiaMittente = Me.CBXcopiamittente.Checked

                    oMail.SendMailWithRecipientsLimit(Me.PageUtility.SystemSettings.Presenter.DefaultSplitMailRecipients)
                    If oMail.Errore = Errori_Db.System Then
                        Me.PNLemail.Visible = False
                        Me.PNLerrore.Visible = True
                        'Me.LBerrore.Text = "Si è verificato un errore, E-mail NON Spedita. Alcuni degli indirizzi di posta inseriti non sono in un formato valido.<BR>Si prega di modificarli e ritentare."
                        oResource.setLabel_To_Value(LBerrore, "InfoErrore2.text")
                        Session("azione") = "loaded"
                    End If

                    If oMail.Errore = Errori_Db.System Then
                        Exit Sub
                    End If

                    EliminaTuttiAllegati()
                    PNLemail.Visible = False
                    PNLinviato.Visible = True

                Catch ex As Exception
                    Session("azione") = "loaded"
                End Try
            Else
                Session("azione") = "loaded"
            End If
        End If
    End Sub

#Region "Gestione Allegati"
    Private Sub LoadAttachments(Optional ByVal mostraErrore As Boolean = True)
        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")
        Catch ex As Exception
            Dim PageUtility As New OLDpageUtility(Me.Context)
            Me.Response.Redirect(PageUtility.GetDefaultLogoutPage, True)
        End Try

        Try
            Dim arrListaFiles As Array
            'Dim oFile As New COL_File
            Dim nomeFile As String
            Dim numFile, dimFile, i As Integer
            Dim fMSG As FileMessage
            Environment.CurrentDirectory = Server.MapPath("./../")

            fMSG = ContentOf.Directory(".\Mail\Home\" & oPersona.ID & "\", numFile, arrListaFiles, False)
            dimFile = ContentOf.Directory_Size(".\Mail\Home\" & oPersona.ID & "\", False)

            If fMSG = FileMessage.Read Then
                If dimFile > 6291456 Then '6MB in byte
                    If mostraErrore = True Then
                        PNLemail.Visible = False
                        PNLerrore.Visible = True
                    Else
                        PNLemail.Visible = True
                        PNLerrore.Visible = False
                    End If

                    '  LBerrore.Text = "Gli allegati sono troppo pesanti: " & dimFile & " Kbyte.Max 2000 Kbyte"
                    oResource.setLabel_To_Value(LBerrore, "InfoErrore1.text")
                    Me.LBerrore.Text = Replace(Me.LBerrore.Text, "#%%#", dimFile)
                    BTNinvia.Disabled = True
                    'BTNinviaAlto.Disabled = True
                Else
                    BTNinvia.Disabled = False
                    'BTNinviaAlto.Disabled = False
                End If
                i = 1
                Dim row As New TableRow
                For Each nomeFile In arrListaFiles
                    Dim oButton As New System.Web.UI.WebControls.ImageButton
                    If i Mod 2 <> 0 Then
                        row = New TableRow
                    End If
                    Dim cella As New TableCell
                    Dim cellaB As New TableCell
                    cella.Controls.Add(New LiteralControl(nomeFile))
                    row.Cells.Add(cella)
                    oButton.CssClass = ""
                    oButton.CommandName = "elimina"
                    oButton.AlternateText = "X"
                    oButton.CommandArgument = nomeFile
                    oButton.ImageUrl = ".\..\images\cestino.gif"
                    oButton.Attributes.Add("onclick", "document.forms[0]." & Replace(Me.TXBnascosto.UniqueID, ":", "_") & ".value='" & nomeFile & "';document.forms[0].submit();return false;")
                    cellaB.Controls.Add(oButton)
                    row.Cells.Add(cellaB)
                    If i Mod 2 = 0 Then
                        TBLattach.Rows.Add(row)
                    ElseIf i Mod 2 <> 0 And i = numFile Then
                        row.Cells.Add(New TableCell)
                        row.Cells.Add(New TableCell)
                        TBLattach.Rows.Add(row)
                    End If
                    i = i + 1
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub EliminaAllegato(ByVal nomeFile As String)
        Dim oPersona As New COL_Persona

        Try
            oPersona = Session("objPersona")
            Dim oFile As New COL_File
            If oFile.Errore = Errore_File.fileNotFound Then
                Response.Write("Non si trova il file")
            Else
                Environment.CurrentDirectory = Server.MapPath("./../")
                Delete.File_FM(".\Mail\Home\" & oPersona.ID & "\" & nomeFile)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub EliminaTuttiAllegati()
        Dim oPersona As New COL_Persona
        Try
            oPersona = Session("objPersona")
            Dim oFile As New COL_File
            If oFile.Errore = Errore_File.dirNotFound Then
                Response.Write("non si trova la directory")
            Else
                Environment.CurrentDirectory = Server.MapPath("./../")
                lm.Comol.Core.File.Delete.Directory(".\Mail\Home\" & oPersona.ID, True)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

    Sub ritornadaerrore(ByVal sender As Object, ByVal e As System.EventArgs)
        PNLemail.Visible = True
        PNLinviato.Visible = False
        PNLerrore.Visible = False
        LoadAttachments(False)
    End Sub

    Private Sub BTNok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNok.Click
        Session("azione") = "loaded"
        Me.PNLemail.Visible = True
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
    End Sub

    Private Sub IMBrubrica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IMBrubrica.Click
        'Me.PNLrubrica.Visible = True
        'Me.TBRa.Visible = True
        ''Me.BTNrubrica.Visible = False
        'Me.IMBrubrica.Visible = False


        'Me.TBRrubrica.Visible = True
        'Me.TBRattach.Visible = False
        'Me.TBRbody.Visible = False
        'Me.TBRcc.Visible = False
        'Me.TBRccn.Visible = False
        'Me.TBRinvia.Visible = False
        'Me.TBRobj.Visible = False

        Me.PNLcontenuto.Visible = False
        Me.PNLrubrica.Visible = True


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
        'Me.PNLrubrica.Visible = True
        'Me.TBRa.Visible = True
        ''Me.BTNrubrica.Visible = False
        'Me.IMBrubrica.Visible = False

        'Me.TBRrubrica.Visible = True
        'Me.TBRattach.Visible = False
        'Me.TBRbody.Visible = False
        'Me.TBRcc.Visible = False
        'Me.TBRccn.Visible = True
        ''Me.BTNrubricaCCN.Visible = False
        'Me.IMBrubricaCCN.Visible = False

        'Me.TBRinvia.Visible = False
        'Me.TBRobj.Visible = False

        Me.PNLrubrica.Visible = True
        Me.PNLcontenuto.Visible = False

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

    Private Sub BTallega_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTallega.Click
        Dim oPersona As New COL_Persona
        'Dim IdComunita As Integer

        'IdComunita = Me.CTRLsorgenteComunita.ComunitaID

        '?!?
        'If IdComunita < 1 Then
        'Exit Sub
        'End If


        oPersona = Session("objPersona")
        If (fileAllega.PostedFile.FileName.ToString <> "") Then

            Dim ofile As New COL_File

            Environment.CurrentDirectory = Server.MapPath("./../")

            'prima di fare l'upload devo controllare che la dimensione totale attuale non superi quella massima

            ofile.Upload(fileAllega, ".\Mail\Home\" & oPersona.Id & "\")

            'Dim strErrore As String
            'Select Case ofile.Errore
            '    Case Errore_File.zeroByte
            '        strErrore = Me.oLocate.getValue("LBAvviso." & Me.Avviso.zeroByte)
            '    Case Errore_File.exsist
            '        strErrore = Me.oLocate.getValue("LBAvviso." & Me.Avviso.Esiste)
            '    Case Errore_File.none
            '        strErrore = Me.oLocate.getValue("LBAvviso." & Me.Avviso.Uploaded)
            'End Select

            Me.TBLattach.Rows.Clear()
            Me.LoadAttachments(True) ', True, strErrore)
        End If
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class