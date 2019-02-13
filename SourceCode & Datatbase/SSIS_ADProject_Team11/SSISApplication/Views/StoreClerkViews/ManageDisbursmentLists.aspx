<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageDisbursmentLists.aspx.cs" Inherits="SSISApplication.Views.StoreClerkViews.ManageDisbursmentLists" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <br /><br /><br /><br />
        <h2>Manage Disbursments</h2>
        <p class="lead"> See the disbursements and update the quantity before collection has been made</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h5>Your collection points : </h5>
            <br />
            <asp:RadioButtonList ID="rdb_colpoint" runat="server" GroupName="xx" AutoPostBack="true" OnSelectedIndexChanged="showDisbursementGridBasedOnColPt" OnRowDataBound="OnRowDataBound">
            </asp:RadioButtonList>
            <br />

        </div>
        <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_disbursement" runat="server" width=100%
                 AutoGenerateColumns="false" OnRowDataBound="RowDataBound2" OnSelectedIndexChanged="SelectedIndexChanged2" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
                <Columns>
                    <asp:BoundField DataField="DisbursementID" HeaderText="Disbursement ID" />
                    <asp:BoundField DataField="DeptName" HeaderText="Department" />
                    <asp:BoundField DataField="collectionPointname" HeaderText="Collection Point" />
                    <asp:BoundField DataField="RetrievalDate" HeaderText="Sent for collection on" />
                    <asp:BoundField DataField="CollectionTime" HeaderText="Collection Time" />
                    <asp:BoundField DataField="DisbursementStatus" HeaderText="Disbursement Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
        </div>
    <br />
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="grdView_disbursementDetails" runat="server" Visible="true" AutoGenerateColumns="false" OnPreRender="gridView_PreRender" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
                <Columns>
                    <asp:BoundField DataField="DisbursementID" HeaderText="Disbursement ID" />
                    <asp:BoundField DataField="InventoryID" HeaderText="InventoryID" Visible="true" />
                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                    <asp:BoundField DataField="collectionPointname" HeaderText="Collection Point" />
                    <asp:TemplateField HeaderText="Disbursed Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" AutoPostBack="false" Text='<%#Eval("DisbursementQuantity") %>' ReadOnly="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Disbursed qty should be =< Required qty" ControlToValidate="TextBox2"></asp:RequiredFieldValidator>
                            <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="lbl" ControlToValidate="TextBox2" ErrorMessage="Can't be more than requested" Operator="LessThanEqual" Type="Integer"></asp:CompareValidator>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        </div>
        <%--<div class="col-md-4">

        </div>--%>
        <div class="row">
        <div class="col-md-4">
            <br /><br /><br />
            <asp:Button ID="btn_SaveQty" runat="server" Text="Update Disbursed Qty" OnClick="btn_SaveQty_Click" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_EditQty" runat="server" Text="Edit Qty" OnClick="btn_EditQty_Click" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btn_Cancel" runat="server" Text="Cancel" OnClick="btn_Cancel_Click" />
            &nbsp;
            <br /><br />
        <asp:Button ID="btn_SendForCollection" runat="server" Text="Send For Collection" OnClick="btn_SendForCollection_Click" />
        </div>
        </div>
  
  
</asp:Content>
