<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ViewRejectedRequests.aspx.cs" Inherits="SSISApplication.Views.DeptHeadViews.ViewRejectedRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Rejected Requests History</h2>
    </div>

    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_ViewRejectedRequests" runat="server" AutoGenerateColumns="false" BackColor="White"  BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-responsive table-striped table-hover" OnRowDataBound="grdView_ViewRejectedRequests_RowDataBound1" OnSelectedIndexChanged="grdView_ViewRejectedRequests_SelectedIndexChanged">
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
                    <asp:BoundField DataField="UserRequestID" HeaderText="User Request ID" />
                    <asp:BoundField DataField="PersonName" HeaderText="Name" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" />
                </Columns>
            </asp:GridView>
        </div>
        <asp:GridView ID="grdview_ViewRejectedRequestDetails" runat="server" AutoGenerateColumns="false" AllowPaging="True" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-responsive table-striped table-hover" AllowSorting="False" OnPageIndexChanging="grdView_ViewRejectedRequests_PageIndexChanging">
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
    </div>
</asp:Content>
