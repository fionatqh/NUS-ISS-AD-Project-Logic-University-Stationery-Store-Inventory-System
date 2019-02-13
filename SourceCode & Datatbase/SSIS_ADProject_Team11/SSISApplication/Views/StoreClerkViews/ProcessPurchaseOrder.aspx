<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessPurchaseOrder.aspx.cs" Inherits="SSISApplication.Views.StoreClerkViews.ProcessPurchaseOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron"><br /><br /><br /><br />
        <h2>Purchase order</h2>
        <p class="lead">Make Purchase Order for low stock inventories according to  re-order level</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_lowStockItems" runat="server" AutoGenerateColumns="false" OnRowDataBound="RowDataBound" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
                <EmptyDataTemplate>
                    <div style="text-align: center">No records found.</div>
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                    <asp:BoundField DataField="UnitOfMeasure" HeaderText="UOM" />
                    <asp:BoundField DataField="ReorderLevel" HeaderText="Reorder Lvl" />
                    <asp:BoundField DataField="ReorderQuantity" HeaderText="Total Qty" />
                    <asp:BoundField DataField="Quantity" HeaderText="Current Qty" />
                    <asp:TemplateField HeaderText="Supplier">
                        <ItemTemplate>
                            <asp:PlaceHolder ID="placeholder1" runat="server"></asp:PlaceHolder>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <div>
        <asp:Button ID="btn_Order" runat="server" Text="Order" OnClick="btn_Order_Click" />
    </div>
</asp:Content>
