<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ManagePendingRequests.aspx.cs" Inherits="SSISApplication.Views.DeptHeadViews.ManagePendingRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Manage Pending Requests</h2>
    </div>
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_PendingRequests" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="grdView_PendingRequests_SelectedIndexChanged" OnRowDataBound="grdView_PendingRequests_RowDataBound1" BackColor="White"  BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-responsive table-striped table-hover">
                <Columns>
                    <asp:BoundField DataField="UserRequestID" HeaderText="User Request ID" />
                    <asp:BoundField DataField="PersonName" HeaderText="Employee Name" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <br />
    <asp:GridView ID="grdView_RequestDetails" runat="server" AutoGenerateColumns="false" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-responsive table-striped table-hover">
        <%--<FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
        <RowStyle BackColor="White" ForeColor="#003399" />
        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
        <SortedAscendingCellStyle BackColor="#EDF6F6" />
        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
        <SortedDescendingCellStyle BackColor="#D6DFDF" />
        <SortedDescendingHeaderStyle BackColor="#002876" />--%>
        <Columns>
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" />
                    <asp:BoundField DataField="UserRequestID" HeaderText="User Request ID" />
                    <asp:BoundField DataField="PersonName" HeaderText="Employee Name" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Stationery Type" />
                    <asp:BoundField DataField="ItemName" HeaderText="Item" />
                    <asp:BoundField DataField="UnitOfMeasure" HeaderText="UOM" />
                    <asp:BoundField DataField="RequestQuantity" HeaderText="Quantity" />
       </Columns>
    </asp:GridView>
    <asp:Label ID="lbl_Reason" runat="server" Text="Reasons for rejection:"></asp:Label>
    <br />
    <div class="row">
        <div class="col-md-6">
            <asp:TextBox ID="txtbox_RejectComments" runat="server" Height="83px" TextMode="MultiLine" ToolTip="Enter reasons for rejection." Width="347px"></asp:TextBox>
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6" style="left: -252px; top: -8px; margin-left: 1120px">
            <asp:Button ID="btn_Reject" runat="server" Text="Reject" OnClick="btn_Reject_Click" BackColor="#FF6600" Font-Bold="True" ForeColor="White" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_Approve" runat="server" Text="Approve" OnClick="btn_Approve_Click" BackColor="#3366FF" Font-Bold="True" ForeColor="White" />
        </div>
    </div>
</asp:Content>