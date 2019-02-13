<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ManageDeliveryOrder.aspx.cs" Inherits="SSISWebSiteApplication.StoreClerkViews.ManageDeliveryOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Delivery Order List</h2>
        
    </div>

    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_Delivery" runat="server" OnSelectedIndexChanged="grdView_Delivery_SelectedIndexChanged" OnRowDataBound="grdView_Delivery_RowDataBound1" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
            </asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <div>
        <asp:SqlDataSource ID="SSIS" runat="server" ConnectionString="<%$ ConnectionStrings:SSISDbModelContext %>" SelectCommand="SELECT [PurchaseOrderID], [PurchaseOrderDate], [DeliveryStatus], [DeliveryDate] FROM [PurchaseOrder]"></asp:SqlDataSource>
        <asp:GridView ID="grdview_DeliveryOrderDetail" runat="server">
        </asp:GridView>
    </div>
</asp:Content>
