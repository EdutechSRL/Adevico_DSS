
Imports System.Web
Imports System.IO
Imports System.Xml
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI.WebControls
Imports System.Configuration
Imports Telerik
Imports COL_BusinessLogic_v2.Comol.Manager

Namespace Localizzazione
    Public NotInheritable Class ResourceManager

#Region "Private Property"
        Private _UserLanguages As String
        Private _CultureInfo As CultureInfo
        Private _ResourcesName As String
        Private _ResourcesLevel1 As String
        Private _ResourcesLevel2 As String
        Private _ResourcesLevel3 As String
        Private _DefaultCultureName As String
        Private _LocalizedValues As Hashtable
        Private _Errori As Errori_Db

#End Region

#Region "Public Property"
        Public Property UserLanguages() As String
            Get
                UserLanguages = _UserLanguages
            End Get
            Set(ByVal Value As String)
                _UserLanguages = Value
            End Set
        End Property
        Public ReadOnly Property CultureInfo() As CultureInfo
            Get
                CultureInfo = _CultureInfo
            End Get
        End Property
        Public Property ResourcesName() As String
            Get
                ResourcesName = _ResourcesName
            End Get
            Set(ByVal Value As String)
                _ResourcesName = Value
            End Set
        End Property
        Public Property Folder_Level1() As String
            Get
                Folder_Level1 = _ResourcesLevel1
            End Get
            Set(ByVal Value As String)
                _ResourcesLevel1 = Value
            End Set
        End Property
        Public Property Folder_Level2() As String
            Get
                Folder_Level2 = _ResourcesLevel2
            End Get
            Set(ByVal Value As String)
                _ResourcesLevel2 = Value
            End Set
        End Property
        Public Property Folder_Level3() As String
            Get
                Folder_Level3 = _ResourcesLevel3
            End Get
            Set(ByVal Value As String)
                _ResourcesLevel3 = Value
            End Set
        End Property
        Public ReadOnly Property Errore() As Errori_Db
            Get
                Errore = _Errori
            End Get
        End Property
        Public ReadOnly Property CurrentCultureName() As String
            Get
                ' Return System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Try
                    Return _CultureInfo.Name
                Catch ex As Exception
                    Return Me._DefaultCultureName
                End Try

            End Get
        End Property
        Public ReadOnly Property DefaultCultureName() As String
            Get
                Return Me._DefaultCultureName
            End Get
        End Property
        Public ReadOnly Property LanguageFilePath() As String
            Get
                Return System.Configuration.ConfigurationManager.AppSettings("languageFilePath")
            End Get
        End Property
        Public ReadOnly Property CompleteFilePath() As String
            Get
                Dim fileName As String = ""
                If _ResourcesLevel1 = "" Then
                    fileName = Me.LanguageFilePath & "\" & Me.CurrentCultureName & "\" & _ResourcesName & ".xml"
                ElseIf _ResourcesLevel1 <> "" And _ResourcesLevel2 = "" Then
                    fileName = Me.LanguageFilePath & "\" & Me.CurrentCultureName & "\" & _ResourcesLevel1 & "\" & _ResourcesName & ".xml"
                ElseIf _ResourcesLevel1 <> "" And _ResourcesLevel2 <> "" And _ResourcesLevel3 = "" Then
                    fileName = Me.LanguageFilePath & "\" & Me.CurrentCultureName & "\" & _ResourcesLevel1 & "\" & _ResourcesLevel2 & "\" & _ResourcesName & ".xml"
                ElseIf _ResourcesLevel1 <> "" And _ResourcesLevel2 <> "" And _ResourcesLevel3 <> "" Then
                    fileName = Me.LanguageFilePath & "\" & Me.CurrentCultureName & "\" & _ResourcesLevel1 & "\" & _ResourcesLevel2 & "\" & _ResourcesLevel3 & "\" & _ResourcesName & ".xml"
                End If
                fileName = Replace(fileName, "\\", "\")
                Return fileName
            End Get
        End Property
        'Private ReadOnly Property LocalizedValues() As Hashtable
        '	Get

        '	End Get
        'End Property

#End Region

        Public Sub New()
            Me._DefaultCultureName = ManagerConfiguration.GetInstance.DefaultLanguage.Codice '  .System.Configuration.ConfigurationManager.AppSettings("defaultCulture")
            Me._Errori = Errori_Db.None
        End Sub

        Public Sub setCulture()
            '  Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Me.UserLanguages)
            Try
                'Thread.CurrentThread.CurrentCulture = New CultureInfo(Me.UserLanguages)
                Thread.CurrentThread.CurrentUICulture = New CultureInfo(Me.UserLanguages)
                _CultureInfo = New CultureInfo(Thread.CurrentThread.CurrentUICulture.LCID, False)
            Catch ex As Exception
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
                _CultureInfo = New CultureInfo(Thread.CurrentThread.CurrentUICulture.LCID, False)
            End Try

        End Sub

#Region "Load Localized Value from XML FILE"
        Private Function SearchFromLocalizedValues(ByVal SearchValue As String) As String
            Dim iMessages As Hashtable = GetResource()
            Dim FoundValue As String = ""
            Try
                FoundValue = CStr(iMessages(SearchValue))
            Catch ex As Exception

            End Try
            Return FoundValue
        End Function
        Public Function GetResource() As Hashtable
            Dim currentCulture As String = CurrentCultureName
            Dim defaultCulture As String = Me.DefaultCultureName
            Dim cacheKey As String = "Localization:" & defaultCulture & ":" & currentCulture & ":" & Me._ResourcesName

            If HttpRuntime.Cache(cacheKey) Is Nothing Then
                Dim resource As New Hashtable

                LoadResource(resource, defaultCulture, cacheKey)
                If defaultCulture <> currentCulture Then
                    Try
                        LoadResource(resource, currentCulture, cacheKey)
                    Catch ex As FileNotFoundException
                    End Try
                End If

            End If
            Return CType(HttpRuntime.Cache(cacheKey), Hashtable)
        End Function
        Private Function GetResourceForLingua(ByVal CodeName As String) As Hashtable
            Dim currentCulture As String = CodeName
            Dim defaultCulture As String = Me.DefaultCultureName
            Dim cacheKey As String = "Localization:" + defaultCulture + ":"c + currentCulture
            If HttpRuntime.Cache(cacheKey) Is Nothing Then
                Dim resource As New Hashtable

                LoadResource(resource, defaultCulture, cacheKey)
                If defaultCulture <> currentCulture Then
                    Try
                        LoadResource(resource, currentCulture, cacheKey)
                    Catch ex As FileNotFoundException
                    End Try
                End If

            End If
            Return CType(HttpRuntime.Cache(cacheKey), Hashtable)
        End Function
        Private Sub LoadResource(ByVal resource As Hashtable, ByVal culture As String, ByVal cacheKey As String)
            Try
                Dim fileName As String = ""
                If _ResourcesLevel1 = "" Then
                    fileName = Me.LanguageFilePath & "\" & culture & "\" & _ResourcesName & ".xml"
                ElseIf _ResourcesLevel1 <> "" And _ResourcesLevel2 = "" Then
                    fileName = Me.LanguageFilePath & "\" & culture & "\" & _ResourcesLevel1 & "\" & _ResourcesName & ".xml"
                ElseIf _ResourcesLevel1 <> "" And _ResourcesLevel2 <> "" And _ResourcesLevel3 = "" Then
                    fileName = Me.LanguageFilePath & "\" & culture & "\" & _ResourcesLevel1 & "\" & _ResourcesLevel2 & "\" & _ResourcesName & ".xml"
                ElseIf _ResourcesLevel1 <> "" And _ResourcesLevel2 <> "" And _ResourcesLevel3 <> "" Then
                    fileName = Me.LanguageFilePath & "\" & culture & "\" & _ResourcesLevel1 & "\" & _ResourcesLevel2 & "\" & _ResourcesLevel3 & "\" & _ResourcesName & ".xml"
                End If
                fileName = Replace(fileName, "\\", "\")

                If lm.Comol.Core.File.Exists.File(fileName) Then
                    Dim xml As New XmlDocument
                    xml.Load(fileName)

                    Try
                        Dim mngr = New XmlNamespaceManager(xml.NameTable)
                        mngr.AddNamespace("comol", "urn:ComolTranslation")


                        For Each n As XmlNode In xml.SelectSingleNode("//comol:Resource", mngr)
                            If n.NodeType <> XmlNodeType.Comment Then
                                resource(n.Attributes("name").Value) = n.InnerText
                            End If
                        Next
                    Catch ex As Exception
                        Try
                            For Each n As XmlNode In xml.SelectSingleNode("Resource")
                                If n.NodeType <> XmlNodeType.Comment Then
                                    resource(n.Attributes("name").Value) = n.InnerText
                                End If
                            Next
                        Catch ex1 As Exception
                            Throw
                        End Try

                    End Try


                    HttpRuntime.Cache.Insert(cacheKey, resource, New Caching.CacheDependency(fileName), DateTime.MaxValue, TimeSpan.Zero)
                End If
            Catch ex As Exception

            End Try
        End Sub
#End Region

#Region "Funzioni WebControl"
        Public Function getValue(ByVal key As String) As String
            Dim iResponse As String
            Dim messages As Hashtable = GetResource()
            iResponse = ""
            Try
                iResponse = CStr(messages(key))
            Catch ex As Exception
                Dim oStringa As String = ""
                oStringa = ex.Message
                Me._Errori = Errori_Db.System
            End Try
            Return iResponse
        End Function
        Public Function getValue(ByVal oControl As WebControl, ByVal ClassName As String) As String
            Dim messages As Hashtable = GetResource()
            Dim iResponse As String

            iResponse = ""
            Try
                Dim oStringa As String = ""
                oStringa = ClassName & "." & oControl.ID
                iResponse = CStr(messages(oStringa))
            Catch ex As Exception
                Me._Errori = Errori_Db.System
            End Try
            Return iResponse
        End Function
        Public Function getValueForLingua(ByVal ValueToSearch As String, ByVal LinguaCode As String) As String
            Dim messages As Hashtable = GetResourceForLingua(New CultureInfo(LinguaCode).Name)
            Dim iResponse As String

            iResponse = ""
            Try
                iResponse = CStr(messages(ValueToSearch))
            Catch ex As Exception
                Me._Errori = Errori_Db.System
            End Try
            Return iResponse
        End Function

        Public Function setValueToControl(ByVal oControl As WebControl, ByVal ClassName As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                oStringa = ClassName & "." & oControl.ID
                Try
                    If oControl.GetType().ToString = "System.Web.UI.WebControl.textbox" Then

                    ElseIf oControl.GetType().ToString = "System.Web.UI.WebControl.Label" Then
                        Dim oLabel As System.Web.UI.WebControls.Label
                        oLabel = oControl
                        oLabel.Text = CStr(messages(oStringa))

                    ElseIf oControl.GetType().ToString = "System.Web.UI.WebControl.LinkButton" Then
                        Dim oLinkButton As System.Web.UI.WebControls.LinkButton
                        oLinkButton = oControl
                        oLinkButton.Text = CStr(messages(oStringa))

                    ElseIf oControl.GetType().ToString = "System.Web.UI.WebControl.ImageButton" Then
                        Dim oImageButton As System.Web.UI.WebControls.ImageButton
                        oImageButton = oControl
                    End If
                Catch ex As Exception

                End Try
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Sub SetValueForLinguaToLabel(ByVal oLabel As System.Web.UI.WebControls.Label, ByVal LinguaCode As String)
            Dim messages As Hashtable = GetResourceForLingua(New CultureInfo(LinguaCode).Name)
            Dim iResponse As String

            iResponse = ""
            Try
                Dim oStringa As String = ""
                oStringa = oLabel.ID & ".text"
                iResponse = CStr(messages(oStringa))
                If iResponse <> "" Then
                    oLabel.Text = iResponse
                End If
            Catch ex As Exception
                Me._Errori = Errori_Db.System
            End Try
        End Sub

        ' Un metodo per tipo di oggetto, sembra ascelta migliore
        Public Function setValueToImageButton(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal setImageUrl As Boolean, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = "" = ""
                Dim stringaRicerca As String = "" = ""

                If setImageUrl Then
                    Try
                        stringaRicerca = CStr(messages(oImageButton.ID & ".ImageUrl"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImageButton.ImageUrl = stringaRicerca
                    End If
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oImageButton.ID & ".AlternateText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImageButton.AlternateText = stringaRicerca
                    End If
                End If

                Dim status As String = ""

                If setStatus Then
                    Try
                        status = CStr(messages(oImageButton.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setValueToImageButton(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal setImageUrl As Boolean, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, ByVal setImageOver As Boolean, ByVal setImageDown As Boolean) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = "" = ""
                Dim stringaRicerca As String = "" = ""

                If setImageUrl Then
                    Try
                        stringaRicerca = CStr(messages(oImageButton.ID & ".ImageUrl"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImageButton.ImageUrl = stringaRicerca
                    End If
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oImageButton.ID & ".AlternateText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImageButton.AlternateText = stringaRicerca
                    End If
                End If

                Dim status, imageOver, imageDown As String
                status = ""
                imageOver = ""
                imageDown = ""
                If setImageOver Then
                    Try
                        imageOver = CStr(messages(oImageButton.ID & ".ImageOver"))
                    Catch ex As Exception
                        imageOver = ""
                    End Try
                End If
                If setImageDown Then
                    Try
                        imageDown = CStr(messages(oImageButton.ID & ".ImageDown"))
                    Catch ex As Exception
                        imageDown = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oImageButton.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        If setImageOver Then
                            oImageButton.Attributes.Add("onmouseout", "window.status='';this.src='" & oImageButton.ImageUrl & "';return true;")
                        Else
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        End If

                        If setImageOver And imageOver <> "" Then
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';this.src='" & imageOver & "';return true;")
                        Else
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    ElseIf setImageOver And imageOver <> "" Then
                        oImageButton.Attributes.Add("onmouseover", "window.status='';this.src='" & imageOver & "';return true;")
                    End If
                ElseIf setImageOver And imageOver <> "" Then
                    oImageButton.Attributes.Add("onmouseover", "window.status='';this.src='" & imageOver & "';return true;")
                End If

                If setImageDown And imageDown <> "" Then
                    If status <> "" Then
                        oImageButton.Attributes.Add("onmousedown", "window.status='" & status & "';this.src='" & imageDown & "';return true;")
                    Else
                        oImageButton.Attributes.Add("onmousedown", "window.status='';this.src='" & imageDown & "';return true;")
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setValueToImageButton(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal setImageUrl As Boolean, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, ByVal setImageOver As Boolean, ByVal setImageDown As Boolean, ByVal setBlockedText As Boolean, ByVal setConfirmText As Boolean) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                If setImageUrl Then
                    Try
                        stringaRicerca = CStr(messages(oImageButton.ID & ".ImageUrl"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImageButton.ImageUrl = stringaRicerca
                    End If
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oImageButton.ID & ".AlternateText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImageButton.AlternateText = stringaRicerca
                    End If
                End If

                Dim status, imageOver, BlockedText, ConfirmText, imageDown As String
                imageOver = ""
                BlockedText = ""
                imageDown = ""
                status = ""
                If setImageOver Then
                    Try
                        imageOver = CStr(messages(oImageButton.ID & ".ImageOver"))
                    Catch ex As Exception
                        imageOver = ""
                    End Try
                End If
                If setImageDown Then
                    Try
                        imageDown = CStr(messages(oImageButton.ID & ".ImageDown"))
                    Catch ex As Exception
                        imageDown = ""
                    End Try
                End If
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oImageButton.ID & ".blocked"))
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oImageButton.ID & ".ConfirmText"))
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oImageButton.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oImageButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        If setImageOver Then
                            oImageButton.Attributes.Add("onmouseout", "window.status='';this.src='" & oImageButton.ImageUrl & "';return true;")
                        Else
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        End If

                        If setImageOver And imageOver <> "" Then
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';this.src='" & imageOver & "';return true;")
                        Else
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    ElseIf setImageOver And imageOver <> "" Then
                        oImageButton.Attributes.Add("onmouseover", "window.status='';this.src='" & imageOver & "';return true;")
                    End If
                ElseIf setImageOver And imageOver <> "" Then
                    oImageButton.Attributes.Add("onmouseover", "window.status='';this.src='" & imageOver & "';return true;")
                End If

                If setBlockedText And setStatus = False Then
                    oImageButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oImageButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If

                If setImageDown And imageDown <> "" Then
                    If status <> "" Then
                        oImageButton.Attributes.Add("onmousedown", "window.status='" & status & "';this.src='" & imageDown & "';return true;")
                    Else
                        oImageButton.Attributes.Add("onmousedown", "window.status='';this.src='" & imageDown & "';return true;")
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLabel(ByVal oLabel As System.Web.UI.WebControls.Label) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oLabel.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLabel.Text = stringaRicerca
                End If
                stringaRicerca = ""
                Try
                    stringaRicerca = CStr(messages(oLabel.ID & ".ToolTip"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLabel.ToolTip = stringaRicerca
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLiteral(ByVal oLiteral As System.Web.UI.WebControls.Literal) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oLiteral.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLiteral.Text = stringaRicerca
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        ''' <summary>
        ''' sets the internazionalization of literal control that has control.text.value in xml
        ''' </summary>
        ''' <param name="oLiteral"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setLiteral(ByVal oLiteral As System.Web.UI.WebControls.Literal, ByRef value As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oLiteral.ID & ".text." & value))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLiteral.Text = stringaRicerca
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Function setInputButton(ByVal oInputButton As System.Web.UI.HtmlControls.HtmlInputButton) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oInputButton.ID & ".value"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oInputButton.Value = stringaRicerca
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLinkButtonLettera(ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal SearchValue As String, ByVal ReplaceValue As String, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages("Lettera.ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        If SearchValue <> "" And ReplaceValue <> "" Then
                            stringaRicerca = stringaRicerca.Replace(SearchValue, ReplaceValue)
                        End If

                        oLinkButton.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                ConfirmText = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages("Lettera.blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                        If SearchValue <> "" And ReplaceValue <> "" And BlockedText <> "" Then
                            BlockedText = BlockedText.Replace(SearchValue, ReplaceValue)
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If

                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages("Lettera.ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                        If SearchValue <> "" And ReplaceValue <> "" And ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace(SearchValue, ReplaceValue)
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages("Lettera.status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        If SearchValue <> "" And ReplaceValue <> "" Then
                            status = status.Replace(SearchValue, ReplaceValue)
                        End If
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLinkButton(ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oLinkButton.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLinkButton.Text = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oLinkButton.ID & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oLinkButton.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oLinkButton.ID & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oLinkButton.ID & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oLinkButton.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLinkButtonToValue(ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal valueToSet As String, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oLinkButton.ID & "." & valueToSet & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLinkButton.Text = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oLinkButton.ID & "." & valueToSet & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oLinkButton.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oLinkButton.ID & "." & valueToSet & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oLinkButton.ID & "." & valueToSet & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oLinkButton.ID & "." & valueToSet & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        ''' <summary>
        ''' Internazionalizza un linkbutton utilizzando le traduzioni di un altro
        ''' </summary>
        ''' <param name="oLinkButton">Controllo da internazionalizzare</param>
        ''' <param name="nameToSet">Nome del controllo del quale copiare la traduzione</param>
        ''' <param name="setStatus"></param>
        ''' <param name="setAlternateText"></param>
        ''' <param name="setBlockedText"></param>
        ''' <param name="setConfirmText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setLinkButtonForName(ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal nameToSet As String, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(nameToSet & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oLinkButton.Text = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(nameToSet & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oLinkButton.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(nameToSet & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(nameToSet & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(nameToSet & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHyperLink(ByVal oHyperLink As System.Web.UI.WebControls.HyperLink, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oHyperLink.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oHyperLink.Text = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oHyperLink.ID & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oHyperLink.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oHyperLink.ID & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oHyperLink.ID & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oHyperLink.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oHyperLink.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oHyperLink.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oHyperLink.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oHyperLink.Attributes.Add("onmouseout", "window.status='';return true;")
                        oHyperLink.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oHyperLink.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oHyperLink.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHyperLink(ByVal oHyperLink As System.Web.UI.WebControls.HyperLink, ByVal valueToSet As String, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oHyperLink.ID & "." & valueToSet & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oHyperLink.Text = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oHyperLink.ID & "." & valueToSet & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oHyperLink.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oHyperLink.ID & "." & valueToSet & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oHyperLink.ID & "." & valueToSet & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oHyperLink.ID & "." & valueToSet & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oHyperLink.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oHyperLink.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oHyperLink.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oHyperLink.Attributes.Add("onmouseout", "window.status='';return true;")
                        oHyperLink.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oHyperLink.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oHyperLink.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Function setHtmlAnchor(ByVal oHtmlAnchor As System.Web.UI.HtmlControls.HtmlAnchor, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oHtmlAnchor.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oHtmlAnchor.InnerText = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oHtmlAnchor.ID & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oHtmlAnchor.Title = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oHtmlAnchor.ID & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oHtmlAnchor.ID & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oHtmlAnchor.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oHtmlAnchor.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oHtmlAnchor.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oHtmlAnchor.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oHtmlAnchor.Attributes.Add("onmouseout", "window.status='';return true;")
                        oHtmlAnchor.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oHtmlAnchor.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oHtmlAnchor.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHtmlAnchorToValue(ByVal oHtmlAnchor As System.Web.UI.HtmlControls.HtmlAnchor, ByVal valueToSet As String, ByVal setStatus As Boolean, ByVal setAlternateText As Boolean, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(valueToSet & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oHtmlAnchor.InnerText = stringaRicerca
                End If

                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(valueToSet & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oHtmlAnchor.Title = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(valueToSet & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(valueToSet & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(valueToSet & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oHtmlAnchor.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oHtmlAnchor.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oHtmlAnchor.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oHtmlAnchor.Attributes.Add("onmouseout", "window.status='';return true;")
                        oHtmlAnchor.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oHtmlAnchor.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oHtmlAnchor.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHtmlButton(ByVal oHtmlButton As System.Web.UI.HtmlControls.HtmlInputButton) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oHtmlButton.UniqueID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oHtmlButton.Value = stringaRicerca
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function



        Public Function setRequiredFieldValidator(ByVal oRequiredField As System.Web.UI.WebControls.RequiredFieldValidator, ByVal setErrorMessage As Boolean, ByVal setText As Boolean) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""

                If setErrorMessage Then
                    Try
                        stringaRicerca = CStr(messages(oRequiredField.ID & ".ErrorMessage"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oRequiredField.ErrorMessage = stringaRicerca
                    End If
                End If

                If setText Then
                    Try
                        stringaRicerca = CStr(messages(oRequiredField.ID & ".Text"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oRequiredField.Text = stringaRicerca
                    End If
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setRegularExpressionValidator(ByVal oRegularExpressionValidator As System.Web.UI.WebControls.RegularExpressionValidator) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""

                Try
                    stringaRicerca = CStr(messages(oRegularExpressionValidator.ID & ".ErrorMessage"))
                    If stringaRicerca <> "" Then
                        oRegularExpressionValidator.ErrorMessage = stringaRicerca
                    End If
                Catch ex As Exception

                End Try

                Try
                    stringaRicerca = CStr(messages(oRegularExpressionValidator.ID & ".text"))
                    If stringaRicerca <> "" Then
                        oRegularExpressionValidator.Text = stringaRicerca
                    End If
                Catch ex As Exception

                End Try


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setRangeValidator(ByVal oRangeValidator As System.Web.UI.WebControls.RangeValidator) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oRangeValidator.ID & ".ErrorMessage"))
                    If stringaRicerca <> "" Then
                        oRangeValidator.ErrorMessage = stringaRicerca
                    End If
                Catch ex As Exception

                End Try
                Try
                    stringaRicerca = CStr(messages(oRangeValidator.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oRangeValidator.Text = stringaRicerca
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        ''' <summary>
        ''' imposta il validator usando i parametri di oRangeValidatorTranslated
        ''' </summary>
        ''' <param name="oRangeValidator">Validator da internazionalizzare</param>
        ''' <param name="oRangeValidatorTranslated">Controllo del quale utilizzare le traduzioni</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setRangeValidator(ByVal oRangeValidator As System.Web.UI.WebControls.RangeValidator, ByVal oRangeValidatorTranslated As System.Web.UI.WebControls.RangeValidator) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oRangeValidatorTranslated.ID & ".ErrorMessage"))
                    If stringaRicerca <> "" Then
                        oRangeValidator.ErrorMessage = stringaRicerca
                    End If
                Catch ex As Exception

                End Try
                Try
                    stringaRicerca = CStr(messages(oRangeValidatorTranslated.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oRangeValidator.Text = stringaRicerca
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setCompareValidator(ByVal oCompareValidator As System.Web.UI.WebControls.CompareValidator) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""

                Try
                    stringaRicerca = CStr(messages(oCompareValidator.ID & ".ErrorMessage"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oCompareValidator.ErrorMessage = stringaRicerca
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        ''' <summary>
        ''' imposta il validator usando i parametri di oCompareValidatorTranslated
        ''' </summary>
        ''' <param name="oCompareValidator">Validator da internazionalizzare</param>
        ''' <param name="oCompareValidatorTranslated">Controllo del quale utilizzare le traduzioni</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setCompareValidator(ByVal oCompareValidator As System.Web.UI.WebControls.CompareValidator, ByVal oCompareValidatorTranslated As System.Web.UI.WebControls.CompareValidator) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""

                Try
                    stringaRicerca = CStr(messages(oCompareValidatorTranslated.ID & ".ErrorMessage"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oCompareValidator.ErrorMessage = stringaRicerca
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Function setCustomValidator(ByVal oCustomValidator As System.Web.UI.WebControls.CustomValidator) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""

                Try
                    stringaRicerca = CStr(messages(oCustomValidator.ID & ".ErrorMessage"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oCustomValidator.ErrorMessage = stringaRicerca
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setValidationSummary(ByVal oValidationSummary As System.Web.UI.WebControls.ValidationSummary) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""

                Try
                    stringaRicerca = CStr(messages(oValidationSummary.ID & ".HeaderText"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oValidationSummary.HeaderText = stringaRicerca
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Function setButton(ByVal oButton As System.Web.UI.WebControls.Button, Optional ByVal setToolTip As Boolean = False, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False, Optional ByVal setStatus As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oButton.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oButton.Text = stringaRicerca
                End If
                If setToolTip Then
                    Try
                        stringaRicerca = CStr(messages(oButton.ID & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oButton.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oButton.ID & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oButton.ID & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oButton.ID & ".status"))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="oButton"></param>
        ''' <param name="ValueToSearch">.text.ValueToSearch</param>
        ''' <param name="setToolTip"></param>
        ''' <param name="setBlockedText"></param>
        ''' <param name="setConfirmText"></param>
        ''' <param name="setStatus"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setButtonByValue(ByVal oButton As System.Web.UI.WebControls.Button, ByVal ValueToSearch As String, Optional ByVal setToolTip As Boolean = False, Optional ByVal setBlockedText As Boolean = False, Optional ByVal setConfirmText As Boolean = False, Optional ByVal setStatus As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oButton.ID & ".text." & ValueToSearch))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oButton.Text = stringaRicerca
                End If
                If setToolTip Then
                    Try
                        stringaRicerca = CStr(messages(oButton.ID & ".ToolTip." & ValueToSearch))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oButton.ToolTip = stringaRicerca
                    End If
                End If

                Dim status, BlockedText, ConfirmText As String
                BlockedText = ""
                status = ""
                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oButton.ID & ".blocked." & ValueToSearch))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If
                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oButton.ID & ".ConfirmText." & ValueToSearch))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setStatus Then
                    Try
                        status = CStr(messages(oButton.ID & ".status." & ValueToSearch))
                    Catch ex As Exception

                    End Try
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        If setBlockedText Then
                            oButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                        ElseIf setConfirmText Then
                            oButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                        Else
                            oButton.Attributes.Add("onclick", "window.status='" & status & "';return true;")
                        End If
                        oButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If

                If setBlockedText And setStatus = False Then
                    oButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                End If
                If setConfirmText And setStatus = False Then
                    oButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function


        Public Function setImage(ByVal oImage As System.Web.UI.WebControls.Image, Optional ByVal setAlternateText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oImage.ID & ".ImageUrl"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oImage.ImageUrl = stringaRicerca
                End If
                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oImage.ID & ".AlternateText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImage.AlternateText = stringaRicerca
                        oImage.ToolTip = stringaRicerca
                    End If
                End If


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setImageToValue(ByVal oImage As System.Web.UI.WebControls.Image, ByVal Value As String, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setToolTip As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oImage.ID & "." & Value & ".ImageUrl"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oImage.ImageUrl = stringaRicerca
                End If
                If setAlternateText Then
                    Try
                        stringaRicerca = CStr(messages(oImage.ID & "." & Value & ".AlternateText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImage.AlternateText = stringaRicerca
                    End If
                End If
                If setToolTip Then
                    Try
                        stringaRicerca = CStr(messages(oImage.ID & "." & Value & ".ToolTip"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oImage.ToolTip = stringaRicerca
                    End If
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setImageButtonNoUrlChange(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setAlternateText Then
                    oStringa = CStr(messages(oImageButton.ID & ".AlternateText"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    End If
                ElseIf setAlternateText Then
                    oStringa = CStr(messages(oImageButton.ID & ".AlternateText_disabled"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    Else
                        oStringa = CStr(messages(oImageButton.ID & ".AlternateText"))
                        If oStringa <> "" Then
                            oImageButton.AlternateText = oStringa
                            oImageButton.ToolTip = oStringa
                        End If
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus Then
                    status = CStr(messages(oImageButton.ID & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                ElseIf setStatus Then
                    status = CStr(messages(oImageButton.ID & ".status_disabled"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    Else
                        status = CStr(messages(oImageButton.ID & ".status"))
                        If status <> "" Then
                            status = status.Replace("'", "\'")
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oImageButton.ID & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oImageButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setImageButton(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal setNormalUrl As Boolean, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setNormalUrl Then
                    Try
                        oStringa = CStr(messages(oImageButton.ID & ".ImageUrl"))
                        If oStringa <> "" Then
                            oImageButton.ImageUrl = oStringa
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    Try
                        oStringa = CStr(messages(oImageButton.ID & ".ImageUrl_disabled"))
                        If oStringa <> "" Then
                            oImageButton.ImageUrl = oStringa
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If setAlternateText And setNormalUrl Then
                    oStringa = CStr(messages(oImageButton.ID & ".AlternateText"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    End If
                ElseIf setAlternateText Then
                    oStringa = CStr(messages(oImageButton.ID & ".AlternateText_disabled"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    Else
                        oStringa = CStr(messages(oImageButton.ID & ".AlternateText"))
                        If oStringa <> "" Then
                            oImageButton.AlternateText = oStringa
                            oImageButton.ToolTip = oStringa
                        End If
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus And setNormalUrl Then
                    status = CStr(messages(oImageButton.ID & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                ElseIf setStatus Then
                    status = CStr(messages(oImageButton.ID & ".status_disabled"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    Else
                        status = CStr(messages(oImageButton.ID & ".status"))
                        If status <> "" Then
                            status = status.Replace("'", "\'")
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oImageButton.ID & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oImageButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setImageButton_To_Value(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal setNormalUrl As Boolean, ByVal valueToSearch As String, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setNormalUrl Then
                    Try
                        oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".ImageUrl"))
                        If oStringa <> "" Then
                            oImageButton.ImageUrl = oStringa
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    Try
                        oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".ImageUrl_disabled"))
                        If oStringa <> "" Then
                            oImageButton.ImageUrl = oStringa
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If setAlternateText And setNormalUrl Then
                    oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".AlternateText"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    End If
                    oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".ToolTip"))
                    If oStringa <> "" Then
                        oImageButton.ToolTip = oStringa
                    End If
                ElseIf setAlternateText Then
                    oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".AlternateText_disabled"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    Else
                        oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".AlternateText"))
                        If oStringa <> "" Then
                            oImageButton.AlternateText = oStringa
                            oImageButton.ToolTip = oStringa
                        End If
                        oStringa = CStr(messages(oImageButton.ID & "." & valueToSearch & ".ToolTip"))
                        If oStringa <> "" Then
                            oImageButton.ToolTip = oStringa
                        End If
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus And setNormalUrl Then
                    status = CStr(messages(oImageButton.ID & "." & valueToSearch & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                ElseIf setStatus Then
                    status = CStr(messages(oImageButton.ID & "." & valueToSearch & ".status_disabled"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    Else
                        status = CStr(messages(oImageButton.ID & "." & valueToSearch & ".status"))
                        If status <> "" Then
                            status = status.Replace("'", "\'")
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oImageButton.ID & "." & valueToSearch & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oImageButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function


        Public Function setStringaTab(ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal Value As Integer) As String
            Dim messages As Hashtable = GetResource()
            Dim iResponse As String

            iResponse = ""
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""



                Try
                    iResponse = CStr(messages(oImageButton.ID & ".url." & Value))

                Catch ex As Exception

                End Try
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = ""
            End Try
            Return iResponse
        End Function

        Public Function setCheckBox(ByVal oCheck As System.Web.UI.WebControls.CheckBox) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oCheck.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oCheck.Text = stringaRicerca
                End If
                Try
                    stringaRicerca = CStr(messages(oCheck.ID & ".ToolTip"))
                    oCheck.ToolTip = stringaRicerca
                Catch ex As Exception

                End Try
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Function setCheckBoxList(ByVal oCheckBoxList As System.Web.UI.WebControls.CheckBoxList, ByVal value As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oCheckBoxList.ID & "." & value))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    Dim oListItem As ListItem
                    oListItem = oCheckBoxList.Items.FindByValue(value)
                    If Not IsNothing(oListItem) Then
                        oListItem.Text = stringaRicerca
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setRadioButtonList(ByVal oRadioButtonList As System.Web.UI.WebControls.RadioButtonList, ByVal value As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""


                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oRadioButtonList.ID & "." & value))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    Dim oListItem As ListItem
                    oListItem = oRadioButtonList.Items.FindByValue(value)
                    If Not IsNothing(oListItem) Then
                        oListItem.Text = stringaRicerca
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function


        Public Function setDropDownList(ByVal oDropDownList As System.Web.UI.WebControls.DropDownList, ByVal value As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oDropDownList.ID & "." & value))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    Dim oListItem As ListItem
                    oListItem = oDropDownList.Items.FindByValue(value)
                    If Not IsNothing(oListItem) Then
                        oListItem.Text = stringaRicerca
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Sub TranslateDropDownList(ByVal oDropDownList As System.Web.UI.WebControls.DropDownList, ByVal oList As IList)
            Dim messages As Hashtable = GetResource()
            Try
                Dim oToolTip As String = Me.SearchFromLocalizedValues(oDropDownList.ID & ".ToolTip")
                If oToolTip <> "" Then
                    oDropDownList.ToolTip = oToolTip
                End If
                If Not IsNothing(oList) Then

                    For Each oValueToSearch As String In oList
                        Dim oValue As String = Me.SearchFromLocalizedValues(oDropDownList.ID & "." & oValueToSearch)
                        If oValue <> "" Then
                            Dim oListItem As ListItem = oDropDownList.Items.FindByValue(oValueToSearch)
                            If Not IsNothing(oListItem) Then
                                oListItem.Text = oValue
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Function setTextBox(ByVal oTextBox As System.Web.UI.WebControls.TextBox) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Try
                    stringaRicerca = CStr(messages(oTextBox.ID & ".text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oTextBox.Text = stringaRicerca
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        ''' <summary>
        ''' Imposta il campo text della label utilizzando il parametro.
        ''' </summary>
        ''' <param name="oLabel"></param>
        ''' <param name="stringaRicerca">parametro di ricerca, nell'xml va impostato senza .text</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function setLabel_To_Value(ByVal oLabel As System.Web.UI.WebControls.Label, ByVal stringaRicerca As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaTesto As String = ""
                Try
                    stringaTesto = CStr(messages(stringaRicerca))
                Catch ex As Exception

                End Try
                If stringaTesto <> "" Then
                    oLabel.Text = stringaTesto
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHeaderDatagrid(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal Index As Integer, ByVal FieldName As String, ByVal setHeader As Boolean, Optional ByVal setLinkText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setHeader Then
                    Try
                        stringaRicerca = CStr(messages(oDatagrid.ID & "." & FieldName & ".HeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oDatagrid.Columns(Index).HeaderText = stringaRicerca
                    End If
                End If

                If setLinkText Then
                    Try
                        stringaRicerca = CStr(messages(oDatagrid.ID & "." & FieldName & ".HeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oDatagrid.Columns(Index).HeaderText = stringaRicerca
                    End If
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHeaderGridView(ByVal GridView As System.Web.UI.WebControls.GridView, ByVal Index As Integer, ByVal FieldName As String, ByVal setHeader As Boolean, Optional ByVal setLinkText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setHeader Then
                    Try
                        stringaRicerca = CStr(messages(GridView.ID & "." & FieldName & ".HeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        GridView.Columns(Index).HeaderText = stringaRicerca
                    End If

                    Try
                        stringaRicerca = CStr(messages(GridView.ID & "." & FieldName & ".AccessibleHeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        GridView.Columns(Index).AccessibleHeaderText = stringaRicerca
                    End If
                End If

                If setLinkText Then
                    Try
                        stringaRicerca = CStr(messages(GridView.ID & "." & FieldName & ".HeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        GridView.Columns(Index).HeaderText = stringaRicerca
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setHeaderOrderbyLink_Datagrid(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal oLinkbutton As System.Web.UI.WebControls.LinkButton, ByVal ValueString As String) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                Try
                    oStringa = CStr(messages(oDatagrid.ID & ".ordinamento." & ValueString & ".status"))
                    If oStringa <> "" Then
                        oStringa = oStringa.Replace("#%%#", oLinkbutton.Text)
                        oStringa = oStringa.Replace("'", "\'")
                    End If

                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='" & oStringa & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='" & oStringa & "';return true;")
                    oLinkbutton.ToolTip = oStringa
                Catch ex As Exception

                End Try
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setImageButton_Datagrid(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal oImageButton As System.Web.UI.WebControls.ImageButton, ByVal ValueString As String, ByVal setNormalUrl As Boolean, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setNormalUrl Then
                    Try
                        oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".ImageUrl"))
                        If oStringa <> "" Then
                            oImageButton.ImageUrl = oStringa
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    Try
                        oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".ImageUrl_disabled"))
                        If oStringa <> "" Then
                            oImageButton.ImageUrl = oStringa
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If setAlternateText And setNormalUrl Then
                    oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".AlternateText"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    End If
                ElseIf setAlternateText Then
                    oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".AlternateText_disabled"))
                    If oStringa <> "" Then
                        oImageButton.AlternateText = oStringa
                        oImageButton.ToolTip = oStringa
                    Else
                        oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".AlternateText"))
                        If oStringa <> "" Then
                            oImageButton.AlternateText = oStringa
                            oImageButton.ToolTip = oStringa
                        End If
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus And setNormalUrl Then
                    status = CStr(messages(oDatagrid.ID & "." & ValueString & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                ElseIf setStatus Then
                    status = CStr(messages(oDatagrid.ID & "." & ValueString & ".status_disabled"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    Else
                        status = CStr(messages(oDatagrid.ID & "." & ValueString & ".status"))
                        If status <> "" Then
                            status = status.Replace("'", "\'")
                            oImageButton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oImageButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                            oImageButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oDatagrid.ID & "." & ValueString & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oImageButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oImageButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setImageButton_GridView(ByVal GridViewID As String, ByVal BaseUrl As String, ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal ValueString As String, ByVal setNormalUrl As Boolean, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""
                Dim stringaTesto As String = "<img src='{0}' alt='{1}' border='0' />"
                Dim url As String = ""
                Dim Tooltip As String = ""

                If setNormalUrl Then
                    Try
                        oStringa = CStr(messages(GridViewID & "." & ValueString & ".ImageUrl"))
                        If oStringa <> "" Then
                            url = BaseUrl & oStringa
                        End If
                    Catch ex As Exception

                    End Try
                Else
                    Try
                        oStringa = CStr(messages(GridViewID & "." & ValueString & ".ImageUrl_disabled"))
                        If oStringa <> "" Then
                            url = BaseUrl & oStringa
                        End If
                    Catch ex As Exception

                    End Try
                End If
                If setAlternateText And setNormalUrl Then
                    oStringa = CStr(messages(GridViewID & "." & ValueString & ".AlternateText"))
                    If oStringa <> "" Then
                        Tooltip = oStringa
                    End If
                ElseIf setAlternateText Then
                    oStringa = CStr(messages(GridViewID & "." & ValueString & ".AlternateText_disabled"))
                    If oStringa <> "" Then
                        Tooltip = oStringa
                    Else
                        oStringa = CStr(messages(GridViewID & "." & ValueString & ".AlternateText"))
                        If oStringa <> "" Then
                            Tooltip = oStringa
                        End If
                    End If
                End If
                oStringa = String.Format(stringaTesto, url, Tooltip)
                oLinkButton.Text = oStringa
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus And setNormalUrl Then
                    status = CStr(messages(GridViewID & "." & ValueString & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                ElseIf setStatus Then
                    status = CStr(messages(GridViewID & "." & ValueString & ".status_disabled"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    Else
                        status = CStr(messages(GridViewID & "." & ValueString & ".status"))
                        If status <> "" Then
                            status = status.Replace("'", "\'")
                            oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                            oLinkButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                            oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                        End If
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(GridViewID & "." & ValueString & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If


                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLinkButton_Datagrid(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal ValueString As String, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False, Optional ByVal SetText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""


                If setAlternateText Then
                    oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".ToolTip"))
                    If oStringa <> "" Then
                        oLinkButton.ToolTip = oStringa
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus Then
                    status = CStr(messages(oDatagrid.ID & "." & ValueString & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oDatagrid.ID & "." & ValueString & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If

                If SetText Then
                    oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".text"))
                    If oStringa <> "" Then
                        oLinkButton.Text = oStringa
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Function setLinkButton_GridView(ByVal GridViewID As String, ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal ValueString As String, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False, Optional ByVal SetText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""


                If setAlternateText Then
                    oStringa = CStr(messages(GridViewID & "." & ValueString & ".ToolTip"))
                    If oStringa <> "" Then
                        oLinkButton.ToolTip = oStringa
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus Then
                    status = CStr(messages(GridViewID & "." & ValueString & ".status"))
                    If status <> "" Then
                        status = status.Replace("'", "\'")
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(GridViewID & "." & ValueString & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                End If

                If SetText Then
                    oStringa = CStr(messages(GridViewID & "." & ValueString & ".text"))
                    If oStringa <> "" Then
                        oLinkButton.Text = oStringa
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
        Public Sub setLinkButton_DatagridToValue(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal oLinkButton As System.Web.UI.WebControls.LinkButton, ByVal ValueString As String, ByVal SearchValue As String, ByVal ReplaceValue As String, Optional ByVal setStatus As Boolean = False, Optional ByVal setAlternateText As Boolean = False, Optional ByVal setConfirmText As Boolean = False, Optional ByVal SetText As Boolean = False, Optional ByVal setBlockedText As Boolean = False)
            Dim messages As Hashtable = GetResource()

            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""


                If SetText Then
                    oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".text"))
                    If oStringa <> "" Then
                        oLinkButton.Text = oStringa
                    End If
                End If
                If setAlternateText Then
                    oStringa = CStr(messages(oDatagrid.ID & "." & ValueString & ".ToolTip"))
                    If oStringa <> "" Then
                        If SearchValue <> "" And ReplaceValue <> "" Then
                            oStringa = oStringa.Replace(SearchValue, ReplaceValue)
                        End If
                        oLinkButton.ToolTip = oStringa
                    End If
                End If
                Dim BlockedText, ConfirmText, status As String
                BlockedText = ""
                status = ""
                If setStatus Then
                    status = CStr(messages(oDatagrid.ID & "." & ValueString & ".status"))
                    If status <> "" Then
                        If SearchValue <> "" And ReplaceValue <> "" Then
                            status = status.Replace(SearchValue, ReplaceValue)
                        End If
                        status = status.Replace("'", "\'")
                        oLinkButton.Attributes.Add("onmouseout", "window.status='';return true;")
                        oLinkButton.Attributes.Add("onfocus", "window.status='" & status & "';return true;")
                        oLinkButton.Attributes.Add("onmouseover", "window.status='" & status & "';return true;")
                    End If
                End If


                ConfirmText = ""
                If setConfirmText Then
                    Try
                        ConfirmText = CStr(messages(oDatagrid.ID & "." & ValueString & ".ConfirmText"))
                        If ConfirmText <> "" Then
                            ConfirmText = ConfirmText.Replace("'", "\'")
                        End If
                        If SearchValue <> "" And ReplaceValue <> "" Then
                            ConfirmText = ConfirmText.Replace(SearchValue, ReplaceValue)
                        End If
                    Catch ex As Exception
                        ConfirmText = ""
                    End Try
                End If

                If setBlockedText Then
                    Try
                        BlockedText = CStr(messages(oDatagrid.ID & "." & ValueString & ".blocked"))
                        If BlockedText <> "" Then
                            BlockedText = BlockedText.Replace("'", "\'")
                        End If
                    Catch ex As Exception
                        BlockedText = ""
                    End Try
                End If


                If setConfirmText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';return confirm('" & ConfirmText & "');")
                ElseIf setConfirmText Then
                    oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';return confirm('" & ConfirmText & "');")
                ElseIf setBlockedText And setStatus = False Then
                    oLinkButton.Attributes.Add("onclick", "window.status='';alert('" & BlockedText & "');return false;")
                ElseIf setBlockedText And setStatus = True Then
                    oLinkButton.Attributes.Add("onclick", "window.status='" & status & "';alert('" & BlockedText & "');return false;")
                End If

            Catch ex As Exception
                Me._Errori = Errori_Db.System
            End Try
        End Sub

        Public Sub setPageDatagrid(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal oLinkbutton As System.Web.UI.WebControls.LinkButton)
            Dim messages As Hashtable = GetResource()

            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""



                Try
                    oStringa = CStr(messages(oDatagrid.ID & ".page"))
                    If oStringa = "" Then
                        oStringa = "Page <#%%#>"
                    End If

                    oStringa = oStringa.Replace("#%%#", oLinkbutton.Text)
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='" & oStringa & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='" & oStringa & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='" & oStringa & "';return true;")
                Catch ex As Exception

                End Try
            Catch ex As Exception
                Me._Errori = Errori_Db.System
            End Try
        End Sub
        Public Sub setPageGridView(ByVal GridViewID As String, ByVal oLinkbutton As System.Web.UI.WebControls.LinkButton)
            Dim messages As Hashtable = GetResource()

            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""



                Try
                    oStringa = CStr(messages(GridViewID & ".page"))
                    If oStringa = "" Then
                        oStringa = "Page <#%%#>"
                    End If

                    oStringa = oStringa.Replace("#%%#", oLinkbutton.Text)
                    oLinkbutton.Attributes.Add("onmouseout", "window.status='';return true;")
                    oLinkbutton.Attributes.Add("onfocus", "window.status='" & oStringa & "';return true;")
                    oLinkbutton.Attributes.Add("onmouseover", "window.status='" & oStringa & "';return true;")
                    oLinkbutton.Attributes.Add("onclick", "window.status='" & oStringa & "';return true;")
                Catch ex As Exception

                End Try
            Catch ex As Exception
                Me._Errori = Errori_Db.System
            End Try
        End Sub

        Public Function getValueForDatagrid(ByVal oDatagrid As System.Web.UI.WebControls.DataGrid, ByVal FieldName As String, ByVal valueToSearch As String) As String
            Dim messages As Hashtable = GetResource()
            Dim iResponse As String

            iResponse = ""
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                Try
                    iResponse = CStr(messages(oDatagrid.ID & "." & FieldName & ".Text." & valueToSearch))

                Catch ex As Exception

                End Try
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = ""
            End Try
            Return iResponse
        End Function
        Public Function getValueForGridView(ByVal GridView As String, ByVal FieldName As String, ByVal valueToSearch As String) As String
            Dim messages As Hashtable = GetResource()
            Dim iResponse As String

            iResponse = ""
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                Try
                    iResponse = CStr(messages(GridView & "." & FieldName & ".Text." & valueToSearch))

                Catch ex As Exception

                End Try
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = ""
            End Try
            Return iResponse
        End Function

        '      Public Function setTabTelerik(ByVal oTab As Telerik.WebControls.Tab, Optional ByVal setToolTip As Boolean = False) As Boolean
        '          Dim messages As Hashtable = GetResource()
        '          Dim iResponse As Boolean

        '          iResponse = False
        '          Try
        '              Dim oStringa As String = ""
        '              Dim stringaRicerca As String = ""

        '              Try
        '                  stringaRicerca = CStr(messages(oTab.ID & ".Text"))
        '              Catch ex As Exception

        '              End Try
        '              If stringaRicerca <> "" Then
        '                  oTab.Text = stringaRicerca
        '              End If

        '              If setToolTip Then
        '                  Try
        '                      stringaRicerca = CStr(messages(oTab.ID & ".ToolTip"))
        '                  Catch ex As Exception
        '                      stringaRicerca = ""
        '                  End Try
        '                  If stringaRicerca <> "" Then
        '                      oTab.ToolTip = stringaRicerca
        '                  End If
        '              End If
        '              iResponse = True
        '          Catch ex As Exception
        '              Me._Errori = Errori_Db.System
        '              iResponse = False
        '          End Try
        '          Return iResponse
        'End Function
        Public Function setRadTab(ByVal oTab As Telerik.Web.UI.RadTab, Optional ByVal setToolTip As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                Try
                    stringaRicerca = CStr(messages(oTab.Value & ".Text"))
                Catch ex As Exception

                End Try
                If stringaRicerca <> "" Then
                    oTab.Text = stringaRicerca
                End If

                If setToolTip Then
                    Try
                        stringaRicerca = CStr(messages(oTab.Value & ".ToolTip"))
                    Catch ex As Exception
                        stringaRicerca = ""
                    End Try
                    If stringaRicerca <> "" Then
                        oTab.ToolTip = stringaRicerca
                    End If
                End If
                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function

        Public Function setHeaderDetailsView(ByVal oDetails As System.Web.UI.WebControls.DetailsView, ByVal Index As Integer, ByVal FieldName As String, ByVal setHeader As Boolean, Optional ByVal setLinkText As Boolean = False) As Boolean
            Dim messages As Hashtable = GetResource()
            Dim iResponse As Boolean

            iResponse = False
            Try
                Dim oStringa As String = ""
                Dim stringaRicerca As String = ""

                If setHeader Then
                    Try
                        stringaRicerca = CStr(messages(oDetails.ID & "." & FieldName & ".HeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oDetails.Fields(Index).HeaderText = stringaRicerca
                    End If
                End If

                If setLinkText Then
                    Try
                        stringaRicerca = CStr(messages(oDetails.ID & "." & FieldName & ".HeaderText"))
                    Catch ex As Exception

                    End Try
                    If stringaRicerca <> "" Then
                        oDetails.Fields(Index).HeaderText = stringaRicerca
                    End If
                End If

                iResponse = True
            Catch ex As Exception
                Me._Errori = Errori_Db.System
                iResponse = False
            End Try
            Return iResponse
        End Function
#End Region

    End Class
End Namespace



Namespace Localizzazione
    Public NotInheritable Class ApplicationResourceManager

#Region "Private Property"
        Private _translations As New Dictionary(Of String, Hashtable)
        Private _UserLanguages As String
        Private _CultureInfo As CultureInfo
        Private _DefaultCultureName As String
        Private _LocalizedValues As Hashtable
        Private _ResourcesName As String
#End Region

#Region "Public Property"
        Public ReadOnly Property UserLanguages() As String
            Get
                UserLanguages = _UserLanguages
            End Get
        End Property
        Public ReadOnly Property CultureInfo() As CultureInfo
            Get
                CultureInfo = _CultureInfo
            End Get
        End Property
        Public ReadOnly Property ResourcesName() As String
            Get
                ResourcesName = _ResourcesName
            End Get
        End Property
        Public ReadOnly Property CurrentCultureName() As String
            Get
                ' Return System.Threading.Thread.CurrentThread.CurrentCulture.Name
                Try
                    Return _CultureInfo.Name
                Catch ex As Exception
                    Return Me._DefaultCultureName
                End Try

            End Get
        End Property
        Public ReadOnly Property DefaultCultureName() As String
            Get
                Return Me._DefaultCultureName
            End Get
        End Property
        Public ReadOnly Property LanguageFilePath() As String
            Get
                Return System.Configuration.ConfigurationManager.AppSettings("languageFilePath")
            End Get
        End Property
#End Region

        Public Sub New(resourceName As String, languageCode As String)
            _ResourcesName = resourceName
            Me._DefaultCultureName = System.Configuration.ConfigurationManager.AppSettings("defaultCulture")
            _UserLanguages = languageCode
        End Sub

        Public Sub setCulture()
            '  Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Me.UserLanguages)
            Try
                'Thread.CurrentThread.CurrentCulture = New CultureInfo(Me.UserLanguages)
                Thread.CurrentThread.CurrentUICulture = New CultureInfo(Me.UserLanguages)
                _CultureInfo = New CultureInfo(Thread.CurrentThread.CurrentUICulture.LCID, False)
            Catch ex As Exception
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
                _CultureInfo = New CultureInfo(Thread.CurrentThread.CurrentUICulture.LCID, False)
            End Try

        End Sub



#Region "Load Localized Value from XML FILE"
        Private Function SearchFromLocalizedValues(ByVal SearchValue As String) As String
            Dim iMessages As Hashtable = GetResource()
            Dim FoundValue As String = ""
            Try
                FoundValue = CStr(iMessages(SearchValue))
            Catch ex As Exception

            End Try
            Return FoundValue
        End Function
        Private Function GetResource() As Hashtable
            Dim currentCulture As String = CurrentCultureName
            Dim defaultCulture As String = Me.DefaultCultureName

            Dim resource As Hashtable = Nothing

            If (_translations.ContainsKey(currentCulture)) Then
                resource = _translations(currentCulture)
            Else
                If (_translations.ContainsKey(defaultCulture)) Then
                    resource = _translations(defaultCulture)
                Else
                    resource = LoadResource(defaultCulture)
                    _translations.Add(defaultCulture, resource)
                End If
                If defaultCulture <> currentCulture Then
                    Try
                        resource = LoadResource(currentCulture)
                        _translations.Add(currentCulture, resource)
                    Catch ex As FileNotFoundException
                    End Try
                End If
            End If
            Return resource
        End Function
        'Private Function GetResourceForLingua(ByVal CodeName As String) As Hashtable
        '    Dim currentCulture As String = CodeName
        '    Dim defaultCulture As String = Me.DefaultCultureName
        '    Dim resource As Hashtable = LoadResource(defaultCulture)
        '    If defaultCulture <> currentCulture Then
        '        Try
        '            resource = LoadResource(currentCulture)
        '        Catch ex As FileNotFoundException
        '        End Try
        '    End If
        '    Return resource
        'End Function
        Private Function LoadResource(ByVal culture As String) As Hashtable
            Dim resource As New Hashtable
            Try
                Dim fileName As String = Me.LanguageFilePath & "\" & culture & "\" & _ResourcesName & ".xml"
                fileName = Replace(fileName, "\\", "\")

                If lm.Comol.Core.File.Exists.File(fileName) Then
                    Dim xml As New XmlDocument
                    xml.Load(fileName)
                    For Each n As XmlNode In xml.SelectSingleNode("Resource")
                        If n.NodeType <> XmlNodeType.Comment Then
                            resource(n.Attributes("name").Value) = n.InnerText
                        End If
                    Next
                End If
            Catch ex As Exception

            End Try
            Return resource
        End Function
#End Region

#Region "Funzioni WebControl"
        Public Function getValue(ByVal key As String) As String
            Dim iResponse As String
            Dim messages As Hashtable = GetResource()
            iResponse = ""
            Try
                iResponse = CStr(messages(key))
            Catch ex As Exception
                Dim oStringa As String = ""
                oStringa = ex.Message
            End Try
            Return iResponse
        End Function
#End Region

    End Class
End Namespace