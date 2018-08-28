Namespace Configuration
    <Serializable(), CLSCompliant(True)> Public Class IcodeonSettings

#Region "Private properties"
        Private _DBname As String
        Private _PlayerBaseUrl As String
        Private _SiteDownloadScorm As Boolean
        Private _OverrideSSLsettings As Boolean
        Private _MappingPath As String
        Private _StatisticsRules As Dictionary(Of Integer, IcodeonStatistics)
        Private _AutoEvaluate As Boolean
#End Region

#Region "Public properties"
        Public Property StatisticsRules() As Dictionary(Of Integer, IcodeonStatistics)
            Get
                Return _StatisticsRules
            End Get
            Set(ByVal value As Dictionary(Of Integer, IcodeonStatistics))
                _StatisticsRules = value
            End Set
        End Property
        Public Property DBname() As String
            Get
                Return _DBname
            End Get
            Set(ByVal value As String)
                _DBname = value
            End Set
        End Property
        Public Property PlayerBaseUrl() As String
            Get
                PlayerBaseUrl = _PlayerBaseUrl
            End Get
            Set(ByVal value As String)
                _PlayerBaseUrl = value
            End Set
        End Property
        Public Property SiteDownloadScorm() As Boolean
            Get
                SiteDownloadScorm = _SiteDownloadScorm
            End Get
            Set(ByVal value As Boolean)
                _SiteDownloadScorm = value
            End Set
        End Property
        Public Property OverrideSSLsettings() As Boolean
            Get
                OverrideSSLsettings = _OverrideSSLsettings
            End Get
            Set(ByVal value As Boolean)
                _OverrideSSLsettings = value
            End Set
        End Property
        Public Property AutoEvaluate() As Boolean
            Get
                AutoEvaluate = _AutoEvaluate
            End Get
            Set(ByVal value As Boolean)
                _AutoEvaluate = value
            End Set
        End Property
        Public Function GetScormLink_Old(ByVal UserId As Integer, ByVal ScormPath As String, ByVal SessionID As String, notSaveStat As Boolean) As String

            Dim WebPlayerString As String = Me._PlayerBaseUrl   'VALIDO SOLO PER TSM, eventualmente mettere in file di configurazione

            Dim UrlString As String
            UrlString = WebPlayerString & "/skins/LMSMain.aspx"
            UrlString &= "?learnerID=" & UserId.ToString
            UrlString &= "&courseID=" & ScormPath.Replace(" ", "%20").Replace("\", "%2F").Replace("/", "%2F")
            UrlString &= "&sessionID=" & SessionID
            UrlString &= "&skinID=default"
            If notSaveStat Then
                UrlString &= "&mode=browse"
            End If
            'VALUTARE SE: fare così, per utilizzare "insieme" più database o mettere in file di configurazione, come fatto per l'URL del player.

            UrlString &= "&domainID=" & _DBname

            'Dovrebbe essereci una sorta di URL ENCODE, ma non ricordo DOVE/QUALE sia.
            Return UrlString

        End Function

        Public Function GetScormRepositoryLink(ByVal FileId As Long, ByVal ScormID As System.Guid, ByVal ForUserID As Integer, ByVal ModuleID As Integer, ByVal CommunityID As Integer, ByVal ActionID As Integer, ByVal Language As String) As String
            Dim url As String = "Modules/Scorm/ScormPlayerLoader.aspx"
            url &= "?FileID=" & FileId.ToString
            url &= "&FileUniqueID=" & ScormID.ToString
            url &= "&ModuleID=" & ModuleID.ToString
            url &= "&CommunityID=" & CommunityID.ToString
            url &= "&ActionID=" & ActionID
            url &= "&ForUserID=" & ForUserID.ToString
            url &= "&Language=" & Language.ToString
            Return url
        End Function
        Public Function GetScormOtherModuleLink(ByVal FileId As Long, ByVal ScormID As System.Guid, ByVal ForUserID As Integer, ByVal ModuleID As Integer, ByVal CommunityID As Integer, ByVal ItemID As String, ByVal ItemTypeID As String, ByVal Language As String) As String
            Dim url As String = "Modules/Scorm/ScormPlayerLoader.aspx"
            url &= "?FileID=" & FileId.ToString
            url &= "&FileUniqueID=" & ScormID.ToString
            url &= "&ModuleID=" & ModuleID.ToString
            url &= "&CommunityID=" & CommunityID.ToString
            url &= "&ItemID=" & ItemID.ToString
            url &= "&ItemTypeID=" & ItemTypeID.ToString
            url &= "&ForUserID=" & ForUserID.ToString
            url &= "&Language=" & Language.ToString
            Return url
        End Function
        Public Function GetScormGenericModuleLink(ByVal FileId As Long, ByVal ScormID As System.Guid, ByVal ForUserID As Integer, ByVal ServiceID As Integer, ByVal CommunityID As Integer, ByVal LinkID As Long, ByVal Language As String, notSaveStat As Boolean) As String
            Dim url As String = "Modules/Scorm/ScormPlayerLoader.aspx"
            url &= "?FileID=" & FileId.ToString
            url &= "&FileUniqueID=" & ScormID.ToString
            url &= "&ModuleID=" & ServiceID.ToString
            url &= "&CommunityID=" & CommunityID.ToString
            url &= "&LinkID=" & LinkID.ToString
            url &= "&ForUserID=" & ForUserID.ToString
            url &= "&Language=" & Language.ToString
            If notSaveStat Then
                url &= "&notSaveStat=" & notSaveStat.ToString
            End If
            Return url
        End Function
        Public Function GetScormOtherModuleLinkUpdateAction(ByVal FileId As Long, ByVal ScormID As System.Guid, ByVal ForUserID As Integer, ByVal ServiceID As Integer, ByVal ServiceActionID As Integer, ByVal CommunityID As Integer, ByVal ItemID As String, ByVal ItemTypeID As String, ByVal Language As String) As String
            Dim url As String = "Modules/Scorm/ScormPlayerLoader.aspx"
            url &= "?FileID=" & FileId.ToString
            url &= "&FileUniqueID=" & ScormID.ToString
            url &= "&ModuleID=" & ServiceID.ToString
            url &= "&CommunityID=" & CommunityID.ToString
            url &= "&ActionID=" & ServiceActionID.ToString
            url &= "&ItemID=" & ItemID.ToString
            url &= "&ItemTypeID=" & ItemTypeID.ToString
            url &= "&ForUserID=" & ForUserID.ToString
            url &= "&Language=" & Language.ToString
            Return url
        End Function
        Public Function GetScormOtherModuleLinkUpdateAction(ByVal FileId As Long, ByVal ScormID As System.Guid, ByVal ForUserID As Integer, ByVal ServiceID As Integer, ByVal ServiceActionID As Integer, ByVal CommunityID As Integer, ByVal LinkID As Long, ByVal Language As String, notSaveStat As Boolean, ByVal WorkingSessionID As System.Guid) As String
            Dim url As String = "Modules/Scorm/ScormPlayerLoader.aspx"
            url &= "?FileID=" & FileId.ToString
            url &= "&FileUniqueID=" & ScormID.ToString
            url &= "&ModuleID=" & ServiceID.ToString
            url &= "&CommunityID=" & CommunityID.ToString
            url &= "&ActionID=" & ServiceActionID.ToString
            url &= "&LinkID=" & LinkID.ToString
            url &= "&ForUserID=" & ForUserID.ToString
            url &= "&Language=" & Language.ToString
            url &= "&WorkingSessionID=" & WorkingSessionID.ToString
            If notSaveStat Then
                url &= "&notSaveStat=" & notSaveStat.ToString
            End If
            Return url
        End Function
        Public Property MappingPath() As String
            Get
                Return _MappingPath
            End Get
            Set(ByVal value As String)
                _MappingPath = value
            End Set
        End Property
#End Region

        Public Sub New()
            Me._SiteDownloadScorm = False
            Me._OverrideSSLsettings = False
            Me._StatisticsRules = New Dictionary(Of Integer, IcodeonStatistics)
            _AutoEvaluate = False
        End Sub

        Public Class IcodeonStatistics
            Public Property Restricted As Boolean
            Public Property RestrictedRoles As List(Of Integer)
            Public Sub New()
                Restricted = False
                RestrictedRoles = New List(Of Integer)
            End Sub
        End Class

        Public Function AllowViewAdvancedStatistics(ByVal IdOrganization As Integer, ByVal IdRole As Integer) As Boolean
            If StatisticsRules.ContainsKey(IdOrganization) AndAlso Not IsNothing(StatisticsRules(IdOrganization)) Then
                Return (StatisticsRules(IdOrganization)).Restricted = False OrElse (StatisticsRules(IdOrganization).Restricted AndAlso (StatisticsRules(IdOrganization)).RestrictedRoles.Contains(IdRole))
            Else
                Return True
            End If
        End Function
    End Class
End Namespace