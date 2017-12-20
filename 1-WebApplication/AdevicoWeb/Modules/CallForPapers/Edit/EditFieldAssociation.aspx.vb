Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.CallForPapers.Presentation
Imports lm.Comol.Modules.CallForPapers.Domain
Imports lm.ActionDataContract
Imports System.Linq
Imports System.Collections.Generic

Public Class EditFieldAssociation
    Inherits PageBase
    Implements IViewEditFieldAssociations


#Region "Context"
    Private _Presenter As EditFieldAssociationsPresenter
    Private ReadOnly Property CurrentPresenter() As EditFieldAssociationsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditFieldAssociationsPresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private ReadOnly Property Portalname As String Implements IViewBaseEditCall.Portalname
        Get
            Return Resource.getValue("Portalname")
        End Get
    End Property
    Private ReadOnly Property PreloadIdCall As Long Implements IViewBaseEditCall.PreloadIdCall
        Get
            If IsNumeric(Me.Request.QueryString("idCall")) Then
                Return CLng(Me.Request.QueryString("idCall"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadIdCommunity As Integer Implements IViewBaseEditCall.PreloadIdCommunity
        Get
            If IsNumeric(Me.Request.QueryString("idCommunity")) Then
                Return CInt(Me.Request.QueryString("idCommunity"))
            Else
                Return 0
            End If
        End Get
    End Property
    Private ReadOnly Property PreloadType As CallForPaperType Implements IViewBaseEditCall.PreloadType
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallForPaperType).GetByString(Request.QueryString("type"), CallForPaperType.CallForBids)
        End Get
    End Property
    Private ReadOnly Property PreloadView As CallStatusForSubmitters Implements IViewBaseEditCall.PreloadView
        Get
            Return lm.Comol.Core.DomainModel.Helpers.EnumParser(Of CallStatusForSubmitters).GetByString(Request.QueryString("View"), CallStatusForSubmitters.SubmissionOpened)
        End Get
    End Property
    Private Property AllowSave As Boolean Implements IViewBaseEditCall.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState("AllowSave") = value
        End Set
    End Property
    Private Property CallType As CallForPaperType Implements IViewBaseEditCall.CallType
        Get
            Return ViewStateOrDefault("CallType", CallForPaperType.CallForBids)
        End Get
        Set(value As CallForPaperType)
            Me.ViewState("CallType") = value
            Resource.setHyperLink(HYPpreviewCallBottom, value.ToString(), True, True)
            Resource.setHyperLink(HYPpreviewCallTop, value.ToString(), True, True)
            Resource.setHyperLink(HYPbackTop, value.ToString, True, True)
            Resource.setHyperLink(HYPbackBottom, value.ToString, True, True)
        End Set
    End Property
    Private Property IdCall As Long Implements IViewBaseEditCall.IdCall
        Get
            Return ViewStateOrDefault("IdCall", 0)
        End Get
        Set(value As Long)
            Me.ViewState("IdCall") = value
        End Set
    End Property
    Private Property IdCommunity As Integer Implements IViewBaseEditCall.IdCommunity
        Get
            Return ViewStateOrDefault("IdCommunity", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCommunity") = value
        End Set
    End Property
    Private Property IdCallModule As Integer Implements IViewBaseEditCall.IdCallModule
        Get
            Return ViewStateOrDefault("IdCallModule", 0)
        End Get
        Set(value As Integer)
            Me.ViewState("IdCallModule") = value
        End Set
    End Property

    Private Property AvailableAttributes As List(Of TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType)) Implements IViewEditFieldAssociations.AvailableAttributes
        Get
            Return ViewStateOrDefault("AvailableAttributes", New List(Of TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType)))
        End Get
        Set(value As List(Of TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType)))
            ViewState("AvailableAttributes") = value
        End Set
    End Property

#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Internal"
    Private Property TranslatedAttributes As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType))
        Get
            Return ViewStateOrDefault("TranslatedAttributes", New List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)))
        End Get
        Set(value As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)))
            ViewState("TranslatedAttributes") = value
        End Set
    End Property

    Private Property AttributesWithTypes As List(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)
        Get
            Return ViewStateOrDefault("AttributesWithTypes", New List(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType))
        End Get
        Set(value As List(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType))
            ViewState("AttributesWithTypes") = value
        End Set
    End Property

    Public WriteOnly Property AllowUpdateTags As Boolean Implements IViewBaseEditCall.AllowUpdateTags
        Set(value As Boolean)

        End Set
    End Property
