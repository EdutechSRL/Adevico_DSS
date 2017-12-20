Public Class UC_EditUrlItemsHeader
    Inherits System.Web.UI.UserControl

    Public Property EditingCssClass As String
        Get
           Return ViewState("EditingCssClass")
        End Get
        Set(value As String)
            ViewState("EditingCssClass") = value
        End Set
    End Property
    Public ReadOnly Property IsInitialized As Boolean
        Get
            Return Not LTscript.Text.Contains(LTplaceholderdScript.Text)
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub InitializeControl(openDialogClass As String, title As String, width As Integer, height As Integer, Optional minWidth As Integer = 0, Optional minHeight As Integer = 0, Optional autoOpen As Boolean = False)
        InitializeControl(openDialogClass, EditingCssClass, title, width, height, minWidth, minHeight, autoOpen)
    End Sub
    Public Sub InitializeControl(openDialogClass As String, cssClass As String, title As String, width As Integer, height As Integer, Optional minWidth As Integer = 0, Optional minHeight As Integer = 0, Optional autoOpen As Boolean = False)
        If Not IsInitialized Then
            Dim sizes As List(Of String) = LTdefaultWindow.Text.Split(",").Where(Function(s) Not String.IsNullOrEmpty(s)).ToList().Where(Function(s) IsNumeric(s)).ToList
            If width = 0 AndAlso sizes.Any Then
                width = CInt(sizes(0))
            End If
            If height = 0 AndAlso sizes.Any Then
                height = CInt(sizes(1))
            End If
            If minWidth = 0 AndAlso sizes.Any Then
                minWidth = CInt(sizes(2))
            End If
            If minHeight = 0 AndAlso sizes.Any Then
                minHeight = CInt(sizes(3))
            End If
            LTscript.Text = LTscript.Text.Replace(LTplaceholderdScript.Text, CTRLscripts.GetScriptInitialized(title, cssClass, openDialogClass, width, height, minWidth, minHeight, autoOpen, LTcloseScripts.Text))
            LTscript.Visible = True
        End If
    End Sub

End Class