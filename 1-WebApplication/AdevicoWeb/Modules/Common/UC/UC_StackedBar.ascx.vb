Public Class UC_StackedBar
    Inherits BaseControl

#Region "Internal"
    Private _DisplayRealValue As Boolean
    Public Property BarItems As List(Of StackedBarItem)
        Get
            Return ViewStateOrDefault("BarItems", New List(Of StackedBarItem))
        End Get
        Set(value As List(Of StackedBarItem))
            ViewState("BarItems") = value
        End Set
    End Property

    Public Property ContainerCssClass As String
        Get
            Return ViewStateOrDefault("ContainerCssClass", "")
        End Get
        Set(value As String)
            ViewState("ContainerCssClass") = value
        End Set
    End Property
    Public Property DisplayRealValue As Boolean
        Get
            Return _DisplayRealValue
        End Get
        Set(value As Boolean)
            _DisplayRealValue = value
        End Set
    End Property
#End Region
   

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '<span class="progressbar"><span class="completed" title="Completed and Passed: 50%">&nbsp;</span><span class="notpassed" title="Not Passed: 0%">&nbsp;</span><span class="started" title="Started: 24%">&nbsp;</span><span class="notstarted" title="Not Started: 26%">&nbsp;</span></span>

        LBprogressbar.Text = "<span class='progressbar " & ContainerCssClass & "' >"
        For Each item As StackedBarItem In BarItems
            LBprogressbar.Text += HtmlElement(item)
        Next
        LBprogressbar.Text += "</span>"

    End Sub

    Public Sub InitializeControl(items As IEnumerable(Of StackedBarItem))
        InitializeControl(items, ContainerCssClass)
    End Sub

    Public Sub InitializeControl(items As IEnumerable(Of StackedBarItem), cssClass As String)
        BarItems = items.ToList()
        LBprogressbar.Text = "<span class='progressbar " & cssClass & "' >"
        For Each item As StackedBarItem In items
            LBprogressbar.Text += HtmlElement(item)
        Next
        LBprogressbar.Text += "</span>"
    End Sub
    'Public Sub InitializeControl(items As IEnumerable(Of StackedBarItem))
    '    InitializeControl("", items)
    'End Sub
    'Public Sub InitializeControl(ByVal cssClass As String, items As IEnumerable(Of StackedBarItem))
    '    ContainerCssClass = cssClass

    'End Sub
    Protected Function HtmlElement(item As StackedBarItem) As String
        Dim title As String = String.Format(item.Title, item.Value)
        Return String.Format("<span class='{0}' title='{1}'>&nbsp;</span>", item.CssClass, title)
    End Function

    Protected Overrides Sub SetCultureSettings()

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()

    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
End Class

<Serializable()> _
Public Class StackedBarItem
    Private _cssclass As String
    Private _title As String
    Private _value As Int32
    Private _Realvalue As Long
    Public Property CssClass As String
        Get
            Return _cssclass
        End Get
        Set(value As String)
            _cssclass = value
        End Set
    End Property
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property
    Public Property Value As Int32
        Get
            Return _value
        End Get
        Set(value As Int32)
            _value = value
        End Set
    End Property
    Public Property RealValue As Long
        Get
            Return _Realvalue
        End Get
        Set(value As Long)
            _Realvalue = value
        End Set
    End Property
End Class