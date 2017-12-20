using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Core.Communities;

namespace lm.Comol.Core.BaseModules.CommunityManagement
{
    [Serializable]
    public class dtoCommunitiesFilters
    {
        public virtual Boolean OnlyFromAvailableOrganizations { get; set; }
        public virtual List<int> AvailableIdOrganizations { get; set; }
        public virtual int IdOrganization { get; set; }
        public virtual int IdcommunityType { get; set; }
        public virtual int IdPerson { get; set; }
        public virtual CommunityStatus Status { get; set; }

        public virtual String Value { get; set; }
        public virtual String StartWith { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int PageIndex { get; set; }
        public virtual Boolean Ascending { get; set; }
        public virtual OrderCommunitiesBy OrderBy { get; set; }
        public virtual SearchCommunitiesBy SearchBy { get; set; }
        public virtual CommunityAvailability Availability { get; set; }
        public virtual Int32 IdResponsible { get; set; }
        public virtual List<lm.Comol.Core.DomainModel.dtoModulePermission> RequiredPermissions { get; set; }
         
        public virtual Int32 IdRemoveCommunityType { get; set; }

        public virtual String CourseCode { get; set; }
        public virtual int Year { get; set; }
        public virtual int IdDegreeType { get; set; }
        public virtual int IdCourseTime { get; set; }
        public virtual List<long> IdTags { get; set; }
        public virtual Boolean WithoutTags { get; set; }
        public virtual long IdTile { get; set; }

        public dtoCommunitiesFilters() {
            IdResponsible = -1;
            IdRemoveCommunityType = -1;
            Year = -1;
            IdDegreeType = -1;
            IdCourseTime = -1;
            OnlyFromAvailableOrganizations = false;
            AvailableIdOrganizations = new List<int>();
            IdTags = new List<long>();
            WithoutTags = false;
            RequiredPermissions = new List<lm.Comol.Core.DomainModel.dtoModulePermission>();
        }

        public dtoCommunitiesFilters(List<lm.Comol.Core.DomainModel.Filters.Filter> filters, lm.Comol.Core.BaseModules.CommunityManagement.CommunityAvailability availability, Int32 idCommunityType = -1, long idTile = -1) {
            //Common
            AvailableIdOrganizations = new List<int>();
            IdTags = new List<long>();
            WithoutTags = false;
            Availability = availability;
            OnlyFromAvailableOrganizations = false;
            RequiredPermissions = new List<lm.Comol.Core.DomainModel.dtoModulePermission>();
            IdRemoveCommunityType = -1;
            IdDegreeType = -1;
            IdCourseTime = -1;
            Year = -1;   
            if(filters==null){
                IdResponsible = -1;
                IdOrganization = -1;
                IdcommunityType = -1;
            }
            else{
                IdOrganization= (int)GetSingleValue(filters, Dashboard.Domain.searchFilterType.organization);
                IdcommunityType = (int)GetSingleValue(filters, Dashboard.Domain.searchFilterType.communitytype, idCommunityType);
                IdResponsible= (int)GetSingleValue(filters, Dashboard.Domain.searchFilterType.responsible);
                switch(IdcommunityType){
                    case (int)lm.Comol.Core.DomainModel.CommunityTypeStandard.Degree:
                        IdDegreeType= (int)GetSingleValue(filters, Dashboard.Domain.searchFilterType.degreetype);
                        break;
                    case (int)lm.Comol.Core.DomainModel.CommunityTypeStandard.UniversityCourse:
                        IdCourseTime= (int)GetSingleValue(filters, Dashboard.Domain.searchFilterType.coursetime);
                        Year = (int)GetSingleValue(filters, Dashboard.Domain.searchFilterType.year);
                        break;
                }
                Status= (CommunityStatus)GetSingleValue(filters, Dashboard.Domain.searchFilterType.status,(long) CommunityStatus.None);
            }

            Value = GetStringValue(filters, Dashboard.Domain.searchFilterType.name, "");
            if (!String.IsNullOrEmpty(Value))
                SearchBy = SearchCommunitiesBy.Contains;
            if (filters.Where(f => f.Name == lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tagassociation.ToString()).Any())
                WithoutTags = filters.Where(f => f.Name == lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tagassociation.ToString() && f.Values.Any() && f.Values.Where(v=> v.Checked).Any()).Any();
            if (idTile > 0)
                IdTile = idTile;
            if (filters.Where(f => f.Name == lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag.ToString()).Any())
            {
                lm.Comol.Core.DomainModel.Filters.Filter tagFilter = filters.Where(f => f.Name == lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag.ToString()).FirstOrDefault();
                if (tagFilter != null && tagFilter.SelectedIds != null)
                    IdTags = tagFilter.SelectedIds.Select(i => i.Id).ToList();

            }
            Status = (CommunityStatus)GetSingleValue(filters, Dashboard.Domain.searchFilterType.status, (long)CommunityStatus.None);
          
        //            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.tag
        //                If keys.Contains(item.ToString) Then
        //                    For Each idTag As String In Request.Form(item.ToString).Split(",")
        //                        .IdTags.Add(CLng(idTag))
        //                    Next
        //                Else
        //                    .IdTags = New List(Of Long)
        //                End If
        //            Case lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType.letters
        //                Dim charInt As Integer = CInt(Request.Form(item.ToString))
        //                Select Case charInt
        //                    Case -1
        //                        .StartWith = ""
        //                    Case -9
        //                        .StartWith = "#"
        //                    Case Else
        //                        .StartWith = Char.ConvertFromUtf32(charInt).ToLower()
        //                End Select
        //        End Select
        //    Next         
        }

        private long GetSingleValue(List<lm.Comol.Core.DomainModel.Filters.Filter> filters, lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType type, long defaultValue = -1)
        {
            lm.Comol.Core.DomainModel.Filters.Filter filter = filters.Where(f => f.Name == type.ToString()).FirstOrDefault();
            if (filter==null)
                return defaultValue;
            else
            {
                switch (filter.FilterType)
                {
                    case DomainModel.Filters.FilterType.Select:
                        if (filter.Selected != null)
                            return filter.Selected.Id;
                        break;
                }
            }
            return defaultValue;
        }

        private String GetStringValue(List<lm.Comol.Core.DomainModel.Filters.Filter> filters, lm.Comol.Core.BaseModules.Dashboard.Domain.searchFilterType type, String defaultValue = "")
        {
            lm.Comol.Core.DomainModel.Filters.Filter filter = filters.Where(f => f.Name == type.ToString()).FirstOrDefault();
            if (filter == null)
                return defaultValue;
            else
            {
                switch (filter.FilterType)
                {
                    case DomainModel.Filters.FilterType.Text:
                        return filter.Value;
                }
            }
            return defaultValue;
        }

    }
}