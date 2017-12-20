Public Class UC_HelperSimpleContainerItems
    Inherits System.Web.UI.UserControl

#Region "Internal"
    Private _ContainerCssClass As String
    Private _associatedControls As New Dictionary(Of ItemType, ItemType)
    Public Property ContainerCssClass As String
        Get
            Return _ContainerCssClass
        End Get
        Set(value As String)
            _ContainerCssClass = value
        End Set
    End Property
    Public Property FirstCssClass As String
        Get
            Return LBfirstItem.CssClass
        End Get
        Set(value As String)
            LBfirstItem.CssClass = value
        End Set
    End Property
    Public Property SecondCssClass As String
        Get
            Return LBsecondItem.CssClass
        End Get
        Set(value As String)
            LBsecondItem.CssClass = value
        End Set
    End Property
    Public Property ThirdCssClass As String
        Get
            Return LBthirdItem.CssClass
        End Get
        Set(value As String)
            LBthirdItem.CssClass = value
        End Set
    End Property
    Public Property FirstAsLabelOf As ItemType
        Get
            Return GetValue(ItemType.First)
        End Get
        Set(value As ItemType)
            SetAsLabel(ItemType.First, value)
        End Set
    End Property
    Public Property SecondAsLabelOf As ItemType
        Get
            Return GetValue(ItemType.Second)
        End Get
        Set(value As ItemType)
            SetAsLabel(ItemType.Second, value)
        End Set
    End Property
    Public Property ThirdAsLabelOf As ItemType
        Get
            Return GetValue(ItemType.Third)
        End Get
        Set(value As ItemType)
            SetAsLabel(ItemType.Third, value)
        End Set
    End Property
#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Internal"
    Public Sub InitializeControl(fValue As String, sValue As String, Optional tValue As String = "")
        LBfirstItem.Text = fValue
        LBsecondItem.Text = sValue
        LBthirdItem.Text = tValue
        LBthirdItem.Visible = Not String.IsNullOrEmpty(tValue)
    End Sub
    Private Sub SetAsLabel(ByVal source As ItemType, ByVal destination As ItemType)
        If source = destination Then
            destination = ItemType.None
        End If
        _associatedControls(source) = destination
        Select Case source
            Case ItemType.First
                SetAssociatedControl(LBfirstItem, destination)
            Case ItemType.Second
                SetAssociatedControl(LBsecondItem, destination)
            Case ItemType.Third
                SetAssociatedControl(LBthirdItem, destination)
        End Select
    End Sub
    Private Sub SetAssociatedControl(ByVal oLabel As Label, ByVal destination As ItemType)
        Select Case destination
            Case ItemType.First
                oLabel.AssociatedControlID = LBfirstItem.ID
            Case ItemType.Second
                oLabel.AssociatedControlID = LBsecondItem.ID
            Case ItemType.Third
                oLabel.AssociatedControlID = LBthirdItem.ID
            Case Else
                oLabel.AssociatedControlID = ""
        End Select
    End Sub
    Private Function GetValue(t As ItemType) As ItemType
        If _associatedControls.ContainsKey(ItemType.First) Then
            Return _associatedControls(ItemType.First)
        Else
            Return ItemType.None
        End If
    End Function
#End Region

    Public Enum ItemType As Integer
        None = 0
        First = 1
        Second = 2
        Third = 3
    End Enum
End Class