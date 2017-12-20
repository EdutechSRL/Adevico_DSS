Imports lm.Comol.UI.Presentation
Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Core.BaseModules.ProviderManagement
Imports lm.Comol.Core.BaseModules.ProviderManagement.Presentation
Imports lm.Comol.Core.Authentication

Public Class UC_EditMacUrlAttribute
    Inherits BaseControl
    Implements IViewEditMacAttribute

#Region "Context"
    Private _Presenter As EditMacAttributePresenter
    Private ReadOnly Property CurrentPresenter() As EditMacAttributePresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New EditMacAttributePresenter(Me.PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
#End Region

#Region "Implements"
    Private Property IdProvider As Long Implements IViewEditMacAttribute.IdProvider
        Get
            Return ViewStateOrDefault("IdProvider", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdProvider") = value
        End Set
    End Property
    Protected Property IdAttribute As Long Implements IViewEditMacAttribute.IdAttribute
        Get
            Return ViewStateOrDefault("IdAttribute", CLng(0))
        End Get
        Set(value As Long)
            ViewState("IdAttribute") = value
        End Set
    End Property
    Private Property AttributeType As UrlMacAttributeType Implements IViewEditMacAttribute.AttributeType
        Get
            Return ViewStateOrDefault("AttributeType", UrlMacAttributeType.url)
        End Get
        Set(value As UrlMacAttributeType)
            ViewState("AttributeType") = value
        End Set
    End Property
    Public ReadOnly Property GetAttribute As BaseUrlMacAttribute Implements IViewEditMacAttribute.GetAttribute
        Get
            Dim dto As BaseUrlMacAttribute
            Select Case AttributeType
                Case UrlMacAttributeType.applicationId
                    Dim aDto As New ApplicationAttribute()
                    aDto.Value = Me.TXBrequiredValue.Text
                    dto = aDto

                Case UrlMacAttributeType.coursecatalogue
                    Dim cDto As New CatalogueAttribute()
                    cDto.AllowMultipleValue = CBXmultipleValue.Checked
                    If cDto.AllowMultipleValue Then
                        cDto.MultipleValueSeparator = TXBinputchar.Text
                    Else
                        cDto.MultipleValueSeparator = ""
                    End If
                    For Each row As RepeaterItem In RPTcatalogueItems.Items
                        Dim oLiteral As Literal = row.FindControl("LTidCatalogueAttributeItem")

                        Dim item As New CatalogueAttributeItem
                        item.Id = oLiteral.Text

                        oLiteral = row.FindControl("LTidCatalogue")
                        If Not String.IsNullOrEmpty(oLiteral.Text) AndAlso IsNumeric(oLiteral.Text) Then
                            item.Catalogue = New lm.Comol.Core.Catalogues.Catalogue With {.Id = CLng(oLiteral.Text)}
                        End If

                        Dim oTextBox As TextBox = row.FindControl("TXBctgCode")
                        item.RemoteCode = oTextBox.Text

                        cDto.Items.Add(item)
                    Next
                    dto = cDto
                Case UrlMacAttributeType.functionId
                    Dim fDto As New FunctionAttribute()
                    fDto.Value = Me.TXBrequiredValue.Text
                    dto = fDto

                Case UrlMacAttributeType.mac
                    dto = New MacAttribute()
                Case UrlMacAttributeType.organization
                    Dim oDto As New OrganizationAttribute()
                    oDto.AllowMultipleValue = CBXmultipleValue.Checked
                    If oDto.AllowMultipleValue Then
                        oDto.MultipleValueSeparator = TXBinputchar.Text
                    Else
                        oDto.MultipleValueSeparator = ""
                    End If
                    For Each row As RepeaterItem In RPTorganizationItems.Items
                        Dim oLiteral As Literal = row.FindControl("LTidOrganizationAttributeItem")

                        Dim item As New OrganizationAttributeItem
                        item.Id = oLiteral.Text

                        Dim oDropDownList As DropDownList = row.FindControl("DDLorgProfile")
                        If oDropDownList.Items.Count > 0 Then
                            item.IdDefaultProfile = CLng(oDropDownList.SelectedValue)
                        End If

                        oDropDownList = row.FindControl("DDLorgPage")
                        If oDropDownList.Items.Count > 0 Then
                            item.IdDefaultPage = CLng(oDropDownList.SelectedValue)
                        End If

                        Dim oTextBox As TextBox = row.FindControl("TXBorgCode")
                        item.RemoteCode = oTextBox.Text

                        oDto.Items.Add(item)
                    Next
                    dto = oDto
                Case (UrlMacAttributeType.profile)
                    Dim pDto As New UserProfileAttribute()
                    pDto.Attribute = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileAttributeType).GetByString(Me.DDLprofileFields.SelectedValue, ProfileAttributeType.skip)
                    dto = pDto
                Case UrlMacAttributeType.compositeProfile
                    Dim cmpDto As New CompositeProfileAttribute()
                    cmpDto.Attribute = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of ProfileAttributeType).GetByString(Me.DDLcompositeProfileFields.SelectedValue, ProfileAttributeType.skip)
                    cmpDto.MultipleValueSeparator = TXBinputchar.Text
                    dto = cmpDto
                Case UrlMacAttributeType.timestamp
                    Dim tDto As New TimestampAttribute()
                    tDto.Format = lm.Comol.Core.DomainModel.Helpers.EnumParser(Of lm.Comol.Core.Authentication.TimestampFormat).GetByString(Me.SLformat.Value, lm.Comol.Core.Authentication.TimestampFormat.utc)
                    tDto.UserFormat = ""

                    dto = tDto
                Case Else
                    dto = New BaseUrlMacAttribute
            End Select
            With dto
                .Id = IdAttribute
                .Description = Me.TXBdescription.Text
                .Name = Me.TXBname.Text
                .QueryStringName = Me.TXBqueryStringName.Text
                .Type = AttributeType
            End With
            Return dto
        End Get
    End Property
    Public Property isInitialized As Boolean Implements IViewEditMacAttribute.isInitialized
        Get
            Return ViewStateOrDefault("isInitialized", False)
        End Get
        Set(value As Boolean)
            ViewState("isInitialized") = value
        End Set
    End Property
    Private Property TranslatedAttributes As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)) Implements IViewEditMacAttribute.TranslatedAttributes
        Get
            Return ViewStateOrDefault("TranslatedAttributes", New List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)))
        End Get
        Set(value As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)))
            ViewState("TranslatedAttributes") = value
        End Set
    End Property
    Private Property AllowSave As Boolean Implements IViewEditMacAttribute.AllowSave
        Get
            Return ViewStateOrDefault("AllowSave", False)
        End Get
        Set(value As Boolean)
            ViewState("AllowSave") = value
        End Set
    End Property
    Private Property TranslatedProfileTypes As List(Of TranslatedItem(Of Integer)) Implements IViewEditMacAttribute.TranslatedProfileTypes
        Get
            Return ViewStateOrDefault("TranslatedProfileTypes", New List(Of TranslatedItem(Of Integer)))
        End Get
        Set(value As List(Of TranslatedItem(Of Integer)))
            ViewState("TranslatedProfileTypes") = value
        End Set
    End Property
    Private Property AvailablePages As Dictionary(Of Long, String) Implements IViewEditMacAttribute.AvailablePages
        Get
            Return ViewStateOrDefault("AvailablePages", New Dictionary(Of Long, String))
        End Get
        Set(value As Dictionary(Of Long, String))
            ViewState("AvailablePages") = value
        End Set
    End Property
    Private Property DisplayMode As Boolean Implements IViewEditMacAttribute.DisplayMode
        Get
            Return ViewStateOrDefault("DisplayMode", False)
        End Get
        Set(value As Boolean)
            ViewState("DisplayMode") = value
        End Set
    End Property
