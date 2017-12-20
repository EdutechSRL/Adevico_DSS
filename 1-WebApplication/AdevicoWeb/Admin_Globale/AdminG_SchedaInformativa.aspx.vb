Imports Comunita_OnLine.ModuloGenerale
Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.CL_persona
Imports COL_BusinessLogic_v2.CL_permessi
Imports COL_BusinessLogic_v2.Comunita

Public Class AdminG_SchedaInformativa
    Inherits System.Web.UI.Page

#Region "Form Dettagli"
    Protected WithEvents PNLDettagli As System.Web.UI.WebControls.Panel
    Protected WithEvents LBtitolo As System.Web.UI.WebControls.Label
    Protected WithEvents BTNindietro As System.Web.UI.WebControls.Button
    Protected WithEvents CTRLDettagli As Comunita_OnLine.UC_DettagliComunita
#End Region

#Region "noquerystring"
    Protected WithEvents BTNlistacmnt As System.Web.UI.WebControls.Button
    Protected WithEvents BTNricercacmnt As System.Web.UI.WebControls.Button
    Protected WithEvents LBnoquery As System.Web.UI.WebControls.Label
    Protected WithEvents PNLnoquery As System.Web.UI.WebControls.Panel
#End Region

#Region " Codice generato da Progettazione Web Form "

    'Chiamata richiesta da Progettazione Web Form.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: questa chiamata al metodo è richiesta da Progettazione Web Form.
        'Non modificarla nell'editor del codice.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Inserire qui il codice utente necessario per inizializzare la pagina
        If Session("objPersona") Is Nothing Then 'se la sessione è scaduta redirecto alla home
            Response.Redirect("./../index.aspx")
        Else
            If Session("idComunita_forAdmin") Is Nothing Then
                Me.PNLDettagli.Visible = False
                Me.PNLnoquery.Visible = True
                Me.LBnoquery.Text = "Errore nell'accesso alla pagina, riprovare."

            Else
                'richiamo il controllo utente
                Me.PNLDettagli.Visible = True
                Me.CTRLDettagli.SetupDettagliComunita(Session("idComunita_forAdmin"))
                Me.BTNindietro.Visible = True
            End If
        End If
    End Sub


    Private Sub BTNindietro_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNindietro.Click
        Me.ReturnToSearchPage()
    End Sub

    Private Sub BTNlistacmnt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNlistacmnt.Click
        Response.Redirect("./adming_listacomunita.aspx")
    End Sub
    Private Sub BTNricercacmnt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNricercacmnt.Click
        Response.Redirect("./adming_ricercacomunita.aspx")
    End Sub

    Private Sub ReturnToSearchPage()
        Try
            If Me.Request.QueryString("from") = "" Then
                Me.Response.Redirect("./AdminG_ListaComunita.aspx")
            Else
                Select Case LCase(Me.Request.QueryString("from"))
                    Case "ricercacomunita"
                        Me.Response.Redirect("./AdminG_ListaComunita.aspx?re_set=true")
                    Case "ricercabypersona"
                        Me.Response.Redirect("./AdminG_RicercaComunita.aspx?re_set=true")
                    Case Else
                        Me.Response.Redirect("./AdminG_ListaComunita.aspx")
                End Select
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Shadows ReadOnly Property Master() As Comunita_OnLine.AdminPortal
        Get
            Return CType(MyBase.Master, Comunita_OnLine.AdminPortal)
        End Get
    End Property
End Class
