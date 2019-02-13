<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewReports.aspx.cs" Inherits="SSISApplication.Views.StoreSupervisorViews.ViewReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2>View Report</h2>
        <p class="lead">Reports to analyze the stock and order by departments </p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <asp:Button ID="btn_departmentOrderReport" runat="server" Text="View Department Orders" OnClick="btn_departmentOrderReport_Click" OnClientClick="target ='_blank';" />
        </div>
        <div class="col-md-4">
            <asp:Button ID="btn_StoreItemReport" runat="server" Text="View Store Items" OnClick="btn_StoreItemReport_Click" OnClientClick="target ='_blank';" />
        </div>
        <div class="col-md-4">
            <asp:Button ID="btn_Report" runat="server" Text="Report" OnClick="btn_Report_Click" OnClientClick="target ='_blank';" />
        </div>
    </div>
    <div>
    </div>
</asp:Content>
