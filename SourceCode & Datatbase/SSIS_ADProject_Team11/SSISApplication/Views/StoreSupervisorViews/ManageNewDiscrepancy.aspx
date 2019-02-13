<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageNewDiscrepancy.aspx.cs" Inherits="SSISApplication.Views.StoreSupervisorViews.ManageNewDiscrepancy"  EnableEventValidation="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Manage Discrepancies</h2>
        <p class="lead">Check new discrepancies and create new adjustment voucher</p>
        <br />
        <p>
            <asp:Button ID="btn_CheckDiscrepancies" runat="server" Text="Check New Discrepancies" OnClick="btn_CheckDiscrepancies_Click" Style="font-size: large" />
        </p>
    </div>
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_ListDiscrepancies" runat="server" OnSelectedIndexChanged="grdView_ListDiscrepancies_SelectedIndexChanged"  OnRowDataBound="grdView_ListDiscrepancies_RowDataBound1" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4">
            </asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <br />
    <br />
    <div>
        <asp:Label ID="lbl_ShowDetails" runat="server" Text="Details of Discrepancy" Style="font-size: large"></asp:Label>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_DetailDiscrepancy" runat="server" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4"></asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <div class="row">
        <div>
            <asp:Label ID="lbl_TotalAmount" runat="server" Text="Total Discrepancy Amount:   " Style="font-size: medium"></asp:Label>
            <asp:Label ID="lbl_SumAmount" runat="server" Text="Label"></asp:Label>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div>
            <asp:Button ID="btn_IssueAdjustment" runat="server" Visible="false" Text="Issue New Adjustment Voucher" OnClick="btn_IssueAdjustment_Click" Style="font-size: large" Width="566px" /><asp:Button ID="btn_SendManager"  visible="false" runat="server" Text="Notify Store Manager" Style="font-size: large" />
        </div>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_DetailAdjustment" runat="server"  CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" AutoGenerateColumns="false" >
                 <Columns>
                    <asp:BoundField DataField="AdjustmentVoucherID" HeaderText="AdjustmentVoucherID" />
                    <asp:BoundField DataField="InventoryID" HeaderText="InventoryID" />
                    <asp:BoundField DataField="AdjustmentQuantity" HeaderText="AdjustmentQuantity" />
                    <asp:TemplateField HeaderText="AdjustmentComments">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_AdjustedComment" runat="server" AutoPostBack="false" Text='<%#Eval("AdjustmentComments") %>' ></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div>
            <asp:Button ID="btn_CreateAdjustment" runat="server" Text="Create" OnClick="btn_CreateAdjustment_Click" Visible="false" Style="font-size: large" />
        </div>
    </div>
</asp:Content>
