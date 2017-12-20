<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RevisioneArgomento.aspx.vb" Inherits="Comunita_OnLine.RevisioneArgomento"%>
<!DOCTYPE HTML PUBLIC "-//W3C//Dtd HTML 4.0 transitional//EN">
<HTML>
	<head runat="server">
		<title>Comunità On Line - Forum</title>
		<script type="text/javascript" language="Javascript" src="./../jscript/generali.js"></script>
		<meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
		<meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		 <LINK href="../Styles.css" type="text/css" rel="stylesheet"/>
        <link href="../Graphics/Generics/css/4_UI_Elements.css" rel="Stylesheet" />
        <asp:Literal ID="Lit_Skin" runat="server"></asp:Literal>

        <link href="../Graphics/Modules/Forum/forum.new.css" rel="Stylesheet" />
        
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	</HEAD>
	<body>
		 <form id="aspnetForm" runat="server">
		 <asp:ScriptManager ID="SCMmanager" runat="server"></asp:ScriptManager>
			<asp:Panel ID="PNLcontenuto" Runat="server" HorizontalAlign="Center" >
                <div class="tablewrapper">
			<!--	<table align =center width="800px" border=1 cellpadding=0 cellspacing=0  bordercolor=#9C9A9C>
					<tr>
						<td align =center width="800px">-->
							<asp:DataGrid 
							    ID="DGPost" Runat="server"
							    DataKeyField="POST_Id" 
							    AutoGenerateColumns="False"
							    CellPadding="0"
							    CellSpacing="0"
							    BorderStyle="None"
							    GridLines="None"
							    CssClass="datagridforum"
                                ShowFooter="true"
                                >
								<ItemStyle CssClass="ForumNW_RowNormal"></ItemStyle>
								<AlternatingItemStyle CssClass="ForumNW_RowAlternato"></AlternatingItemStyle>
								<Columns>
									<asp:TemplateColumn>
										<HeaderTemplate>			
											<table class="table light fullwidth forumtable posts review">
                                                <tr class="tableheader">
													<th class="author">
														<asp:label ID="LBautore_t" Runat= server >Author</asp:label>
													</th>
													<th class="message" >
														<asp:label ID="LBmessaggio_t" Runat= server >Message</asp:Label>
													</th>
												</tr>											
										</HeaderTemplate>
										<ItemTemplate>											
												<tr class="tablerow <%# DataBinder.Eval(Container.DataItem, "stile") %>" >
													<td class="author <%# DataBinder.Eval(Container.DataItem, "stile2") %>">

                                                        <div class="authorcontent">
                                                            <div class="linewrapper name">
                                                                <asp:Label ID="LBautore" Runat=server CssClass="ForumNW_Autore"><%# DataBinder.Eval(Container.DataItem, "PRSN_Anagrafica") %></asp:Label>
                                                            </div>
                                                            <div class="linewrapper role">
                                                                <asp:Label ID="LBruolo" Runat=server CssClass="ForumNW_Ruolo"><%# DataBinder.Eval(Container.DataItem, "TPRF_nome") %></asp:Label>
                                                            </div>
                                                            <div class="linewrapper image">
                                                                <asp:Image ID="IMGprofilo" Runat=server Width="80px" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "PRSN_FotoPath") %>'></asp:Image>
                                                            </div>
                                                            <div class="linewrapper stats">
                                                                <asp:Label ID="LBtotalePost_t" Runat=server CssClass="ForumNW_Ruolo">Post inseriti:</asp:Label>&nbsp;
																	<span Class="ForumNW_Ruolo"><%# DataBinder.Eval(Container.DataItem, "TotalePost") %></span>
                                                            </div>
                                                        </div>

														
													</td>
													
													
													<td class="message <%# DataBinder.Eval(Container.DataItem, "stile2") %>">
                                                        <div class="messagecontent">
                                                                    <div class="header">
                                                                        <div class="linewrapper title clearfix">
                                                                            <span class="name">
                                                                                <span class="title">
                                                                            <asp:Image ID="IMGmessaggio" Runat=server ImageAlign=Middle BorderStyle=None ></asp:Image>
																	<a name="post_<%# DataBinder.Eval(Container.DataItem, "POST_ID") %>">
																		<asp:Label id="LBtitoloMessaggio" Runat="server" CssClass="ForumNW_SubjectPost"><%# DataBinder.Eval(Container.DataItem, "POST_Subject") %></asp:Label>
																	</a>
                                                                                    </span>
                                                                                <span class="separator">|</span>                                                                                    
                                                                                <span class="details">
																	<asp:Label id="LBdataPost" Runat="server" CssClass="ForumNW_datapost"></asp:Label>
																	<asp:Label id="LBdataPostModificato" Runat="server" CssClass="ForumNW_datapost"></asp:Label>
																	&nbsp;<asp:Label id="LBapprovato" Runat="server"></asp:Label>
                                                                                    </span>
                                                                            </span>
                                                                        <span class="mark">
                                                                            &nbsp;
                                                                        </span>
                                                                        </div>
                                                                        
                                                                    </div>
                                                                    <div class="body">
                                                                        <div class="post">
                                                                            <asp:Label ID="LBmessaggio" Runat=server CssClass="Testo_campoSmall"><%# DataBinder.Eval(Container.DataItem, "POST_Body") %></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="footer">

                                                                    </div>
                                                                </div>

														
													</td>
												</tr>	
												<tr class="tablerow separator" >
													<td colspan=2 class="ForumNW_separatore">
														&nbsp;
													</td>
												</tr>											
										</ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
									</asp:TemplateColumn>
									<asp:BoundColumn DataField="POST_Approved" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="stile" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="stile2" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="TPRF_nome" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="TotalePost" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="POST_Approved" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="Post_parentId" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="POST_IdRuolo" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="POST_PRSN_Id" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="POST_Id" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="POST_Body" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="FRIM_id" Visible=False ></asp:BoundColumn>
									<asp:BoundColumn DataField="posizione" Visible=False ></asp:BoundColumn>
								</Columns>
							</asp:DataGrid>
				<!--		</td>
					</tr>
				</table>-->
                </div>
			</asp:Panel>
		</form>
	</body>
</HTML>
