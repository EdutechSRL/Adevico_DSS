Imports COL_BusinessLogic_v2
Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.FileLayer
Imports System.Collections.Generic
Imports COL_BusinessLogic_v2.UCServices
Imports COL_BusinessLogic_v2.UCServices.Services_Coordinamento
Imports lm.ActionDataContract
Imports lm.Comol.Modules.EduPath.Domain
Imports lm.Comol.UI.Presentation
Imports lm.Comol.Modules.EduPath.BusinessLogic

Public MustInherit Class EPcommonPagebaseEduPath
#Region "PageBase"
    Inherits Base
    Implements IviewGeneric
    Private _History As HistoryElement
    Private _Permission As Services_EduPath
    Private _Service As Services_EduPath

    Public ReadOnly Property Permission() As COL_BusinessLogic_v2.UCServices.Services_EduPath
        Get
            If IsNothing(_Permission) Then
                If Me.ComunitaCorrenteID = 0 Then
                    _Permission = New Services_EduPath()
                Else
                    _Permission = Me.ElencoServizi.Find(Services_EduPath.Codex)
                    If IsNothing(_Permission) Then
                        _Permission = New Services_EduPath(COL_Comunita.GetPermessiForServizioByPersona(MyBase.UtenteCorrente.ID, MyBase.ComunitaCorrenteID, Services_EduPath.Codex))
                    End If
                End If
            End If
            Permission = _Permission
        End Get
    End Property

    Public Function PermissionOtherCommunity(cmntid As Int32) As COL_BusinessLogic_v2.UCServices.Services_EduPath

        'If IsNothing(_Permission) Then
        If cmntid = 0 Then
            _Permission = New Services_EduPath()
        Else
            _Permission = Me.ElencoServizi.Find(Services_EduPath.Codex)
            If IsNothing(_Permission) Then
                _Permission = New Services_EduPath(COL_Comunita.GetPermessiForServizioByPersona(MyBase.UtenteCorrente.ID, cmntid, Services_EduPath.Codex))
            End If
        End If
        'End If
        Return _Permission

    End Function

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

  


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.VerifyAuthentication = False OrElse Not IsSessioneScaduta(Me.VerifyAuthentication) Then
            If Page.IsPostBack = False Then
                Me.SetInternazionalizzazione()
            End If

            If HasPermessi() Then
                If AlwaysBind Or (Not AlwaysBind And Me.IsPostBack = False) Then
                    If Not CheckModuleStatus OrElse AuthorizedByModuleStatus() Then
                        Me.BindDati()
                    End If
                    If Page.IsPostBack = False Then
                        Me.RegistraAccessoPagina()
                    End If
                End If
            Else
                Me.BindNoPermessi()
            End If
        End If
    End Sub

    Public MustOverride ReadOnly Property AlwaysBind() As Boolean
    Public MustOverride ReadOnly Property VerifyAuthentication() As Boolean

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
        Dim minutes As Long = 5
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

#Region "Context"
    Protected ReadOnly Property CurrentCommunityID() As Integer
        Get
            Dim qs_communityId As String = Request.QueryString("ComId")
            If IsNumeric(qs_communityId) Then
                Return qs_communityId
            Else
                Return PageUtility.CurrentContext.UserContext.CurrentCommunityID
            End If
        End Get
    End Property
    Protected _serviceEP As lm.Comol.Modules.EduPath.BusinessLogic.Service

    Protected ReadOnly Property ServiceEP As Service
        Get
            If IsNothing(_serviceEP) Then
                _serviceEP = New Service(PageUtility.CurrentContext)
            End If
            Return _serviceEP
        End Get
    End Property

    Protected _serviceAssignment As ServiceAssignment
    Protected ReadOnly Property ServiceAssignment As ServiceAssignment
        Get
            If IsNothing(_serviceAssignment) Then
                _serviceAssignment = ServiceEP.ServiceAssignments
            End If
            Return _serviceAssignment
        End Get
    End Property
    Protected _ServiceStat As ServiceStat
    Protected ReadOnly Property ServiceStat As ServiceStat
        Get
            If IsNothing(_ServiceStat) Then
                _ServiceStat = ServiceEP.ServiceStat
            End If
            Return _ServiceStat
        End Get
    End Property

    Private _ServiceCertification As lm.Comol.Core.Certifications.Business.CertificationsService
    Protected ReadOnly Property ServiceCF As lm.Comol.Core.Certifications.Business.CertificationsService
        Get
            If IsNothing(_ServiceCertification) Then
                _ServiceCertification = New lm.Comol.Core.Certifications.Business.CertificationsService(PageUtility.CurrentContext)
            End If
            Return _ServiceCertification
        End Get
    End Property
    Protected _ServiceRepository As lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository

    Protected ReadOnly Property ServiceRepository As lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository
        Get
            If IsNothing(_ServiceRepository) Then
                _ServiceRepository = New lm.Comol.Core.BaseModules.FileRepository.Business.ServiceRepository(PageUtility.CurrentContext)
            End If
            Return _ServiceRepository
        End Get
    End Property
