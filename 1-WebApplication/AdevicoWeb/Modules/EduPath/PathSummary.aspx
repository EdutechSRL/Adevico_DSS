<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/AjaxPortal.Master" CodeBehind="PathSummary.aspx.vb" Inherits="Comunita_OnLine.PathSummary" %>
<%@ MasterType VirtualPath="~/AjaxPortal.Master" %>
<%@ Register TagPrefix="CTRL" TagName="GridPager" Src="~/UC/UC_PagerControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPHserviceLocalization" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<script src="../../Scripts/jquery.anchor.js" type="text/javascript"></script>
  <script type="text/javascript" src="<%#ResolveUrl("~/Jscript/FileDownloadCookie.js")%>"></script>
  
  <link href="../../Content/jquery.treeTable/jquery.treeTable.css" type="text/css" rel="Stylesheet" />
  <script src="../../Scripts/jquery.treeTable.js" type="text/javascript"></script>

  <link href="../../Graphics/Modules/Edupath/css/edupath.css" rel="Stylesheet" />
  <script type="text/javascript">
      $(function () {
          $(".treetable").treeTable({ clickableNodeNames: true });

          $(".clearIt").click(function () {
              // $(".clearMe input").val("");
              $(this).parents(".fieldrow").find("input").val("");
          });
          //$("tr").anchor();
      });
  </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleContent" runat="server">

