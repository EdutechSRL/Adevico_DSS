<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UC_Filters.ascx.vb" Inherits="Comunita_OnLine.UC_Filters" %>
 <div class="filter grid_{{filter.GridSize}} {{filter.Type}} {{filter.CssClass}}" ng-class="{autoupdate: filter.AutoUpdate}" ng-repeat="filter in filters" >
    <ng-include src="'<%=GetBaseUrl()%>filters/filter-'+filter.Type+'.html'"> </ng-include>
</div>