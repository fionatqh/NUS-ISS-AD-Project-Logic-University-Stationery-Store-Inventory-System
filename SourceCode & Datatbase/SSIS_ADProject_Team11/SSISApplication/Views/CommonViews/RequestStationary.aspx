<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequestStationary.aspx.cs" Inherits="SSISApplication.Views.CommonViews.RequestStationary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2> Request Stationery</h2>
        </div>
    <div class="row">
        <div class="col-md-6" >
            <asp:GridView runat="server" ID="Gridview_GetInventory" AutoGenerateColumns="False" RowDataBound="Gridview_GetInventory_RowDataBound" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
               
                <Columns>
                    <asp:TemplateField HeaderText="Inventoryid" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbl_InventoryID" Text='<%# Bind("InventoryID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Item Name " DataField="ItemName"></asp:BoundField>
                    <asp:BoundField HeaderText="Unit Of Measure " DataField="UnitOfMeasure"></asp:BoundField>
                    <asp:TemplateField HeaderText="price">
                        <ItemTemplate>
                            <asp:Label runat="server" ItemStyle-Width="15%" ID="lbl_price" Text='<%#Bind("Price") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity" ConvertEmptyStringToNull="False">
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="100%" ItemStyle-Width="15%" ID="txt_quantity" AutoPostBack="false"  ></asp:TextBox>
                     
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Amount">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_TotalAmount" ItemStyle-Width="15%"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
          <div class="row">
        <div class="col-md-4">
          
                        <asp:Button ID="btn_gettotals" runat="server" Text="Get totals" OnClick="GetTotals_Click" />

                       <%-- <asp:Label ID="lbl_totalamount" runat="server" />--%>
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                   
                        <asp:Button ID="btn_RequestButton" runat="server" Text="Request stationery" OnClick="RequestButton_Click" />
                   
        </div>
    </div>
    <%-- <script type="text/javascript">
        function Showalert() {
            alert('No item is selected to request stationary.Inorder to place stationery order enter quantity');
        }
</script>--%>
    <asp:Label ID="lbl_Comment" Visible="false" runat="server" />
</asp:Content>
