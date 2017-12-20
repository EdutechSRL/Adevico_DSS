Imports COL_BusinessLogic_v2.Comunita
Imports COL_BusinessLogic_v2.UCServices
Imports lm.Comol.Modules.Standard.Glossary.Domain
Imports lm.Comol.Modules.Standard.Glossary.Domain.Dto
Imports lm.Comol.Modules.Standard.Glossary.MVP

Public Class UC_GlossarySelect
    Inherits BaseControl
    Implements iViewGlossarySelector

#Region "Internal"

    Private _presenter As GlossarySelectorPresenter

    Private ReadOnly Property Presenter As GlossarySelectorPresenter
        Get
            If IsNothing(_presenter) Then
                Me._presenter = New GlossarySelectorPresenter(MyBase.PageUtility.CurrentContext, Me)
            End If
            Return _presenter
        End Get
    End Property

#End Region

#Region "Page And PageBase"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.SetInternazionalizzazione()
    End Sub

    Protected Overrides Sub SetCultureSettings()
        MyBase.SetCulture("UC_GlossarySelector", "Glossary")
    End Sub

    Protected Overrides Sub SetInternazionalizzazione()
        If IsNothing(MyBase.Resource) Then
            Me.SetCultureSettings()
        End If

        With MyBase.Resource
        End With
    End Sub

    Public Overrides ReadOnly Property VerifyAuthentication As Boolean
        Get
            Return True
        End Get
    End Property

    Private Sub DG_Glossary_ItemDataBound(sender As Object, e As DataGridItemEventArgs) _
        Handles DG_Glossary.ItemDataBound
        If e.Item.ItemType = ListItemType.Header Then
            Dim Lbl_GlossaryName_t, Lbl_GlossaryItmNum_t As Label
            Try
                Lbl_GlossaryName_t = e.Item.FindControl("Lbl_GlossaryName_t")
                Lbl_GlossaryItmNum_t = e.Item.FindControl("Lbl_GlossaryItmNum_t")
            Catch ex As Exception

            End Try

            If Not IsNothing(Lbl_GlossaryName_t) Then
                Me.Resource.setLabel(Lbl_GlossaryName_t)
            End If

            If Not IsNothing(Lbl_GlossaryItmNum_t) Then
                Me.Resource.setLabel(Lbl_GlossaryItmNum_t)
            End If

        ElseIf e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim itm As GlossaryGroup = e.Item.DataItem

            Dim HF_Id As HiddenField
            Try
                HF_Id = e.Item.FindControl("HF_Id")
                HF_Id.Value = itm.Id.ToString()
            Catch ex As Exception

            End Try


            Dim Lbl_GlossaryName, Lbl_GlossaryItmNum As Label

            Try
                Lbl_GlossaryName = e.Item.FindControl("Lbl_GlossaryName")
                Lbl_GlossaryName.Text = itm.Name

            Catch ex As Exception
                e.Item.Visible = False
                Exit Sub
            End Try

            Try
                Lbl_GlossaryItmNum = e.Item.FindControl("Lbl_GlossaryItmNum")
                Lbl_GlossaryItmNum.Text = itm.TotalItems.ToString()
            Catch ex As Exception
                Lbl_GlossaryItmNum.Text = "0"
                Exit Sub
            End Try

        End If
    End Sub

#End Region

#Region "MVP"

    Public Function InitControl(SourceCommunityId As Integer) As Boolean Implements iViewGlossarySelector.InitControl

        Return Me.Presenter.BindGlossaries(SourceCommunityId, Me.HasComPermission(SourceCommunityId))
    End Function

#End Region