#End Region

#Region "Inherits"
    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

#Region "Control"
    Public Event AddCatalogueAttributeItem(idOwner As Long, idCatalogue As Long, remoteCode As String)
    Public Event AddOrganizationAttributeItem(idOwner As Long, idOrganization As Int32, idDefaultPage As Long, idDefaultProfile As Int32, remoteCode As String)
    Public Event AddMacAttributeItem(idOwner As Long, idAttributeItem As Long)
    Public Event AddCompositeAttributeItem(idOwner As Long, idAttributeItem As Long)
    Public Event RemoveAttributeItem(idOwner As Long, idAttributeItem As Long, type As UrlMacAttributeType)
    Public Event VirtualDeleteAttribute(idAttribute As Long)
    Public Event EditAttribute(idAttribute As Long)
    Public Event UndoEditing(idAttribute As Long)
    Public Event SavedAttribute(idAttribute As Long)
    Public Event UnsavedAttribute(idAttribute As Long)
    Public Event InvalidCode(idAttribute As Long)
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            Me.SetInternazionalizzazione()
        End If
    End Sub

#Region "Inherits"
    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_AuthenticationProvider", "Modules", "ProviderManagement")
    End Sub
    Protected Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setButton(BTNdeleteAttribute, True)
            .setButton(BTNeditAttribute, True)
            .setLabel(LBmacUrlAttributeName_t)
            .setRequiredFieldValidator(RFVname, True, False)

            .setLabel(LBmacUrlAttributeMultipleValue_t)
            .setLiteral(LTmacUrlAttributeMultipleValue)
            .setLabel(LBmacUrlAttributeDescription_t)
            .setLabel(LBmacUrlAttributeQueryStringName_t)
            .setRequiredFieldValidator(RFVqueryStringName, True, False)

            .setLabel(LBmacUrlAttributeRequiredValue_t)
            .setRequiredFieldValidator(RFVrequiredValue, True, False)
            .setLabel(LBtimeStampAttributeFormatType_t)
            .setLabel(LBprofileAttribute_t)
            .setLiteral(LTmacAttributeItemsTitle)
            .setLiteral(LTorganizationAttributeItemsTitle)

            .setButton(BTNcancelAttributeEditing, True)
            .setButton(BTNsaveAttributeSettings, True)
            .setLiteral(LTcatalogueAttributeItemsTitle)

            .setLabel(LBcompositeProfileAttribute_t)
            .setLiteral(LTcompositeAttributeItemsTitle)

        End With
    End Sub