#End Region

    Private Sub EditCallAvailability_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Inherits"
    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        Me.CurrentPresenter.InitView()
    End Sub

    Public Overrides Sub BindNoPermessi()
        Master.ShowNoPermission = True
    End Sub
    Public Overrides Function HasPermessi() As Boolean
        Return True
    End Function

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_EditCall", "Modules", "CallForPapers")
    End Sub
    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            LBnocalls.Text = Resource.getValue("LBnoCalls." & CallType.ToString())

            .setLabel(LBcollapseAllTop)
            .setLabel(LBexpandAllTop)
            .setHyperLink(HYPpreviewCallBottom, CallType.ToString(), True, True)
            .setHyperLink(HYPpreviewCallTop, CallType.ToString(), True, True)
            .setHyperLink(HYPbackTop, CallType.ToString, True, True)
            .setHyperLink(HYPbackBottom, CallType.ToString, True, True)
            .setButton(BTNsaveFieldAssociationsBottom, True, , , True)
            .setButton(BTNsaveFieldAssociationsTop, True, , , True)
        End With
    End Sub

    Public Overrides Sub RegistraAccessoPagina()

    End Sub
    Public Overrides Sub ShowMessageToPage(errorMessage As String)

    End Sub
#End Region

#Region "Implements"
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleRequestForMembership.ActionType) Implements IViewEditFieldAssociations.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleRequestForMembership.ObjectType.RequestForMembership, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub SendUserAction(idCommunity As Integer, idModule As Integer, idCall As Long, action As ModuleCallForPaper.ActionType) Implements IViewEditFieldAssociations.SendUserAction
        PageUtility.AddActionToModule(idCommunity, idModule, action, PageUtility.CreateObjectsList(idModule, ModuleCallForPaper.ObjectType.CallForPaper, idCall.ToString), InteractionType.UserWithLearningObject)
    End Sub
    Private Sub DisplayNoPermission(idCommunity As Integer, idModule As Integer) Implements IViewBase.DisplayNoPermission
        Me.PageUtility.AddActionToModule(idCommunity, idModule, IIf(CallType = CallForPaperType.CallForBids, ModuleCallForPaper.ActionType.NoPermission, ModuleRequestForMembership.ActionType.NoPermission), , InteractionType.UserWithLearningObject)
        Me.BindNoPermessi()
    End Sub

    Private Sub DisplaySessionTimeout() Implements IViewBase.DisplaySessionTimeout
        Dim idCommunity As Integer = PreloadIdCommunity
        Dim webPost As New lm.Comol.Core.DomainModel.Helpers.LogoutWebPost(PageUtility.GetDefaultLogoutPage)
        Dim dto As New lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl()
        dto.Display = lm.Comol.Core.DomainModel.Helpers.dtoExpiredAccessUrl.DisplayMode.SameWindow

        dto.DestinationUrl = RootObject.EditCallTemplateMail(CallType, PreloadIdCall, idCommunity, PreloadView)

        If idCommunity > 0 Then
            dto.IdCommunity = idCommunity
        End If
        webPost.Redirect(dto)
    End Sub

    Private Sub SetActionUrl(action As CallStandardAction, url As String) Implements IViewBaseEditCall.SetActionUrl
        If action = CallStandardAction.PreviewCall Then
            Me.HYPpreviewCallBottom.Visible = True
            Me.HYPpreviewCallBottom.NavigateUrl = BaseUrl & url
            Me.HYPpreviewCallTop.Visible = True
            Me.HYPpreviewCallTop.NavigateUrl = BaseUrl & url
        Else 'Select Case action
            '    Case CallStandardAction.List
            Me.HYPbackBottom.Visible = True
            Me.HYPbackBottom.NavigateUrl = BaseUrl & url
            Me.HYPbackTop.Visible = True
            Me.HYPbackTop.NavigateUrl = BaseUrl & url
            '    Case CallStandardAction.Manage
            '        Me.HYPmanage.Visible = True
            '        Me.HYPmanage.NavigateUrl = BaseUrl & url
            '    Case CallStandardAction.Add
            '        Me.HYPcreateCall.Visible = True
            '        Me.HYPcreateCall.NavigateUrl = BaseUrl & url
            'End Select
        End If
    End Sub

    Private Sub SetContainerName(action As CallStandardAction, name As String, itemName As String) Implements IViewBaseEditCall.SetContainerName
        Dim title As String = Me.Resource.getValue("serviceTitle." & action.ToString & "." & CallType.ToString())
        Master.ServiceTitle = String.Format(title, IIf(Len(itemName) > 70, Left(itemName, 70) & "...", itemName))
        If String.IsNullOrEmpty(name) Then
            Master.ServiceTitleToolTip = String.Format(title, itemName)
        Else
            Dim tooltip As String = Me.Resource.getValue("serviceTitle.Community." & action.ToString() & "." & CallType.ToString())
            If Not String.IsNullOrEmpty(tooltip) Then
                Master.ServiceTitleToolTip = String.Format(tooltip, name)
            End If
        End If
    End Sub

    Private Sub LoadWizardSteps(idCall As Long, type As lm.Comol.Modules.CallForPapers.Domain.CallForPaperType, idCommunity As Integer, steps As List(Of lm.Comol.Core.Wizard.NavigableWizardItem(Of WizardCallStep))) Implements IViewBaseEditCall.LoadWizardSteps
        Me.CTRLsteps.InitalizeControl(idCall, type, idCommunity, PreloadView, steps)
    End Sub

    Private Sub LoadUnknowCall(idCommunity As Integer, idModule As Integer, idCall As Long, type As CallForPaperType) Implements IViewBaseEditCall.LoadUnknowCall
        Me.MLVsettings.SetActiveView(VIWempty)
        Me.LBnocalls.Text = Resource.getValue("LBnoCalls." & type.ToString)
    End Sub
    Public Sub HideErrorMessages() Implements IViewEditFieldAssociations.HideErrorMessages
        CTRLmessages.Visible = False
    End Sub
    Private Sub DisplayNoAssociations() Implements IViewEditFieldAssociations.DisplayNoAssociations
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayNoAssociations"), Helpers.MessageType.alert)
    End Sub
    Public Sub DisplaySettingsSaved() Implements IViewEditFieldAssociations.DisplaySettingsSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayFieldAssociationsSettingsSaved"), Helpers.MessageType.success)
    End Sub

    Private Sub DisplaySettingsUnSaved() Implements IViewEditFieldAssociations.DisplaySettingsUnSaved
        CTRLmessages.Visible = True
        CTRLmessages.InitializeControl(Resource.getValue("DisplayFieldAssociationsSettingsUnSaved"), Helpers.MessageType.error)
    End Sub
    Private Function GetAssociations() As List(Of dtoFieldAssociation) Implements IViewEditFieldAssociations.GetAssociations
        Dim associations As New List(Of dtoFieldAssociation)
        For Each row As RepeaterItem In (From r As RepeaterItem In Me.RPTsections.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oRepeater As Repeater = row.FindControl("RPTfields")
            associations.AddRange(GetAssociations(oRepeater))
        Next
        Return associations
    End Function
    Private Function GetAssociations(oRepeater As Repeater) As List(Of dtoFieldAssociation)
        Dim associations As New List(Of dtoFieldAssociation)
        For Each row As RepeaterItem In (From r As RepeaterItem In oRepeater.Items Where r.ItemType = ListItemType.AlternatingItem OrElse r.ItemType = ListItemType.Item Select r).ToList()
            Dim oDropDownList As DropDownList = row.FindControl("DDLprofileFields")
            If oDropDownList.Enabled Then
                Dim association As New dtoFieldAssociation
                association.Id = CLng(DirectCast(row.FindControl("LTidAssociation"), Literal).Text)
                association.IdField = CLng(DirectCast(row.FindControl("LTidField"), Literal).Text)
                association.Attribute = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.ProfileAttributeType).GetByString(oDropDownList.SelectedValue, lm.Comol.Core.Authentication.ProfileAttributeType.skip)
                associations.Add(association)
            End If
        Next
        Return associations
    End Function
    Private Sub LoadAssociations(associations As List(Of dtoCallSection(Of dtoFieldAssociation))) Implements IViewEditFieldAssociations.LoadAssociations
        LoadAttributesWithTypes()

        Me.MLVsettings.SetActiveView(VIWsettings)
        Me.RPTsections.DataSource = associations
        Me.RPTsections.DataBind()

        If Me.RPTsections.Items.Count > 0 Then
            For Each sectionRow As RepeaterItem In RPTsections.Items
                Dim oRepeater As Repeater = sectionRow.FindControl("RPTfields")

                If Not IsNothing(oRepeater) AndAlso oRepeater.Items.Count > 0 Then
                    Dim TRfieldRow As HtmlTableRow = oRepeater.Items(oRepeater.Items.Count - 1).FindControl("TRfieldRow")
                    TRfieldRow.Attributes.Add("class", "last")
                End If
            Next
        End If
    End Sub

    'Private Sub LoadAttributes()
    '    'Dim items As New List(Of TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType))

    '    'Dim skipItem As New TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType) With {.Id = lm.Comol.Core.Authentication.ProfileAttributeType.skip, .Translation = Me.Resource.getValue("ProfileAttributeType.skip")}


    '    'Dim attributes As New List(Of lm.Comol.Core.Authentication.ProfileAttributeType)
    '    'attributes.AddRange([Enum].GetValues(GetType(lm.Comol.Core.Authentication.ProfileAttributeType)))

    '    'For Each att As lm.Comol.Core.Authentication.ProfileAttributeType In attributes.Where(Function(a) a <> lm.Comol.Core.Authentication.ProfileAttributeType.skip AndAlso a <> lm.Comol.Core.Authentication.ProfileAttributeType.unknown)
    '    '    items.Add(New TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType) With {.Id = att, .Translation = Me.Resource.getValue("ProfileAttributeType." & att.ToString)})
    '    'Next

    '    'AvailableAttributes = New List(Of TranslatedItem(Of lm.Comol.Core.Authentication.ProfileAttributeType))
    '    'AvailableAttributes.Add(skipItem)
    '    'AvailableAttributes.AddRange(items.OrderByDescending(Function(a) a.Translation))
    '    LoadAttributesWithTypes()
    'End Sub

    Private Sub LoadAttributesWithTypes()
        Dim oService As New lm.Comol.Core.BaseModules.ProfileManagement.Business.ProfileManagementService(PageUtility.CurrentContext)
        Dim items As List(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) = oService.GetAllProfileAttributes(PageUtility.SystemSettings.Presenter.DefaultTaxCodeRequired)
        Dim result As New List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType))
        Dim skipItem As New TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) With {.Id = New lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType(lm.Comol.Core.Authentication.ProfileAttributeType.skip, False), .Translation = Me.Resource.getValue("ProfileAttributeType.skip")}

        If IsNothing(items) OrElse items.Count = 0 Then
            result.Add(skipItem)
        Else
            result = (From s In items Select New TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) With {.Id = s, .Translation = IIf(s.Mandatory AndAlso s.UserType <> UserTypeStandard.AllUserType, "(*)", "") & Me.Resource.getValue("ProfileAttributeType." & s.Attribute.ToString)}).OrderBy(Function(t) t.Translation).ToList()
            If Not items.Select(Function(i) i.Attribute).ToList().Contains(lm.Comol.Core.Authentication.ProfileAttributeType.skip) Then
                result.Insert(0, skipItem)
            End If
        End If
        TranslatedAttributes = result
    End Sub
