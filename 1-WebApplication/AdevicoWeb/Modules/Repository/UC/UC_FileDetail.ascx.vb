Imports lm.Comol.Modules.Base.Presentation
Imports lm.Comol.Modules.Base.DomainModel
Imports lm.Comol.UI.Presentation
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Core.DomainModel

Partial Public Class UC_FileDetail
    Inherits BaseControlWithLoad
    Implements IViewCommunityFileDetail

#Region "Iview"
    Private _Presenter As CRfileDetailPresenter
    Private _BaseUrl As String
    Private _CurrentContext As lm.Comol.Core.DomainModel.iApplicationContext
    Public ReadOnly Property CurrentContext() As lm.Comol.Core.DomainModel.iApplicationContext
        Get
            If IsNothing(_CurrentContext) Then
                _CurrentContext = New lm.Comol.Core.DomainModel.ApplicationContext() With {.UserContext = SessionHelpers.CurrentUserContext, .DataContext = SessionHelpers.CurrentDataContext}
            End If
            Return _CurrentContext
        End Get
    End Property
    Public Overloads ReadOnly Property BaseUrl() As String
        Get
            If _BaseUrl = "" Then
                _BaseUrl = Me.PageUtility.BaseUrl
            End If
            Return _BaseUrl
        End Get
    End Property
    Public ReadOnly Property CurrentPresenter() As CRfileDetailPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New CRfileDetailPresenter(Me.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property
    Public Function CommunityRepositoryPermission(ByVal CommunityID As Integer) As lm.Comol.Modules.Base.DomainModel.ModuleCommunityRepository Implements lm.Comol.Modules.Base.Presentation.IViewCommunityFileDetail.CommunityRepositoryPermission
        Dim oModule As ModuleCommunityRepository = Nothing

        If CommunityID = 0 Then
            oModule = ModuleCommunityRepository.CreatePortalmodule(Me.CurrentContext.UserContext.UserTypeID)
        Else
            oModule = (From sb In ManagerPersona.GetPermessiServizio(Me.CurrentContext.UserContext.CurrentUser.Id, Services_File.Codex) _
                  Where sb.CommunityID = CommunityID Select New ModuleCommunityRepository(New Services_File(sb.PermissionString))).FirstOrDefault
            If IsNothing(oModule) Then
                oModule = New ModuleCommunityRepository
            End If
        End If

        Return oModule
    End Function
    Public Property MaxResult() As Integer Implements IViewCommunityFileDetail.MaxResult
        Get
            If IsNumeric(Me.ViewState("MaxResult")) AndAlso Me.ViewState("MaxResult") > 0 Then
                Return Me.ViewState("MaxResult")
            Else
                Return 10
            End If
        End Get
        Set(ByVal value As Integer)
            Me.ViewState("MaxResult") = value
        End Set
    End Property
#End Region
#Region "Base"
    Public Overrides ReadOnly Property AlwaysBind() As Boolean
        Get
            Return False
        End Get
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Base"
    Public Overrides Sub BindDati()

    End Sub

    Public Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_CommunityFile", "Generici", "UC_File")
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        With MyBase.Resource
            .setLabel(Me.LBallow_t)
            .setLabel(Me.LBcommunity_t)
            .setLabel(Me.LBcreatedOn_t)
            .setLabel(Me.LBdeny_t)
            .setLabel(Me.LBdescription_t)
            .setLabel(Me.LBdownloaded_t)
            .setLabel(Me.LBname_t)
            .setLabel(Me.LBpath_t)
            .setLabel(Me.LBtype_t)
            .setLabel(Me.LBvisibleTo_t)
            .setLabel(Me.LBcreatedOn)
            Me.LBitemNotFound.Text = .getValue("noitem")
            .setLabel(Me.LBdenyPersons_t)
            .setLabel(Me.LBdenyRoles_t)
            .setLabel(Me.LBallowRoles_t)
            .setLabel(Me.LBallowPersons_t)
        End With
    End Sub

    Public Sub InitializeControl(ByVal FileId As Long)
        Me.SetInternazionalizzazione()
        Me.TRallow.Visible = False
        Me.TRdeny.Visible = False
        Me.DVallowRoles.Visible = False
        Me.DVallowPersons.Visible = False
        Me.DVdenyPersons.Visible = False
        Me.DVdenyRoles.Visible = False
        Me.CurrentPresenter.InitView(FileId)
    End Sub
#End Region


    Public Sub LoadFolder(ByVal oFolder As dtoCommunityItemRepository, ByVal Communityname As String) Implements IViewCommunityFileDetail.LoadFolder
        Me.MLVdetails.SetActiveView(Me.VIWitem)
        Me.TRdownloaded.Visible = False
        Me.LoadItemData(oFolder.File, Communityname)
    End Sub

    Public Sub LoadFile(ByVal oFile As dtoCommunityItemRepository, ByVal Communityname As String) Implements IViewCommunityFileDetail.LoadFolderContent
        Me.MLVdetails.SetActiveView(Me.VIWitem)
        Me.TRdownloaded.Visible = True
        Me.Resource.setLabel(Me.LBdownloaded)
        Me.LBdownloaded.Text = String.Format(Me.LBdownloaded.Text, oFile.File.Downloads)
        Me.LBcreatedOn_t.Text = Me.Resource.getValue("uploaded")
        Me.LoadItemData(oFile.File, Communityname)

        Me.LBtype.Text = Resource.getValue("RepositoryItemType." & oFile.File.RepositoryItemType.ToString)
    End Sub

    Private Sub LoadItemData(ByVal oFile As CommunityFile, ByVal Communityname As String)
        Me.LBcreatedOn.Text = String.Format(Me.LBcreatedOn.Text, oFile.Owner.SurnameAndName, oFile.CreatedOn.ToString("dd/MM/yy"), oFile.CreatedOn.ToString("HH:mm"))
        Me.LBcommunity.Text = Communityname
        If String.IsNullOrEmpty(oFile.Description) Then
            TRdescription.Visible = False
        Else
            TRdescription.Visible = True
            Me.LBdescription.Text = oFile.Description
        End If

        Me.LBname.Text = oFile.DisplayName

        If oFile.isFile Then
            Dim Size As Long = oFile.Size
            If Size > 0 Then
                If Size < 1024 Then
                    Me.LBname.Text &= " (" & 1 & " kb) "
                Else
                    Size = Size / 1024
                    If Size < 1024 Then
                        LBname.Text &= " (" & Math.Round(Size) & " kb) "
                    Else
                        LBname.Text &= " (" & Math.Round(Size / 1024, 2) & " mb) "
                    End If
                End If
            End If
        End If
        Dim visible As String = "visible"
        If oFile.isSCORM OrElse oFile.isVideocast Then
            visible &= "P."
        Else
            visible &= "D."
        End If
        visible &= oFile.isVisible.ToString
        Me.LBvisibleTo.Text = Me.Resource.getValue(visible)
        Me.LBpath.Text = Me.Resource.getValue("BaseFolder") & Me.CurrentPresenter.GetFolderPath(oFile.FolderId)
        Me.MLVdetails.SetActiveView(Me.VIWitem)
    End Sub
    Public Sub LoadNoDetails(ByVal ItemId As Long, ByVal CommunityId As Integer) Implements IViewCommunityFileDetail.LoadNoDetails
        Me.MLVdetails.SetActiveView(Me.VIWerror)
    End Sub

    Public ReadOnly Property Portalname() As String Implements IViewCommunityFileDetail.Portalname
        Get
            Return Me.Resource.getValue("Portalname")
        End Get
    End Property

    Public Sub LoadAllowPermission(ByVal ForCommunity As Boolean, ByVal Roles As List(Of String), ByVal Persons As List(Of String), ByVal MorePerson As Boolean) Implements IViewCommunityFileDetail.LoadAllowPermission
        Me.TRallow.Visible = True
        If ForCommunity Then
            Me.LBallow.Text = Me.Resource.getValue("AllowForCommunity")
        Else
            Me.LBallow.Text = ""
        End If
        Me.LBallowRoles.Text = ""
        If Roles.Count > 0 Then
            Me.DVallowRoles.Visible = True
            For Each Role As String In Roles
                If String.IsNullOrEmpty(Me.LBallowRoles.Text) Then
                    Me.LBallowRoles.Text = Role
                Else
                    Me.LBallowRoles.Text &= ", " & Role
                End If
            Next
            Me.LBallowRoles.Text &= "."
        End If
        Me.LBallowPersons.Text = ""
        If Persons.Count > 0 Then
            Me.DVallowPersons.Visible = True
            For Each Person As String In Persons
                If String.IsNullOrEmpty(Me.LBallowRoles.Text) Then
                    Me.LBallowPersons.Text = Person
                Else
                    Me.LBallowPersons.Text &= ", " & Person
                End If
            Next
            Me.LBallowPersons.Text &= IIf(MorePerson <= 0, ".", " .... ")
            If MorePerson > 0 Then
                Me.LBallowPersons.ToolTip = String.Format(Me.Resource.getValue("MorePersonAllow"), MorePerson)
            End If
        End If
    End Sub

    Public Sub LoadDenyPermission(ByVal ForCommunity As Boolean, ByVal Roles As List(Of String), ByVal Persons As List(Of String), ByVal MorePerson As Boolean) Implements IViewCommunityFileDetail.LoadDenyPermission
        Me.TRdeny.Visible = True
        If ForCommunity Then
            Me.LBdeny.Text = Me.Resource.getValue("DenyForCommunity")
        Else
            Me.LBdeny.Text = ""
        End If
        Me.LBdenyRoles.Text = ""
        If Roles.Count > 0 Then
            Me.DVdenyRoles.Visible = True
            For Each Role As String In Roles
                If String.IsNullOrEmpty(Me.LBallowRoles.Text) Then
                    Me.LBdenyRoles.Text = Role
                Else
                    Me.LBdenyRoles.Text &= ", " & Role
                End If
            Next
            Me.LBdenyRoles.Text &= "."
        End If
        Me.LBdenyPersons.Text = ""
        If Persons.Count > 0 Then
            Me.DVallowPersons.Visible = True
            For Each Person As String In Persons
                If String.IsNullOrEmpty(Me.LBallowRoles.Text) Then
                    Me.LBdenyPersons.Text = Person
                Else
                    Me.LBdenyPersons.Text &= ", " & Person
                End If
            Next
            Me.LBdenyPersons.Text &= IIf(MorePerson <= 0, ".", " .... ")
            If MorePerson > 0 Then
                Me.LBdenyPersons.ToolTip = String.Format(Me.Resource.getValue("MorePersonDeny"), MorePerson)
            End If
        End If
    End Sub

    Public Sub LoadNoDenyPermission() Implements IViewCommunityFileDetail.LoadNoDenyPermission
        Me.TRdeny.Visible = False
    End Sub
End Class