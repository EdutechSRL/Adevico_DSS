Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class ListGlossary
    Inherits GLpageBase
    Implements IViewGlossaryList

#Region "Context"

    Private _Presenter As GlossaryListPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryListPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryListPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        CurrentPresenter.InitView(PreloadIsFirstOpen)
    End Sub

    Private Sub SetTitle(name As String) Implements IViewGlossaryList.SetTitle
        Dim base As String = "Glossary.ServiceTitle.List"
        If Not String.IsNullOrEmpty(name) Then
            base &= ".Community"
        End If

        If Not String.IsNullOrEmpty(name) Then
            Master.ServiceTitle = String.Format(Resource.getValue(base), name)
            Master.ServiceTitleToolTip = String.Format(Resource.getValue(base & ".ToolTip"), name)
            Master.ServiceNopermission = String.Format(Resource.getValue(base & ".NoPermission"), name)
            LTpageTitle_t.Text = String.Format(Resource.getValue(base), name)
        Else
            Master.ServiceTitle = Resource.getValue(base)
            Master.ServiceTitleToolTip = Resource.getValue(base & ".ToolTip")
            Master.ServiceNopermission = Resource.getValue(base & ".NoPermission")
            LTpageTitle_t.Text = Resource.getValue(base)
        End If
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        CTRLglossaries.Visible = false
        PNLnoPermision.Visible = True
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        'With Me.Resource
        '    .setHyperLink(HYPglossaryNew, False, False, False, False)
        'End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Private Sub LoadViewData(idCommunity As Integer, manageShareEnabled As Boolean, manageShare As Boolean) Implements IViewGlossaryList.LoadViewData
        ' Inizializzazione UC lista  glossari con relativi filtri, paginatore ecc
        CTRLglossaries.InitializeControl(idCommunity, manageShareEnabled)
        'HYPglossaryNew.NavigateUrl = ApplicationUrlBase & RootObject.GlossaryAdd(idCommunity)
    End Sub

    Public Sub GoToGlossaryView(ByVal idGlossary As Long) Implements IViewGlossaryList.GoToGlossaryView
        Response.Redirect(ApplicationUrlBase & RootObject.GlossaryView(idGlossary, Me.IdCommunity))
    End Sub

#End Region
End Class