#End Region

    Private Sub RPTsections_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTsections.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim section As dtoCallSection(Of dtoFieldAssociation) = DirectCast(e.Item.DataItem, dtoCallSection(Of dtoFieldAssociation))
            Dim oLiteral As Literal = e.Item.FindControl("LTidSection")
            oLiteral.Text = section.Id

            Dim oLabel As Label = e.Item.FindControl("LBsectionName")
            oLabel.Text = section.Name
        End If
    End Sub
    Protected Sub RPTfields_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim item As dtoFieldAssociation = DirectCast(e.Item.DataItem, dtoFieldAssociation)

            Dim oLiteral As Literal = e.Item.FindControl("LTidField")
            oLiteral.Text = item.IdField

            oLiteral = e.Item.FindControl("LTidAssociation")
            oLiteral.Text = item.Id

            oLiteral = e.Item.FindControl("LTfieldName")
            oLiteral.Text = item.Name


            ' Dim oDropDownList As DropDownList = e.Item.FindControl("DDLprofileFields")
            Dim oDropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown = e.Item.FindControl("DDLprofileFields")
            LoadAttributesIntoDropDown(oDropDownList, item.Attribute)

            oDropDownList.Enabled = AllowSave AndAlso item.Type <> FieldType.CheckboxList AndAlso item.Type <> FieldType.Disclaimer AndAlso item.Type <> FieldType.DropDownList AndAlso item.Type <> FieldType.RadioButtonList AndAlso item.Type <> FieldType.Note AndAlso item.Type <> FieldType.FileInput

        Else
            Dim oLiteral As Literal = e.Item.FindControl("LTfieldName_t")
            Resource.setLiteral(oLiteral)

            oLiteral = e.Item.FindControl("LTmatchProfileItem_t")
            Resource.setLiteral(oLiteral)
        End If
    End Sub

    Private Sub LoadAttributesIntoDropDown(ByVal dropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown, attribute As lm.Comol.Core.Authentication.ProfileAttributeType)
        Dim items As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)) = TranslatedAttributes
        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList


        dropDownList.Items.Clear()

        Dim skipItem As TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) = items.Where(Function(a) a.Id.Attribute = lm.Comol.Core.Authentication.ProfileAttributeType.skip).FirstOrDefault()
        If Not IsNothing(skipItem) Then
            dropDownList.Items.Add(New ListItem() With {.Value = skipItem.Id.Attribute.ToString, .Text = skipItem.Translation})
        End If

        If (items.Where(Function(a) a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).Any()) Then
            dropDownList.AddItemGroup(Resource.getValue("GroupBy.MandatoryCommon"))
            AddItems(dropDownList, items.Where(Function(a) a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).OrderBy(Function(i) i.Translation).ToList())
        End If
        If (items.Where(Function(a) a.Id.UserType <> UserTypeStandard.AllUserType AndAlso oUserTypes.Where(Function(u) u.ID = CInt(a.Id.UserType)).Any()).Any()) Then
            Dim userType
            For Each userType In items.Where(Function(a) a.Id.UserType <> UserTypeStandard.AllUserType).Select(Function(a) a.Id.UserType).Distinct().ToList()
                dropDownList.AddItemGroup(String.Format(Resource.getValue("GroupBy.UserTypeStandard"), oUserTypes.Where(Function(u) u.ID = CInt(userType)).Select(Function(u) u.Descrizione).FirstOrDefault()))

                AddItems(dropDownList, items.Where(Function(a) a.Id.UserType = userType).OrderBy(Function(i) i.Translation).ToList())
            Next
        End If
        If (items.Where(Function(a) Not a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).Any()) Then
            dropDownList.AddItemGroup(Resource.getValue("GroupBy.NotMandatoryCommon"))
            AddItems(dropDownList, items.Where(Function(a) Not a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).OrderBy(Function(i) i.Translation).ToList())
        End If
        Try
            dropDownList.SelectedValue = attribute.ToString
        Catch ex As Exception
            dropDownList.SelectedValue = lm.Comol.Core.Authentication.ProfileAttributeType.skip.ToString
        End Try
    End Sub
    Private Sub AddItems(ByVal dropDownList As lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown, items As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)))
        For Each item As TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) In items
            dropDownList.Items.Add(New ListItem() With {.Value = item.Id.Attribute.ToString, .Text = item.Translation})
        Next
    End Sub

    Private Sub BTNsaveFieldAssociationsBottom_Click(sender As Object, e As System.EventArgs) Handles BTNsaveFieldAssociationsBottom.Click, BTNsaveFieldAssociationsTop.Click
        Me.CurrentPresenter.SaveSettings(GetAssociations())
    End Sub

  
End Class