#End Region

#Region "Implements"
    Public Sub InitializeDisplayControl(idProvider As Long, attribute As lm.Comol.Core.Authentication.BaseUrlMacAttribute, editAvailable As Boolean) Implements IViewEditMacAttribute.InitializeDisplayControl
        Me.BTNdeleteAttribute.Visible = editAvailable
        Me.BTNeditAttribute.Visible = editAvailable
        CurrentPresenter.InitView(idProvider, attribute)
    End Sub
    Public Sub InitializeEditControl(idProvider As Long, attribute As lm.Comol.Core.Authentication.BaseUrlMacAttribute, saveEnabled As Boolean) Implements IViewEditMacAttribute.InitializeEditControl
        AllowSave = saveEnabled
        Me.BTNeditAttribute.Visible = False
        Me.BTNsaveAttributeSettings.Visible = AllowSave
        CurrentPresenter.InitView(idProvider, attribute, saveEnabled, PageUtility.SystemSettings.Presenter.DefaultTaxCodeRequired)
    End Sub
    Public Function ValidateContent() As Boolean Implements IViewEditMacAttribute.ValidateContent
        Dim result As Boolean = True
        Select Case AttributeType
            Case UrlMacAttributeType.organization
                '  result= CurrentPresenter.isValidRemoteCode(
        End Select
        Return result
    End Function

    Private Sub DisplayAttributeUnknown() Implements IViewEditMacAttribute.DisplayAttributeUnknown
        MLVattribute.SetActiveView(VIWunknown)
    End Sub
    Private Sub DisplaySessionTimeout() Implements IViewEditMacAttribute.DisplaySessionTimeout
        MLVattribute.SetActiveView(VIWempty)
    End Sub
    Private Sub LoadAttribute(attribute As lm.Comol.Core.Authentication.BaseUrlMacAttribute, allowSave As Boolean) Implements lm.Comol.Core.BaseModules.ProviderManagement.Presentation.IViewEditMacAttribute.LoadAttribute
        DVeditArea.Visible = Not DisplayMode
        If IsNothing(attribute) Then
            MLVattribute.SetActiveView(VIWempty)
        Else
            MLVattribute.SetActiveView(VIWattribute)
            MLVadvanced.SetActiveView(VIWnoneAttribute)
            DVqueryStringName.Visible = True
            CBXmultipleValue.Disabled = False
            If Not DisplayMode Then
                Select Case attribute.Type
                    Case UrlMacAttributeType.applicationId
                        Dim aAtt As ApplicationAttribute = DirectCast(attribute, ApplicationAttribute)
                        DVrequiredValue.Visible = True
                        Me.TXBrequiredValue.Text = aAtt.Value

                    Case UrlMacAttributeType.coursecatalogue
                        Dim cAtt As CatalogueAttribute = DirectCast(attribute, CatalogueAttribute)
                        DVmultipleValue.Visible = True
                        CBXmultipleValue.Checked = cAtt.AllowMultipleValue
                        If cAtt.AllowMultipleValue Then
                            Me.TXBinputchar.Text = cAtt.MultipleValueSeparator
                        End If
                        Initialize(cAtt)
                    Case UrlMacAttributeType.functionId
                        Dim fAtt As FunctionAttribute = DirectCast(attribute, FunctionAttribute)

                        DVrequiredValue.Visible = True
                        Me.TXBrequiredValue.Text = fAtt.Value

                    Case UrlMacAttributeType.mac
                        Me.Initialize(DirectCast(attribute, MacAttribute))
                    Case UrlMacAttributeType.compositeProfile
                        Dim cmpAtt As CompositeProfileAttribute = DirectCast(attribute, CompositeProfileAttribute)
                        DVmultipleValue.Visible = True
                        CBXmultipleValue.Checked = True
                        CBXmultipleValue.Disabled = True
                        Me.TXBinputchar.Text = cmpAtt.MultipleValueSeparator
                        DVqueryStringName.Visible = False

                        Me.Initialize(cmpAtt)
                    Case UrlMacAttributeType.organization
                        Dim oAtt As OrganizationAttribute = DirectCast(attribute, OrganizationAttribute)
                        DVmultipleValue.Visible = True

                        CBXmultipleValue.Checked = oAtt.AllowMultipleValue
                        If oAtt.AllowMultipleValue Then
                            Me.TXBinputchar.Text = oAtt.MultipleValueSeparator
                        End If
                        Me.Initialize(oAtt)
                    Case UrlMacAttributeType.profile
                        Dim pAtt As UserProfileAttribute = DirectCast(attribute, UserProfileAttribute)
                        LoadProfileDropDownList(pAtt.Attribute, DDLprofileFields)
                        Me.MLVadvanced.SetActiveView(VIWprofile)

                    Case UrlMacAttributeType.timestamp
                        Dim tAtt As TimestampAttribute = DirectCast(attribute, TimestampAttribute)
                        'Me.TXBinputchar.Text = tAtt.UserFormat
                        Me.SLformat.Value = tAtt.Format.ToString()
                        Me.MLVadvanced.SetActiveView(VIWtimestamp)
                    Case Else

                End Select
                With attribute
                    IdAttribute = .Id
                    Me.TXBdescription.Text = .Description
                    Me.TXBname.Text = .Name
                    Me.TXBqueryStringName.Text = .QueryStringName
                    AttributeType = .Type
                    LTmacUrlAttributeName.Text = .Name
                    LBmacUrlAttributeType.Text = Me.Resource.getValue("UrlMacAttributeType." & .Type.ToString)

                    Me.TXBdescription.ReadOnly = Not allowSave
                    Me.TXBname.ReadOnly = Not allowSave
                    Me.TXBqueryStringName.ReadOnly = Not allowSave
                    LBmacUrlAttributeQueryName.Text = "?" & attribute.QueryStringName & "=[" & Me.Resource.getValue("QuerySampleValue") & "]"
                End With
                BTNdeleteAttribute.Visible = allowSave

            Else
                IdAttribute = attribute.Id
                AttributeType = attribute.Type
                LTmacUrlAttributeName.Text = attribute.Name
                LBmacUrlAttributeType.Text = Me.Resource.getValue("UrlMacAttributeType." & attribute.Type.ToString)
                LBmacUrlAttributeQueryName.Text = "?" & attribute.QueryStringName & "=[" & Me.Resource.getValue("QuerySampleValue") & "]"
            End If
            LBmacUrlAttributeQueryName.Visible = Not (attribute.Type = UrlMacAttributeType.compositeProfile)
        End If
    End Sub
    Private Sub LoadAvailableProfileAttributes(items As List(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)) Implements IViewEditMacAttribute.LoadAvailableProfileAttributes
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
    Private Sub LoadSystemProfileTypes() Implements IViewEditMacAttribute.LoadSystemProfileTypes
        TranslatedProfileTypes = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) _
                                                      Where o.ID <> Main.TipoPersonaStandard.Guest Select New TranslatedItem(Of Integer) With {.Id = o.ID, .Translation = o.Descrizione}).ToList
    End Sub
    Private Function SaveAttribute(attribute As BaseUrlMacAttribute, ByRef validCodes As Boolean) As Boolean Implements IViewEditMacAttribute.SaveAttribute
        Return Not IsNothing(Me.CurrentPresenter.SaveSettings(IdProvider, IdAttribute, attribute, validCodes))
    End Function
