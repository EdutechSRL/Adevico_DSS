Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Coordinamento
Imports lm.ActionDataContract
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.Standard.ForumModule.Domain
Imports lm.Comol.Modules.Standard.ForumModule.Business

Public MustInherit Class PageBaseForum
#Region "PageBase"
    Inherits Base
    Implements IviewGeneric
    Private _History As HistoryElement

    Sub New()
        MyBase.New()
    End Sub
    Public ReadOnly Property History() As HistoryElement Implements IviewBase.History
        Get
            If PageIstance = False Then
                Try
                    _History = LoadHystoryElement(0, 0)
                Catch ex As Exception
                    _History = Nothing
                End Try
            Else
                PageIstance = True
            End If
            History = _History
        End Get
    End Property
    Private Function LoadHystoryElement(ByVal Indice As Integer, ByVal PadreID As Integer) As HistoryElement
        Dim oElement As HistoryElement = Nothing

        Try
            If Indice <= UBound(Session("ArrComunita"), 2) Then
                Dim ID, RuoloID As Integer
                Dim nome As String = ""
                Dim percorso As String = ""
                Dim isAccessoLibero As Boolean = False
                ID = Session("ArrComunita")(0, Indice)
                nome = Session("ArrComunita")(1, Indice)
                percorso = Session("ArrComunita")(2, Indice)
                RuoloID = Session("ArrComunita")(3, Indice)
                isAccessoLibero = COL_Comunita.GetAccessoLibero(ID)

                If Me.isNonIscrittoComunita And isAccessoLibero And RuoloID = Main.TipoRuoloStandard.AccessoNonAutenticato Then
                    oElement = New HistoryElement(ID, PadreID, nome, percorso, RuoloID, LoadHystoryElement(Indice + 1, ID), isAccessoLibero)
                ElseIf RuoloID < 0 Then
                    oElement = LoadHystoryElement(Indice + 1, ID)
                Else
                    oElement = New HistoryElement(ID, PadreID, nome, percorso, RuoloID, LoadHystoryElement(Indice + 1, ID), isAccessoLibero)
                End If

            End If
        Catch ex As Exception
            oElement = Nothing
        End Try
        Return oElement
    End Function
    Public Property PostItSistema() As COL_BusinessLogic_v2.COL_PostIt Implements IviewBase.PostItSistema
        Get
            Try
                PostItSistema = DirectCast(Me.Application.Item("oSystemPostIt"), COL_PostIt)
            Catch ex As Exception
                PostItSistema = Nothing
                Me.Application.Item("ShowSystemPostIt") = False
            End Try
        End Get
        Set(ByVal value As COL_BusinessLogic_v2.COL_PostIt)
            Me.Application.Item("oSystemPostIt") = value
        End Set
    End Property
    Public Property ShowPostItSistema() As Boolean Implements IviewBase.ShowPostItSistema
        Get
            Try
                ShowPostItSistema = DirectCast(Me.Application.Item("ShowSystemPostIt"), Boolean)
            Catch ex As Exception
                Me.Application.Item("ShowSystemPostIt") = False
                ShowPostItSistema = False
            End Try
        End Get
        Set(ByVal value As Boolean)
            Me.Application.Item("ShowSystemPostIt") = value
        End Set
    End Property
    Public Property RiepilogoPostIt() As Integer Implements IviewBase.RiepilogoPostIt
        Get
            Try
                RiepilogoPostIt = DirectCast(Session("Popupwin"), Integer)
            Catch ex As Exception
                RiepilogoPostIt = 0
            End Try

        End Get
        Set(ByVal value As Integer)
            Session("Popupwin") = value
        End Set
    End Property

    Public Property UtentiConnessi() As Integer Implements IviewGeneric.UtentiConnessi
        Get
            Try
                UtentiConnessi = Me.Application.Item("utentiConnessi")
            Catch ex As Exception
                UtentiConnessi = 1
            End Try
        End Get
        Set(ByVal value As Integer)
            Me.Application.Lock()
            Me.Application.Item("utentiConnessi") = value
            Me.Application.UnLock()
        End Set
    End Property

    Public MustOverride ReadOnly Property AlwaysBind() As Boolean
    Public MustOverride ReadOnly Property VerifyAuthentication() As Boolean


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.VerifyAuthentication = False OrElse Not IsSessioneScaduta(Me.VerifyAuthentication) Then
            If Page.IsPostBack = False Then
                Me.SetInternazionalizzazione()
            End If

            If HasPermessi() Then
                If AlwaysBind Or (Not AlwaysBind And Me.IsPostBack = False) Then
                    Me.BindDati()
                    If Page.IsPostBack = False Then
                        Me.RegistraAccessoPagina()
                    End If
                End If
            Else
                Me.BindNoPermessi()
            End If
        End If
    End Sub



    Public MustOverride Function HasPermessi() As Boolean Implements IviewGeneric.HasPermessi
    Public MustOverride Sub RegistraAccessoPagina() Implements IviewGeneric.RegistraAccessoPagina

    Public MustOverride Sub BindNoPermessi() Implements IviewGeneric.BindNoPermessi




    Public Sub CambiaComunitaFromHistory(ByVal ComunitaID As Integer) Implements IviewBase.CambiaComunitaFromHistory
        Dim DestinazioneID As Integer
        DestinazioneID = TrovaComunitaDestinazione(ComunitaID)

        If DestinazioneID <= 0 Then
            Me.GoToPortale()
        ElseIf DestinazioneID > 0 Then

            Session("AdminForChange") = False
            Session("CMNT_path_forAdmin") = ""
            Session("idComunita_forAdmin") = ""
            Session("CMNT_path_forNews") = ""
            Session("CMNT_ID_forNews") = ""


            Me.RetrieveServices(Me.RuoloCorrenteID, ComunitaID)
            Session("ArrComunita") = Me.RemoveFromHystoryElement(0, Session("ArrComunita"), ComunitaID)
            If COL_Comunita.ShowCover(DestinazioneID, Me.UtenteCorrente.ID) Then
                Me.RedirectToUrl("Modules/Repository/Cover.aspx")
            Else
                Me.RedirectToUrl(PlainRedirectToDefaultPage(DestinazioneID, Me.UtenteCorrente.ID))  ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunità a cui ero loggato e non quella corrente
            End If
            '	Response.Redirect("./../Comunita/comunita.aspx") ' se non eseguo il redirect non mi fa vedere il menu aggiornato
        End If
    End Sub
    Private Function TrovaComunitaDestinazione(ByVal ComunitaID As Integer) As Integer
        Dim oIscrizione As New COL_RuoloPersonaComunita
        Dim FoundComunitaID As Integer

        Try
            oIscrizione.Estrai(ComunitaID, Me.UtenteCorrente.ID)
            If oIscrizione.Errore = Errori_Db.None AndAlso oIscrizione.Abilitato AndAlso oIscrizione.Attivato And (oIscrizione.TipoRuolo.Id = Main.TipoRuoloStandard.AccessoNonAutenticato Or oIscrizione.TipoRuolo.Id > 0) Then
                Dim oComunita As New COL_Comunita
                oComunita.Id = ComunitaID
                If (oComunita.isBloccata() = False) Then
                    Session("IdComunita") = ComunitaID
                    If Session("LogonAs") = False Then
                        oIscrizione.UpdateUltimocollegamento()
                    End If
                    Session("IdRuolo") = oIscrizione.TipoRuolo.Id
                    Session("RLPC_ID") = oIscrizione.Id
                    Return ComunitaID
                End If
            ElseIf ComunitaID > 0 Then
                FoundComunitaID = TrovaComunitaDestinazione(Me.History.FindFatherElementByID(ComunitaID).ComunitaID)
            End If
        Catch ex As Exception
            FoundComunitaID = ComunitaID
        End Try

        Return FoundComunitaID
    End Function
    Private Sub RetrieveServices(ByVal RuoloID As Integer, ByVal ComunitaID As Integer)
        Dim oListaServizi As GenericCollection(Of PlainServizioComunita)
        oListaServizi = PlainServizioComunita.ElencaByComunita(RuoloID, ComunitaID)

        If oListaServizi.Count > 0 Then
            Dim ArrPermessi(oListaServizi.Count - 1, 2) As String
            Dim indice As Integer = 0
            For Each oServizio As PlainServizioComunita In oListaServizi
                ArrPermessi(indice, 0) = oServizio.Codice
                ArrPermessi(indice, 1) = oServizio.ID
                ArrPermessi(indice, 2) = oServizio.Permessi
                indice += 1
            Next
            Session("ArrPermessi") = ArrPermessi
        Else
            Session("ArrPermessi") = Nothing
        End If
    End Sub

    Public Sub CambiaComunita(ByVal OrganizzazioneID As Integer, ByVal ComunitaID As Integer, ByVal TipoComunitaID As Integer, ByVal RuoloID As Integer, ByVal iListaServizi As ServiziCorrenti, ByVal iHistory As HistoryElement) Implements IviewBase.CambiaComunita
        Session("TPCM_ID") = TipoComunitaID
        Session("idComunita") = ComunitaID
        Session("IdRuolo") = RuoloID

        Me._History = iHistory
        Me.SetElencoServizi(iListaServizi)


        Dim ArrComunita(,) As String = Nothing
        Dim Indice As Integer = 0

        If Not IsNothing(iHistory) Then
            If iHistory.ComunitaID = ComunitaID Then
                ReDim Preserve ArrComunita(3, Indice)
                ArrComunita(0, Indice) = iHistory.ComunitaID
                ArrComunita(1, Indice) = iHistory.Nome
                ArrComunita(2, Indice) = iHistory.Percorso
                ArrComunita(3, Indice) = iHistory.RuoloID
            ElseIf Not IsNothing(iHistory.SubElement) Then
                ReDim Preserve ArrComunita(3, Indice)
                ArrComunita(0, Indice) = iHistory.ComunitaID
                ArrComunita(1, Indice) = iHistory.Nome
                ArrComunita(2, Indice) = iHistory.Percorso
                ArrComunita(3, Indice) = iHistory.RuoloID
                Me.LoadFromHystoryElement(Indice + 1, ArrComunita, iHistory.SubElement, ComunitaID)
            End If
        End If
        Session("ArrComunita") = ArrComunita
        Dim ArrPermessi(,) As String = Nothing
        Indice = 0
        If iListaServizi.Count > 0 Then
            ReDim Preserve ArrComunita(Indice, 2)
            For Each oElement As ServiceElement In iListaServizi
                ArrPermessi(Indice, 0) = oElement.ServizioCode
                ArrPermessi(Indice, 1) = oElement.ServizioID
                ArrPermessi(Indice, 2) = oElement.ServizioConcreto.PermessiAssociati
            Next
        End If
        Session("ArrPermessi") = ArrPermessi
        Session("ORGN_id") = OrganizzazioneID

    End Sub
    Private Sub LoadFromHystoryElement(ByVal Indice As Integer, ByVal ArrComunita(,) As String, ByVal oElement As HistoryElement, ByVal ComunitaID As Integer)
        If oElement.ComunitaID = ComunitaID Then
            ReDim Preserve ArrComunita(3, Indice)
            ArrComunita(0, Indice) = oElement.ComunitaID
            ArrComunita(1, Indice) = oElement.Nome
            ArrComunita(2, Indice) = oElement.Percorso
            ArrComunita(3, Indice) = oElement.RuoloID
        ElseIf Not IsNothing(oElement.SubElement) Then
            ReDim Preserve ArrComunita(3, Indice)
            ArrComunita(0, Indice) = oElement.ComunitaID
            ArrComunita(1, Indice) = oElement.Nome
            ArrComunita(2, Indice) = oElement.Percorso
            ArrComunita(3, Indice) = oElement.RuoloID
            Me.LoadFromHystoryElement(Indice + 1, ArrComunita, oElement.SubElement, ComunitaID)
        End If
    End Sub
    Private Function RemoveFromHystoryElement(ByVal Indice As Integer, ByVal ArrComunita(,) As String, ByVal ComunitaID As Integer) As String(,)
        Try
            If ArrComunita(0, Indice) = ComunitaID Then
                ReDim Preserve ArrComunita(3, Indice)
                Return ArrComunita
            Else
                Return Me.RemoveFromHystoryElement(Indice + 1, ArrComunita, ComunitaID)
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function



    Public Function ViewGetPostBackEventReference(ByVal argomenti As String) As String Implements IviewBase.ViewGetPostBackEventReference
        Return Page.ClientScript.GetPostBackEventReference(Me, argomenti)
    End Function

    Public Sub ShowAlertError(ByVal errorMessage As String) Implements IviewGeneric.ShowAlertError
        errorMessage = errorMessage.Replace("'", "\'")
        Me.ClientScript.RegisterStartupScript(Me.GetType(), "errorMessage", String.Format("window.alert('{0}');", errorMessage), True)
    End Sub
    Public Sub ShowAlertMessage(ByVal message As String) Implements IviewGeneric.ShowAlertMessage
        message = message.Replace("'", "\'")
        Me.ClientScript.RegisterStartupScript(Me.GetType(), "ShowMessage", String.Format("window.alert('{0}');", message), True)
    End Sub

    Public MustOverride Sub ShowMessageToPage(ByVal errorMessage As String) Implements IviewGeneric.ShowMessageToPage


    Public Sub WriteLogoutAccessCookie(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostPage As String, ByVal ForDownload As Boolean)
        Dim oHttpCookie As New HttpCookie("LogoutAccess")
        Dim minutes As Long = Me.SystemSettings.BlogSettings.ValidationTime
        oHttpCookie.Expires = Now.AddMinutes(minutes)
        oHttpCookie.Values.Add("PersonID", PersonID)
        oHttpCookie.Values.Add("PersonLogin", PersonLogin)
        oHttpCookie.Values.Add("PostPage", Replace(PostPage, "&", "#_#"))
        oHttpCookie.Values.Add("Download", ForDownload)
        oHttpCookie.Values.Add("CommunityID", CommunityID)
        If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
            Response.Cookies.Set(oHttpCookie)
        Else
            Response.Cookies.Add(oHttpCookie)
        End If
        'Dim secured As HttpCookie
        'Dim domain As String = Me.SystemSettings.BlogSettings.DomainCookie
        'Dim minutes As Long = Me.SystemSettings.BlogSettings.ValidationTime
        'Dim hash As New Hashtable()
        'hash.Add("PersonID", PersonID)
        'hash.Add("PersonLogin", PersonLogin)
        'hash.Add("PostPage", PostPage)
        'hash.Add("Download", ForDownload)
        'hash.Add("CommunityID", CommunityID)
        'hash.Add("expire", Now.AddMinutes(minutes))
        'hash.Add("domain", domain)
        'secured = SecuredCookie.encode_cookie("LogoutAccess", domain, minutes, hash)

        'If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
        '    Response.Cookies.Set(secured)
        'Else
        '    Response.Cookies.Add(secured)
        'End If
    End Sub
    Public Function ReadLogoutAccessCookie(ByVal PersonID As Integer, ByVal PersonLogin As String) As dtoLogoutAccess
        Dim iResponse As New dtoLogoutAccess
        Dim oValues As New Hashtable
        Try
            For Each key As String In Request.Cookies("LogoutAccess").Values.Keys
                oValues(key) = Request.Cookies("LogoutAccess").Values(key)
            Next
            '    SecuredCookie.decode_cookie(Request.Cookies("LogoutAccess"))
        Catch ex As Exception

        End Try
        If Not IsNothing(oValues) Then
            iResponse.CommunityID = oValues.Item("CommunityID")
            iResponse.PersonID = oValues.Item("PersonID")
            iResponse.PersonLogin = oValues.Item("PersonLogin")
            iResponse.isDownloading = oValues.Item("Download")
            If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = PersonID AndAlso iResponse.PersonLogin = PersonLogin)) OrElse iResponse.PersonID <= 0 Then
                If Not String.IsNullOrEmpty(oValues.Item("PostPage")) Then
                    iResponse.PageUrl = Replace(oValues.Item("PostPage"), "#_#", "&")
                End If
            Else
                iResponse.PageUrl = ""
            End If
        End If
        Return iResponse
        'Dim iResponse As New dtoLogoutAccess
        'Dim oValues As New Hashtable
        'Try
        '    oValues = SecuredCookie.decode_cookie(Request.Cookies("LogoutAccess"))
        'Catch ex As Exception

        'End Try
        'If Not IsNothing(oValues) Then
        '    iResponse.CommunityID = oValues.Item("CommunityID")
        '    iResponse.PersonID = oValues.Item("PersonID")
        '    iResponse.PersonLogin = oValues.Item("PersonLogin")
        '    iResponse.PageUrl = oValues.Item("PostPage")
        '    iResponse.isDownloading = oValues.Item("Download")
        '    If (iResponse.PersonID > 0 AndAlso (iResponse.PersonID = PersonID AndAlso iResponse.PersonLogin = PersonLogin)) OrElse iResponse.PersonID <= 0 Then
        '        iResponse.PageUrl = oValues.Item("Download")
        '    Else
        '        iResponse.PageUrl = ""
        '    End If
        'End If
        'Return iResponse
    End Function
    Public Sub ClearLogoutAccessCookie()
        Response.Cookies("LogoutAccess").Expires = Now.AddDays(-1)
    End Sub

