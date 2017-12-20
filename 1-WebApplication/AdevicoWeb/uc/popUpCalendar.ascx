<%@ Control Language="vb" AutoEventWireup="false" Codebehind="popUpCalendar.ascx.vb" Inherits="Comunita_OnLine.popUpCalendar" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
 <asp:panel id="pnlCalendar" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px" runat="server" Height="86px" Width="145px">

  <asp:Calendar id="Calendar1" runat="server" Height="86" Width="145" FirstDayOfWeek=Monday 
  BackColor="White" BorderColor="Black" BorderStyle="Solid"
  NextMonthText="<IMG src='./images/monthright.gif' border='0'>"
  PrevMonthText="<IMG src='./images/monthleft.gif' border='0'>">
     <TodayDayStyle BackColor="#FFFFC0"></TodayDayStyle>
     <DayStyle Font-Size="8pt" Font-Names="Arial"></DayStyle>
     <DayHeaderStyle Font-Size="10pt" Font-Underline="True" Font-Names="Arial"
     BorderStyle="None" BackColor="#E0E0E0"></DayHeaderStyle>
     <SelectedDayStyle Font-Size="8pt" Font-Names="Arial" Font-Bold="True"
     ForeColor="White" BackColor="Navy"></SelectedDayStyle>
     <TitleStyle Font-Size="10pt" Font-Names="Arial" Font-Bold="True"
     ForeColor="White" BackColor="Navy"></TitleStyle>
     <OtherMonthDayStyle ForeColor="Gray"></OtherMonthDayStyle>
  </asp:Calendar>

</asp:panel>