#End Region

#Region "ProfileAttribute"
    Private Sub LoadProfileDropDownList(attribute As ProfileAttributeType, oDropDownList As Global.lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown)
        Dim items As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)) = TranslatedAttributes
        Dim oUserTypes As List(Of COL_TipoPersona) = (From o In COL_TipoPersona.ListForCreate(Me.PageUtility.LinguaID) Where o.ID <> Main.TipoPersonaStandard.Guest Select o).ToList


        oDropDownList.Items.Clear()

        Dim skipItem As TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) = items.Where(Function(a) a.Id.Attribute = lm.Comol.Core.Authentication.ProfileAttributeType.skip).FirstOrDefault()
        If Not IsNothing(skipItem) Then
            oDropDownList.Items.Add(New ListItem() With {.Value = skipItem.Id.Attribute.ToString, .Text = skipItem.Translation})
        End If

        If (items.Where(Function(a) a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).Any()) Then
            oDropDownList.AddItemGroup(Resource.getValue("GroupBy.MandatoryCommon"))
            AddItems(items.Where(Function(a) a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).OrderBy(Function(i) i.Translation).ToList(), oDropDownList)
        End If
        If (items.Where(Function(a) a.Id.UserType <> UserTypeStandard.AllUserType AndAlso oUserTypes.Where(Function(u) u.ID = CInt(a.Id.UserType)).Any()).Any()) Then
            Dim userType
            For Each userType In items.Where(Function(a) a.Id.UserType <> UserTypeStandard.AllUserType).Select(Function(a) a.Id.UserType).Distinct().ToList()
                oDropDownList.AddItemGroup(String.Format(Resource.getValue("GroupBy.UserTypeStandard"), oUserTypes.Where(Function(u) u.ID = CInt(userType)).Select(Function(u) u.Descrizione).FirstOrDefault()))

                AddItems(items.Where(Function(a) a.Id.UserType = userType).OrderBy(Function(i) i.Translation).ToList(), oDropDownList)
            Next
        End If
        If (items.Where(Function(a) Not a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).Any()) Then
            oDropDownList.AddItemGroup(Resource.getValue("GroupBy.NotMandatoryCommon"))
            AddItems(items.Where(Function(a) Not a.Id.Mandatory AndAlso a.Id.UserType = UserTypeStandard.AllUserType AndAlso a.Id.Attribute <> lm.Comol.Core.Authentication.ProfileAttributeType.skip).OrderBy(Function(i) i.Translation).ToList(), oDropDownList)
        End If
        Try
            oDropDownList.SelectedValue = attribute.ToString
        Catch ex As Exception
            oDropDownList.SelectedValue = lm.Comol.Core.Authentication.ProfileAttributeType.skip.ToString
        End Try
    End Sub
    Private Sub AddItems(items As List(Of TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType)), oDropDownList As Global.lm.Comol.Core.BaseModules.Web.Controls.ExtendedDropDown)
        For Each item As TranslatedItem(Of lm.Comol.Core.BaseModules.ProfileManagement.dtoProfileAttributeType) In items
            oDropDownList.Items.Add(New ListItem() With {.Value = item.Id.Attribute.ToString, .Text = item.Translation})
        Next
    End Sub
