<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewInventoryStatus.aspx.cs" Inherits="SSISApplication.Views.StoreClerkViews.ViewInventoryStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br/><br /><br /><br/>
    <div class="jumbotron">
        <h2>View Inventory Status</h2>
        <p class="lead"> Inventories available on catalog and its status </p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_inventory" runat="server" OnRowDataBound="grdView_inventory_RowDataBound" AutoGenerateColumns="True"
                EmptyDataText="No data available." CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
            </asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <div>
         <asp:Button ID="btn_PurchaseOrder" runat="server" OnClick="btn_PurchaseOrder_Click" Text="Purchase Order" />
    </div>

</asp:Content>
