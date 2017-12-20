Imports COL_Questionario
Imports System.Collections.Generic
Imports Comol.Entity.Configuration

Partial Public Class EsameGestione
    Inherits PageBaseQuestionario


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub LNBQuestionarioAdmin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LNBQuestionarioAdmin.Click
        Me.RedirectToUrl(COL_Questionario.RootObject.QuestionarioAdmin + "?type=" + Me.QuestionarioCorrente.tipo.ToString() & "&IdQ=" & Me.QuestionarioCorrente.id & "&idLanguage=" & Me.QuestionarioCorrente.idLingua)
    End Sub

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return (MyBase.Servizio.Admin Or MyBase.Servizio.QuestionariSuInvito)
    End Function

    Public Overrides ReadOnly Property isCompileForm() As Boolean
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetControlliByPermessi()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EsameGestione", "Questionari")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLinkButton(LNBQuestionarioAdmin, False, False)
            Master.ServiceTitle = .getValue("")
        End With
    End Sub
    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Master.ShowDocType = True
    End Sub
End Class