<asp:Label runat="server" ID="LBservice">**Path Summary</asp:Label> - <asp:Label runat="server" ID="LBserviceCommunity">**Path Summary</asp:Label>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPHservice" runat="server">
<asp:MultiView runat="server" ID="MLVpathsummary">
    <asp:View runat="server" ID="VIWpathsummary">
        <div class="pathsummary">
            <div class="DivEpButton">
                <asp:HyperLink ID="HYPlistEduPath" runat="server" Text="**list edu path" CssClass="Link_Menu"></asp:HyperLink>
            </div>
            <div class="fieldobject filters">
                <div class="fieldrow">
                    
                    <asp:Label runat="server" ID="LBuserfilterTitle" CssClass="fieldlabel">**User</asp:Label>                                        
                    <asp:TextBox runat="server" ID="TXBuserFilter"></asp:TextBox><a runat="server" id="Aclearit" class="clearIt Link_Menu">**cancella</a>
                </div>
                <div class="fieldrow">                    
                    <asp:Label runat="server" ID="LBcommunityRoleTitle" CssClass="fieldlabel">**Community Role</asp:Label>
                    <asp:DropDownList runat="server" ID="DDLroleFilter" />                    
                </div>
                <div class="fieldrow">
                    <asp:CheckBox runat="server" ID="CHBshowall" Visible="true" Text="Show hidden" Checked="false" />
                </div>
                <div class="fieldrow">                    
                    <asp:Button runat="server" ID="BTNupdateFilter" Text="**Update Filter" CssClass="linkMenu" />
                </div>
            </div>   
            <div class="fieldobject legend">
                <div class="fieldrow">
                    <asp:Label runat="server" ID="LBlegend" CssClass="fieldlabel">**Legend</asp:Label>
                    <span class="status" >
                        <span class="statusitem">
                            <asp:label CssClass="gray" runat="server" ID="LBnotstarted">&nbsp;</asp:label>                        
                            <asp:Label CssClass="label" runat="server" ID="LBnotstartedlabel">**non iniziati</asp:Label>
                        </span>
                        <span class="statusitem">
                            <asp:label CssClass="yellow" runat="server" ID="LBstarted">&nbsp;</asp:label>
                            <asp:Label CssClass="label" runat="server" ID="LBstartedlabel">**iniziati</asp:Label>
                        </span>
                        <span class="statusitem">
                            <asp:Label CssClass="green" runat="server" ID="LBcompleted">&nbsp;</asp:Label>   
                            <asp:Label CssClass="label" runat="server" ID="LBcompletedlabel">**completati</asp:Label>
                        </span>
                   </span>
               </div>
            </div>
            <div class="fieldobject info" runat="server" id="DIVinfo" visible="false">
                <div class="fieldrow">
                    <asp:Label runat="server" ID="LBpathNameTitle" CssClass="fieldlabel">**Path Name</asp:Label>
                    <asp:Label runat="server" ID="LBpathName">**Path</asp:Label>
                </div>
            </div>
            <div class="pager top" runat="server" id="DVpagerTop">
                <asp:literal ID="LTpageTop" runat="server">**Go to page: </asp:literal><CTRL:GridPager ID="PGgridTop" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            <asp:Repeater runat="server" ID="RPTusers">
                <HeaderTemplate>
                   <table class="treetable table light">
                    <thead>
                        <tr>
                            <th class="name"><asp:Label runat="server" ID="LBnameheader">**User name / Path name</asp:Label></th>
                            <th class="status"><asp:Label runat="server" ID="LBstatusheader">**Status</asp:Label></th>
                            <th class="completion"><asp:Label runat="server" ID="LBcompletionheader">**Completion</asp:Label></th>
                            <th class="startdate"><asp:Label runat="server" ID="LBstartdateheader">**StartDate</asp:Label></th>
                            <th class="enddate"><asp:Label runat="server" ID="LBenddateheader">**EndDate</asp:Label></th>
                            <th class="deadline"><asp:Label runat="server" ID="LBdeadlineheader">**Deadline</asp:Label></th>
                            <th class="actions"><asp:Label runat="server" ID="LBactionheader">**Actions</asp:Label></th>
                        </tr>
                    </thead> 
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>

                    <tr class="user <%#Me.Zero(Container.Dataitem.Total) %><%#Me.ExpandIfOnlyOne(Container.Dataitem.Total) %> <%#Me.ExpandMe(Container.Dataitem.Person.Id) %>" id="user-<%# Container.Dataitem.Person.Id %>">                        
                        <td class="name"><a name="u<%#Container.Dataitem.Person.Id %>"></a><asp:Label runat="server" ID="LBusername">**Username</asp:Label></td>
                        <td class="status">
                          <asp:label CssClass="gray" runat="server" ID="LBnotstarted">**0</asp:label>
                          <asp:label CssClass="yellow" runat="server" ID="LBstarted">**0</asp:label>
                          <asp:Label CssClass="green" runat="server" ID="LBcompleted">**0</asp:Label>                          
                        </td>
                        <td class="completion"><asp:Label runat="server" ID="LBcompletion">&nbsp;</asp:Label></td>
                        <td class="startdate"><asp:Label runat="server" ID="LBstartdate">&nbsp;</asp:Label></td>
                        <td class="enddate"><asp:Label runat="server" ID="LBenddate">&nbsp;</asp:Label></td>
                        <td class="deadline"><asp:Label runat="server" ID="LBdeadline">&nbsp;</asp:Label></td>
                        <td class="actions">
                            <span class="icons">
                                <%--<span class="icon infoalt"></span>--%>
                                <asp:HyperLink runat="server" ID="HYPinfo" CssClass="icon infoalt"></asp:HyperLink>
                                <asp:HyperLink runat="server" ID="HYPstats" CssClass="icon stats" Visible="false"></asp:HyperLink>
                            </span>
                        </td>
                    </tr>                    
                    <asp:Repeater runat="server" ID="RPTpaths">
                      <ItemTemplate>
                      <tr class="path child-of-user-<%# Container.Dataitem.IdPerson %>">
                        <td class="name"><a name="u<%# Container.Dataitem.IdPerson %>p<%# Container.Dataitem.IdPath %>"></a><asp:Label runat="server" ID="LBpathname">**PathName</asp:Label></td>
                        <td class="status">
                            <span class="big <%# Me.Status(Container.Dataitem.Status) %>" title="<%# Me.StatusTitle(Container.Dataitem.Status) %>">&nbsp;</span>
                        </td>
                        <td class="completion"><asp:Label runat="server" ID="LBcompletion">**</asp:Label></td>
                        <td class="startdate"><asp:Label runat="server" ID="LBstartdate">**---</asp:Label></td>
                        <td class="enddate"><asp:Label runat="server" ID="LBenddate">**---</asp:Label></td>
                        <td class="deadline"><asp:Label runat="server" ID="LBdeadline">**---</asp:Label></td>
                        <td class="actions">
                            <span class="icons">
                                <%--<span class="icon stats"></span>--%>
                                <asp:HyperLink runat="server" ID="HYPstats" CssClass="icon stats"></asp:HyperLink>
                                <asp:HyperLink runat="server" ID="HYPcertificates" CssClass="icon certificate"></asp:HyperLink>
                            </span>
                        </td>
                      </tr>
                      </ItemTemplate>
                    </asp:Repeater>

                </ItemTemplate>                
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div class="pager bottom" runat="server" id="DVpagerBottom">
                <asp:literal ID="LTpageBottom" runat="server">**Go to page: </asp:literal><CTRL:GridPager ID="PGgridBottom" runat="server" EnableQueryString="false"></CTRL:GridPager>
            </div>
            
            <%--<table class="treetable table light">
            <thead>
                <tr>
                    <th class="name">*User name / Path name</th>
                    <th class="status">*Status</th>
                    <th class="completion">*Completion</th>
                    <th class="startdate">*StartDate</th>
                    <th class="enddate">*EndDate</th>
                    <th class="deadline">*Deadline</th>
                    <th class="actions">*Actions</th>
                </tr>
            </thead>
            <tbody>
   <tr id="user-4" class="user">
             <td class="name">Nome utente piuttosto lunghetto anche troppo</td>
              <td class="status">
                  <span class="gray">1</span>
                  <span class="yellow">2</span>
                  <span class="green">3</span>
              </td>
            <td class="completion"></td>
       <td class="startdate"></td>
           <td class="enddate"></td>
       <td class="deadline"></td>
       <td class="actions"><span class="icons">
                 <span class="icon info"></span>
             </span></td>
          </tr>
       <tr id="user-4-ep-1" class="path child-of-user-4">
             <td class="name">Percorso formativo delle segretarie d'azienda riformate
             </td>
              <td class="status">
                  <span class="big gray">&nbsp;</span>
              </td>
           <td class="completion">8h [min 10h] / 12h</td>
           <td class="startdate"></td>
           <td class="enddate"></td>
           <td class="deadline">20giorni</td>
           <td class="actions">
               <span class="icons">
                 <span class="icon stats"></span>
             </span>
           </td>
          </tr>
   <tr id="user-4-ep-2" class="path child-of-user-4">
             <td class="name">Percorso formativo delle segretarie d'azienda riformate

             </td>
              <td class="status">
                  <span class="big yellow">&nbsp;</span>
              </td>
           <td class="completion">8h [min 10h] / 12h</td>
       <td class="startdate"></td>
       <td class="enddate"></td>
           <td class="deadline">20giorni</td>
       <td class="actions">
               <span class="icons">
                 <span class="icon stats"></span>
             </span>
           </td>
          </tr>
       <tr id="user-4-ep-3" class="path child-of-user-4">
             <td class="name">Percorso formativo delle segretarie d'azienda riformate

             </td>
              <td class="status">
                  <span class="big yellow">&nbsp;</span>
              </td>
           <td class="completion">8h [min 10h] / 12h</td>
           <td class="startdate"></td>
           <td class="enddate"></td>
           <td class="deadline">20giorni</td>
           <td class="actions">
           <span class="icons">
                 <span class="icon stats"></span>
             </span>
           </td>
          </tr>
       <tr id="user-4-ep-4" class="path child-of-user-4">
             <td class="name">Percorso formativo delle segretarie d'azienda riformate

             </td>
              <td class="status">
                  <span class="big green">&nbsp;</span>
              </td>
           <td class="completion">8h [min 10h] / 12h</td>
           <td class="startdate"></td>
           <td class="enddate"></td>
           <td class="deadline">20giorni</td>
           <td class="actions">
           <span class="icons">
                 <span class="icon stats"></span>
             </span>
           </td>
          </tr>
   <tr id="user-4-ep-5" class="path child-of-user-4">
             <td class="name">Percorso formativo delle segretarie d'azienda riformate

             </td>
              <td class="status">
                  <span class="big green">&nbsp;</span>
              </td>
           <td class="completion">8h [min 10h] / 12h</td>
       <td class="startdate"></td>
       <td class="enddate"></td>
           <td class="deadline">20giorni</td>
       <td class="actions">
       <span class="icons">
                 <span class="icon stats"></span>
             </span>
       </td>
          </tr>
   <tr id="user-4-ep-6" class="path child-of-user-4">
             <td class="name">Percorso formativo delle segretarie d'azienda riformate
             </td>
              <td class="status">
                  <span class="big green">&nbsp;</span>
              </td>
           <td class="completion">8h [min 10h] / 12h</td>
       <td class="startdate"></td>
       <td class="enddate"></td>
           <td class="deadline">20giorni</td>
       <td class="actions">
       <span class="icons">
                 <span class="icon stats"></span>
             </span>
       </td>
          </tr>
   </tbody>
    <tfoot></tfoot>
</table>--%>
        </div>
    </asp:View>
    <asp:View runat="server" ID="VIWerror">
        <div id="DVerror" align="center">
                <div class="DivEpButton">
                    <asp:HyperLink ID="HYPerror" runat="server" CssClass="Link_Menu" />
                </div>
                <div align="center">
                    <asp:Label ID="LBerror" runat="server" CssClass="messaggio">**error</asp:Label>
                </div>
            </div>
    </asp:View>
</asp:MultiView>

</asp:Content>
