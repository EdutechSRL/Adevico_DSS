Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports COL_BusinessLogic_v2.UCServices
Imports COL_Questionario

Public MustInherit Class BaseControlQuestionario
    Inherits BaseControlSession
    Implements IviewQuestionarioBase


    Private _Servizio As Services_Questionario

    Public Sub New()
        MyBase.New()
    End Sub

	'Public Property NewLinguaID() As Integer Implements IviewQuestionarioBase.NewLinguaID
	'    Get
	'        Try
	'            If IsNumeric(Session("NewLinguaID")) Then
	'                NewLinguaID = CInt(Session("NewLinguaID"))
	'            Else
	'                NewLinguaID = 0
	'            End If
	'        Catch ex As Exception
	'            NewLinguaID = 0
	'        End Try
	'    End Get
	'    Set(ByVal value As Integer)
	'        Session("NewLinguaID") = value
	'    End Set
	'End Property


    Public Property GruppoQuestionariID() As Integer Implements IviewQuestionarioBase.GruppoQuestionariID
        Get
            Try
                GruppoQuestionariID = DirectCast(Session("GruppoQuestionariID"), Integer)
            Catch ex As Exception
                GruppoQuestionariID = 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("GruppoQuestionariID") = value
        End Set
    End Property
    Public Property GruppoDefaultID() As Integer Implements IviewQuestionarioBase.GruppoDefaultID
        Get
            Try
                GruppoDefaultID = DirectCast(Session("GruppoDefaultID"), Integer)
            Catch ex As Exception

            End Try
        End Get
        Set(ByVal value As Integer)
            Session("GruppoDefaultID") = value
        End Set
    End Property

    Public ReadOnly Property Invito() As COL_Questionario.UtenteInvitato Implements IviewQuestionarioBase.Invito
        Get
            Try
                Invito = DirectCast(Session("Invito"), COL_Questionario.UtenteInvitato)
            Catch ex As Exception
                Invito = New COL_Questionario.UtenteInvitato(0)
            End Try
        End Get
    End Property

    Public Property QuestionarioCorrente() As COL_Questionario.Questionario Implements IviewQuestionarioBase.QuestionarioCorrente
        Get
            Try
                QuestionarioCorrente = DirectCast(Session("QuestionarioCorrente"), COL_Questionario.Questionario)
            Catch ex As Exception
                QuestionarioCorrente = Nothing
            End Try
        End Get
        Set(ByVal value As COL_Questionario.Questionario)
            Session("QuestionarioCorrente") = value
        End Set
    End Property

    Public Property DomandaCorrente() As COL_Questionario.Domanda Implements IviewQuestionarioBase.DomandaCorrente
        Get
            Try
                DomandaCorrente = DirectCast(Session("DomandaCorrente"), COL_Questionario.Domanda)
            Catch ex As Exception
                DomandaCorrente = Nothing
            End Try
        End Get
        Set(ByVal value As COL_Questionario.Domanda)
            Session("DomandaCorrente") = value
        End Set
    End Property

    Public Property PaginaCorrenteID() As Integer Implements IviewQuestionarioBase.PaginaCorrenteID
        Get
            Try
                PaginaCorrenteID = Session("idPagina")
            Catch ex As Exception
                PaginaCorrenteID = 0
            End Try
        End Get
        Set(ByVal value As Integer)
            Session("idPagina") = value
        End Set
    End Property

    Public ReadOnly Property Servizio() As COL_BusinessLogic_v2.UCServices.Services_Questionario Implements IviewQuestionarioBase.Servizio
        Get
            If IsNothing(_Servizio) Then
                _Servizio = Me.ElencoServizi.Find(Services_Questionario.Codex)
            End If
            Servizio = _Servizio
        End Get
    End Property

    Public Property LibreriaCorrente() As COL_Questionario.Questionario Implements IviewQuestionarioBase.LibreriaCorrente
        Get
            Try
                LibreriaCorrente = DirectCast(Session("LibreriaCorrente"), COL_Questionario.Questionario)
            Catch ex As Exception
                LibreriaCorrente = Nothing
            End Try
        End Get
        Set(ByVal value As COL_Questionario.Questionario)
            Session("LibreriaCorrente") = value
        End Set
    End Property

    Public Property GruppoCorrente() As COL_Questionario.QuestionarioGruppo Implements IviewQuestionarioBase.GruppoCorrente
        Get
            Try
                GruppoCorrente = DirectCast(Session("GruppoCorrente"), COL_Questionario.QuestionarioGruppo)
            Catch ex As Exception
                GruppoCorrente = Nothing
            End Try
        End Get
        Set(ByVal value As COL_Questionario.QuestionarioGruppo)
            Session("GruppoCorrente") = value
        End Set
    End Property

    Public Overrides Function IsSessioneScaduta() As Boolean
        Dim isScaduta As Boolean = True

        If Not IsNothing(Me.UtenteCorrente) Then
            If Me.UtenteCorrente.Id > 0 Then
                isScaduta = False
            End If
        ElseIf Me.EncryptedQueryString("idQ", SecretKeyUtil.EncType.Questionario) <> "" Then
            Try
                If CInt(Me.EncryptedQueryString("idQ", SecretKeyUtil.EncType.Questionario)) > 0 Then
                    isScaduta = False
                End If
            Catch ex As Exception

            End Try
        End If
        If isScaduta Then
            Dim alertMSG As String
            alertMSG = Me.Resource.getValue("LogoutMessage")
            If alertMSG <> "" Then
                alertMSG = alertMSG.Replace("'", "\'")
            Else
                alertMSG = "Session timeout"
			End If
            '    Dim UrlRedirect As String = MyBase.ApplicationUrlBase & Me.SystemSettings.Presenter.DefaultStartPage
            Dim UrlRedirect As String = PageUtility.GetDefaultLogoutPage ' Me.DefaultUrl
			Response.Write("<script language='javascript'>function AlertLogout(Messaggio,pagina){" & vbCrLf & "alert(Messaggio);" & vbCrLf & "document.location.replace(pagina);" & vbCrLf & "} " & vbCrLf & "AlertLogout('" & alertMSG & "','" & UrlRedirect & "');</script>")
            isScaduta = True
        ElseIf Me.isPortalCommunity And Me.ComunitaCorrenteID > 0 Then
            MyBase.ExitToLimbo()
            isScaduta = True
        End If
        Return isScaduta
    End Function
    Public MustOverride Sub SetControlliByPermessi()
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.SetInternazionalizzazione()
            SetControlliByPermessi()
        End If
    End Sub

