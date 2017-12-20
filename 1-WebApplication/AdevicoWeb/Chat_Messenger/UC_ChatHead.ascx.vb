Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.CL_persona
Imports lm.Comol.Core.File

Public Class UC_ChatHead
    Inherits System.Web.UI.UserControl

    Protected WithEvents IMGlogo As System.Web.UI.WebControls.Image
    Protected WithEvents LinkLogo As System.Web.UI.HtmlControls.HtmlAnchor
    Protected WithEvents LblNomCom As System.Web.UI.WebControls.Label
    Protected WithEvents oPage As Comunita_OnLine.Chat_Ext

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

        If Not Page.IsPostBack Then
            Dim oImpostazioni As New COL_ImpostazioniUtente
            Try
                If IsNothing(Session("oImpostazioni")) Then
                    Me.Bind_Logo()
                Else
                    oImpostazioni = Session("oImpostazioni")
                    If oImpostazioni.ShowLogo Then
                        Me.Bind_Logo()
                    Else
                        Me.LinkLogo.Visible = False
                        Me.IMGlogo.Visible = False
                    End If
                End If
            Catch ex As Exception
                Me.Bind_Logo()
            End Try

            'Me.SetCulture(Session("LinguaCode"))
            'Me.SetupInternazionalizzazione()

            'Me.CTRLnews.Visible = Me.n_ShowNews
            'Me.TBRnews.Visible = Me.n_ShowNews
        End If
        'Me.LblNomCom.Text = Me.oPage.IdComChat
    End Sub

#Region "Gestione logo"
    Private Sub Bind_Logo()
        Dim found As Boolean = False

        Try
            If Session("Limbo") = True Then
                found = GetLogoIstituzione()
            Else
                Dim ArrComunita(,) As String = Session("ArrComunita")
                Dim oComunita As New COL_Comunita
                Dim CMNT_ID, ORGN_ID, i, totale As Integer
                Dim oArray As String(,)

                oComunita.Id = ArrComunita(0, 0)
                oComunita.Estrai()
                ORGN_ID = oComunita.Organizzazione.Id

                Try
                    oArray = Me.Application.Item("ArrayLogo")
                Catch ex As Exception
                    oArray = Nothing
                End Try

                If Not IsNothing(oArray) Then
                    totale = UBound(oArray, 2)
                    For i = 0 To totale
                        If oArray(0, i) = ORGN_ID Then
                            If Exists.File(Server.MapPath("./../" & oArray(1, i))) Then
                                Me.IMGlogo.ImageUrl = "./../" & oArray(1, i)
                                If oArray(2, i) = "" Then
                                    Me.LinkLogo.HRef = "#"
                                    Me.LinkLogo.Attributes.Add("onclick", "window.status='';return false;")
                                    Me.LinkLogo.Attributes.Add("onmouseover", "window.status='';return true;")
                                    Me.LinkLogo.Attributes.Add("onfocus", "window.status='';return true;")
                                    Me.LinkLogo.Attributes.Add("onmouseout", "window.status='';return true;")
                                    Me.LinkLogo.Disabled = True
                                Else
                                    If InStr(oArray(2, i), "http://") > 0 Then
                                        Me.LinkLogo.HRef = oArray(2, i)
                                    Else
                                        Me.LinkLogo.HRef = "http://" & oArray(2, i)
                                    End If
                                    Me.LinkLogo.Disabled = False
                                    Me.LinkLogo.Attributes.Add("onclick", "window.status='" & Replace(oArray(2, i), "'", "\'") & "';return true;")
                                    Me.LinkLogo.Attributes.Add("onmouseover", "window.status='" & Replace(oArray(2, i), "'", "\'") & "';return true;")
                                    Me.LinkLogo.Attributes.Add("onfocus", "window.status='" & Replace(oArray(2, i), "'", "\'") & "';return true;")
                                    Me.LinkLogo.Attributes.Add("onmouseout", "window.status='';return true;")
                                End If

                                found = True
                            End If
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            found = GetLogoIstituzione()
        End Try
        If Not found Then
            Me.IMGlogo.ImageUrl = "./../images/NoLogo.gif"
            Me.LinkLogo.HRef = "#"
            Me.LinkLogo.Attributes.Add("onclick", "Javascript:return false;")
        End If
    End Sub
    Private Function GetLogoIstituzione() As Boolean
        Dim oIstituzione As New COL_Istituzione
        Try
            oIstituzione.Id = Session("ISTT_ID")
            oIstituzione.Estrai()

            If oIstituzione.Logo <> "" Then
                If Exists.File(Server.MapPath("./../images/Logo/" & oIstituzione.Logo)) Then
                    Me.IMGlogo.ImageUrl = "./../images/Logo/" & oIstituzione.Logo
                    If oIstituzione.HomePage <> "" Then
                        If InStr(oIstituzione.HomePage, "http://") > 0 Then
                            Me.LinkLogo.HRef = oIstituzione.HomePage
                        Else
                            Me.LinkLogo.HRef = "http://" & oIstituzione.HomePage
                        End If
                    Else
                        Me.LinkLogo.HRef = "#"
                        Me.LinkLogo.Attributes.Add("onclick", "Javascript:return false;")
                    End If
                    Return True
                End If
            End If
        Catch ex As Exception

        End Try
        Return False
    End Function
#End Region

End Class

'NOTE:
'Versione con il solo logo da inserire in chat_ext


