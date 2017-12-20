Public Class UC_OpenDialogHeaderScripts
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property OpendialogCssClassPrefix As String
        Get
            Return LTopendialogcssclassprefix.Text
        End Get
    End Property


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetScriptInitialized(title As String, dialogIdentifyer As String, openDialogClass As String, width As Integer, height As Integer, Optional minWidth As Integer = 0, Optional minHeight As Integer = 0, Optional autoOpen As Boolean = False, Optional closeScript As String = "") As String
        Dim script As String = GetBaseScript()

        If autoOpen Then
            script = script.Replace(GetPlaceholderAutoOpen, GetAutoOpenScript)
        Else
            script = script.Replace(GetPlaceholderAutoOpen, "")
        End If
        script = script.Replace(GetPlaceholderTitle, title.Replace("'", "\'"))
        script = script.Replace(GetPlaceholderDialogIdentifyer, dialogIdentifyer)
        script = script.Replace(GetPlaceholderOpendialog, openDialogClass)
        script = script.Replace(GetPlaceholderCloseScripts, closeScript)

        Dim sizes As List(Of String) = GetWwindowDefaultSizes.Split(",").Where(Function(s) Not String.IsNullOrEmpty(s)).ToList().Where(Function(s) IsNumeric(s)).ToList
        If width > 0 Then
            script = script.Replace(GetPlaceholderWidth, width)
        Else
            script = script.Replace(GetPlaceholderWidth, sizes(0))
        End If
        If height > 0 Then
            script = script.Replace(GetPlaceholderHeight, height)
        Else
            script = script.Replace(GetPlaceholderHeight, sizes(1))
        End If
        If minWidth > 0 Then
            script = script.Replace(GetPlaceholderMinWidth, minWidth)
        ElseIf sizes(2) > width Then
            script = script.Replace(GetPlaceholderMinWidth, width)
        Else
            script = script.Replace(GetPlaceholderMinWidth, sizes(2))
        End If
        If minHeight > 0 Then
            script = script.Replace(GetPlaceholderMinHeight, minHeight)
        ElseIf sizes(3) > height Then
            script = script.Replace(GetPlaceholderMinWidth, height)
        Else
            script = script.Replace(GetPlaceholderMinHeight, sizes(3))
        End If

        Return script

    End Function
    Public Function GetBaseScript() As String
        Return LTbaseScript.Text
    End Function
    Public Function GetAutoOpenScript() As String
        Return LTautoOpenScript.Text
    End Function
    Public Function GetPlaceholderAutoOpen() As String
        Return LTplaceholderAutoOpen.Text
    End Function
    Public Function GetPlaceholderTitle() As String
        Return LTplaceholderTitle.Text
    End Function
    Public Function GetPlaceholderWidth() As String
        Return LTplaceholderdWidth.Text
    End Function
    Public Function GetPlaceholderHeight() As String
        Return LTplaceholderdHeight.Text
    End Function
    Public Function GetPlaceholderMinWidth() As String
        Return LTplaceholderdMinWidth.Text
    End Function
    Public Function GetPlaceholderMinHeight() As String
        Return LTplaceholderdMinHeight.Text
    End Function
    Public Function GetPlaceholderOpendialog() As String
        Return LTplaceholderOpendialog.Text
    End Function
    Public Function GetPlaceholderDialogIdentifyer() As String
        Return LTplaceholderdDialogIdentifyer.Text
    End Function
    Public Function GetPlaceholderCloseScripts() As String
        Return LTplaceholderdCloseScripts.Text
    End Function
    Public Function GetDivDialogCssClassPrefix() As String
        Return LTdivdialogcssclassprefix.Text
    End Function
    Public Function GetWwindowDefaultSizes() As String
        Return LTwindowSizes.Text
    End Function


End Class