#End Region

    Protected ReadOnly Property BuiltInGroup_byCRole() As Int64
        Get
            If IsNothing(Session("BuiltInGroup_C" & Me.CurrentCommunityID & "U" & CurrentUserId)) OrElse Not IsNumeric(Session("BuiltInGroup_C" & Me.CurrentCommunityID & "U" & CurrentUserId)) Then
                Session("BuiltInGroup_C" & Me.CurrentCommunityID & "U" & CurrentUserId) = ServiceFM.GetBuiltInGroup_byCRole(CurrentCommunityID)
            End If
            Return Session("BuiltInGroup_C" & Me.CurrentCommunityID & "U" & CurrentUserId)
        End Get
    End Property


    Protected ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return Me.CurrentContext.UserContext.CurrentCommunityID
            End If
        End Get
    End Property

    Protected _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Protected ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property

    Protected _serviceFM As ServiceForum

    Protected ReadOnly Property ServiceFM As ServiceForum
        Get
            If IsNothing(_serviceFM) Then
                _serviceFM = New ServiceForum(Me.CurrentContext)
            End If
            Return _serviceFM
        End Get
    End Property

    Protected Sub hideControl(ByRef oControl As Object)
        Try
            oControl.Visible = False
        Catch ex As Exception

        End Try
        oControl.Style("Display") = "None"
    End Sub

    Protected ReadOnly Property CurrentUserId() As Integer
        Get
            Return Me.CurrentContext.UserContext.CurrentUserID
        End Get
    End Property

End Class

