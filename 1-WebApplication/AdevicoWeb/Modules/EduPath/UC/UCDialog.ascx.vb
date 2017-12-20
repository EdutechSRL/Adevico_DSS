Public Class UCDialog
    Inherits System.Web.UI.UserControl

#Region "Property"
    'Private _commandArgument As String



    Private Function ViewStateOrDefault(Of T)(ByVal Key As String, ByVal DefaultValue As T) As T
        If (ViewState(Key) Is Nothing) Then
            Return DefaultValue
        Else
            Return ViewState(Key)
        End If
    End Function

    Public Property MultiSelection As Boolean
        Get
            Return ViewStateOrDefault("MultiSelection", False)
        End Get
        Set(ByVal value As Boolean)
            RBLoptions.Visible = Not value
            CHBoptions.Visible = value
            ViewState("MultiSelection") = value
        End Set
    End Property

    Public Property CommandArgument As String
        Get

            Return ViewStateOrDefault("CommandArgument", "")
        End Get
        Set(ByVal value As String)
            ViewState("CommandArgument") = value
        End Set
    End Property

    'Private _commandName As String
    Public Property CommandName As String
        Get
            Return ViewStateOrDefault("CommandName", "")
        End Get
        Set(ByVal value As String)
            ViewState("CommandName") = value
        End Set
    End Property

    'Private _serverSideCancel As Boolean = True
    Public Property ServerSideCancel As Boolean
        Get
            Return ViewStateOrDefault("ServerSideCancel", True)
        End Get
        Set(ByVal value As Boolean)
            ViewState("ServerSideCancel") = value
        End Set
    End Property

    Public ReadOnly Property JSDialogModal As String
        Get
            Return IIf(DialogModal, "true", "false")
        End Get
    End Property

    Public ReadOnly Property JSServerSideClass As String
        Get
            Return IIf(ServerSideCancel, "ServerSide", "ClientSide")
        End Get
    End Property

    Public Property SelectedIndex As Integer
        Get
            Return IIf(MultiSelection, CHBoptions.SelectedIndex, RBLoptions.SelectedIndex)
        End Get
        Set(ByVal value As Integer)
            RBLoptions.SelectedIndex = value
        End Set
    End Property

    'Public Property SelectedIndexes As IList(Of Integer)
    '    Get
    '        Return CHBoptions.
    '    End Get
    '    Set(ByVal value As Integer)
    '        RBLoptions.SelectedIndex = value
    '    End Set
    'End Property

    'Private _dialogResult As Integer = -3
    Public Property DialogResult As Integer
        Get
            Return ViewStateOrDefault("DialogResult", -3)
        End Get
        Set(ByVal value As Integer)
            ViewState("DialogResult") = value
        End Set
    End Property

    Public Property DialogResults As IList(Of Integer)
        Get
            Return ViewStateOrDefault("DialogResults", New List(Of Integer))
        End Get
        Set(ByVal value As IList(Of Integer))
            ViewState("DialogResults") = value
        End Set
    End Property

    'Private _optionsList As IList(Of String)
    Public Property OptionsList As IList(Of String)
        Get
            Return ViewStateOrDefault("DataSource", New List(Of String))
        End Get
        Set(ByVal value As IList(Of String))
            ViewState("DataSource") = value
        End Set
    End Property

    Public Property DataSource As IList(Of String)
        Get
            Return OptionsList
        End Get
        Set(ByVal value As IList(Of String))
            OptionsList = value
        End Set
    End Property

    'Private _dialogID As String
    Public Property DialogID As String
        Get
            Return ViewStateOrDefault("DialogID", "")
        End Get
        Set(ByVal value As String)
            ViewState("DialogID") = value
        End Set
    End Property

    'Private _dialogClass As String
    Public Property DialogClass As String
        Get
            Return ViewStateOrDefault("DialogClass", "")
        End Get
        Set(ByVal value As String)
            ViewState("DialogClass") = value
        End Set
    End Property

    'Private _dialogTitle As String = "Dialog"
    Public Property DialogTitle As String
        Get
            Return ViewStateOrDefault("DialogTitle", "Dialog")
        End Get
        Set(ByVal value As String)
            ViewState("DialogTitle") = value
        End Set
    End Property

    'Private _dialogOk As String = "Ok"
    Public Property DialogOk As String
        Get
            Return BTNok.Text
        End Get
        Set(ByVal value As String)
            BTNok.Text = value
        End Set
    End Property

    'Private _dialogCancel As String = "Cancel"
    Public Property DialogCancel As String
        Get
            Return BTNcancel.Text
        End Get
        Set(ByVal value As String)
            BTNcancel.Text = value
        End Set
    End Property

    'Private _dialogText As String = "Question"
    Public Property DialogText As String
        Get
            Return ViewStateOrDefault("DialogText", "Question")
        End Get
        Set(ByVal value As String)
            ViewState("DialogText") = value
        End Set
    End Property

    'Private _dialogModal As Boolean = True
    Public Property DialogModal As Boolean
        Get
            Return ViewStateOrDefault("DialogModal", True)
        End Get
        Set(ByVal value As Boolean)
            ViewState("DialogModal") = value
        End Set
    End Property

    Public Property DialogPreventClose As Boolean
        Get
            Return ViewStateOrDefault("DialogPreventClose", True)
        End Get
        Set(ByVal value As Boolean)
            ViewState("DialogPreventClose") = value
        End Set
    End Property

    Public Property ConfirmationOnly As Boolean
        Get
            Return ViewStateOrDefault("ConfirmationOnly", False)
        End Get
        Set(ByVal value As Boolean)
            ViewState("ConfirmationOnly") = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'RBLoptions.DataSource = OptionsList
        'RBLoptions.DataBind()

    End Sub

    Public Sub InitializeControl(ByVal optionsList As IList(Of String), ByVal defaultIndex As Integer, Optional ByVal multiSelection As Boolean = False, Optional ByVal defaultIndexes As IList(Of Integer) = Nothing)
        DataSource = optionsList
        BTNcancel.Visible = ConfirmationOnly = False
        Me.MultiSelection = multiSelection
        If (multiSelection) Then
            CHBoptions.DataSource = DataSource
            CHBoptions.DataBind()
            If (Not defaultIndexes Is Nothing) Then
                For Each item As Integer In defaultIndexes
                    CHBoptions.Items(item).Selected = True
                Next
            End If
        Else
            RBLoptions.DataSource = DataSource
            RBLoptions.DataBind()
            RBLoptions.SelectedIndex = defaultIndex
           
        End If

    End Sub

    Public Sub DataBind()


        If (Me.MultiSelection) Then
            CHBoptions.DataSource = DataSource
            CHBoptions.DataBind()
        Else
            RBLoptions.DataSource = DataSource
            RBLoptions.DataBind()
        End If

        BTNcancel.Visible = ConfirmationOnly = False


        If DataSource.Count > 0 Then
            HRline.Visible = True
        Else
            HRline.Visible = False
        End If
    End Sub

    Public Event ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String)

    Public Event ButtonPressedMulti(ByVal dialogResult As Integer, ByVal dialogResults As IList(Of Integer), ByVal CommandArgument As String, ByVal CommandName As String)

    'Public Event ButtonPressed(ByVal dialogResult As Integer, ByVal CommandArgument As String, ByVal CommandName As String)

    Private Sub BTNok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNok.Click
        DialogResult = SelectedIndex

        If (Me.MultiSelection) Then

            'DialogResults = New List(Of Integer)
            'Dim k As Integer = 0
            'For Each item As ListItem In CHBoptions.Items
            '    If (item.Selected = True) Then
            '        DialogResults.Add(k)
            '        k = k + 1
            '    End If
            'Next

            DialogResults = (From item As ListItem In CHBoptions.Items Where item.Selected = True Select CHBoptions.Items.IndexOf(item)).ToList()

            RaiseEvent ButtonPressedMulti(DialogResult, DialogResults, Me.HIDcommandArgument.Value, Me.HIDcommandName.Value)
        Else

            RaiseEvent ButtonPressed(DialogResult, Me.HIDcommandArgument.Value, Me.HIDcommandName.Value)
        End If

    End Sub

    Private Sub BTNcancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BTNcancel.Click
        DialogResult = -2

        If (Me.MultiSelection) Then
            DialogResults = New List(Of Integer)

            'Dim k As Integer = 0
            'For Each item As ListItem In CHBoptions.Items
            '    If (item.Selected = True) Then
            '        DialogResults.Add(k)
            '        k = k + 1
            '    End If
            'Next

            'DialogResults = (From item As ListItem In CHBoptions.Items Where item.Selected = True Select CHBoptions.Items.IndexOf(item)).ToList()

            RaiseEvent ButtonPressedMulti(DialogResult, DialogResults, Me.HIDcommandArgument.Value, Me.HIDcommandName.Value)
        Else

            RaiseEvent ButtonPressed(DialogResult, Me.HIDcommandArgument.Value, Me.HIDcommandName.Value)
        End If

    End Sub
End Class