Public Class UC_ExpandAndCollapse
    Inherits DBbaseControl

#Region "Internal"
    Private _ExpandText As String
    Private _ExpandToolTip As String
    Private _CollapseText As String
    Private _CollapseToolTip As String
    Private _InLineTranslation As Boolean
    Public Property InLineTranslation As Boolean
        Get
            Return _InLineTranslation
        End Get
        Set(value As Boolean)
            _InLineTranslation = value
        End Set
    End Property
    Public Property ExpandText As String
        Get
            Return _ExpandText
        End Get
        Set(value As String)
            _ExpandText = value
            If InLineTranslation Then
                LBspanExpandList.Text = value
            End If
        End Set
    End Property
    Public Property ExpandToolTip As String
        Get
            Return _ExpandToolTip
        End Get
        Set(value As String)
            _ExpandToolTip = value
            If InLineTranslation Then
                LBspanExpandList.Text = value
            End If
        End Set
    End Property
    Public Property CollapseText As String
        Get
            Return _CollapseText
        End Get
        Set(value As String)
            _CollapseText = value
            If InLineTranslation Then
                LBspanCollapseList.Text = value
            End If
        End Set
    End Property
    Public Property CollapseToolTip As String
        Get
            Return _CollapseToolTip
        End Get
        Set(value As String)
            _CollapseToolTip = value
            If InLineTranslation Then
                LBspanCollapseList.ToolTip = value
            End If
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            If Not InLineTranslation Then
                .setLabel(LBspanExpandList)
                .setLabel(LBspanCollapseList)
            End If
        End With
    End Sub
End Class