#End Region

#Region "MacAttribute"
    Private Sub Initialize(attribute As MacAttribute)
        Me.MLVadvanced.SetActiveView(VIWmac)
        Me.RPTmacAttributeItems.DataSource = attribute.Items.Where(Function(i) i.Deleted = BaseStatusDeleted.None).OrderBy(Function(i) i.DisplayOrder).ToList()
        Me.RPTmacAttributeItems.DataBind()
    End Sub
    Private Sub RPTmacAttributeItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTmacAttributeItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim att As MacAttributeItem = DirectCast(e.Item.DataItem, MacAttributeItem)
            Dim oLabel As Label = e.Item.FindControl("LBmoveMacAttributeItem")
            oLabel.ToolTip = Resource.getValue("LBmoveMacAttributeItem.ToolTip")
            oLabel.Visible = AllowSave

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = att.DisplayOrder

            oLabel = e.Item.FindControl("LBmacAttributeItem")
            oLabel.Text = att.Attribute.Name

            Dim oButton As Button = e.Item.FindControl("BTNdeleteMacAttributeItem")
            Resource.setButton(oButton, True)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBmacAttributeItemsTitle_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oDiv As HtmlControl = e.Item.FindControl("DVfooter")
            If AllowSave Then
                Dim oDropDown As DropDownList = e.Item.FindControl("DDLattributes")
                oDropDown.DataSource = Me.CurrentPresenter.GetAvailableAttributesForMac(IdProvider, IdAttribute)
                oDropDown.DataTextField = "Name"
                oDropDown.DataValueField = "Id"
                oDropDown.DataBind()

                If (oDropDown.Items.Count > 0) Then
                    Dim oButton As Button = e.Item.FindControl("BTNaddMacAttributeitem")
                    Resource.setButton(oButton, True)
                    oButton.Visible = (AllowSave AndAlso oDropDown.Items.Count > 0)
                Else
                    oDiv.Visible = False
                End If
            Else
                oDiv.Visible = False
            End If
        End If
    End Sub
    Private Sub RPTmacAttributeItems_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTmacAttributeItems.ItemCommand
        Dim idAttributeItem As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idAttributeItem = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Dim validCodes As Boolean
            SaveAttribute(GetAttribute, validCodes)
            RaiseEvent RemoveAttributeItem(IdAttribute, idAttributeItem, UrlMacAttributeType.mac)
        ElseIf e.CommandName = "addoption" Then
            Dim oDropdown As DropDownList = e.Item.FindControl("DDLattributes")
            If oDropdown.SelectedIndex >= 0 Then
                Dim validCodes As Boolean
                SaveAttribute(GetAttribute, validCodes)
                RaiseEvent AddMacAttributeItem(IdAttribute, CLng(oDropdown.SelectedValue))
            End If
        End If
    End Sub
