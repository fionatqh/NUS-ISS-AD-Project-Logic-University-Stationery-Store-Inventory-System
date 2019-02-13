<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdjustmentVouchers.aspx.cs" Inherits="SSISApplication.Views.StoreSupervisorViews.AdjustmentVouchers"  EnableEventValidation="False" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Adjustment Vouchers</h2>
        <p class="lead">View history of adjustment vouchers</p>
        <br />
        <p>
            <asp:Button ID="btn_HistoryAdjustment" runat="server" Text="View History Of Adjustment Voucher" Style="font-size: medium" OnClick="btn_HistoryAdjustment_Click" /></p>
    </div>
    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_ListAdjustment" runat="server" OnRowDataBound="grdView_ListAdjustment_RowDataBound" OnSelectedIndexChanged="grdView_ListAdjustment_SelectedIndexChanged"  CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4"></asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
    <br />
    <br />
    <div>
        <asp:Label ID="lbl_DetailsAdjustment" runat="server" Text="Details of Adjustment Voucher" Style="font-size: large" Visible="false"></asp:Label>
    </div>
    <br />
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_DetailsAdjustment" runat="server"  CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" ></asp:GridView>
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>
