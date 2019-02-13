<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ViewApprovedRequests.aspx.cs" Inherits="SSISApplication.Views.DeptHeadViews.ViewApprovedRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h3>Approved Requests History</h3>
    </div>

    <div class="row">
        <div class="col-md-6">
            <br />
            <br />
            <asp:GridView ID="grdView_ViewApprovedRequests" runat="server" AutoGenerateColumns="false" BackColor="White"  BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-responsive table-striped table-hover" OnRowDataBound="grdView_ViewApprovedRequests_RowDataBound1" OnSelectedIndexChanged="grdView_ViewApprovedRequests_SelectedIndexChanged">
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
             
            <br />
            <asp:GridView ID="grdview_ViewApprovedRequestDetails" runat="server" AutoGenerateColumns="false" AllowPaging="True"
              AllowSorting="False" OnPageIndexChanging="grdView_ViewApprovedRequests_PageIndexChanging" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-responsive table-striped table-hover">
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
            <br />
        </div>
    </div>

</asp:Content>