#End Region

#Region "CompositeProfileAttribute"
    Private Sub Initialize(attribute As CompositeProfileAttribute)
        Me.MLVadvanced.SetActiveView(VIWcomposite)
        LoadProfileDropDownList(attribute.Attribute, DDLcompositeProfileFields)
        Me.RPTcompositeAttributeItems.DataSource = attribute.Items.Where(Function(i) i.Deleted = BaseStatusDeleted.None).OrderBy(Function(i) i.DisplayOrder).ToList()
        Me.RPTcompositeAttributeItems.DataBind()
    End Sub

    Private Sub RPTcompositeAttributeItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcompositeAttributeItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim att As CompositeAttributeItem = DirectCast(e.Item.DataItem, CompositeAttributeItem)
            Dim oLabel As Label = e.Item.FindControl("LBmoveUrlAttributeItem")
            oLabel.ToolTip = Resource.getValue("LBmoveUrlAttributeItem.ToolTip")
            oLabel.Visible = AllowSave

            Dim hidden As HtmlInputHidden = e.Item.FindControl("HDNdisplayOrder")
            hidden.Value = att.DisplayOrder

            oLabel = e.Item.FindControl("LBurlAttributeItem")
            oLabel.Text = att.Attribute.Name

            Dim oButton As Button = e.Item.FindControl("BTNdeleteUrlAttributeItem")
            Resource.setButton(oButton, True)

        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBurlAttributeItemsTitle_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oDiv As HtmlControl = e.Item.FindControl("DVfooter")
            If AllowSave Then
                Dim oDropDown As DropDownList = e.Item.FindControl("DDLattributes")
                oDropDown.DataSource = Me.CurrentPresenter.GetAvailableAttributesForComposite(IdProvider, IdAttribute)
                oDropDown.DataTextField = "Name"
                oDropDown.DataValueField = "Id"
                oDropDown.DataBind()

                If (oDropDown.Items.Count > 0) Then
                    Dim oButton As Button = e.Item.FindControl("BTNaddUrlAttributeitem")
                    Resource.setButton(oButton, True)
                    oButton.Visible = (AllowSave AndAlso oDropDown.Items.Count > 0)
                Else
                    oDiv.Visible = False
                End If
            Else
                oDiv.Visible = False
            End If
        End If
    End Sub
    Private Sub RPTcompositeAttributeItems_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcompositeAttributeItems.ItemCommand
        Dim idAttributeItem As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idAttributeItem = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Dim validCodes As Boolean
            SaveAttribute(GetAttribute, validCodes)
            RaiseEvent RemoveAttributeItem(IdAttribute, idAttributeItem, UrlMacAttributeType.compositeProfile)
        ElseIf e.CommandName = "addoption" Then
            Dim oDropdown As DropDownList = e.Item.FindControl("DDLattributes")
            If oDropdown.SelectedIndex >= 0 Then
                Dim validCodes As Boolean
                SaveAttribute(GetAttribute, validCodes)
                RaiseEvent AddCompositeAttributeItem(IdAttribute, CLng(oDropdown.SelectedValue))
            End If
        End If
    End Sub
#End Region