#Region "iView"

    Public Sub BindGlossary(GlossaryGroups As IList(Of GlossaryGroup)) Implements iViewGlossarySelector.BindGlossary
        If GlossaryGroups.Count() > 0 Then
            Lbl_NoGlossary.Visible = False
            Me.DG_Glossary.Visible = True

            Me.DG_Glossary.DataSource = GlossaryGroups
            Me.DG_Glossary.DataBind()

        Else
            Me.DG_Glossary.Visible = False
            Lbl_NoGlossary.Visible = True
            Me.Resource.setLabel(Lbl_NoGlossary)
        End If
    End Sub

    Public Function GetSelectedGlossaryIds() As IList(Of Long) Implements iViewGlossarySelector.GetSelectedGlossaryIds

        Dim Ids As New List(Of Long)

        For Each item As DataGridItem In Me.DG_Glossary.Items
            If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                Dim Id As Long = 0
                Dim Cbx_SelectItem As CheckBox
                Try
                    Cbx_SelectItem = item.FindControl("Cbx_SelectItem")
                Catch ex As Exception
                End Try

                If Not IsNothing(Cbx_SelectItem) AndAlso Cbx_SelectItem.Checked Then
                    Dim HF_Id As HiddenField
                    Try
                        HF_Id = item.FindControl("HF_Id")
                        Id = Convert.ToInt64(HF_Id.Value)
                    Catch ex As Exception
                    End Try

                    If Id > 0 Then
                        Ids.Add(Id)
                    End If

                End If


            End If
        Next

        Return Ids
    End Function

    Public Function GetSelectedGlossarySummary() As IList(Of GlossaryGroupSummaryDTO) _
        Implements iViewGlossarySelector.GetSelectedGlossarySummary
        Dim Items As New List(Of GlossaryGroupSummaryDTO)

        For Each item As DataGridItem In Me.DG_Glossary.Items
            If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then

                Dim Cbx_SelectItem As CheckBox
                Try
                    Cbx_SelectItem = item.FindControl("Cbx_SelectItem")
                Catch ex As Exception
                End Try

                If Not IsNothing(Cbx_SelectItem) AndAlso Cbx_SelectItem.Checked Then
                    Dim Group As New GlossaryGroupSummaryDTO
                    Dim HF_Id As HiddenField
                    Dim Lbl_GlossaryName, Lbl_GlossaryItmNum As Label
                    Try
                        HF_Id = item.FindControl("HF_Id")
                        Lbl_GlossaryName = item.FindControl("Lbl_GlossaryName")
                        Lbl_GlossaryItmNum = item.FindControl("Lbl_GlossaryItmNum")
                        Group.Id = Convert.ToInt64(HF_Id.Value)
                        Group.Name = Lbl_GlossaryName.Text
                        Group.NumItem = Lbl_GlossaryItmNum.Text

                        Items.Add(Group)
                    Catch ex As Exception
                    End Try
                End If
            End If
        Next

        Return Items
    End Function

    Public Function MakeCopy(DestinationCommunityId As Integer, ByVal IsNewCommunity As Boolean) As Integer _
        Implements iViewGlossarySelector.MakeCopy

        '    Nuova comunità
        If (IsNewCommunity AndAlso DestinationCommunityId > 0) Then
            Return Me.Presenter.CopyGlossary(DestinationCommunityId, True)

            'Comunità esistente: controllo permessi
        ElseIf (DestinationCommunityId > 0 AndAlso Me.HasComPermission(DestinationCommunityId)) Then
            Return Me.Presenter.CopyGlossary(DestinationCommunityId, True)


        Else
            Return 0
        End If
    End Function

#End Region

    
    ''' <summary>
    '''     Controlla i permessi del servizio sulla comunità indicata
    ''' </summary>
    ''' <param name="CommunityId">ID Comunità</param>
    ''' <returns>Permesso sulla comunità</returns>
    ''' <remarks>
    '''     1. Rivedere permessi:
    '''     -  controllare che siano implementati correttamente
    '''     -  se solo GlossaryManage, fare return diretto, altrimenti aggiungere ul resto (Es: Edit Term)
    ''' </remarks>
    Public Function HasComPermission(ByVal CommunityId As Int32)

        Dim GlossaryService As Services_Glossary =
                New Services_Glossary(COL_Comunita.GetPermessiForServizioByCode(TipoRuoloStandard.AdminComunità,
                                                                                CommunityId, Services_Glossary.Codex))

        If GlossaryService.ManageGlossary Then
            Return True
        Else
            Return False
        End If
    End Function
End Class