Public Class TicketHelper

    Public Const DateFormat As String = "dd/MM/yyyy"
    Public Const TimeFormat As String = "HH:mm"
    Public Const DateTimeFormat As String = "dd/MM/yyyy HH:mm"

    ''' <summary>
    ''' Imposta un literal del repeater, sul valore indicato
    ''' </summary>
    ''' <param name="Item">Item corrente del repeater: e.Item</param>
    ''' <param name="LitName">ID assegnato al literal</param>
    ''' <param name="Value">Valore da assegnare</param>
    ''' <remarks></remarks>
    Public Shared Sub SetRPTLiteral( _
                                   ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                                   ByVal LitName As String, _
                                   ByVal Value As String)
        Dim lit As Literal = Item.FindControl(LitName)

        If Not IsNothing(lit) Then
            lit.Text = Value
        End If
    End Sub

    'ResourceManager
    Public Shared Sub SetRPTLiteral( _
                                   ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                                   ByVal LitName As String, _
                                   ByVal Resource As ResourceManager)
        Dim lit As Literal = Item.FindControl(LitName)

        If Not IsNothing(lit) AndAlso Not IsNothing(Resource) Then
            Resource.setLiteral(lit)
        End If

    End Sub

    Public Enum DateTimeMode
        OnlyDate
        OnlyTime
        DateAndTime
    End Enum

    Public Shared Sub SetRPTLiteral( _
                               ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                               ByVal LitName As String, _
                               ByVal Value As DateTime, _
                               ByVal Mode As DateTimeMode)
        Dim lit As Literal = Item.FindControl(LitName)

        If Not IsNothing(lit) Then
            Select Case Mode
                Case DateTimeMode.OnlyDate
                    lit.Text = Value.ToString(DateFormat)
                Case DateTimeMode.OnlyTime
                    lit.Text = Value.ToString(TimeFormat)
                Case DateTimeMode.DateAndTime
                    lit.Text = Value.ToString(DateTimeFormat)
            End Select

        End If
    End Sub

    ''' <summary>
    ''' Imposta le label del repeater
    ''' </summary>
    ''' <param name="Item">Item corrente del repeater: e.Item </param>
    ''' <param name="LblName">Nome della label</param>
    ''' <param name="Value">Valore da impostare</param>
    ''' <param name="MaxChar">Massimo caratteri visualizzabili per il value. 
    ''' Se minore di 3 non viene considerato.
    ''' Se il valore supera tale valore, il valore è tagliato e sostituito con ...
    ''' Il valore completo è inserito nel ToolTip della label</param>
    ''' <remarks></remarks>
    Public Shared Sub SetRPTLabel(ByRef Item As System.Web.UI.WebControls.RepeaterItem, ByVal LblName As String, ByVal Value As String, Optional ByVal MaxChar As Integer = 0)
        Dim lbl As Label = Item.FindControl(LblName)

        If Not IsNothing(lbl) Then
            lbl.Text = Value
            lbl.ToolTip = Value

            If Not IsNothing(lbl) Then
                If (MaxChar > 3) Then
                    If (Value.Length > MaxChar) Then
                        lbl.Text = Value.Remove(MaxChar - 3) & "..."
                    End If
                End If

            End If
        End If

    End Sub

    ''' <summary>
    ''' Imposta le label del repeater
    ''' </summary>
    ''' <param name="Item">Item corrente del repeater: e.Item </param>
    ''' <param name="LblName">Nome della label</param>
    ''' <param name="Value">Valore da impostare</param>
    ''' <param name="MaxChar">Massimo caratteri visualizzabili per il value. 
    ''' Se minore di 3 non viene considerato.
    ''' Se il valore supera tale valore, il valore è tagliato e sostituito con ...
    ''' Il valore completo è inserito nel ToolTip della label</param>
    ''' <remarks></remarks>
    Public Shared Sub SetRPTLabelAlt(ByRef Item As System.Web.UI.WebControls.RepeaterItem, ByVal LblName As String, ByVal Value As String, ByVal alt As String, Optional ByVal MaxChar As Integer = 0)
        Dim lbl As Label = Item.FindControl(LblName)

        If Not IsNothing(lbl) Then
            lbl.Text = Value
            lbl.ToolTip = alt

            If Not IsNothing(lbl) Then
                If (MaxChar > 3) Then
                    If (Value.Length > MaxChar) Then
                        lbl.Text = Value.Remove(MaxChar - 3) & "..."
                    End If
                End If

            End If
        End If

    End Sub
    Public Shared Sub SetRPTLabel( _
                           ByRef Item As System.Web.UI.WebControls.RepeaterItem, _
                           ByVal LblName As String, _
                           ByVal Value As DateTime, _
                           ByVal Mode As DateTimeMode)
        Dim lbl As System.Web.UI.WebControls.Label = Item.FindControl(LblName)

        If Not IsNothing(lbl) Then
            Select Case Mode
                Case DateTimeMode.OnlyDate
                    lbl.Text = Value.ToString(DateFormat)
                Case DateTimeMode.OnlyTime
                    lbl.Text = Value.ToString(TimeFormat)
                Case DateTimeMode.DateAndTime
                    lbl.Text = Value.ToString(DateTimeFormat)
            End Select

        End If
    End Sub

    ''' <summary>
    ''' Mi indica la classe css da applicare agli elementi "INFO" nelle liste Ticket (man/res e utente).
    ''' Messo qui perchè riguarda ESCLUSIVAMENTE le VIEW
    ''' </summary>
    ''' <remarks>Modificare gli elementi dell'enum, NECESSITA modificare i relativi CSS</remarks>
    Public Enum InfoFields
        open
        inprogress
        request
        draft
        closedSolved
        closedUnsolved
    End Enum

    Public Const SessionExtUser As String = "TICKET.CurrentExtUser"
    Public Const SessionLanguageCode As String = "LinguaCode"
    Public Const SessionLanguageId As String = "LinguaID"
    Public Const SessionLanguage As String = "UserLanguage"

    'Public Function Settings() As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_NotificationSettings
    '    Dim sets As New lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_NotificationSettings

    '    With sets
    '        .BaseUrl = 
    '    End With

    '    Return sets
    'End Function

    'Public Shared Property CurrentExternalUser() As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User
    '    Get
    '        
    '    End Get
    '    Set(value As lm.Comol.Core.BaseModules.Tickets.Domain.DTO.DTO_User)
    '        System.Web.SessionState.HttpSessionState.("TICKET.CurrentExtUser") = value
    '    End Set
    'End Property

    Public Enum AnchorType
        editor
        first
        last
        firstNews
    End Enum


    Public Shared Sub CheckDropDownButton(ByVal DVcontainer As System.Web.UI.HtmlControls.HtmlControl, ByVal itemCount As Integer, Optional ByVal hideIfEmpty As Boolean = True)

        If (IsNothing(DVcontainer) OrElse Not DVcontainer.Visible) Then
            Exit Sub
        End If

        DVcontainer.Attributes("class") = "ddbuttonlist"

        If (itemCount > 1) Then
            'più elementi: ddbutton attiva
            DVcontainer.Attributes("class") &= " enabled"
        ElseIf (itemCount = 1) Then
            '1 elemento: ddbutton disattiva
            DVcontainer.Attributes("class") &= " disabled"
        ElseIf (hideIfEmpty) Then
            'SE VUOTO lo nascondo.
            DVcontainer.Visible = False
        End If

    End Sub

End Class
