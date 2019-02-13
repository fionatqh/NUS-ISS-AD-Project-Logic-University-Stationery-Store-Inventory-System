<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageRetrievals.aspx.cs" Inherits="SSISApplication.Views.StoreClerkViews.ManageRetrievals" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <br />
        <br />
        <br />
        <br />
        <h2>Manage Retrievals</h2>
        <p class="lead">See the retrieval and update the quantity before sending to collection</p>
    </div>
    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_retrieval" runat="server" Width="100%"
                AutoGenerateColumns="false" OnRowDataBound="RowDataBound2" OnSelectedIndexChanged="SelectedIndexChanged2" CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>
                    <%--<asp:BoundField DataField="RetrievalID" HeaderText="Retrieval ID" />
                    <asp:BoundField DataField="DeptName" HeaderText="Department" />
                    <asp:BoundField DataField="collectionPointname" HeaderText="Collection Point" Visible="false"/>
                    <asp:BoundField DataField="RetrievalDate" HeaderText="Retrieved on" />
                    <asp:BoundField DataField="CollectionTime" HeaderText="Collection Time" />--%>

                    <asp:BoundField DataField="RetrievalID" HeaderText="Retrieval ID" />
                    <asp:BoundField DataField="RetrievalDate" HeaderText="Retrieved on" />
                </Columns>

            </asp:GridView>
        </div>
    </div>
    <br />
    <br />

    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_retrievalDetails" runat="server" Visible="true" AutoGenerateColumns="false" OnPreRender="gridView_PreRender" CssClass="table table-responsive table-striped table-hover">
                <Columns>
                    <asp:BoundField DataField="RetrievalID" HeaderText="Retrieval ID" />
                    <asp:BoundField DataField="ItemNumber" HeaderText="Item Number" Visible="true" />
                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                    <asp:BoundField DataField="collectionPointname" HeaderText="Collection Point" Visible="false" />
                    <asp:BoundField DataField="RetrievalDate" HeaderText="Retrieval Date" Visible="false" />
                    <asp:TemplateField HeaderText="Retrieved Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_RetrievalQuantity" runat="server" AutoPostBack="false" Text='<%#Eval("RetrievalQuantity") %>' ReadOnly="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Retrieved qty should be =< Required qty" ControlToValidate="txt_RetrievalQuantity"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <br />
    <strong>
    <asp:Label ID="lbl_disbursementNotif" runat="server" Text="The respective disbursements for the retrieval is as the following, please update the disbursement quantity according to the updated retrieval quantity on &quot;Disbursements&quot;" Visible="False" Font-Size="Medium"></asp:Label>
    <br />
    </strong>
    <br />

    <div class="row">
        <div class="col-md-4">
            <asp:GridView ID="grdView_processRequ" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" OnPreRender="gridView_PreRender" CssClass="table table-responsive table-striped table-hover">
                <EmptyDataTemplate>
                    <div style="text-align: center">No records found.</div>
                </EmptyDataTemplate>

                <Columns>
                    <%--Cells[0] --%>
                    <asp:BoundField DataField="InventoryID" HeaderText="Inventory ID" />
                    <%-- old Cells[0] --%>
                    <%--Cells[1] --%>
                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                    <%-- old Cells[2] --%>

                    <%--Cells[2] --%>
                    <asp:TemplateField HeaderText="Inventory Qty"><%-- old Cells[1] --%>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_qtyInventory" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_qtyInventory" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--Cells[3] --%>
                    <asp:BoundField DataField="UnitOfMeasure" HeaderText="UOM" />
                    <%-- old Cells[5] --%>

                    <%--Cells[4] --%>
                    <asp:BoundField DataField="DeptName" HeaderText="Department" />
                    <%-- old Cells[3] --%>

                    <%--Cells[5] --%>
                    <asp:BoundField DataField="DisbursementID" HeaderText="DisbursementID" />
                    <%-- old Cells[3] --%>

                    <%--Cells[6] --%>
                    <asp:TemplateField HeaderText="Disbursement Qty"><%-- old Cells[7] --%>
                        <ItemTemplate>
                            <asp:TextBox ID="txt_DisbursementQuantity" runat="server" AutoPostBack="false" Text='<%# Bind("DisbursementQuantity") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Qty cannot be blank" ControlToValidate="txt_DisbursementQuantity" ToolTip="Input number of retrieved item"></asp:RequiredFieldValidator>
                            <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="lbl" ControlToValidate="Lbl_qtyRetrieved" ErrorMessage="Can't be more than requested" Operator="LessThanEqual" Type="Integer"></asp:CompareValidator>--%>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
        </div>
    </div>
    <div>
        <asp:Button ID="btn_SaveQty" runat="server" Text="Update Retrieved Qty" OnClick="btn_SaveQty_Click" />
        <asp:Button ID="btn_EditQty" runat="server" Text="Edit Qty" OnClick="btn_EditQty_Click" />
        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" OnClick="btn_Cancel_Click" />
        <asp:Button ID="btn_UpdateDisb" runat="server" Text="Update Disbursement" OnClick="btn_UpdateDisb_Click" Visible="False" />

    </div>
    <div>
        <%--Cells[6] --%>        <%-- old Cells[7] --%>
    </div>
    <br />
    <br />
    <div>
        <asp:Label ID="lbl_ShowDisbursementID" runat="server" Text="You have created Disbursement ID:" Visible="false"></asp:Label>
    </div>
</asp:Content>
