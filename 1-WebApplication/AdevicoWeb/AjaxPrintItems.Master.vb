Public Class AjaxPrintItems
    Inherits System.Web.UI.MasterPage
    Private _Resource As ResourceManager
    Private _PageUtility As OLDpageUtility
    Public ReadOnly Property IsoCode As String
        Get
            Return New System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID, False).TwoLetterISOLanguageName
        End Get
    End Property
    Private ReadOnly Property PageUtility() As OLDpageUtility
        Get
            If IsNothing(_PageUtility) Then
                _PageUtility = New OLDpageUtility(Me.Context)
            End If
            Return _PageUtility
        End Get
    End Property
    Public WriteOnly Property ServiceTitle() As String
        Set(ByVal value As String)
            Me.LBtitle.Text = value
        End Set
    End Property
    Public WriteOnly Property ServiceTitleToolTip() As String
        Set(ByVal value As String)
            Me.LBtitle.ToolTip = value
        End Set
    End Property
    Public WriteOnly Property ServiceNopermission() As String
        Set(ByVal value As String)
            Me.LBNopermessi.Text = value
        End Set
    End Property
    Public Property ShowNoPermission() As Boolean
        Get
            Return (Me.MLVservice.GetActiveView Is Me.VIWnoPermission)
        End Get
        Set(ByVal value As Boolean)
            If value Then
                Me.MLVservice.SetActiveView(Me.VIWnoPermission)
            Else
                Me.MLVservice.SetActiveView(Me.VIWservice)
            End If
        End Set
    End Property

#Region "DialogOpenOnPostback"
    Public Property OpenDialogOnPostback As Boolean
        Get
            Return Not HDNwindowopened.Attributes("class").Contains("disabled")
        End Get
        Set(ByVal value As Boolean)
            Dim cssClass As String = HDNwindowopened.Attributes("class")
            If value Then
                cssClass = Replace(cssClass, "disabled", "")
            ElseIf Not cssClass.Contains("disabled") Then
                cssClass &= " disabled"
            End If
            HDNwindowopened.Attributes("class") = cssClass
        End Set
    End Property
    Public Sub ClearOpenedDialogOnPostback()
        HDNwindowopened.Value = ""
    End Sub
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsNothing(Me._Resource) AndAlso Not Page.IsPostBack Then
            SetCulture("pg_ISAuthenticationPage", "Authentication")
            _Resource.setButton(BTNprintItems, True)
        End If
    End Sub
    Public Sub SetCulture(ByVal ResourcePage As String, ByVal ResourceFolder As String)
        Me._Resource = New ResourceManager

        Me._Resource.UserLanguages = Me.PageUtility.LinguaCode
        Me._Resource.ResourcesName = ResourcePage
        Me._Resource.Folder_Level1 = ResourceFolder
        Me._Resource.setCulture()
    End Sub

#Region "DocType"
    Public Property ShowDocType As Boolean
        Get
            Return Me.Lit_DocType.Visible
        End Get
        Set(ByVal value As Boolean)
            Me.Lit_DocType.Visible = value
        End Set
    End Property

    Public ReadOnly Property DocTypeClass As String
        Get
            If ShowDocType Then
                Return "ok-doctype"
            Else
                Return "no-doctype"
            End If
        End Get

    End Property
#End Region

End Class