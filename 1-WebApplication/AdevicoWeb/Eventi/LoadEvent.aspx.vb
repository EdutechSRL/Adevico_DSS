Imports COL_BusinessLogic_v2.Eventi

Partial Public Class LoadEvent
    Inherits PageBase


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication() As Boolean
        Get
            Return True
        End Get
    End Property
    Public Overrides Sub BindDati()
        If Me.ComunitaCorrenteID = Me.PreLoadedCommunityID AndAlso Me.PreLoadedEventID > 0 Then
            Dim oOrario As New COL_Orario With {.Id = Me.PreLoadedEventID}
            oOrario.Estrai()

            If oOrario.Errore = Errori_Db.None Then
                Session("ORRI_id") = Me.PreLoadedEventID
                Session("LBVisibileFine") = oOrario.Fine
                Session("LBVisibileInizio") = oOrario.Inizio
                If oOrario.Inizio.DayOfWeek = 0 Then
                    Session("dtInizioSett") = oOrario.Inizio.AddDays(-6)
                Else
                    Session("dtInizioSett") = oOrario.Inizio.AddDays(1 - oOrario.Inizio.DayOfWeek)
                End If
                Me.RedirectToUrl("Eventi/calendarioSettimanale.aspx")
            End If
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()

    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()

    End Sub

    Public Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides Sub ShowMessageToPage(ByVal errorMessage As String)

    End Sub

    Private _CommunityID As Integer
    Private _EventID As Integer
    Public ReadOnly Property PreLoadedCommunityID() As Integer
        Get
            Try
                If _CommunityID = 0 Then
                    _CommunityID = CInt(Request.QueryString("CommunityID"))
                End If
            Catch ex As Exception

            End Try
            Return _CommunityID
        End Get
    End Property
    Public ReadOnly Property PreLoadedEventID() As Integer
        Get
            Try
                If _EventID = 0 Then
                    _EventID = CInt(Request.QueryString("EventID"))
                End If
            Catch ex As Exception

            End Try
            Return _EventID
        End Get
    End Property


End Class