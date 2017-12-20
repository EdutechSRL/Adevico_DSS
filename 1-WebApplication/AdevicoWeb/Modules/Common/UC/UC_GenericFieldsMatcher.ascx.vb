Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Core.BaseModules.Repository.Presentation

Public Class UC_GenericFieldsMatcher
    Inherits BaseControl
    Implements IViewGenericFieldsMatcher

#Region "Context"
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Private ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Private _Presenter As GenericFieldsMatcherPresenter
    Private ReadOnly Property CurrentPresenter() As GenericFieldsMatcherPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GenericFieldsMatcherPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Public Property isInitialized As Boolean Implements IViewGenericFieldsMatcher.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Public ReadOnly Property SourceFields As List(Of ExternalColumnComparer(Of String, Integer)) Implements IViewGenericFieldsMatcher.SourceFields
        Get
            Dim results As New List(Of ExternalColumnComparer(Of String, Integer))
            Dim MaxNumber As Integer = 0
            For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
                Dim column As New ExternalColumnComparer(Of String, Integer)
                column.SourceColumn = DirectCast(row.FindControl("LBsourceName"), Label).Text
                Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
                Try
                    column.DestinationColumn = DestinationFields.Where(Function(f) f.Id = CInt(oDropDownList.SelectedValue)).FirstOrDefault
                Catch ex As Exception
                    column.DestinationColumn = New DestinationItem(Of Integer) With {.InputType = InputType.skip}
                End Try
                column.InputType = column.DestinationColumn.InputType
                Dim number As String = DirectCast(row.FindControl("LTcolumnNumber"), Literal).Text
                Integer.TryParse(number, column.Number)
                If MaxNumber < column.Number Then
                    MaxNumber = column.Number
                End If
                results.Add(column)
            Next
            Return results
        End Get
    End Property
    'Private Property RequiredFields As List(Of DestinationItem(Of Integer)) Implements IViewGenericFieldsMatcher.RequiredFields
    '    Get
    '        Return ViewStateOrDefault("RequiredFields", New List(Of DestinationItem(Of Integer)))
    '    End Get
    '    Set(value As List(Of DestinationItem(Of Integer)))
    '        ViewState("RequiredFields") = value
    '    End Set
    'End Property
    Private Property DestinationFields As List(Of DestinationItem(Of Integer)) Implements IViewGenericFieldsMatcher.DestinationFields
        Get
            Return ViewStateOrDefault("DestinationFields", New List(Of DestinationItem(Of Integer)))
        End Get
        Set(value As List(Of DestinationItem(Of Integer)))
            ViewState("DestinationFields") = value
        End Set
    End Property
    Public ReadOnly Property isValid As Boolean Implements IViewGenericFieldsMatcher.isValid
        Get
            Dim results As List(Of dtoInvalidMatch(Of DestinationItem(Of Int32))) = Me.CurrentPresenter.GetInvalidItems(SourceFields)

            ChangeAllDestinationColumnClass("")
            If IsNothing(results) OrElse results.Count = 0 Then
                Return True
            Else
                If Me.RPTmatcher.Items.Count > 1 AndAlso (Me.RPTmatcher.Items.Count - 1) = results.Where(Function(r) r.InvalidMatch = InvalidMatch.IgnoredRequiredItem).Count Then
                    Me.LBinfo.Text = Resource.getValue("InvalidMatch." & InvalidMatch.IgnoredAllItems.ToString)
                Else
                    Dim temp As String = ""
                    Dim duplicated As List(Of dtoInvalidMatch(Of DestinationItem(Of Int32))) = results.Where(Function(r) r.InvalidMatch = InvalidMatch.DuplicatedItem).ToList()
                    'Dim ignoredRequiredItem As List(Of dtoInvalidMatch(Of DestinationItem(Of Int32))) = results.Where(Function(r) r.InvalidMatch = InvalidMatch.IgnoredRequiredItem).ToList()


                    temp = TranslateErrors(duplicated, InvalidMatch.DuplicatedItem)
                    If temp <> "" Then
                        temp &= "<br/>"
                    End If
                    temp &= TranslateErrors(results, InvalidMatch.IgnoredAlternativeRequiredItem)
                    If temp <> "" Then
                        temp &= "<br/>"
                    End If
                    temp &= TranslateErrors(results, InvalidMatch.IgnoredRequiredItem)
                    If temp <> "" Then
                        temp &= "<br/>"
                    End If
                    Me.LBinfo.Text = temp
                    ChangeDestinationColumnClass(results, "DDLerror")


                   
                    'If duplicated.Count = 1 Then
                    '    temp = String.Format(Resource.getValue("InvalidMatch." & InvalidMatch.DuplicatedItem.ToString), DestinationFields.Where(Function(t) t.Id = duplicated(0).Attribute.Id).Select(Function(t) t.Name).FirstOrDefault())
                    'ElseIf duplicated.Count > 1 Then
                    '    temp = String.Format(Resource.getValue("InvalidMatch." & InvalidMatch.DuplicatedItems.ToString), String.Join(", ", DestinationFields.Where(Function(t) duplicated.Select(Function(d) d.Attribute.Id).ToList.Contains(t.Id)).Select(Function(t) t.Name).ToArray))
                    'End If

                    'IgnoredAlternativeRequiredItem()

                    'If temp <> "" Then
                    '    temp &= "<br/>"
                    'End If
                    'If ignoredRequiredItem.Count = 1 Then
                    '    temp &= String.Format(Resource.getValue("InvalidMatch." & InvalidMatch.IgnoredRequiredItem.ToString), DestinationFields.Where(Function(t) t.Id = ignoredRequiredItem(0).Attribute.Id).Select(Function(t) t.Name).FirstOrDefault())
                    'ElseIf ignoredRequiredItem.Count > 1 Then
                    '    temp &= String.Format(Resource.getValue("InvalidMatch." & InvalidMatch.IgnoredRequiredItems.ToString), String.Join(", ", DestinationFields.Where(Function(t) ignoredRequiredItem.Select(Function(d) d.Attribute.Id).ToList.Contains(t.Id)).Select(Function(t) t.Name).ToArray))
                    'End If
                    'Me.LBinfo.Text = temp
                    'ChangeDestinationColumnClass(results, "DDLerror")
                End If
                Return False
            End If
        End Get
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region
  
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_CSVgenericImport", "Modules", "Common")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(LBemptyMessage)
        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeControl(columns As List(Of ExternalColumnComparer(Of String, Integer)), fields As List(Of DestinationItem(Of Int32))) Implements IViewGenericFieldsMatcher.InitializeControl
        Me.CurrentPresenter.InitView(columns, fields)
    End Sub
    'Public Sub InitializeControl(columns As List(Of ExternalColumnComparer(Of String, Integer)), aFields As List(Of DestinationItem(Of Int32)), rFields As List(Of DestinationItem(Of Int32))) Implements IViewGenericFieldsMatcher.InitializeControl
    '    Me.CurrentPresenter.InitView(columns, aFields, rFields)
    'End Sub

    Private Sub LoadItems(source As List(Of ExternalColumnComparer(Of String, Integer))) Implements IViewGenericFieldsMatcher.LoadItems
        Me.MLVcontrolData.SetActiveView(VIWmatchColumns)
        Me.RPTmatcher.DataSource = source
        Me.RPTmatcher.DataBind()
    End Sub
    'Private Sub ReloadAvailableAttributes(items As List(Of ProfileAttributeType)) Implements IViewGenericFieldsMatcher.ReloadAvailableAttributes
    '    Attributes = items

    '    Dim translations As List(Of TranslatedItem(Of String))
    '    translations = TranslatedAttributes
    '    For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
    '        Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
    '        Dim selected As String = oDropDownList.SelectedValue
    '        oDropDownList.DataSource = translations
    '        oDropDownList.DataTextField = "Translation"
    '        oDropDownList.DataValueField = "Id"
    '        oDropDownList.DataBind()
    '        Try
    '            oDropDownList.SelectedValue = selected
    '        Catch ex As Exception
    '            oDropDownList.SelectedValue = ProfileAttributeType.skip.ToString
    '        End Try
    '    Next

    'End Sub
    Private Sub DisplaySessionTimeout() Implements IViewGenericFieldsMatcher.DisplaySessionTimeout
        Me.MLVcontrolData.SetActiveView(VIWempty)
    End Sub
    Private Sub ChangeDestinationColumnClass(items As List(Of dtoInvalidMatch(Of DestinationItem(Of Integer))), cssClass As String)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
            Dim item As dtoInvalidMatch(Of DestinationItem(Of Integer)) = items.Where(Function(i) i.Attribute.Id = CInt(oDropDownList.SelectedValue)).FirstOrDefault()
            Dim oLabel As Label = row.FindControl("LBerror")

            If Not IsNothing(item) Then
                If String.IsNullOrEmpty(cssClass) Then
                    oDropDownList.CssClass = "Testo_Campo"
                Else
                    oDropDownList.CssClass = "Testo_Campo" & " " & cssClass
                    oLabel.ToolTip = Resource.getValue("Field.InvalidMatch." & item.InvalidMatch.ToString)
                End If
                oLabel.Visible = Not String.IsNullOrEmpty(cssClass)
            Else
                oDropDownList.CssClass = "Testo_Campo"
                oLabel.Visible = False
            End If
        Next
    End Sub
    Private Sub ChangeAllDestinationColumnClass(cssClass As String)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTmatcher.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oDropDownList As DropDownList = row.FindControl("DDLdestination")
            Dim oLabel As Label = row.FindControl("LBerror")
            If String.IsNullOrEmpty(cssClass) Then
                oDropDownList.CssClass = "Testo_Campo"
            Else
                oDropDownList.CssClass = "Testo_Campo" & " " & cssClass
            End If
            oLabel.Visible = False
        Next
    End Sub