#End Region

#Region "Preload"
    'Protected Friend ReadOnly Property PreloadIsMooc() As Boolean
    '    Get
    '        Dim isMoocObject As Boolean = False
    '        Boolean.TryParse(Request.QueryString("isMooc"), isMoocObject)
    '        Return isMoocObject
    '    End Get
    'End Property
    Protected Friend ReadOnly Property PreloadIsMooc() As Boolean
        Get
            Dim isMoocObject As Boolean = False
            Boolean.TryParse(Request.QueryString("isMooc"), isMoocObject)
            Return isMoocObject
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIsFromReadOnly() As Boolean
        Get
            Dim isFromReadOnly As Boolean = False
            Boolean.TryParse(Request.QueryString("freadonly"), isFromReadOnly)
            Return isFromReadOnly
        End Get
    End Property
    Protected Friend ReadOnly Property PreloadIsForReadOnly() As Boolean
        Get
            Dim isForReadOnly As Boolean = False
            Boolean.TryParse(Request.QueryString("readonly"), isForReadOnly)
            Return isForReadOnly
        End Get
    End Property

#End Region

#Region "QueryString"
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
#End Region

    ''' <summary>
    ''' Ricorsiva, toglie tutti i "br" a fine stringa (inseriti ad es. dall'editor telerik)
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function removeBRfromStringEnd(ByRef value As String) As String
        value = value.Trim
        If value.EndsWith("<br>") Then
            Return removeBRfromStringEnd(value.Remove(value.Length - 4)) 'vengono rimossi anche piu' br consecutivi
        Else
            Return value
        End If
    End Function

    Protected ReadOnly Property isAutoEp As Boolean
        Get
            Return ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Auto)

        End Get
    End Property
    Protected ReadOnly Property isTimeEp As Boolean
        Get
            Return ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Time)
        End Get
    End Property
    Protected ReadOnly Property isMarkEp As Boolean
        Get
            Return Not ServiceEP.CheckEpType(PathType, lm.Comol.Modules.EduPath.Domain.EPType.Time)
        End Get
    End Property
    Protected _PathType As EPType
    Protected MustOverride ReadOnly Property PathType As EPType


    'Protected ReadOnly Property _TimeStatTEMP As DateTime
    '    Get
    '        Return DateTime.Now
    '    End Get
    'End Property

#Region "hideControl"
    'Protected Sub hideControl(ByRef oControl As HtmlControls.HtmlGenericControl)
    '    oControl.Style("Display") = "None"
    'End Sub
    'Protected Sub hideControl(ByRef oControl As Label)
    '    oControl.Style("Display") = "None"
    'End Sub
    'Protected Sub hideControl(ByRef oControl As TextBox)
    '    oControl.Style("Display") = "None"
    'End Sub
    Protected Sub hideControl(ByRef oControl As Object)
        Try
            oControl.Visible = False
            oControl.Style("Display") = "None"
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub hideControl(ByRef oControl As HtmlGenericControl)
        Try
            oControl.Visible = False
            oControl.Style("Display") = "None"
        Catch ex As Exception

        End Try

    End Sub
