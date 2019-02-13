<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProcessRequisitions.aspx.cs" Inherits="SSISApplication.Views.StoreClerkViews.ProcessRequisitions" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <br /><br /><br /><br /><br /><br />
        <h2>Process Requisitions</h2>
        <p class="lead"> Retrieve requested items from pending department requests and send them for collection</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h5>Your collection points : </h5>
            <br />
            <asp:RadioButtonList ID="rdb_colpoint" runat="server" GroupName="xx" AutoPostBack="true" OnSelectedIndexChanged="showGridViewBasedOnColPt" OnRowDataBound="OnRowDataBound">
            </asp:RadioButtonList>
        </div>
        <div class="col-md-4">
            <asp:GridView ID="grdView_processRequ" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnPreRender="gridView_PreRender" OnRowDataBound="OnRowDataBound" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
                <EmptyDataTemplate>
                    <div style="text-align: center">No records found.</div>
                </EmptyDataTemplate>

                <Columns>
                    <%--Cells[0] --%> <asp:BoundField DataField="InventoryID" HeaderText="Inventory ID" /> <%-- old Cells[0] --%>
                    <%--Cells[1] --%> <asp:BoundField DataField="ItemName" HeaderText="Item Name" /> <%-- old Cells[2] --%>

                    <%--Cells[2] --%> <asp:TemplateField HeaderText="Inventory Qty"> <%-- old Cells[1] --%>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_qtyInventory" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_qtyInventory" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--Cells[3] --%> <asp:BoundField DataField="UnitOfMeasure" HeaderText="UOM" /> <%-- old Cells[5] --%>
                    
                    <%--Cells[4] --%> <asp:BoundField DataField="DeptName" HeaderText="Department" /> <%-- old Cells[3] --%>

                    <%--Cells[5] --%> <asp:TemplateField HeaderText="Requested Qty"> <%-- old Cells[6] --%>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_qtyRequested" runat="server" Text='<%# Bind("Pending_Quantity") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_qtyRequested" runat="server" Text='<%# Bind("Pending_Quantity") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--Cells[6] --%> <asp:BoundField DataField="Earliest_Request_Date" HeaderText="Earliest Request Date" /> <%-- old Cells[4] --%>

                    <%--Cells[7] --%> <asp:TemplateField HeaderText="Retrieved Qty"> <%-- old Cells[7] --%>
                        <ItemTemplate>
                            <asp:TextBox ID="txt_qtyRetrieved" runat="server" AutoPostBack="false" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Qty cannot be blank" ControlToValidate="txt_qtyRetrieved" ToolTip="Input number of retrieved item"></asp:RequiredFieldValidator>
                            <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="lbl" ControlToValidate="Lbl_qtyRetrieved" ErrorMessage="Can't be more than requested" Operator="LessThanEqual" Type="Integer"></asp:CompareValidator>--%>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
        </div>
    </div>
    <div class="col-md-4">
        <asp:Button ID="btn_MakeRetrievalAndDisbursement" runat="server" OnClick="btn_MakeRetrievalAndDisbursement_Click" Text="Make Retrieval and Disbursement"  />
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn_RetrieveItems" runat="server" OnClick="btn_RetrieveItems_Click" Text="Retrieve Items"  />
        <asp:Label ID="lbl_ShowDisbursementID" runat="server" Text="You have created Disbursement ID:" Visible="false"></asp:Label>
    </div>
</asp:Content>
