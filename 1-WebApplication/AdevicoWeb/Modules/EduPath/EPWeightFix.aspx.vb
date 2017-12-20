Imports lm.Comol.Core.DomainModel
Imports lm.Comol.Modules.EduPath.Domain

Public Class EPWeightFix
    Inherits PageBaseEduPath

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Overrides ReadOnly Property AlwaysBind As Boolean
        Get
            Return False

        End Get
    End Property

    Public Overrides Sub BindDati()

        MLVsummary.SetActiveView(VIWsummary)



        List = ServiceEP.PathsWeightAutoCorrect()

        RPTfix.DataSource = List
        RPTfix.DataBind()

        If (List.Count > 0) Then

            TRempty.Visible = False
            BTNfixall.Visible = True
        Else
            TRempty.Visible = True
            BTNfixall.Visible = False
        End If

        If Page.IsPostBack Then


        End If

    End Sub


    Private _list As IList(Of dtoPathWeightFix)
    Public Property List As IList(Of dtoPathWeightFix)
        Get
            Return ViewStateOrDefault("list", _list)
        End Get
        Set(value As IList(Of dtoPathWeightFix))
            _list = value
            ViewState("list") = _list
        End Set
    End Property


    Public Overrides Sub BindNoPermessi()
        'MLVsummary.SetActiveView(VIWerror)
        ShowMessageToPage(Me.Resource.getValue("Error.NotPermission"))
    End Sub

    Public Overrides Function HasPermessi() As Boolean
        Return CurrentContext.UserContext.UserTypeID = UserTypeStandard.SysAdmin
    End Function

    Protected Overrides ReadOnly Property PathType As lm.Comol.Modules.EduPath.Domain.EPType
        Get

        End Get
    End Property

    Public Overrides Sub RegistraAccessoPagina()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("pg_Summary", "EduPath")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With Me.Resource
            .setLabel(LBactualHeader)
            .setLabel(LBassignedHeader)
            .setLabel(LBcommunityNameHeader)
            .setLabel(LBexpectedHeader)
            .setLabel(LBpathNameHeader)
            .setLabel(LBservicePathSummaryFix)
            .setCheckBox(CHBselectAll)
            .setButton(BTNfixall)
            .setLabel(LBempty)
        End With
    End Sub

    Public Overrides Sub ShowMessageToPage(errorMessage As String)
        MLVsummary.SetActiveView(VIWerror)
        LBerror.Text = errorMessage
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            If IsSessioneScaduta(False) Then
                RedirectOnSessionTimeOut(Request.Url.AbsoluteUri, CurrentCommunityID)
            End If
            Return False
        End Get
    End Property

    Private Sub RPTfix_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTfix.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim dto As dtoPathWeightFix = e.Item.DataItem

            Dim olabel As Label
            olabel = e.Item.FindControl("LBcommunityname")
            olabel.Text = dto.CommunityName

            olabel = e.Item.FindControl("LBpathname")
            olabel.Text = dto.Name

            olabel = e.Item.FindControl("LBexpected")
            olabel.Text = dto.CalculatedWeightAuto

            olabel = e.Item.FindControl("LBactual")
            olabel.Text = dto.SavedWeightAuto

            olabel = e.Item.FindControl("LBassigned")
            olabel.Text = dto.SavedWeight

            Dim oHid As HiddenField
            oHid = e.Item.FindControl("HIDpathId")
            oHid.Value = dto.Id

            Dim oChb As CheckBox
            oChb = e.Item.FindControl("CHBfix")
            oChb.Checked = dto.Fix
        End If
    End Sub

    Private Sub EPWeightFix_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        Me.Master.ShowDocType = True
    End Sub

    Private Sub BTNfixall_Click(sender As Object, e As System.EventArgs) Handles BTNfixall.Click
        Dim fixMe As IList(Of dtoPathWeightFix) = New List(Of dtoPathWeightFix)

        For Each item As RepeaterItem In RPTfix.Items
            If (item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.Item) Then

                Dim oHid As HiddenField
                oHid = item.FindControl("HIDpathId")

                Dim oChb As CheckBox
                oChb = item.FindControl("CHBfix")

                If oChb.Checked Then
                    Dim dto As dtoPathWeightFix = (From i In List Where i.Id = Integer.Parse(oHid.Value) Select i).FirstOrDefault()
                    fixMe.Add(dto)
                End If

            End If
        Next
        If (fixMe.Count > 0) Then
            ServiceEP.FixPathsWeightAutoCorrect(fixMe)
        End If
        BindDati()
    End Sub

    Private Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click

        ServiceEP.FixPathDescriptionUrls(Integer.Parse(TXBpathid.Text), "src=""/elle3/", "src=""/comol_elle3/")

    End Sub

    Protected Overrides Sub NotifyModuleStatus(status As lm.Comol.Core.DomainModel.ModuleStatus)
        CTRLmoduleStatusMessage.Visible = True
        CTRLmoduleStatusMessage.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.alert)
    End Sub

    Protected Overrides Sub NotifyUnavailableModule(status As lm.Comol.Core.DomainModel.ModuleStatus)
        MLVsummary.SetActiveView(VIWmessages)
        CTRLmessages.InitializeControl(Resource.getValue("EduPath.ModuleStatus." & status.ToString), lm.Comol.Core.DomainModel.Helpers.MessageType.error)
    End Sub

    Protected Overrides ReadOnly Property CheckModuleStatus As Boolean
        Get
            Return True
        End Get
    End Property
End Class