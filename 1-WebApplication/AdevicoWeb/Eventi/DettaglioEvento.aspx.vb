Imports COL_BusinessLogic_v2.Eventi
Imports COL_BusinessLogic_v2.CL_persona

Imports COL_BusinessLogic_v2

Public Class DettaglioEvento_aspx
    Inherits System.Web.UI.Page
    Protected oResource As ResourceManager

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents LBNomeEvento As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoEvento As System.Web.UI.WebControls.Label
    Protected WithEvents LBcomunita As System.Web.UI.WebControls.Label
    Protected WithEvents LBluogo As System.Web.UI.WebControls.Label
    Protected WithEvents LBInizio As System.Web.UI.WebControls.Label
    Protected WithEvents LBFine As System.Web.UI.WebControls.Label
    Protected WithEvents LBaula As System.Web.UI.WebControls.Label
    Protected WithEvents HLlink As System.Web.UI.WebControls.HyperLink
    Protected WithEvents LBgiorno As System.Web.UI.WebControls.Label
    Protected WithEvents LBdata As System.Web.UI.WebControls.Label
    Protected WithEvents LBmese As System.Web.UI.WebControls.Label
    Protected WithEvents BTNChiudi As System.Web.UI.WebControls.Button
    Protected WithEvents LBLProgramma As System.Web.UI.WebControls.Label
    Protected WithEvents LBLNote As System.Web.UI.WebControls.Label
    Protected WithEvents LBannoAccademico As System.Web.UI.WebControls.Label
    Protected WithEvents LBAnnoAccTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBcomunitaTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBAulaTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBProgramma As System.Web.UI.WebControls.Label
    Protected WithEvents LBvisibile As System.Web.UI.WebControls.Label
    Protected WithEvents LBvisibileTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBReferenteTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBReferente As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoEventoTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBluogoTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBInizioTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBFineTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBLinkTesto As System.Web.UI.WebControls.Label
    Protected WithEvents LBNoteTesto As System.Web.UI.WebControls.Label
    Protected WithEvents TBRcomunita As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRluogo As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRaula As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRinizio As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRfine As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRanno As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRreferente As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlink As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRvisibile As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRnote As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRprogramma As System.Web.UI.WebControls.TableRow

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
        Try
            If Request.QueryString("tipo") > 0 Then         'è un EVENTO
                Dim oEvento As New COL_Evento
                Dim oOrario As New COL_Orario
                Dim oProgrammaEvento As New COL_Programma_Evento
                Dim IDOrario As String
                IDOrario = Request.QueryString("ORRI_id")
                oOrario.Id = CInt(IDOrario)
                oOrario.Estrai()
                oOrario.Evento.Estrai()
                oOrario.Evento.TipoEvento.Estrai()
                oOrario.Evento.Comunita.Estrai()

                oProgrammaEvento.Id = CInt(IDOrario)
                Try
                    oProgrammaEvento.Estrai()
                    LBLProgramma.Text = oProgrammaEvento.ProgrammaSvolto
                Catch ex As Exception

                End Try

                LBNomeEvento.Text = oOrario.Evento.Nome
                LBluogo.Text = oOrario.Evento.Luogo

                HLlink.Text = oOrario.Link

                Try
                    If oOrario.Link.Substring(0, 7).CompareTo("http://") <> 0 Then
                        HLlink.NavigateUrl = "http://" & oOrario.Link
                    Else
                        HLlink.NavigateUrl = oOrario.Link
                    End If
                Catch ex As Exception

                End Try

                LBLNote.Text = oOrario.Note

                LBReferente.Text = oOrario.Referente

                LBtipoEvento.Text = oOrario.Evento.TipoEvento.Nome
                If oOrario.Visibile = True Then
                    LBvisibile.Text = "SI"
                Else
                    LBvisibile.Text = "NO"
                End If

                LBcomunita.Text = oOrario.Evento.Comunita.Nome

                LBannoAccademico.Text = oOrario.Evento.AnnoAccademico & "-" & oOrario.Evento.AnnoAccademico + 1

                LBInizio.Text = oOrario.Inizio.ToString("HH:mm")
                If oOrario.Inizio.Date < oOrario.Fine.Date Then
                    LBFine.Text = oOrario.Fine.ToString("HH:mm") & " " & oResource.getValue("dettaglioEvento.del") & " " & oOrario.Fine.ToString("dd-MMMM-yy", oResource.CultureInfo.DateTimeFormat)
                Else
                    LBFine.Text = oOrario.Fine.ToString("HH:mm")
                End If

                LBaula.Text = oOrario.Luogo

                LBdata.Text = oOrario.Inizio.ToString("dd", oResource.CultureInfo.DateTimeFormat)
                LBgiorno.Text = oOrario.Inizio.ToString("dddd", oResource.CultureInfo.DateTimeFormat)
                LBmese.Text = oOrario.Inizio.ToString("MMMM  yyyy", oResource.CultureInfo.DateTimeFormat)

            ElseIf Request.QueryString("tipo") = -1 Then      ' è un evento personale (REMINDER)
                Dim oReminder As New COL_Reminder
                Dim oPersona As New COL_Persona

                oPersona = Session("objPersona")
                oReminder.idPersona = oPersona.Id
                oReminder.Id = Request.QueryString("ORRI_id")
                oReminder.Estrai_da_id()

                LBNomeEvento.Text = oReminder.Oggetto
                LBluogo.Text = oReminder.Luogo

                HLlink.Text = oReminder.Link

                Try
                    If oReminder.Link.Substring(0, 7).CompareTo("http://") <> 0 Then
                        HLlink.NavigateUrl = "http://" & oReminder.Link
                    Else
                        HLlink.NavigateUrl = oReminder.Link
                    End If
                Catch ex As Exception

                End Try

                LBLNote.Text = oReminder.Testo
                LBtipoEvento.Text = "evento personale"

                Me.TBRcomunita.Visible = False
                Me.TBRluogo.BackColor = System.Drawing.Color.FromName("#f0f0d5")
                Me.TBRaula.Visible = False
                Me.TBRinizio.BackColor = System.Drawing.Color.FromName("#f5f5f5")
                Me.TBRfine.BackColor = System.Drawing.Color.FromName("#f0f0d5")
                Me.TBRanno.Visible = False
                Me.TBRreferente.Visible = False
                Me.TBRlink.BackColor = System.Drawing.Color.FromName("#f5f5f5")
                Me.TBRvisibile.Visible = False
                Me.TBRnote.BackColor = System.Drawing.Color.FromName("#f0f0d5")
                Me.TBRprogramma.Visible = False

                LBInizio.Text = oReminder.Inizio.ToString("HH:mm", oResource.CultureInfo.DateTimeFormat)

                If oReminder.Inizio.Date < oReminder.Fine.Date Then
                    LBFine.Text = oReminder.Fine.ToString("HH:mm", oResource.CultureInfo.DateTimeFormat) & " " & oResource.getValue("dettaglioEvento.del") & " " & oReminder.Fine.ToString("dd-MMMM-yy", oResource.CultureInfo.DateTimeFormat)
                Else
                    LBFine.Text = oReminder.Fine.ToString("HH:mm", oResource.CultureInfo.DateTimeFormat)
                End If

                LBaula.Visible = False
                LBAulaTesto.Visible = False
                LBdata.Text = oReminder.Inizio.ToString("dd", oResource.CultureInfo.DateTimeFormat)
                LBgiorno.Text = oReminder.Inizio.ToString("dddd", oResource.CultureInfo.DateTimeFormat)
                LBmese.Text = oReminder.Inizio.ToString("MMMM  yyyy", oResource.CultureInfo.DateTimeFormat)
            End If
            If Not Page.IsPostBack Then
                BTNChiudi.Attributes.Add("onclick", "ChiudiMi();return false;")
            End If
        Catch ex As Exception

        End Try
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
            Response.Write("<script language='javascript'>function AlertLogoutClose(Messaggio){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "window.close();" & vbCrLf & "} " & vbCrLf & "AlertLogoutClose('" & alertMSG & "');</script>")
            Return True
        End If
    End Function

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResource = New ResourceManager
        oResource.UserLanguages = Code
        oResource.ResourcesName = "pg_DettaglioEvento"
        oResource.Folder_Level1 = "Eventi"
        oResource.setCulture()
    End Sub

    Private Sub SetupInternazionalizzazione()
        With oResource
            .setLabel(LBtipoEventoTesto)
            .setLabel(LBcomunitaTesto)
            .setLabel(LBluogoTesto)
            .setLabel(LBInizioTesto)
            .setLabel(LBFineTesto)
            .setLabel(LBAnnoAccTesto)
            .setLabel(LBReferenteTesto)
            .setLabel(LBLinkTesto)
            .setLabel(LBvisibileTesto)
            .setLabel(LBNoteTesto)
            .setLabel(LBProgramma)
            .setButton(BTNChiudi, True, , , True)
        End With
    End Sub
#End Region
End Class
