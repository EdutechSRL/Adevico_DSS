Imports lm.Comol.Modules.Standard.GlossaryNew.Domain.dto
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.Iview.UC
Imports lm.Comol.Modules.Standard.GlossaryNew.Presentation.UC

Public Class UC_GlossaryImportTerms
    Inherits GLbaseControl
    Implements IViewUC_GlossaryImportTerms

#Region "Context"

    Private _Presenter As UC_GlossaryImportTermsPresenter

    Private ReadOnly Property CurrentPresenter() As UC_GlossaryImportTermsPresenter
        Get
            If IsNothing(_Presenter) Then
                _Presenter = New UC_GlossaryImportTermsPresenter(PageUtility.CurrentContext, Me)
            End If
            Return _Presenter
        End Get
    End Property

#End Region

    Protected Overrides Sub SetInternazionalizzazione()
        With Resource
            SWHcommunity.SetText(Resource, True, False)
            .setLabel(LBMappedTo)
            .setLinkButton(LBChooseToCommunity, False, True, False, False)
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Public Sub SetTitle(ByVal name As String) Implements IViewUC_GlossaryImportTerms.SetTitle
    End Sub

    Public Sub BindDati(ByVal idCommunity As Int32, ByVal idGlossary As Int64)
        Me.IdGlossary = idGlossary
        CurrentPresenter.InitView()
    End Sub

    Public Sub LoadViewData(ByVal community As DTO_ImportCommunity, ByVal showCommunityMapper As Boolean) Implements IViewUC_GlossaryImportTerms.LoadViewData
        LBcommunityName.Text = community.Name

        Dim all = community.GlossaryList
        Dim filtered = community.GlossaryList.Where(Function(f) f.TermList.Count > 0)

        RPTGlossaries.DataSource = filtered

        RPTGlossaries.DataBind()
        'SWHcommunity.SetText(Resource, True, False)

        LBmapTo.Visible = showCommunityMapper
        LTControlId.Text = Guid.NewGuid().ToString()
        LTSelectedCommunity.Text = "-1"

        With Resource
            SWHcommunity.SetText(Resource, True, False)
            .setLabel(LBMappedTo)
            .setLinkButton(LBChooseToCommunity, False, True, False, False)
        End With
    End Sub

    Private index As Integer = 0

    Public Sub RPTGlossaries_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles RPTGlossaries.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim itemData As DTO_ImportGlossary = e.Item.DataItem
            Dim LBglossaryName As Label = e.Item.FindControl("LBglossaryName")
            If Not IsNothing(LBglossaryName) Then
                LBglossaryName.Text = itemData.Name
            End If

            Dim SWHGlossary As UC_Switch = e.Item.FindControl("SWHGlossary")
            If Not IsNothing(SWHGlossary) Then
                SWHGlossary.SetText(Resource, True, False)
            End If

            Dim RPTerms As Repeater = e.Item.FindControl("RPTerms")
            If Not IsNothing(RPTerms) Then
                RPTerms.DataSource = itemData.TermList
                RPTerms.DataBind()
            End If

            Dim LBselectAll As Label = e.Item.FindControl("LBselectAll")
            If Not IsNothing(LBselectAll) Then
                Resource.setlabel(LBselectAll)
            End If

            Dim LBselectNone As Label = e.Item.FindControl("LBselectNone")
            If Not IsNothing(LBselectNone) Then
                Resource.setlabel(LBselectNone)
            End If

            index += 1

        End If
    End Sub

    Public Sub RPTerms_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim itemData As DTO_ImportTerm = e.Item.DataItem

            Dim LBterm As Label = e.Item.FindControl("LBterm")
            If Not IsNothing(LBterm) Then
                LBterm.Text = itemData.Name
            End If

            Dim CBXterm As CheckBox = e.Item.FindControl("CBXterm")
            If Not IsNothing(CBXterm) Then
                CBXterm.Checked = true
                CBXterm.Attributes.Add("idTerm", itemData.Id)
            End If

        End If
    End Sub

    'Public Sub LNBselectAll_Click(sender As Object, e As EventArgs)
    '    Dim button As LinkButton = sender
    '    Dim index As Integer = button.CommandArgument
    '    changeCheckState(index, True)
    'End Sub

    'Public Sub LNBselectNone_Click(sender As Object, e As EventArgs)
    '    Dim button As LinkButton = sender
    '    Dim index As Integer = button.CommandArgument
    '    changeCheckState(index, False)
    'End Sub

    'Private Sub changeCheckState(ByVal index As Integer, ByVal checked As Boolean)
    '    Dim RPTerms As Repeater = RPTGlossaries.Items(index).FindControl("RPTerms")
    '    If Not IsNothing(RPTerms) Then

    '        For Each item As RepeaterItem In RPTerms.Items
    '            Dim CBXterm As CheckBox = item.FindControl("CBXterm")
    '            If Not IsNothing(CBXterm) Then
    '                CBXterm.Checked = checked
    '            End If
    '        Next
    '    End If
    'End Sub

    Public Function GetSelectedTermIds(ByVal isDefault As Boolean) As List(Of Long)
        Dim result As New List(Of Long)
        For Each item As List(Of Long) In GetSelectedTermIdsToCommunity(isDefault).Values
            result.AddRange(item)
        Next
        Return result.Distinct().ToList()
    End Function

    Public Function GetSelectedTermIdsToCommunity(ByVal isDefault As Boolean) As Dictionary(Of Int32, List(Of Int64))
        Dim result As New Dictionary(Of Int32, List(Of Int64))
        Dim mappedCommunityId As Integer = - 1

        Try
            mappedCommunityId = Convert.ToInt32(LTSelectedCommunity.Text)
        Catch ex As Exception
            mappedCommunityId = - 1
        End Try
        If isDefault Then
            mappedCommunityId = - 2
        End If

        If SWHcommunity.Status Then
            If (mappedCommunityId > 0 Or mappedCommunityId = - 2) Then
                For Each itemGlossary As RepeaterItem In RPTGlossaries.Items
                    Dim SWHGlossary As UC_Switch = itemGlossary.FindControl("SWHGlossary")
                    If Not IsNothing(SWHGlossary) Then
                        If SWHGlossary.Status Then
                            'Dim label As Label = itemGlossary.FindControl("LBglossaryName")

                            Dim RPTerms As Repeater = itemGlossary.FindControl("RPTerms")
                            If Not IsNothing(RPTerms) Then
                                For Each item As RepeaterItem In RPTerms.Items
                                    Dim CBXterm As CheckBox = item.FindControl("CBXterm")
                                    If Not IsNothing(CBXterm) Then
                                        If CBXterm.Checked Then
                                            Dim idTerm As Integer = CBXterm.Attributes.Item("idTerm")
                                            If Not result.ContainsKey(mappedCommunityId) Then
                                                Dim list = New List(Of Int64)
                                                result.Add(mappedCommunityId, list)
                                            End If
                                            result(mappedCommunityId).Add(idTerm)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If
        End If
        Return result
    End Function

    Protected Sub ClickCommunity(ByVal sender As Object, ByVal e As EventArgs)
        Dim e1 As CustomArgs = New CustomArgs
        e1.Key = LTControlId.Text
        RaiseBubbleEvent(sender, e1)
    End Sub

    Public Sub SetMappedCommunity(ByVal key As String, ByVal value As String, ByVal communityName As String)
        If (LTControlId.Text = key) Then
            LTSelectedCommunity.Text = value
            LBChooseToCommunity.Text = communityName
        End If
    End Sub
End Class

Public Class CustomArgs
    Inherits EventArgs

    Property Key() As String
End Class
