Namespace Configuration

    <Serializable(), CLSCompliant(True)> Public Class TopBarSetting

        Private _Tools As IList(Of TBS_Tool)
        Private _ExtHelps As IList(Of TBS_Tool)

        Private _LanguageUrl As Url
        Private _Languages As IDictionary(Of Integer, String)

        Private _HomeUrl As Url
        Private _LogoutUrl As Url
        Private _HelpUrl As Url
        Private _UserUrl As Url

        Private _Order As IList(Of TBS_Order)

        Private _LanguageLoginUrl As Url
        'Private _LoginCss As String


        ''' <summary>
        ''' Elenco degli elementi del menu strumenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExtHelps As IList(Of TBS_Tool)
            Get
                Return _ExtHelps
            End Get
            Set(ByVal value As IList(Of TBS_Tool))
                _ExtHelps = value
            End Set
        End Property

        ''' <summary>
        ''' Elenco degli elementi del menu strumenti
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Tools As IList(Of TBS_Tool)
            Get
                Return _Tools
            End Get
            Set(ByVal value As IList(Of TBS_Tool))
                _Tools = value
            End Set
        End Property

        ''' <summary>
        ''' Url a cui reindirizzare al cambio di lingua (di sistema)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LanguageUrl As Url
            Get
                Return _LanguageUrl
            End Get
            Set(ByVal value As Url)
                _LanguageUrl = value
            End Set
        End Property

        ''' <summary>
        ''' Elenco delle lingue disponibili.
        ''' </summary>
        ''' <value>
        ''' Key (Integer) = Id Lingua
        ''' Value (String) = Nome visualizzato della lingua
        ''' </value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Languages As IDictionary(Of Integer, String)
            Get
                Return _Languages
            End Get
            Set(ByVal value As IDictionary(Of Integer, String))
                _Languages = value
            End Set
        End Property

        Public Property HomeUrl As Url
            Get
                Return _HomeUrl
            End Get
            Set(ByVal value As Url)
                _HomeUrl = value
            End Set
        End Property

        Public Property LogoutUrl As Url
            Get
                Return _LogoutUrl
            End Get
            Set(ByVal value As Url)
                _LogoutUrl = value
            End Set
        End Property

        Public Property HelpUrl As Url
            Get
                Return _HelpUrl
            End Get
            Set(ByVal value As Url)
                _HelpUrl = value
            End Set
        End Property

        Public Property UserUrl As Url
            Get
                Return _UserUrl
            End Get
            Set(ByVal value As Url)
                _UserUrl = value
            End Set
        End Property

        Public Property Order As IList(Of TBS_Order)
            Get
                Return _Order
            End Get
            Set(ByVal value As IList(Of TBS_Order))
                _Order = value
            End Set
        End Property

        Public Property LanguageLoginUrl As Url
            Get
                Return _LanguageLoginUrl
            End Get
            Set(ByVal value As Url)
                _LanguageLoginUrl = value
            End Set
        End Property

        'Public Property LoginCss As String
        '    Get
        '        Return _LoginCss
        '    End Get
        '    Set(ByVal value As String)
        '        _LoginCss = value
        '    End Set
        'End Property


        Public Sub New()
            Me.Languages = New Dictionary(Of Integer, String)
            Me.Tools = New List(Of TBS_Tool)
            Me.ExtHelps = New List(Of TBS_Tool)
        End Sub

        ''' <summary>
        ''' Crea una stringa html con il codice per la visualizzazione della Top Bar
        ''' </summary>
        ''' <param name="Parameter"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' Esclusa la barra Memo/Chat
        ''' Vedere poi dove riposizionare il codice
        ''' </remarks>
        Public Function Render(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""

            'With Parameter
            Html &= "<ul>"

            Dim CanRender As Boolean = True
            Try
                CanRender = Not IsNothing(Order) OrElse (Order.Count > 0)
            Catch ex As Exception
                CanRender = False
            End Try

            If CanRender Then
                For Each item As TBS_Order In Order
                    Select Case item.Name
                        Case "Welcome"
                            Html &= RenderWelcome(Parameter)
                        Case "Home"
                            Html &= RenderHome(Parameter)
                        Case "Strumenti"
                            Html &= RenderTools(Parameter)
                        Case "Languages"
                            Html &= RenderLanguages(Parameter)
                        Case "Help"
                            Html &= RenderHelp(Parameter)
                        Case "Logout"
                            Html &= RenderLogOut(Parameter)
                    End Select
                Next

            Else
                Html &= RenderWelcome(Parameter)
                Html &= RenderHome(Parameter)
                Html &= RenderTools(Parameter)
                Html &= RenderLanguages(Parameter)
                Html &= RenderHelp(Parameter)
                Html &= RenderLogOut(Parameter)
            End If

            Html &= "</ul>"
            'End With

            Return Html
        End Function

        Private Function RenderWelcome(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""

            With Parameter
                'Welcom
                Html &= "    <li id=""greetings"">"
                Html &= "        <span>" & .Welcome_t & ",</span>"
                Html &= "        <strong>"
                Html &= Me.RenderUrl(Me.UserUrl, If(Me.UserUrl.IsHttps, .BaseUrlHttps, .BaseUrl), .UserName, .Profilo_t, .PersonTypeId)
                'Html &= "            <a href="""
                'Html &= Me.UserUrl.Url
                ''& .BaseUrl & Me.UserUrl 
                'Html &= """ Tooltip=""" &  & """>" & .UserName & "</a>"
                Html &= "        </strong>"
                Html &= "    </li>"
            End With

            Return Html
        End Function
        Private Function RenderHome(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""

            With Parameter
                Html &= "    <li id=""Top_Home"">"

                Html &= Me.RenderUrl(Me.HomeUrl, If(Me.HomeUrl.IsHttps, .BaseUrlHttps, .BaseUrl), .Home_t, .Home_t, .PersonTypeId, True, False, "Home")

                'Html &= "        <a href="""
                'Html &= If(Me.HomeUrl.IsHttps, .BaseUrlHTTPS, .BaseUrl)
                'Html &= Me.HomeUrl.Url
                'Html &= """ Tooltip=""" & .Home_t & """ class=""Home"">" & .Home_t & "</a>"
                Html &= "    </li>"
            End With

            Return Html
        End Function
        Private Function RenderTools(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""

            With Parameter
                Html &= "    <li id=""Top_Tools"">"
                Html &= "        <a class=""menu"" href=""#"">" & .Tool_t & "</a>"

                Html &= "        <ul class=""sub"">"

                For Each Tool As TBS_Tool In Me.Tools
                    Html &= RenderToolElements(Parameter, Tool)
                Next

                Html &= "        </ul>"
                Html &= "    </li>"
            End With

            Return Html
        End Function

        Private Function RenderToolElements(ByVal Parameter As RenderParameter_DTO, ByVal Tool As TBS_Tool) As String

            Dim HTML As String = ""

            With Parameter
                Dim IntName As String = ""

                If Tool.LocalNames.ContainsKey(.LanguageId) Then
                    IntName = Tool.LocalNames(.LanguageId)
                End If

                If IntName = "" Then
                    If Tool.LocalNames.ContainsKey(-1) Then
                        IntName = Tool.LocalNames(-1)
                    Else
                        IntName = ""
                    End If

                End If

                If Not IntName = "" AndAlso Tool.Enabled AndAlso Not IsNothing(Tool.Url) Then

                    ' APERTURA LI
                    If (IsNothing(Tool.Url.DisabledTypeIds) OrElse Not (Tool.Url.DisabledTypeIds.Contains(.PersonTypeId))) Then
                        HTML &= "            <li>"
                    Else
                        HTML &= "            <li class=""Menu_Disable"">"
                    End If

                    'ELEMENTO
                    If (Tool.Code = "SysPassword") AndAlso Tool.HasAccessByAuthenticationProvider(.AutenticationProviderTypes) Then
                        HTML &= Me.RenderUrl(Tool.Url, If(Tool.Url.IsHttps, .BaseUrlHttps, .BaseUrl), IntName, IntName, .PersonTypeId)

                    ElseIf (Tool.Code = "SysSettings") AndAlso Tool.HasAccessByAuthenticationProvider(.AutenticationProviderTypes) Then
                        HTML &= Me.RenderUrl(Tool.Url, If(Tool.Url.IsHttps, .BaseUrlHttps, .BaseUrl), IntName, IntName, .PersonTypeId)

                    ElseIf Tool.Code.StartsWith("Sys") AndAlso Tool.HasAccessByAuthenticationProvider(.AutenticationProviderTypes) Then
                        HTML &= Me.RenderUrl(Tool.Url, If(Tool.Url.IsHttps, .BaseUrlHttps, .BaseUrl), IntName, IntName, .PersonTypeId)

                    ElseIf Tool.Enabled AndAlso (IsNothing(Tool.PersonTypeIds) OrElse (Tool.PersonTypeIds.Contains(.PersonTypeId))) Then 'Andalso (IsNothing(Tool.AutenticationIds) orelse Tool.AutenticationIds.Contains(PERSONID)!!!
                        Dim Enabled As Boolean = False
                        If Tool.ServiceId = 0 OrElse .ActiveServices.Contains(Tool.Code) Then
                            Enabled = True
                        End If
                        HTML &= Me.RenderUrl(Tool.Url, If(Tool.Url.IsHttps, .BaseUrlHttps, .BaseUrl), IntName, IntName, .PersonTypeId, Enabled, Tool.Url.IsTargetBlank)
                    End If
                    'CHIUSURA LI
                    HTML &= "</li>"
                End If
            End With
            Return HTML
        End Function


        Private Function RenderLanguages(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""


            With Parameter
                Dim LangHtml As String = ""
                Dim HeadLangHtml As String = ""

                LangHtml &= "        <ul class=""sub"">"
                Dim lang As KeyValuePair(Of Integer, String)
                For Each lang In Me.Languages

                    If lang.Key = .CurrentLanguageId Then
                        HeadLangHtml &= " <li id=""Top_Lang"">"
                        'HeadLangHtml &= " <a class=""menu"" href=""#"">" & lang.Value & "</a>"
                        Dim blankUrl As New Url()
                        blankUrl.Url = "#"
                        HeadLangHtml &= Me.RenderUrl(blankUrl, "", lang.Value, lang.Value, .PersonTypeId, True, False, "menu")
                        'Il LI rimane aperto per contenere l'UL con le altre lingue.
                        'LangHtml &= "<a href=""#"" title="""" class=""Lang_Current"">" & lang.Value & "</a>"
                    Else
                        LangHtml &= "<li>"

                        LangHtml &= Me.RenderUrl(Me.LanguageUrl, If(Me.LanguageUrl.IsHttps, .BaseUrlHttps, .BaseUrl), lang.Value, lang.Value, .PersonTypeId, True, , , lang.Key)

                        'LangHtml &= "<a href="""
                        'LangHtml &= If(Me.LanguageUrl.IsHttps, .BaseUrlHttps, .BaseUrl)
                        'LangHtml &= Me.LanguageUrl.Url
                        'LangHtml &= lang.Key & """ title="""">" & lang.Value & "</a>"
                        LangHtml &= "</li>"
                    End If


                Next

                LangHtml &= "        </ul>"
                LangHtml &= " </li>"

                'SE la lingua corrente non è tra le lingue in elenco...
                If HeadLangHtml = "" Then
                    HeadLangHtml &= " <li>"
                    HeadLangHtml &= Me.RenderUrl(Me.LanguageUrl, If(Me.LanguageUrl.IsHttps, .BaseUrlHttps, .BaseUrl), lang.Value, lang.Value, .PersonTypeId, False)
                    '<a class=""menu"" href=""#"">" & .LanguageDef_t & "</a>" 
                    'Il LI rimane aperto per contenere l'UL con le altre lingue.
                End If

                Html &= HeadLangHtml & LangHtml
            End With

            Return Html
        End Function
        Public Function RenderLoginLanguages(ByVal BaseUrl As String, ByVal BaseUrlHttps As String, ByVal CurrentLanguageId As Integer, ByVal LanguageDef_t As String) As String
            Dim Html As String = ""


            'With Parameter
            Dim LangHtml As String = ""
            Dim HeadLangHtml As String = ""

            LangHtml &= "        <ul class=""sub"">"
            Dim lang As KeyValuePair(Of Integer, String)
            For Each lang In Me.Languages

                If lang.Key = CurrentLanguageId Then
                    HeadLangHtml &= " <li id=""Top_Lang""><a class=""menu"" href=""#"">" & lang.Value & "</a>" 'Il LI rimane aperto per contenere l'UL con le altre lingue.
                    'LangHtml &= "<a href=""#"" title="""" class=""Lang_Current"">" & lang.Value & "</a>"
                Else
                    LangHtml &= "<li>"
                    LangHtml &= "<a href="""
                    LangHtml &= If(Me.LanguageLoginUrl.IsHttps, BaseUrlHttps, BaseUrl)
                    LangHtml &= Me.LanguageLoginUrl.Url
                    LangHtml &= lang.Key & """ title="""">" & lang.Value & "</a>"
                    LangHtml &= "</li>"
                End If


            Next

            LangHtml &= "        </ul>"
            LangHtml &= " </li>"

            'SE la lingua corrente non è tra le lingue in elenco...
            If HeadLangHtml = "" Then
                HeadLangHtml &= " <li><a class=""menu"" href=""#"">" & LanguageDef_t & "</a>" 'Il LI rimane aperto per contenere l'UL con le altre lingue.
            End If

            Html &= HeadLangHtml & LangHtml
            'End With

            Return Html
        End Function
        Public Function RenderHelp(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""

            With Parameter
                Html &= " <li id=""Top_Help"">"

                If Not IsNothing(ExtHelps) AndAlso ExtHelps.Count() > 0 Then

                    Html &= " <a class=""menu"" href=""#"">" & .Help_t & "</a>"
                    Html &= "        <ul class=""sub"">"

                    For Each ExtHelp As TBS_Tool In Me.ExtHelps
                        Html &= RenderToolElements(Parameter, ExtHelp)
                    Next

                    Html &= "        </ul>"
                Else
                    Html &= Me.RenderUrl(Me.HelpUrl, If(Me.HelpUrl.IsHttps, .BaseUrlHttps, .BaseUrl), .Help_t, .Help_t, .PersonTypeId, True, True)
                End If


                Html &= "</li>"
            End With

            Return Html
        End Function
        Private Function RenderLogOut(ByVal Parameter As RenderParameter_DTO) As String
            Dim Html As String = ""

            With Parameter
                Html &= " <li id=""Top_Logout"">"
                'Html &= "     <a href="""
                'Html &= If(Me.LogoutUrl.IsHttps, .BaseUrlHTTPS, .BaseUrl)
                'Html &= Me.LogoutUrl.Url
                'Html &= """>" & .Logout_t & "</a>" 
                Html &= Me.RenderUrl(Me.LogoutUrl, If(Me.LogoutUrl.IsHttps, .BaseUrlHttps, .BaseUrl), .Logout_t, .Logout_t, .PersonTypeId, True)
                Html &= "</li>"
            End With

            Return Html
        End Function

        Private Function RenderUrl(ByVal oUrl As Url, ByVal BaseUrl As String, ByVal Text As String, ByVal ToolTip As String, ByVal UserType As Integer, Optional ByVal IsEnable As Boolean = True, Optional ByVal TargetBlank As Boolean = False, Optional ByVal CssClass As String = "", Optional ByVal UrlParameter As String = "") As String

            Dim UrlStr As String

            If IsEnable AndAlso (IsNothing(oUrl.DisabledTypeIds) OrElse Not (oUrl.DisabledTypeIds.Contains(UserType))) Then
                UrlStr = "            <a href="""
                UrlStr &= BaseUrl & oUrl.Url & UrlParameter & """"
                UrlStr &= " Title=""" & ToolTip & """"

                If TargetBlank Then
                    UrlStr &= " target=""_blank"""
                End If

                If CssClass <> "" Then
                    UrlStr &= " class=""" & CssClass & """"
                End If

                UrlStr &= ">" & Text & "</a>"
            Else
                UrlStr = "<span class=""Menu_Disable"" title=""" & ToolTip & """>" & Text & "</span>"
            End If

            Return UrlStr
        End Function
    End Class



    ''' <summary>
    ''' Rappresenta la singola voce associata ad un servizio
    ''' </summary>
    ''' <remarks>
    ''' Valido anche per le voci di sistema, non associate a servizi
    ''' </remarks>
    <Serializable(), CLSCompliant(True)> Public Class TBS_Tool
        Public Sub New()
            'AutenticationIds = New Integer()
            'PersonTypeIds = New List(Of Integer)
            LocalNames = New Dictionary(Of Integer, String)
            _Enabled = True
        End Sub

        Private _ServiceId As Integer
        Private _Code As String
        Private _AutendicationIds As List(Of Integer)
        Private _PersonTypeIds As Integer()
        Private _Url As Url
        Private _LocalNames As IDictionary(Of Integer, String)
        Private _Enabled As Boolean

        Public Property ServiceId As Integer
            Get
                Return _ServiceId
            End Get
            Set(ByVal value As Integer)
                _ServiceId = value
            End Set
        End Property

        ''' <summary>
        ''' Codice del servizio
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' "Sys_" è usato per i "servizi" di sistema (cambio Password, Impostazioni, etc...)
        ''' </remarks>
        Public Property Code As String
            Get
                Return _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property

        ''' <summary>
        ''' ID tipi di autenticazioni per i quali la voce è abilitata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AutenticationIds As List(Of Integer)
            Get
                Return _AutendicationIds
            End Get
            Set(ByVal value As List(Of Integer))
                _AutendicationIds = value
            End Set
        End Property

        ''' <summary>
        ''' ID tipi persona per i quali la voce è abilitata
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PersonTypeIds As Integer()
            Get
                Return _PersonTypeIds
            End Get
            Set(ByVal value As Integer())
                _PersonTypeIds = value
            End Set
        End Property

        ''' <summary>
        ''' Url a cui si viene rediretti 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Url As Url
            Get
                Return _Url
            End Get
            Set(ByVal value As Url)
                _Url = value
            End Set
        End Property

        Public Property Enabled As Boolean
            Get
                Return _Enabled
            End Get
            Set(value As Boolean)
                _Enabled = value
            End Set
        End Property

        ''' <summary>
        ''' Localizzazione dei nomi visualizzati
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LocalNames As IDictionary(Of Integer, String)
            Get
                Return _LocalNames
            End Get
            Set(ByVal value As IDictionary(Of Integer, String))
                _LocalNames = value
            End Set
        End Property


        Public Function HasAccessByAuthenticationProvider(idTypes As List(Of Integer)) As Boolean
            If IsNothing(AutenticationIds) Then
                Return True
            Else
                Return AutenticationIds.Where(Function(t) idTypes.Contains(t)).Any()
            End If
        End Function

    End Class

    <Serializable(), CLSCompliant(True)> _
    Public Class RenderParameter_DTO
        Public BaseUrl As String
        Private _BaseUrlHTTPS As String = ""
        Public Property BaseUrlHttps
            Get
                If _BaseUrlHTTPS = "" Then
                    _BaseUrlHTTPS = BaseUrl
                End If
                Return _BaseUrlHTTPS
            End Get
            Set(ByVal value)
                _BaseUrlHTTPS = value
            End Set
        End Property


        Public UserName As String
        'Public LinkProfile As String

        'Public LinkChangePassword As String
        'Public ChangePassword_t As String

        'Public LinkSettings As String
        'Public Settings_t As String

        'Internazionalizzazioni
        Public Welcome_t As String
        Public LanguageDef_t As String
        Public Help_t As String
        Public Profilo_t As String
        Public Home_t As String
        Public Tool_t As String
        Public Logout_t As String

        Public AutenticationProviderTypes As New List(Of Integer)
        Public PersonTypeId As Integer
        Public LanguageId As Integer
        Public ActiveServices() As String
        'Public Services As ServiziCorrenti

        Public CurrentLanguageId As Integer

    End Class

    <Serializable(), CLSCompliant(True)> Public Class TBS_Order
        Public Sub New()
        End Sub

        Private _ItemName As String
        Private _ItemOrder As Integer

        ''' <summary>
        ''' Nome dell'elemento del menu
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Il nome dev'essere quello definito...</remarks>
        Public Property Name As String
            Get
                Return _ItemName
            End Get
            Set(ByVal value As String)
                _ItemName = value
            End Set
        End Property

        ''' <summary>
        ''' Incrementale: ordine dei menu
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Order As Integer
            Get
                Return _ItemOrder
            End Get
            Set(ByVal value As Integer)
                _ItemOrder = value
            End Set
        End Property

    End Class

    Public Class Url
        Public Url As String
        Public IsHttps As Boolean
        Public DisabledTypeIds() As Integer
        Public IsTargetBlank As Boolean = False
    End Class
    ' ''' <summary>
    ' ''' Rappresenta una singola voce, associata ad una lingua
    ' ''' </summary>
    ' ''' <remarks></remarks>
    '<Serializable(), CLSCompliant(True)> Public Class TBS_LocalName
    '    Private _LanguageId As Integer
    '    Private _Text As String

    '    ''' <summary>
    '    ''' ID lingua
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Property Id As Integer
    '        Get
    '            Return _LanguageId
    '        End Get
    '        Set(ByVal value As Integer)
    '            _LanguageId = value
    '        End Set
    '    End Property

    '    ''' <summary>
    '    ''' Testo localizzato
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Property Text
    '        Get
    '            Return _Text
    '        End Get
    '        Set(ByVal value)
    '            _Text = value
    '        End Set
    '    End Property
    'End Class

End Namespace