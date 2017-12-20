Imports System.Diagnostics
Imports lm.Comol.Core.DomainModel.Helpers
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain
Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview

Public Class GlossaryListOrder
    Inherits GLpageBase
    Implements IViewGlossaryListOrder

#Region "Context"

    Private _Presenter As GlossaryListOrderPresenter

    Private ReadOnly Property CurrentPresenter() As GlossaryListOrderPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New GlossaryListOrderPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

    Public Property _IdDefault As Long
        Get
            Return Me.ViewStateOrDefault("_IdDefault", -1)
        End Get
        Set(value As Long)
            Me.ViewState("_IdDefault") = value
        End Set
    End Property

    Public Property _IsDefault As Boolean
        Get
            Return Me.ViewStateOrDefault("_IsDefault", True)
        End Get
        Set(value As Boolean)
            Me.ViewState("_IsDefault") = value
        End Set
    End Property

    Public Property ListUnpublished As List(Of Int64)
        Get
            Return Me.ViewStateOrDefault("ListUnpublished", New List(Of Int64))
        End Get
        Set(value As List(Of Int64))
            Me.ViewState("ListUnpublished") = value
        End Set
    End Property


#End Region

#Region "Overrides"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Overrides Sub BindDati()
        Master.ShowNoPermission = False
        HYPback.NavigateUrl = String.Format("{0}{1}", ApplicationUrlBase, RootObject.GlossaryList(PreloadIdCommunity, IdGlossary))
        CurrentPresenter.InitView()
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewGlossaryListOrder.SetTitle
        Dim titleString = Resource.getValue("GlossaryListOrder")
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString
    End Sub

    Public Overrides Sub BindNoPermessi()
        Resource.setLabel(LBLNoPermission)
        PNLnoPermision.Visible = True
        PNLmain.Visible = False
    End Sub

    Public Overrides Sub SetInternazionalizzazione()
        Dim titleString = Resource.getValue("GlossaryListOrder")
        Master.ServiceTitle = titleString
        Master.ServiceTitleToolTip = titleString
        Master.ServiceNopermission = titleString
        LTpageTitle_t.Text = titleString

        With Resource
            '.setLiteral(LTglossariesSorting)
            .setLiteral(LTglossariesSortingInfo)
            .setHyperLink(HYPback, False, False, False, False)
            .setLinkButton(LNBsave, False, False, False, False)
        End With
    End Sub

    Protected Friend Overrides Sub DisplaySessionTimeout()
        Dim Url As String = RootObject.GlossaryList(IdCommunity, IdGlossary)
        RedirectOnSessionTimeOut(Url, IdCommunity)
    End Sub

#End Region

#Region "Custom"

    Public Sub LoadViewData(ByVal glossaryList As List(Of DTO_Glossary)) Implements IViewGlossaryListOrder.LoadViewData
        _IsDefault = True

        Dim defaultList As New List(Of DTO_Glossary)
        Dim otherList As New List(Of DTO_Glossary)

        For Each glo As DTO_Glossary In glossaryList
            If (glo.IsDefault) Then
                defaultList.Add(glo)
            Else
                otherList.Add(glo)
            End If
        Next
        
        RPTlistDefault.DataSource = defaultList
        RPTlist.DataSource = otherList

        ListUnpublished = New List(Of Long)()
        For Each item As DTO_Glossary In glossaryList
            If (Not item.IsPublished) Then
                ListUnpublished.Add(item.Id)
            End If
        Next
        RPTlistDefault.DataBind()
        RPTlist.DataBind()
    End Sub


    Private Sub RPTlist_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTlist.ItemDataBound, RPTlistDefault.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim dtoGlossary As DTO_Glossary = e.Item.DataItem

            Dim LTname As Literal = e.Item.FindControl("LTname")
            If Not IsNothing(LTname) Then
                'LTname.Text = dtoGlossary.Id & "->" & dtoGlossary.Name
                LTname.Text = dtoGlossary.Name

            End If

            Dim LBinfotag As Label = e.Item.FindControl("LBinfotag")
            If Not IsNothing(LBinfotag) Then

                If ListUnpublished.Contains(dtoGlossary.Id) Then
                    LBinfotag.Text = Resource.getValue("LTunpublished.text")
                    LBinfotag.Visible = True
                Else
                    LBinfotag.Visible = False
                End If
            End If

            If dtoGlossary.IsDefault AndAlso dtoGlossary.IdCommunity = IdCommunity Then
                _IdDefault = dtoGlossary.Id
            End If
            Debug.WriteLine(dtoGlossary)
        End If
    End Sub

    Private Sub LNBsave_Click(sender As Object, e As EventArgs) Handles LNBsave.Click
        Dim list As List(Of Int64) = New List(Of Int64)()
        If Not String.IsNullOrEmpty(Me.HIFreorderedData.Value) Then
            list.Add(_IdDefault)
            Dim strCleanded = HIFreorderedData.Value.Replace("srt[]=", String.Empty).Split("&")
            For Each itm As String In strCleanded
                Try
                    list.Add(Convert.ToInt64(itm))
                Catch ex As Exception
                End Try
            Next
            CurrentPresenter.ReorderGlossary(list, _IdDefault)
        Else
            ShowMessage(SaveStateEnum.None, MessageType.none)
        End If
    End Sub

    Public Function GetGlossaryClass(ByVal id As Long) As String
        If _IsDefault Then
            _IsDefault = False
            Return " default"
        Else
            If ListUnpublished.Contains(id) Then
                Return " unpublished"
            End If
            Return String.Empty
        End If
    End Function

    Public Sub ShowMessage(ByVal saveStateEnum As SaveStateEnum, ByVal type As MessageType) Implements IViewGlossaryListOrder.ShowMessage
        SetMessage(Resource.getValue(String.Format("SaveStateEnum.{0}", saveStateEnum)), type)
    End Sub

    Public Sub SetMessage(ByVal message As String, ByVal type As MessageType)
        If (type = MessageType.none OrElse String.IsNullOrEmpty(message)) Then
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text)
            CTRLmessagesInfo.Visible = False
        Else
            DVmessages.Attributes.Add("class", Me.LTmessageheaderCss.Text.Replace(" hide", ""))
            CTRLmessagesInfo.Visible = True
            CTRLmessagesInfo.InitializeControl(message, type)
        End If
    End Sub

#End Region
End Class
