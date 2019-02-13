<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequestStatus.aspx.cs" EnableEventValidation="false" Inherits="SSISApplication.Views.CommonViews.RequestStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Request Status</h2>
        </div>
    <div class="row">
        <div class="col-md-4">
            
            <asp:Label ID="lbl_From" runat="server" Text="From"></asp:Label>
            <asp:TextBox ID="txt_startdate" runat="server" width="150px" TextMode="DateTime" Tooltip="enter start date in dd/MM/yyyy"></asp:TextBox>
            <asp:ImageButton ID="img_startcalender" ImageUrl="~/images/calendar-solid.png" ImageAlign="Bottom"
                OnClick="StartShow_Date" runat="server" Height="15px" Width="15px" />
             
            <asp:Calendar ID="cldr_Startdate" visible="false" runat="server" OnSelectionChanged="StartDate_Selected"></asp:Calendar>
              
           
        </div>
        <div class="col-md-4">
            <asp:Label ID="ToLabel" runat="server" Text="To"></asp:Label>
             <asp:TextBox ID="txt_enddate" runat="server" width="150px" TextMode="DateTime" Tooltip="enter end date in dd/MM/yyyy"></asp:TextBox>
            <asp:ImageButton ID="img_endcalender" ImageUrl="~/images/calendar-solid.png" ImageAlign="Bottom"
                OnClick="EndShow_Date" runat="server" Height="15px" Width="15px" />
            <asp:Calendar ID="cldr_Enddate" runat="server" visible="false" OnSelectionChanged="EndDate_Selected"></asp:Calendar>
        </div>
        <div class="col-md-4">
            
            <asp:Button ID="btn_search" runat="server" OnClick="SearchButton_Click" Text="search" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="Gridview_Allrequestsandstatus" runat="server" AutoGenerateColumns="False" AllowSorting="False"  OnSelectedIndexChanged="Gridview_Allrequestsandstatus_SelectedIndexChanged" BorderStyle="None" BorderWidth="1px" CellPadding="4" OnRowDataBound="Gridview_Allrequestsandstatus_RowDataBound1" CssClass="table table-responsive table-striped table-hover">
                 
                <Columns>
                    <asp:BoundField HeaderText="Request Id" DataField="UserRequestID" ItemStyle-Width="50%">
                        <ItemStyle Width="50%"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Order date" DataField="UserRequestDate" ItemStyle-Width="50%">
                        <ItemStyle Width="50%"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Status" DataField="UserRequestStatusName" ItemStyle-Width="50%">
                        <ItemStyle Width="50%"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="GridView_RequestDetails" runat="server" AutoGenerateColumns="false"  OnRowDataBound="GridView_RequestDetails_RowDataBound"  CssClass="table table-responsive table-striped table-hover" BorderStyle="None" BorderWidth="1px" CellPadding="4" >
                <Columns>
                    <asp:BoundField DataField="UserRequestStatusName" HeaderText="Status" />
                    <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" />
                    <asp:TemplateField HeaderText="RequestQuantity">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_requestquantity" Text='<%# Bind("RequestQuantity") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inventoryid" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbl_inventoryID" Text='<%# Bind("InventoryID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UserRequestID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lbl_userrequestID" Text='<%# Bind("UserRequestID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_cancel" runat="server" Text="cancel"   OnClick="CancelButton_Click"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_save" runat="server" Text="save"  OnClick="SaveButton_Click" />
        </div>
    </div>
    <asp:Label ID="lbl_Comment" Visible="false" runat="server" />
</asp:Content>