#Region "Querystring"
    Public Const qs_ownerType As String = "owType="
    Public Const qs_questType As String = "type="
    Public Const qs_owner As String = "owId="
    Public Const qs_ownerG As String = "owGUID="
    Public Const qs_quest As String = "IdQ="
    Public Const qs_Persona As String = "IdP="

    Public ReadOnly Property qs_questTypeId() As String
        Get
            Return Request.QueryString("type")
        End Get
    End Property
    Public ReadOnly Property qs_ownerTypeId() As String
        Get
            Dim QueryStringItem As String = Request.QueryString("owType")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return "0"
            End If
        End Get
    End Property
    Public ReadOnly Property qs_ownerId() As String
        Get
            Dim QueryStringItem As String = Request.QueryString("owId")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return "0"
            End If
        End Get
    End Property
    Public ReadOnly Property qs_ownerGUID() As System.Guid
        Get
            Dim QueryStringItem As String = Request.QueryString("owGUID")
            If RootObject.isGUID(QueryStringItem) Then
                Return New System.Guid(QueryStringItem)
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property qs_questId() As Integer
        Get
            Dim QueryStringItem As String = Request.QueryString("IdQ")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property qs_PersonaId() As Integer
        Get
            Dim QueryStringItem As String = Request.QueryString("IdP")
            If IsNumeric(QueryStringItem) Then
                Return QueryStringItem
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property GetOwnerQueryString() As String
        Get
            Dim query As String = ""
            If Not String.IsNullOrEmpty(Request.QueryString("owId")) And IsNumeric(Request.QueryString("owId")) Then
                query = "&owId=" & Request.QueryString("owId")
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("owType")) And IsNumeric(Request.QueryString("owType")) Then
                query &= "&owType=" & Request.QueryString("owType")
            End If
            If Not String.IsNullOrEmpty(Request.QueryString("owGUID")) Then
                If RootObject.isGUID(Request.QueryString("owGUID")) Then
                    query &= "&owGUID=" & Request.QueryString("owGUID")
                End If
            End If
            Return query
        End Get
    End Property
#End Region
    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
End Class