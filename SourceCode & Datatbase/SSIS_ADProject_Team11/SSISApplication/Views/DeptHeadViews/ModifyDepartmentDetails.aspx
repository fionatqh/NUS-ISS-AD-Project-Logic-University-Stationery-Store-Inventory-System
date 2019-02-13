<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyDepartmentDetails.aspx.cs" Inherits="SSISApplication.Views.DeptHeadViews.ModifyDepartmentDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Modify Department Details</h2>
    </div>
    <div class="row">
        <div class="col-md-4" style="left: 1px; top: 21px">
            <br />
            <asp:Label ID="lbl_DeptRepName" runat="server" Font-Bold="True" Text="Current Department Representative: "></asp:Label>
            <asp:Label ID="lbl_CurrentDeptRep" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lbl_SelectDepRep" runat="server" Text="Select department representative:"></asp:Label>
            <asp:DropDownList ID="ddl_Employee_Name" runat="server" ToolTip="Select an employee">
            </asp:DropDownList>
        </div>
        <br />
        <br />
        <div class="col-md-4" style="left: 1px; top: 21px">
            <br />
            <asp:Label ID="lbl_ColPtName" runat="server" Font-Bold="True" Text="Current Collection Point: "></asp:Label>
            <asp:Label ID="lbl_CurrentColPt" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lbl_SelectColPt" runat="server" Text="Select Collection Point: "></asp:Label>
            <asp:RadioButtonList ID="rbl_CollPt" runat="server" ToolTip="Select a collection point" CellSpacing="5" CellPadding="5">
            </asp:RadioButtonList>
        </div>
        <br />
        <br />
        <div class="col-md-4" style ="left: 1px; top: 21px">
            <br />
            <asp:Label ID="lbl_DeptHead" runat="server" Font-Bold="True" Text="Current Department Head: "></asp:Label>
            <asp:Label ID="lbl_CurrentDeptHead" runat="server" Text="Label"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lbl_Delegate" runat="server" Text="Delegate employee:"></asp:Label>
            <asp:DropDownList ID="ddl_Employee_Name2" runat="server" ToolTip="Select an employee">
            </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="lbl_StartDate" runat="server" Text="Start Date:"></asp:Label>
            <asp:TextBox ID="txt_StartDate" runat="server" width="150px" TextMode="DateTime" Tooltip="Enter start date in dd/MM/yyyy"></asp:TextBox>
            <asp:ImageButton ID="calendar" ImageUrl="~/images/calendar-solid.png" ImageAlign="Bottom"
                OnClick="Show_StartDate" runat="server" Height="15px" Width="15px" />
             <br />
            <asp:Calendar ID="cldr_StartDate" visible="false" runat="server" OnSelectionChanged="StartDate_Selected"></asp:Calendar>
            <br />
            <asp:Label ID="lbl_EndDate" runat="server" Text="End Date:"></asp:Label>
            <asp:TextBox ID="txt_EndDate" runat="server" width="150px" TextMode="DateTime" Tooltip="Enter end date in dd/MM/yyyy"></asp:TextBox>
            <asp:ImageButton ID="calendar1" ImageUrl="~/images/calendar-solid.png" ImageAlign="Bottom"
                OnClick="Show_EndDate" runat="server" Height="15px" Width="15px" />
            <asp:Calendar ID="cldr_EndDate" visible="false" runat="server" OnSelectionChanged="EndDate_Selected"></asp:Calendar>
            <br />
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-md-4" style="margin-left: 920px">
            <asp:Button ID="btn_SaveChanges" runat="server" OnClick="saveChanges_Click" Text="Save Changes" BackColor="#3366FF" Font-Bold="True" ForeColor="White" />
        </div>
    </div>
</asp:Content>
