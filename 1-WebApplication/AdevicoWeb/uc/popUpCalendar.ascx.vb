Public Class popUpCalendar
    Inherits System.Web.UI.UserControl
    Protected WithEvents Calendar1 As System.Web.UI.WebControls.Calendar
    Protected WithEvents pnlCalendar As System.Web.UI.WebControls.Panel

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub
    Public Sub displayCalendar(ByVal sCalToolText As String, ByVal dSelectedDate As Date, ByVal sDateFieldName As String, ByVal iTop As Integer, ByVal iLeft As Integer)
        '************************************************************************
        'Purpose: Displays & hides the calendar.
        'Input: sCalToolText - Text to use as the tooltip text for the calendar
        ' dSelectedDate - Date to set as the selected date for the calendar
        ' txtDate - string containing name of applicable textbox
        ' iTop - top position of calendar
        ' iLeft - left position of calendar
        '************************************************************************

        'If the calendar is visible, but it is displayed for a different field
        'than the one selected, then hide it
        If pnlCalendar.Visible = True And Calendar1.Attributes.Item("selectedfield") <> sDateFieldName Then
            hideCalendar()
        End If

        'If the calendar is hidden, then position it and show it, otherwise hide it
        If pnlCalendar.Visible = False Then

            'Set the location of the calendar
            pnlCalendar.Style.Item("top") = iTop
            pnlCalendar.Style.Item("left") = iLeft

            'If a valid date is passed in, then set this date as the selected date
            'on the calendar. Otherwise display the current month and year
            If IsDate(dSelectedDate) Then
                'Setting the selected date property will tell the calendar to
                'highlight the date assigned to this property.
                Calendar1.SelectedDate = dSelectedDate

                'Setting the visible date property will tell the calendar to
                'initially display the month and year of the date assigned to
                'this property.
                Calendar1.VisibleDate = dSelectedDate
            Else
                Calendar1.SelectedDate = #12:00:00 AM#
                Calendar1.VisibleDate = Now
            End If

            'Set the tooltip text and id of the selected field
            Calendar1.ToolTip = sCalToolText
            Calendar1.Attributes.Item("SelectedField") = sDateFieldName

            'Show the calendar
            pnlCalendar.Visible = True
        Else
            'Hide the calendar
            hideCalendar()
        End If

    End Sub

    Public Sub Calendar1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calendar1.SelectionChanged
        '************************************************************************
        'Purpose: Write the selected date to the appropriate text field.
        '************************************************************************
        Dim txtDate As TextBox

        'get the textbox that the date should be written to
        txtDate = Page.FindControl(Calendar1.Attributes.Item("SelectedField"))

        'Write value to appropriate textbox
        txtDate.Text = Calendar1.SelectedDate

        'Hide the calendar
        hideCalendar()

    End Sub

    Public Sub hideCalendar()
        'Hide the calendar
        pnlCalendar.Visible = False
    End Sub
End Class