#Region "OrganizationAttribute"
    Private Sub Initialize(attribute As OrganizationAttribute)
        Me.MLVadvanced.SetActiveView(VIWorganization)
        Me.RPTorganizationItems.DataSource = attribute.Items.Where(Function(i) i.Deleted = BaseStatusDeleted.None).OrderBy(Function(i) i.Organization.Name).ToList()
        Me.RPTorganizationItems.DataBind()
    End Sub
    Private Sub RPTorganizationItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTorganizationItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim att As OrganizationAttributeItem = DirectCast(e.Item.DataItem, OrganizationAttributeItem)
            Dim oLabel As Label = e.Item.FindControl("LBorgName")
            oLabel.Text = att.Organization.Name

            Dim oDropDownList As DropDownList = e.Item.FindControl("DDLorgProfile")
            oDropDownList.DataSource = TranslatedProfileTypes
            oDropDownList.DataTextField = "Translation"
            oDropDownList.DataValueField = "Id"
            oDropDownList.DataBind()

            If Not IsNothing(oDropDownList.Items.FindByValue(att.IdDefaultProfile)) Then
                oDropDownList.SelectedValue = att.IdDefaultProfile
            End If
            oDropDownList = e.Item.FindControl("DDLorgPage")
            oDropDownList.DataSource = AvailablePages
            oDropDownList.DataTextField = "Value"
            oDropDownList.DataValueField = "Key"
            oDropDownList.DataBind()

            Dim oTextBox As TextBox = e.Item.FindControl("TXBorgCode")
            oTextBox.Text = att.RemoteCode

            Dim oButton As Button = e.Item.FindControl("BTNdeleteOrganizationAttributeItem")
            Resource.setButton(oButton, True)
            oButton.Visible = AllowSave
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBorgNameTitle_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBorgProfileTitle_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBorgPageTitle_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBorgCodeTitle_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oDiv As HtmlControl = e.Item.FindControl("DVfooter")
            If (AllowSave) Then
                Dim oDropDown As DropDownList = e.Item.FindControl("DDLorganization")
                oDropDown.DataSource = Me.CurrentPresenter.GetAvailableorganizations(IdProvider, IdAttribute)
                oDropDown.DataTextField = "Value"
                oDropDown.DataValueField = "Key"
                oDropDown.DataBind()

                If oDropDown.Items.Count > 0 Then
                    oDropDown = e.Item.FindControl("DDLorgProfile")
                    oDropDown.DataSource = TranslatedProfileTypes
                    oDropDown.DataTextField = "Translation"
                    oDropDown.DataValueField = "Id"
                    oDropDown.DataBind()

                    oDropDown = e.Item.FindControl("DDLorgPage")
                    oDropDown.DataSource = AvailablePages
                    oDropDown.DataTextField = "Value"
                    oDropDown.DataValueField = "Key"
                    oDropDown.DataBind()

                    Dim oButton As Button = e.Item.FindControl("BTNaddOrganizationAttributeitem")
                    Resource.setButton(oButton, True)
                Else
                    oDiv.Visible = False
                End If
            Else
                oDiv.Visible = False
            End If

        End If
    End Sub
    Private Sub RPTorganizationItems_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTorganizationItems.ItemCommand
        Dim idAttributeItem As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idAttributeItem = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Dim validCodes As Boolean
            SaveAttribute(GetAttribute, validCodes)
            RaiseEvent RemoveAttributeItem(IdAttribute, idAttributeItem, UrlMacAttributeType.organization)
        ElseIf e.CommandName = "addoption" Then
            Dim oDropdown As DropDownList = e.Item.FindControl("DDLorganization")
            Dim remoteCode As String = DirectCast(e.Item.FindControl("TXBorgCode"), TextBox).Text
            If oDropdown.SelectedIndex >= 0 Then
                Dim oProfile As DropDownList = e.Item.FindControl("DDLorgProfile")
                Dim oPage As DropDownList = e.Item.FindControl("DDLorgPage")
                Dim idProfile As Integer = 0
                Dim idPage As Long = 0
                If oPage.SelectedIndex > -1 Then
                    idPage = CLng(oPage.SelectedValue)
                End If
                If oProfile.SelectedIndex > -1 Then
                    idProfile = CInt(oProfile.SelectedValue)
                End If
                Dim validCodes As Boolean
                SaveAttribute(GetAttribute, validCodes)
                If (validCodes) Then
                    RaiseEvent AddOrganizationAttributeItem(IdAttribute, oDropdown.SelectedValue, idPage, idProfile, remoteCode)
                Else
                    RaiseEvent UnsavedAttribute(IdAttribute)
                End If

            End If
        End If
    End Sub
#End Region