#End Region

    Protected ReadOnly Property SubActivityTypeCssClass(item As SubActivityType) As String
        Get
            Select Case item
                Case SubActivityType.None
                    Return "generic"
                Case SubActivityType.File
                    Return "file"
                Case SubActivityType.Forum
                    Return "forum"
                Case SubActivityType.Quiz
                    Return "quiz"
                Case SubActivityType.Text
                    Return "assignment"
                Case SubActivityType.Wiki
                    Return "wiki"
                    'Case SubActivityType.Wik
                    '    Return "wiki"
                Case Else
                    Return "generic"
            End Select
        End Get
    End Property
    Protected ReadOnly Property SubActivityCssClass(item As dtoSubActivity) As String
        Get
            Return SubActivityTypeCssClass(item.ContentType)
        End Get
    End Property

    Public Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            ViewState(Key) = DefaultValue
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function
    Protected Sub RedirectOnSessionTimeOut(ByVal destinationUrl As String, ByVal idCommunity As Integer, Optional ByVal mustBeInCommunity As Boolean = True)
        If String.IsNullOrEmpty(destinationUrl) OrElse (mustBeInCommunity AndAlso idCommunity = 0) Then
            Response.Redirect(PageUtility.GetDefaultLogoutPage)
        Else
            Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
            Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
            dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

            If destinationUrl.StartsWith("http") Then
                Dim localPath As String = "http://" & Request.Url.Host & BaseUrl
                localPath = Replace(localPath, "///", "//")
                destinationUrl = destinationUrl.Replace(localPath, "")
                localPath = Replace(localPath, "http://", "https://")
                destinationUrl = destinationUrl.Replace(localPath, "")
            End If

            dto.DestinationUrl = destinationUrl

            If idCommunity > 0 Then
                dto.IdCommunity = idCommunity
            End If

            webPost.Redirect(dto)
        End If
    End Sub


    Private _Status As Dictionary(Of Integer, lm.Comol.Core.DomainModel.ModuleStatus)
    Private Function AuthorizedByModuleStatus() As Boolean
        Dim result As Boolean = True
        Dim idCommunity As Integer = CurrentCommunityID
        If IsNothing(_Status) Then
            _Status = New Dictionary(Of Integer, lm.Comol.Core.DomainModel.ModuleStatus)()
        End If
        If Not _Status.ContainsKey(idCommunity) Then
            _Status(idCommunity) = ServiceEP.GetEdupathServiceStatus(idCommunity)
        End If
        Select Case _Status(idCommunity)
            Case lm.Comol.Core.DomainModel.ModuleStatus.DisableForCommunity, lm.Comol.Core.DomainModel.ModuleStatus.DisableForSystem
                NotifyUnavailableModule(_Status(idCommunity))
                result = False
            Case lm.Comol.Core.DomainModel.ModuleStatus.DisableForCommunityAvailableForAdmin, lm.Comol.Core.DomainModel.ModuleStatus.DisableForSystemAvailableForAdmin
                NotifyModuleStatus(_Status(idCommunity))
        End Select
        Return result
    End Function
    Protected MustOverride Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
    Protected MustOverride Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
    Protected MustOverride ReadOnly Property CheckModuleStatus As Boolean

    'Protected Friend Sub SetIsMooc(ByVal idPath As Long)
    '    If Not _IsMoocPath.HasValue Then
    '        Dim idPath As Long = ServiceEP.GetPathId_ByActivityId(CurrentActivityID)
    '        If (idPath > 0) Then
    '            IsMoocPath = ServiceEP.IsMooc(idPath)
    '            If PreloadIsMooc AndAlso Not IsMoocPath Then
    '                IsMoocPath = PreloadIsMooc
    '            End If
    '        Else
    '            IsMoocPath = PreloadIsMooc
    '        End If
    '    End If
    'End Sub
    Protected Friend Function GetIsMoocPath(ByVal idPath As Long) As Boolean
        If (idPath > 0) Then
            Return ServiceEP.IsMooc(idPath)
        End If
        Return IsMoocPath
    End Function

    Protected Friend Function VerifyDuration(pathWeight As Long, pathWeightAuto As Long) As TimeError
        If pathWeight = pathWeightAuto Then
            Return TimeError.autoEqual
        ElseIf pathWeight > pathWeightAuto Then
            Return TimeError.autoLessThenPath
        ElseIf pathWeight < pathWeightAuto Then
            Return TimeError.autoOverThenPath
        End If
    End Function

    Protected Friend Enum TimeError As Integer
        none = 0
        autoLessThenPath = 1
        autoOverThenPath = 2
        autoEqual = 3
    End Enum

#Region "Internal"
    Protected MustOverride ReadOnly Property IsMoocPath() As Boolean
    Public Function GetCssFileByType() As String
        If IsMoocPath Then
            Return "mooc-"
        Else
            Return ""
        End If
    End Function
#End Region
End Class