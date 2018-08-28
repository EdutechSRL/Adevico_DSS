Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports COL_Questionario
Imports System.Web.UI.WebControls
Imports System.Math
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Web
Imports CrystalDecisions.Shared


Public Class GestioneStampa
    Inherits PageBaseQuestionario

    Public Sub creaReportElencoUtenti(ByVal oUtenti As List(Of UtenteInvitato))
        Dim message As String = ""
        Dim cr As New ReportDocument

        Try
            cr.Load(Server.MapPath(RootObject.ReportElencoUtentiInvitati))

            cr.SetDataSource(oUtenti)

            cr.SetParameterValue("nomeQuestionario", Me.QuestionarioCorrente.nome)

            cr.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.Current.Response, True, "ElencoUtenti")

        Catch ex As Exception
            message = ex.Message
        End Try
    End Sub

    Public Sub creaReportFogliUtentiInvitatiDomande(ByVal oUtenti As List(Of UtenteInvitato), ByVal baseurl As String)
        Dim message As String = ""
        Dim oAllDomande As New List(Of Domanda)

        For Each oUt As UtenteInvitato In oUtenti
            Dim oDomandeUt As New List(Of Domanda)
            oDomandeUt = DALUtenteInvitato.readDomandeQuestRandomUtenteInvitato(Me.QuestionarioCorrente.id, oUt.ID)
            oAllDomande.AddRange(oDomandeUt)
        Next

        Dim cr As New ReportDocument

        Try
            cr.Load(Server.MapPath(RootObject.ReportFogliUtentiInvitatiDomande))

            cr.SetDataSource(oUtenti)

            cr.Subreports(0).SetDataSource(oAllDomande)

            cr.SetParameterValue("nomeQuestionario", Me.QuestionarioCorrente.nome)

            cr.SetParameterValue("autoreQuestionario", Me.QuestionarioCorrente.creator)

            cr.SetParameterValue("urlQuestionario", baseurl & COL_Questionario.RootObject.urlQuestionarioEsame)

            cr.ExportToHttpResponse(ExportFormatType.PortableDocFormat, HttpContext.Current.Response, True, "FogliUtenti")

        Catch ex As Exception
            message = ex.Message
        End Try
    End Sub


#Region "metodi non usati"

    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean

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

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub
#End Region

    Public Overrides ReadOnly Property LoadDataByUrl As Boolean
        Get
            Return False
        End Get
    End Property
End Class