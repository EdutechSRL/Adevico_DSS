Imports COL_BusinessLogic_v2

Imports COL_BusinessLogic_v2.CL_persona

Public Class UC_DettagliLink
    Inherits System.Web.UI.UserControl
    Private oResourceDettagli As ResourceManager

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

#Region "dettagliLink"
    Protected WithEvents TBLinfo As System.Web.UI.WebControls.Table
    Protected WithEvents TBRnome As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRlink As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRdescrizione As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRcreatoil As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRmodificatoil As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRautore As System.Web.UI.WebControls.TableRow
    Protected WithEvents TBRimportato As System.Web.UI.WebControls.TableRow

    Protected WithEvents LBnomeLink As System.Web.UI.WebControls.Label
    Protected WithEvents LBnomeLink_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlink_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBlink As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescrizione_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBdescrizione As System.Web.UI.WebControls.Label
    Protected WithEvents LBcreato_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBcreato As System.Web.UI.WebControls.Label
    Protected WithEvents LBmodificato_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBmodificato As System.Web.UI.WebControls.Label
    Protected WithEvents LBautore_t As System.Web.UI.WebControls.Label
    Protected WithEvents LBautore As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoLink As System.Web.UI.WebControls.Label
    Protected WithEvents LBtipoLink_t As System.Web.UI.WebControls.Label
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IsNothing(oResourceDettagli) Then
            SetCulture(Session("LinguaCode"))
        End If
        If Page.IsPostBack = False Then
            Me.SetupInternazionalizzazione()
        End If
    End Sub

#Region "Bind"
    Public Sub Bind_dettagli(ByVal Link_ID As Integer)
        Dim oRaccoltaLink As New COL_RaccoltaLink
        Try

            With oRaccoltaLink
                .ID = Link_ID
                .Estrai()
                If .Errore = Errori_Db.None Then
                    Me.TBLinfo.Visible = True
                    If .Nome <> "" Then
                        Me.TBRnome.Visible = True
                        Me.LBnomeLink.Text = .Nome
                    Else
                        Me.TBRnome.Visible = False
                    End If
                    If .Descrizione <> "" Then
                        Me.TBRdescrizione.Visible = True
                        Me.LBdescrizione.Text = .Descrizione
                    Else
                        Me.TBRdescrizione.Visible = False
                    End If

                    If .Url <> "" Then
                        Me.TBRlink.Visible = True
                        If InStr(.Url, "http://") > 0 Then
                            Me.LBlink.Text = "<a href=" & """" & .Url & """" & " target=_blank class=Linksmall_Under11>" & .Url & "</a>"
                        Else
                            Me.LBlink.Text = "<a href=" & """" & "http://" & .Url & """" & " target=_blank class=Linksmall_Under11>" & .Url & "</a>"
                        End If
                    Else
                        Me.TBRlink.Visible = False
                    End If

                    If Not Equals(oRaccoltaLink.CreatoIl, New Date) Then
                        Me.TBRcreatoil.Visible = True
                        Me.LBcreato.Text = FormatDateTime(oRaccoltaLink.CreatoIl, DateFormat.ShortDate) & " " & FormatDateTime(oRaccoltaLink.CreatoIl, DateFormat.ShortTime)
                    Else
                        Me.TBRcreatoil.Visible = False
                    End If
                    If Not Equals(oRaccoltaLink.ModificatoIl, New Date) Then
                        Me.TBRmodificatoil.Visible = True
                        Me.LBmodificato.Text = FormatDateTime(oRaccoltaLink.ModificatoIl, DateFormat.ShortDate) & " " & FormatDateTime(oRaccoltaLink.ModificatoIl, DateFormat.ShortTime)
                    Else
                        Me.TBRmodificatoil.Visible = False
                    End If

                    If .isFromPersonale Then
                        If .isCartella Then
                            Me.LBtipoLink.Text = Me.oResourceDettagli.getValue("LBtipoLink.cartella.importata")
                        Else
                            Me.LBtipoLink.Text = Me.oResourceDettagli.getValue("LBtipoLink.link.importata")
                        End If
                    Else
                        If .isCartella Then
                            Me.LBtipoLink.Text = Me.oResourceDettagli.getValue("LBtipoLink.cartella")
                        Else
                            Me.LBtipoLink.Text = Me.oResourceDettagli.getValue("LBtipoLink.link")
                        End If
                    End If

                    Dim oPersona As New COL_Persona
                    oPersona.Id = .PRSN_ID
                    oPersona.Estrai(Session("LinguaID"))
                    If oPersona.Errore = Errori_Db.None Then
                        LBautore.Text = oPersona.Cognome & " " & oPersona.Nome
                    End If

                Else
                    Me.TBLinfo.Visible = False
                End If
            End With

            'If oRaccoltaLink.isFromPersonale Then
            '    Me.TBRimportato.Visible = True
            '    Me.LBimportato.Text = "Link personale importato"
            'Else
            '    Me.TBRimportato.Visible = False
            'End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Localizzazione"
    Private Sub SetCulture(ByVal Code As String)
        oResourceDettagli = New ResourceManager

        oResourceDettagli.UserLanguages = Code
        oResourceDettagli.ResourcesName = "pg_UC_DettagliLink"
        oResourceDettagli.Folder_Level1 = "Generici"
        oResourceDettagli.Folder_Level2 = "UC_Link"
        oResourceDettagli.setCulture()
    End Sub
    Private Sub SetupInternazionalizzazione()
        With oResourceDettagli
            .setLabel(Me.LBcreato_t)
            .setLabel(Me.LBmodificato_t)
            .setLabel(Me.LBautore_t)
            .setLabel(Me.LBdescrizione_t)
            .setLabel(Me.LBnomeLink_t)
            .setLabel(Me.LBlink_t)
            .setLabel(Me.LBtipoLink_t)
        End With
    End Sub
#End Region

    
End Class