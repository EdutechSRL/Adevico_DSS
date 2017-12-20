Imports COL_BusinessLogic_v2.Comunita
Imports COL_Questionario
Partial Public Class QuestionarioPageUI
    Inherits System.Web.UI.MasterPage
    
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
        PHHeader.Controls.Add(Page.LoadControl(RootObject.ucHeaderCompile))
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