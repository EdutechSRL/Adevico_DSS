Imports COL_BusinessLogic_v2.Comunita
Imports COL_Questionario
Partial Public Class QuestionarioPage
    Inherits System.Web.UI.MasterPage
    '    Public Shared oServizio As New UCServices.Services_Questionario
    '    Private oResource As New ResourceManager

    '    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '        'If IsNothing(oResource) Then
    '        '    SetCulture(Session("LinguaCode"))
    '        'End If

    '        If Me.SessioneScaduta() Then
    '            Exit Sub
    '        End If

    '        getPermessi()
    '    End Sub

    '    Private Sub getPermessi()
    '        Dim oTipoPermesso As UCServices.Services_File.PermissionType
    '        Dim PermessiAssociati As String
    '        Try
    '            PermessiAssociati = Permessi(oServizio.Codex, Me.Page)
    '            If Not (PermessiAssociati = "") Then
    '                oServizio.PermessiAssociati = PermessiAssociati
    '            End If
    '        Catch ex As Exception
    '            oServizio.PermessiAssociati = "00000000000000000000000000000000"
    '        End Try

    '    End Sub
    '    Private Sub SetCulture(ByVal Code As String)
    '        oResource = New ResourceManager

    '        oResource.UserLanguages = Code
    '        oResource.ResourcesName = "pg_WikiMaster"
    '        oResource.Folder_Level1 = "Wiki"
    '        oResource.setCulture()
    '    End Sub

    '    Private Sub SetupInternazionalizzazione()
    '        With oResource
    '            '.setLinkButton(LNBCartellaPrincipale, True, True)
    '            '.setLinkButton(LNBListaSezioni, True, True)

    '            '.setLinkButton(LNBRicerca, True, True)
    '            '.setLinkButton(LNBCreaWiki, True, True)

    '            'If (oServizio.GestioneWiki Or oServizio.Admin Or oServizio.GestioneSezioni) And Not oWiki.id Is Nothing Then
    '            '    LNBNuovaSezione.Visible = True
    '            '    .setLinkButton(LNBNuovaSezione, True, True)
    '            'End If


    '        End With

    '    End Sub

    '#Region "controllo sessione"

    '    Private Function SessioneScaduta() As Boolean
    '        Dim oPersona As COL_Persona
    '        Dim isScaduta As Boolean = True
    '        Try
    '            oPersona = Session("objPersona")
    '            If oPersona.Id > 0 Then
    '                isScaduta = False
    '                Return False
    '            End If
    '        Catch ex As Exception

    '        End Try
    '        If isScaduta Then
    '            Dim alertMSG As String
    '            alertMSG = oResource.getValue("LogoutMessage")
    '            If alertMSG <> "" Then
    '                alertMSG = alertMSG.Replace("'", "\'")
    '            Else
    '                alertMSG = "Session timeout"
    '            End If
    '            Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & "http://" & Me.Request.Url.Host & GetPercorsoApplicazione(Me.Request) & "/Index.aspx" & "');</script>")
    '            Response.End()
    '            Return True
    '        Else
    '            Try
    '                If Session("Limbo") = True Then
    '                    Return False
    '                Else
    '                    Dim CMNT_ID As Integer = 0
    '                    Try
    '                        CMNT_ID = Session("idComunita")
    '                    Catch ex As Exception
    '                        CMNT_ID = 0
    '                    End Try

    '                    If CMNT_ID <= 0 Then
    '                        Me.ExitToLimbo()
    '                        Return True
    '                    End If
    '                End If
    '            Catch ex As Exception
    '                Me.ExitToLimbo()
    '                Return True
    '            End Try
    '        End If
    '    End Function

    '    Private Sub ExitToLimbo()
    '        Session("Limbo") = True
    '        Session("ORGN_id") = 0
    '        Session("IdRuolo") = ""
    '        Session("ArrPermessi") = ""
    '        Session("RLPC_ID") = ""

    '        Session("AdminForChange") = False
    '        Session("CMNT_path_forAdmin") = ""
    '        Session("idComunita_forAdmin") = ""
    '        Session("TPCM_ID") = ""
    '        Me.Response.Expires = 0
    '        Me.Response.Redirect("./EntrataComunita.aspx", True)
    '    End Sub

    '#End Region


    'Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Dim i As Integer
    '    If Page.IsPostBack = False Then
    '        i = 10
    '    End If

    '    If Page.IsCallback Then
    '        i = 20
    '    End If
    'End Sub
    Private _Resource As ResourceManager
    Protected ReadOnly Property Resource() As ResourceManager
        Get
            Try
                If IsNothing(_Resource) Then
                    _Resource = New ResourceManager
                End If
                Resource = _Resource
            Catch ex As Exception
                Resource = New ResourceManager
            End Try
        End Get
    End Property
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetCulture("pg_QuestionarioMaster", "Questionari")
        Dim tipoQuest As Questionario.TipoQuestionario
        If Not Request.QueryString("type") Is Nothing Then
            tipoQuest = Request.QueryString("type")
        ElseIf Not Session("QuestionarioCorrente") Is Nothing Then
            tipoQuest = DirectCast(Session("QuestionarioCorrente"), Questionario).tipo
        Else
            tipoQuest = -1
        End If
        With Me.resource
            Select Case tipoQuest
                Case Questionario.TipoQuestionario.LibreriaDiDomande
                    LBTitolo.Text = .getValue("LibreriaDiDomande")
                Case Questionario.TipoQuestionario.Modello
                    LBTitolo.Text = .getValue("Modello")
                Case Questionario.TipoQuestionario.Autovalutazione
                    LBTitolo.Text = .getValue("Autovalutazione")
                Case Questionario.TipoQuestionario.Sondaggio
                    LBTitolo.Text = .getValue("Sondaggio")
                Case Questionario.TipoQuestionario.Meeting
                    LBTitolo.Text = .getValue("Meeting")
                Case Else
                    LBTitolo.Text = .getValue("Questionari")
            End Select
        End With
    End Sub
    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
        Me._Resource = New ResourceManager
        Me._Resource.UserLanguages = Me.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub
    Protected Property LinguaCode() As String
        Get
            'Ricontrollare, va'...
            Dim CodeLingua As String = ""
            Try
                If Session("LinguaCode") = "" Then
                    'Try
                    '	CodeLingua = Request.UserLanguages(0)
                    'Catch ex As Exception

                    'End Try
                    If Request.Browser.Cookies = True Then
                        Try
                            CodeLingua = Request.Cookies("LinguaCode").Value
                        Catch ex As Exception
                        End Try
                    End If
                Else
                    Try
                        CodeLingua = Session("LinguaCode")
                    Catch ex As Exception
                    End Try
                End If
            Catch ex As Exception
                CodeLingua = ""
            End Try
            If CodeLingua = "" Then
                CodeLingua = GetSystemSettings.DefaultLanguage.Codice
            End If
            Return CodeLingua
        End Get
        Set(ByVal value As String)
            Session("LinguaCode") = value
        End Set
    End Property
    Private Function GetSystemSettings() As ComolSettings
        Dim oSettings As ComolSettings
        oSettings = ManagerConfiguration.GetInstance
        Return oSettings
    End Function
End Class