#End Region
    Private Function TranslateErrors(sourceItems As List(Of dtoInvalidMatch(Of DestinationItem(Of Int32))), ByVal match As InvalidMatch) As String
        Dim translation As String = ""
        Dim items As List(Of dtoInvalidMatch(Of DestinationItem(Of Int32))) = sourceItems.Where(Function(r) r.InvalidMatch = match).ToList()

        If items.Count = 1 Then
            translation &= String.Format(Resource.getValue("InvalidMatch." & match.ToString), sourceItems(0).Attribute.Name)
        ElseIf items.Count > 1 Then
            translation &= String.Format(Resource.getValue("InvalidMatch." & match.ToString), String.Join(", ", sourceItems.Select(Function(t) t.Attribute.Name).ToArray))
        End If
        Return translation
    End Function

    Private Sub RPTmatcher_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmatcher.ItemDataBound
        Dim itemType As ListItemType = e.Item.ItemType

        If itemType = ListItemType.Item OrElse itemType = ListItemType.AlternatingItem Then

            Dim item As ExternalColumnComparer(Of String, Integer) = DirectCast(e.Item.DataItem, ExternalColumnComparer(Of String, Integer))

            Dim oDropDownList As DropDownList = e.Item.FindControl("DDLdestination")
            oDropDownList.DataSource = DestinationFields
            oDropDownList.DataTextField = "Name"
            oDropDownList.DataValueField = "Id"
            oDropDownList.DataBind()
            Try
                oDropDownList.SelectedValue = item.DestinationColumn.Id
            Catch ex As Exception

            End Try

        ElseIf itemType = ListItemType.Header Then

            Dim oLabel As Label = e.Item.FindControl("LBsourceColumn")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBdestinationColumn")
            Me.Resource.setLabel(oLabel)
        End If
    End Sub
End Class