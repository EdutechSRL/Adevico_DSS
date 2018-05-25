Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Coordinamento
Imports lm.ActionDataContract

Public MustInherit Class PageBase
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
    Public ReadOnly Property PreLoadedContentView As lm.Comol.Core.DomainModel.ContentView
        Get
            If IsNumeric(Request.QueryString("CV")) Then
                Try
                    Return Request.QueryString("CV")
                Catch ex As Exception
                    Return lm.Comol.Core.DomainModel.ContentView.viewAll
                End Try
            Else
                Return lm.Comol.Core.DomainModel.ContentView.viewAll
            End If
        End Get
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


    ''' <summary>
    ''' TEMPORANEA PER LE VECCHIE PAGINE - RESTITUISCE SEMPRE TRUE, I PERMESSI SONO CALCOLATI DAL PRESENTERE
    ''' </summary>
    ''' <remarks></remarks>
    Public MustOverride Function HasPermessi() As Boolean Implements IviewGeneric.HasPermessi

    ''' <summary>
    ''' TEMPORANEA PER LE VECCHIE PAGINE
    ''' </summary>
    ''' <remarks></remarks>
    Public MustOverride Sub RegistraAccessoPagina() Implements IviewGeneric.RegistraAccessoPagina
    ''' <summary>
    ''' SERVE PER VISUALIZZARE LA VIEW PRESENTE NELLA MASTER PER MOSTRARE IL MESSAGGIO DEI NO PERMESSI
    ''' </summary>
    ''' <remarks></remarks>
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
            If COL_Comunita.ShowCover(DestinazioneID, Me.UtenteCorrente.Id) Then
                Me.RedirectToUrl("Modules/Repository/Cover.aspx")
            Else
                Me.RedirectToUrl(PlainRedirectToDefaultPage(DestinazioneID, Me.UtenteCorrente.Id))  ' se non faccio il redirect mi esegue prima il page_load dell'header e quindi vedo l'id della comunità a cui ero loggato e non quella corrente
            End If
            '	Response.Redirect("./../Comunita/comunita.aspx") ' se non eseguo il redirect non mi fa vedere il menu aggiornato
        End If
    End Sub
    Private Function TrovaComunitaDestinazione(ByVal ComunitaID As Integer) As Integer
        Dim oIscrizione As New COL_RuoloPersonaComunita
        Dim FoundComunitaID As Integer

        Try
            oIscrizione.Estrai(ComunitaID, Me.UtenteCorrente.Id)
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


    Public Sub WriteLogoutAccessCookie(item As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl)
        Dim cookie As New HttpCookie("LogoutAccess")
        Dim minutes As Long = 5

        cookie.Expires = Now.AddMinutes(minutes)
        cookie.Values.Add("Display", item.Display.ToString())
        cookie.Values.Add("CodeLanguage", item.CodeLanguage)
        cookie.Values.Add("IdCommunity", item.IdCommunity.ToString())
        cookie.Values.Add("IdLanguage", item.IdLanguage.ToString())
        cookie.Values.Add("IdPerson", item.IdPerson.ToString())
        cookie.Values.Add("Preserve", item.Preserve.ToString())
        cookie.Values.Add("GetType", item.GetType().ToString())
        cookie.Values.Add("DestinationUrl", item.DestinationUrl.Replace("&", "#_#"))
        cookie.Values.Add("PreviousUrl", item.PreviousUrl.Replace("&", "#_#"))
        cookie.Values.Add("IsForDownload", item.IsForDownload.ToString())
        cookie.Values.Add("SendToHomeDashboard", item.SendToHomeDashboard.ToString())
        If (From k In Response.Cookies.AllKeys Where k = "LogoutAccess").Any Then
            Response.Cookies.Set(cookie)
        Else
            Response.Cookies.Add(cookie)
        End If
    End Sub
    Public Function ReadLogoutAccessCookie() As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl
        Dim item As lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl = Nothing
        Dim values As New Hashtable
        Try
            For Each key As String In Request.Cookies("LogoutAccess").Values.Keys
                values(key) = Request.Cookies("LogoutAccess").Values(key)
            Next
        Catch ex As Exception

        End Try
        If Not IsNothing(values) Then
            If values.Item("GetType") = "lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl" Then
                item = New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl
                With item
                    .Display = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode).GetByString(values.Item("Display"), lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.None)
                    .CodeLanguage = values.Item("CodeLanguage")
                    If IsNumeric(values.Item("IdLanguage")) Then
                        .IdLanguage = CInt(values.Item("IdLanguage"))
                    End If
                    If IsNumeric(values.Item("IdCommunity")) Then
                        .IdCommunity = CInt(values.Item("IdCommunity"))
                    End If
                    If IsNumeric(values.Item("IdPerson")) Then
                        .IdPerson = CInt(values.Item("IdPerson"))
                    End If
                    Try
                        .Preserve = CBool(values.Item("Preserve"))
                    Catch ex As Exception

                    End Try
                    .DestinationUrl = values.Item("DestinationUrl")
                    If Not String.IsNullOrEmpty(.DestinationUrl) Then
                        .DestinationUrl = .DestinationUrl.Replace("#_#", "&")
                    End If
                    Try
                        .IsForDownload = CBool(values.Item("IsForDownload"))
                    Catch ex As Exception

                    End Try
                    Try
                        .SendToHomeDashboard = CBool(values.Item("SendToHomeDashboard"))
                    Catch ex As Exception

                    End Try
                    .PreviousUrl = values.Item("PreviousUrl")
                    If Not String.IsNullOrEmpty(.PreviousUrl) Then
                        .PreviousUrl = .PreviousUrl.Replace("#_#", "&")
                    End If
                End With
                If String.IsNullOrEmpty(item.DestinationUrl) Then
                    ClearLogoutAccessCookie()
                    item = Nothing
                End If
            End If
        End If
        Return item
    End Function

    Public Sub WriteLogoutAccessCookie(ByVal CommunityID As Integer, ByVal PersonID As Integer, ByVal PersonLogin As String, ByVal PostPage As String, ByVal ForDownload As Boolean, InPopupWindow As Boolean)
        Dim oHttpCookie As New HttpCookie("LogoutAccess")
        Dim minutes As Long = 5
        oHttpCookie.Expires = Now.AddMinutes(minutes)
        oHttpCookie.Values.Add("PersonID", PersonID)
        oHttpCookie.Values.Add("PersonLogin", PersonLogin)
        oHttpCookie.Values.Add("PostPage", Replace(PostPage, "&", "#_#"))
        oHttpCookie.Values.Add("Download", IIf(ForDownload AndAlso PostPage.Contains(".repository"), ForDownload, InPopupWindow))
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

    Public Function GetQuery(key As String, defvalue As String) As String
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As String = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function GetQuery(key As String, defvalue As Int32) As Int32
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As Int32 = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function GetQuery(key As String, defvalue As Int64) As Int64
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As Int64 = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function GetQuery(key As String, defvalue As Boolean) As Boolean
        Dim byquery As String = Server.UrlDecode(Request.QueryString(key))
        Dim result As Boolean = defvalue

        If (Page.IsPostBack) Then
            result = defvalue
        Else
            If (Not String.IsNullOrEmpty(byquery)) Then
                result = byquery
            Else
                result = defvalue
            End If
        End If

        Return result
    End Function
    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function


    Public Function GetExternalUsersLong() As Dictionary(Of String, Long)
        Dim result As New Dictionary(Of String, Long)
        If (From k In Session.Keys Where k = "TICKET.CurrentExtUser" Select k).Any() Then
            Dim tUser As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User = Nothing
            Try
                tUser = DirectCast(Session("TICKET.CurrentExtUser"), lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
            Catch ex As Exception
            End Try
            If Not IsNothing(tUser) Then
                result.Add(lm.Comol.Core.BaseModules.Tickets.ModuleTicket.UniqueCode, tUser.UserId)
            End If

        End If
        Return result
    End Function

#Region "DateTimeFormat"
    Protected Friend Function GetDateTimeString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Return GetDateToString(datetime, defaultString) & " " & GetTimeToString(datetime, defaultString)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetDateToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Dim pattern As String = Resource.CultureInfo.DateTimeFormat.ShortDatePattern
            If (pattern.Contains("yyyy")) Then
                pattern = pattern.Replace("yyyy", "yy")
            End If
            Return datetime.Value.ToString(pattern)
        Else
            Return defaultString
        End If
    End Function
    Protected Friend Function GetTimeToString(ByVal datetime As DateTime?, defaultString As String)
        If datetime.HasValue Then
            Return datetime.Value.ToString(Resource.CultureInfo.DateTimeFormat.ShortTimePattern)
        Else
            Return defaultString
        End If
    End Function
#End Region

End Class