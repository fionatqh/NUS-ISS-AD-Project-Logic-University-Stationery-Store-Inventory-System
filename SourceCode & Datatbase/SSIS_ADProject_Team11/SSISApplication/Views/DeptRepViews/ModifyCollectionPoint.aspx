<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyCollectionPoint.aspx.cs" Inherits="SSISApplication.Views.DeptRepViews.ModifyCollectionPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Modify Collection point</h2>
        
    </div>
    <div class="row">
        <div class="col-md-4">
            <asp:RadioButtonList ID="rdb_CollectionPoints" runat="server" OnSelectedIndexChanged="rdb_CollectionPoints_SelectedIndexChanged">
            </asp:RadioButtonList>
            <asp:Label ID="lbl_Status" runat="server" Text=""></asp:Label>
        </div>
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
            <asp:Button ID="btn_Confirm" runat="server" OnClick="btn_Confirm_Click" Text="Change Collection Point" />
        </div>
    </div>
</asp:Content>