#Region "CatalogueAttribute"
    Private Sub Initialize(attribute As CatalogueAttribute)
        Me.MLVadvanced.SetActiveView(VIWcatalogue)
        Me.RPTcatalogueItems.DataSource = attribute.Items.Where(Function(i) i.Deleted = BaseStatusDeleted.None).OrderBy(Function(i) i.Catalogue.Name).ToList()
        Me.RPTcatalogueItems.DataBind()
    End Sub
    Private Sub RPTcatalogueItems_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTcatalogueItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim att As CatalogueAttributeItem = DirectCast(e.Item.DataItem, CatalogueAttributeItem)
            Dim oLabel As Label = e.Item.FindControl("LBctgName")
            oLabel.Text = att.Catalogue.Name

            Dim oTextBox As TextBox = e.Item.FindControl("TXBctgCode")
            oTextBox.Text = att.RemoteCode

            Dim oButton As Button = e.Item.FindControl("BTNdeleteCatalogueAttributeItem")
            Resource.setButton(oButton, True)
            oButton.Visible = AllowSave
        ElseIf e.Item.ItemType = ListItemType.Header Then
            Dim oLabel As Label = e.Item.FindControl("LBctgNameTitle_t")
            Me.Resource.setLabel(oLabel)
            oLabel = e.Item.FindControl("LBctgCodeTitle_t")
            Me.Resource.setLabel(oLabel)

        ElseIf e.Item.ItemType = ListItemType.Footer Then
            Dim oDiv As HtmlControl = e.Item.FindControl("DVfooter")
            If (AllowSave) Then
                Dim oDropDown As DropDownList = e.Item.FindControl("DDLcatalogues")
                oDropDown.DataSource = Me.CurrentPresenter.GetAvailableCatalogues(IdProvider, IdAttribute)
                oDropDown.DataTextField = "Value"
                oDropDown.DataValueField = "Key"
                oDropDown.DataBind()

                If oDropDown.Items.Count > 0 Then
                    Dim oButton As Button = e.Item.FindControl("BTNaddCatalogueAttributeitem")
                    Resource.setButton(oButton, True)
                Else
                    oDiv.Visible = False
                End If
            Else
                oDiv.Visible = False
            End If
        End If
    End Sub
    Private Sub RPTcatalogueItems_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles RPTcatalogueItems.ItemCommand
        Dim idAttributeItem As Long = 0
        If Not String.IsNullOrEmpty(e.CommandArgument) AndAlso IsNumeric(e.CommandArgument) Then
            idAttributeItem = CLng(e.CommandArgument)
        End If
        If e.CommandName = "virtualDelete" Then
            Dim validCodes As Boolean
            SaveAttribute(GetAttribute, validCodes)
            RaiseEvent RemoveAttributeItem(IdAttribute, idAttributeItem, UrlMacAttributeType.coursecatalogue)
        ElseIf e.CommandName = "addoption" Then
            Dim oDropdown As DropDownList = e.Item.FindControl("DDLcatalogues")
            Dim remoteCode As String = DirectCast(e.Item.FindControl("TXBctgCode"), TextBox).Text
            If oDropdown.SelectedIndex >= 0 Then
                Dim validCodes As Boolean
                SaveAttribute(GetAttribute, validCodes)
                RaiseEvent AddCatalogueAttributeItem(IdAttribute, CLng(oDropdown.SelectedValue), remoteCode)
            End If
        End If
    End Sub
#End Region

    Private Sub BTNdeleteAttribute_Click(sender As Object, e As System.EventArgs) Handles BTNdeleteAttribute.Click
        RaiseEvent VirtualDeleteAttribute(IdAttribute)
    End Sub
    Private Sub BTNeditAttribute_Click(sender As Object, e As System.EventArgs) Handles BTNeditAttribute.Click
        RaiseEvent EditAttribute(IdAttribute)
    End Sub
    Private Sub BTNsaveAttributeSettings_Click(sender As Object, e As System.EventArgs) Handles BTNsaveAttributeSettings.Click
        Dim validCodes As Boolean
        If SaveAttribute(GetAttribute, validCodes) Then
            If validCodes Then
                RaiseEvent SavedAttribute(IdAttribute)
            Else
                RaiseEvent UnsavedAttribute(IdAttribute)
            End If
        ElseIf Not validCodes Then
            RaiseEvent UnsavedAttribute(IdAttribute)
        Else
            RaiseEvent InvalidCode(IdAttribute)
        End If
    End Sub
    Private Sub BTNcancelAttributeEditing_Click(sender As Object, e As System.EventArgs) Handles BTNcancelAttributeEditing.Click
        RaiseEvent UndoEditing(IdAttribute)
    End Sub

End Class