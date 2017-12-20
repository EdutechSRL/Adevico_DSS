Imports lm.Comol.Core.DomainModel

Public Class FilterHelpers
    Private _Resource As New Dictionary(Of String, ResourceManager)
    Private idLanguage As Integer
    Private languageCode As String
    ''' <summary>
    ''' Current language code
    ''' </summary>
    ''' <param name="lId"></param>
    ''' <param name="lCode"></param>
    ''' <remarks></remarks>
    Sub New(lId As Integer, lCode As String)
        languageCode = lCode
        idLanguage = lId
    End Sub
    Sub New(language As iLanguage)
        languageCode = language.Code
        idLanguage = language.Id
    End Sub
    Protected Function GetResource(modulecode As String) As ResourceManager
        If _Resource.ContainsKey(modulecode) AndAlso Not IsNothing(_Resource(modulecode)) Then
            Return _Resource(modulecode)
        Else
            Select Case modulecode
                Case lm.Comol.Core.Tag.Domain.ModuleTags.UniqueCode, lm.Comol.Core.Dashboard.Domain.ModuleDashboard.UniqueCode
                    Dim dResource As New ResourceManager
                    dResource.UserLanguages = languageCode
                    dResource.ResourcesName = "pg_Dashboard"
                    dResource.Folder_Level1 = "Modules"
                    dResource.Folder_Level2 = "Dashboard"
                    dResource.setCulture()
                    _Resource(modulecode) = dResource
                    Return dResource

                Case Else
                    Return New ResourceManager
            End Select
        End If
    End Function

    Protected Friend Sub AnalyzeFiltersForTranslation(filters As List(Of lm.Comol.Core.DomainModel.Filters.Filter), filtermodule As String, filtermodulescope As String)
        Dim resource As ResourceManager = GetResource(filtermodule)
        For Each filter As lm.Comol.Core.DomainModel.Filters.Filter In filters
            Select Case filtermodule
                Case lm.Comol.Core.Dashboard.Domain.ModuleDashboard.UniqueCode
                    Select Case filtermodulescope
                        Case "Tile"
                            AnalyzeFiltersForModuleDashboardTilesTranslation(filter, resource)
                        Case Else
                            AnalyzeFiltersForModuleDashboardTranslation(filter, resource)
                    End Select
                Case lm.Comol.Core.Tag.Domain.ModuleTags.UniqueCode
                    AnalyzeFiltersForModuleTagsTranslation(filter, filtermodulescope, resource)
            End Select
        Next
    End Sub
    Private Sub AnalyzeFiltersForModuleDashboardTranslation(filter As lm.Comol.Core.DomainModel.Filters.Filter, resource As ResourceManager)
        Select Case filter.Name
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.communitytype.ToString
                Dim items As List(Of TranslatedItem(Of Integer)) = (From t As COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita In COL_BusinessLogic_v2.Comunita.COL_Tipo_Comunita.PlainLista(idLanguage, True) Order By t.Descrizione Select New TranslatedItem(Of Integer) With {.Id = t.ID, .Translation = t.Descrizione}).ToList()

                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("AllTypes")
                End If
                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("AllTypes")
                End If
                'items.Add(New TranslatedItem(Of Integer) With {.Id = -1, .Translation = Resource.getValue("")})

                filter.Label = resource.getValue("LBfilterCommunityTypes")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.degreetype.ToString
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("AllDegreetype")
                End If
                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("AllDegreetype")
                End If
                filter.Label = resource.getValue("LBfilterDegreeType")
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.coursetime.ToString
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("AllCourseTime")
                End If
                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("AllCourseTime")
                End If
                filter.Label = resource.getValue("LBfilterCourseTime")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.organization.ToString
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("AllOrganizations")
                End If
                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("AllOrganizations")
                End If
                filter.Label = resource.getValue("LBfilterOrganization")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.responsible.ToString
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("AllResponsible")
                End If
                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("AllResponsible")
                End If
                filter.Label = resource.getValue("LBfilterResponsible")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.status.ToString
                For Each item As lm.Comol.Core.DomainModel.Filters.FilterListItem In filter.Values
                    item.Name = resource.getValue("CommunityStatus." & DirectCast(CInt(item.Id), lm.Comol.Core.Communities.CommunityStatus).ToString)
                Next
                If Not IsNothing(filter.Selected) Then
                    filter.Selected.Name = resource.getValue("CommunityStatus." & DirectCast(CInt(filter.Selected.Id), lm.Comol.Core.Communities.CommunityStatus).ToString)
                End If
                filter.Label = resource.getValue("LBfilterCommunityStatus")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.year.ToString
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("AllYear")
                End If
                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("AllYear")
                End If
                filter.Label = resource.getValue("LBfilterAccademicYear")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.name.ToString
                filter.Label = resource.getValue("LBfilterName")

            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag.ToString
                filter.Label = resource.getValue("LBfilterTag")

                filter.Placeholder = resource.getValue("Tag-Data-Placeholder")
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.letters.ToString
                filter.Label = resource.getValue("LBfilterLetters")
                filter.GridSize = 12
                If Not IsNothing(filter.Values) Then
                    If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                        filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault().Name = resource.getValue("Letters.All")
                    End If
                    If filter.Values.Where(Function(v) v.Id = -9).Any() Then
                        filter.Values.Where(Function(v) v.Id = -9).FirstOrDefault().Name = resource.getValue("Letters.OtherChars")
                    End If
                End If
            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tagassociation.ToString
                filter.Values.FirstOrDefault.Name = resource.getValue("TagAssociationText")
                filter.Label = resource.getValue("LBfilterTagAssociation")
        End Select

    End Sub
    Private Sub AnalyzeFiltersForModuleDashboardTilesTranslation(filter As lm.Comol.Core.DomainModel.Filters.Filter, resource As ResourceManager)
        Select Case filter.Name
            Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.modifiedby.ToString
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("Tiles.AllModifiedBy")
                End If

                If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                    filter.Selected.Name = resource.getValue("Tiles.AllModifiedBy")
                End If

                For Each item As Filters.FilterListItem In filter.Values.Where(Function(v) String.IsNullOrEmpty(v.Name))
                    item.Name = resource.getValue("UnknownUserName") & item.Id.ToString()
                Next

                filter.Label = resource.getValue("LBfilter.Tiles.modifiedby")
            Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.name.ToString
                filter.Label = resource.getValue("LBfilterName")
            Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.status.ToString
                For Each item As lm.Comol.Core.DomainModel.Filters.FilterListItem In filter.Values
                    item.Name = resource.getValue("TagStatus." & DirectCast(CInt(item.Id), lm.Comol.Core.Dashboard.Domain.AvailableStatus).ToString)
                Next
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("Tiles.AllStatus")
                End If
                If Not IsNothing(filter.Selected) Then
                    Select Case filter.Selected.Id
                        Case -1
                            filter.Selected.Name = resource.getValue("Tiles.AllStatus")
                        Case Else
                            filter.Selected.Name = resource.getValue("AvailableStatus." & DirectCast(CInt(filter.Selected.Id), lm.Comol.Core.Dashboard.Domain.AvailableStatus).ToString)
                    End Select

                End If
                filter.Label = resource.getValue("LBfilter.Tiles.status")

            Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.type.ToString
                For Each item As lm.Comol.Core.DomainModel.Filters.FilterListItem In filter.Values
                    item.Name = resource.getValue("TileType." & DirectCast(CInt(item.Id), lm.Comol.Core.Dashboard.Domain.TileType).ToString)
                Next
                If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                    filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("Tiles.AllTypes")
                End If
                If Not IsNothing(filter.Selected) Then
                    Select Case filter.Selected.Id
                        Case -1
                            filter.Selected.Name = resource.getValue("Tiles.AllTypes")
                        Case Else
                            filter.Selected.Name = resource.getValue("TileType." & DirectCast(CInt(filter.Selected.Id), lm.Comol.Core.Dashboard.Domain.TileType).ToString)
                    End Select

                End If
                filter.Label = resource.getValue("LBfilter.Tiles.types")
            Case lm.Comol.Core.BaseModules.Tiles.Domain.searchFilterType.letters.ToString
                filter.Label = resource.getValue("LBfilterLetters")
                filter.GridSize = 12
                If Not IsNothing(filter.Values) Then
                    If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                        filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault().Name = resource.getValue("Letters.All")
                    End If
                    If filter.Values.Where(Function(v) v.Id = -9).Any() Then
                        filter.Values.Where(Function(v) v.Id = -9).FirstOrDefault().Name = resource.getValue("Letters.OtherChars")
                    End If
                End If
        End Select

    End Sub
    Private Sub AnalyzeFiltersForModuleTagsTranslation(filter As lm.Comol.Core.DomainModel.Filters.Filter, filtermodulescope As String, resource As ResourceManager)
        Select Case filtermodulescope
            Case "Community"
                Select Case filter.Name
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.organization.ToString
                        If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                            filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("Tags.AllOrganizations")
                        End If
                        If filter.Values.Where(Function(v) v.Id = -3).Any() Then
                            filter.Values.Where(Function(v) v.Id = -3).FirstOrDefault.Name = resource.getValue("Tags.System")
                        End If
                        If Not IsNothing(filter.Selected) Then
                            Select Case filter.Selected.Id
                                Case -1
                                    filter.Selected.Name = resource.getValue("Tags.AllOrganizations")
                                Case -3
                                    filter.Selected.Name = resource.getValue("Tags.System")
                            End Select
                        End If
                        filter.Label = resource.getValue("LBfilter.Tags.organization")

                    Case lm.Comol.Core.Tag.Domain.searchFilterType.createdby.ToString
                        If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                            filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("Tags.AllCreatedBy")
                        End If

                        If Not IsNothing(filter.Selected) AndAlso filter.Selected.Id = -1 Then
                            filter.Selected.Name = resource.getValue("Tags.AllCreatedBy")
                        End If

                        For Each item As Filters.FilterListItem In filter.Values.Where(Function(v) String.IsNullOrEmpty(v.Name))
                            item.Name = resource.getValue("UnknownUserName") & item.Id.ToString()
                        Next

                        filter.Label = resource.getValue("LBfilter.Tags.createdby")
                    Case lm.Comol.Core.Tag.Domain.searchFilterType.status.ToString
                        For Each item As lm.Comol.Core.DomainModel.Filters.FilterListItem In filter.Values
                            item.Name = resource.getValue("TagStatus." & DirectCast(CInt(item.Id), lm.Comol.Core.Dashboard.Domain.AvailableStatus).ToString)
                        Next
                        If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                            filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault.Name = resource.getValue("Tags.AllStatus")
                        End If
                        If Not IsNothing(filter.Selected) Then
                            Select Case filter.Selected.Id
                                Case -1
                                    filter.Selected.Name = resource.getValue("Tags.AllStatus")
                                Case Else
                                    filter.Selected.Name = resource.getValue("TagStatus." & DirectCast(CInt(filter.Selected.Id), lm.Comol.Core.Dashboard.Domain.AvailableStatus).ToString)
                            End Select

                        End If
                        filter.Label = resource.getValue("LBfilter.Tags.status")

                    Case lm.Comol.Core.Tag.Domain.searchFilterType.name.ToString
                        filter.Label = resource.getValue("LBfilter.Tags.name")

                    Case lm.Comol.Core.Tag.Domain.searchFilterType.letters.ToString
                        filter.Label = resource.getValue("LBfilter.Tags.letters")
                        filter.GridSize = 12
                        If Not IsNothing(filter.Values) Then
                            If filter.Values.Where(Function(v) v.Id = -1).Any() Then
                                filter.Values.Where(Function(v) v.Id = -1).FirstOrDefault().Name = resource.getValue("Letters.All")
                            End If
                            If filter.Values.Where(Function(v) v.Id = -9).Any() Then
                                filter.Values.Where(Function(v) v.Id = -9).FirstOrDefault().Name = resource.getValue("Letters.OtherChars")
                            End If
                        End If
                End Select
        End Select
    End Sub
End Class
