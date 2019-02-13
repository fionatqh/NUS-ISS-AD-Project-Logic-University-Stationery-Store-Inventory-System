<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDisbursments.aspx.cs" Inherits="SSISApplication.Views.DeptRepViews.ViewDisbursments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="row">
        <div class="jumbotron">
            <h2>View Disbursments</h2>
        </div>

        <div class="row">
        <div class="col-md-4">
            
            <asp:Label ID="lbl_From" runat="server" Text="From"></asp:Label>
            <asp:TextBox ID="txt_startdate" runat="server" width="150px" TextMode="Date" Tooltip="enter start date in dd/MM/yyyy"></asp:TextBox>
            <asp:ImageButton ID="img_startcalender" ImageUrl="~/images/calendar-solid.png" ImageAlign="Bottom"
                OnClick="StartShow_Date" runat="server" Height="15px" Width="15px" />
             
            <asp:Calendar ID="cldr_Startdate" visible="false" runat="server" OnSelectionChanged="StartDate_Selected"></asp:Calendar>
              
           
        </div>
        <div class="col-md-4">
            <asp:Label ID="ToLabel" runat="server" Text="To"></asp:Label>
             <asp:TextBox ID="txt_enddate" runat="server" width="150px" TextMode="Date" Tooltip="enter end date in dd/MM/yyyy"></asp:TextBox>
            <asp:ImageButton ID="img_endcalender" ImageUrl="~/images/calendar-solid.png" ImageAlign="Bottom"
                OnClick="EndShow_Date" runat="server" Height="15px" Width="15px" />
            <asp:Calendar ID="cldr_Enddate" runat="server" visible="false" OnSelectionChanged="EndDate_Selected"></asp:Calendar>
        </div>
        <div class="col-md-4">
            
            <asp:Button ID="btn_search" runat="server" OnClick="btn_Search_Click" Text="search" />
        </div>


            <br />
            <br />
            <div class="row">
                <div class="col-md-6">


                    <asp:GridView ID="grdView_ViewDisbursement"
                        runat="server"
                        AutoGenerateColumns="False"
                        AllowPaging="True"
                        AllowSorting="True" OnSorting="grdView_ViewDisbursement_Sorting"
                        DataKeyNames="DisbursementID"
                        OnSelectedIndexChanged="grdView_ViewDisbursement_SelectedIndexChanged"
                        AutoGenerateSelectButton="True" OnPageIndexChanging="grdView_ViewDisbursement_PageIndexChanging" CssClass="table table-responsive table-striped table-hover"  BorderStyle="None" BorderWidth="1px" CellPadding="4" >
                        <Columns>
                            <asp:BoundField HeaderText="DisbursementID" DataField="DisbursementID" />
                            <asp:BoundField HeaderText="Disbursement Date" DataField="DisbursementDate" />
                            <asp:BoundField HeaderText="Disbursement Status" DataField="DisbursementStatus" />
                            <asp:BoundField HeaderText="Collection Point" DataField="CollectionPoint" Visible="false" />
                            <asp:TemplateField HeaderText="CollectionPoint">
                                <ItemTemplate>
                                    <asp:Label ID="CollectionPoint" runat="server" Text='<%# GetCollectionName(Eval("CollectionPoint").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                       
                        
                             </Columns>
                    </asp:GridView>
                    <asp:GridView ID="grdView_ViewDisbursementDetails"
                        runat="server"
                        AutoGenerateColumns="False"
                        AllowPaging="True"
                        AllowSorting="True"
                        DataKeyNames="DisbursementID"
                        AutoGenerateSelectButton="False" CssClass="table table-responsive table-striped table-hover">
                        <Columns>
                            <asp:BoundField DataField="ItemName" HeaderText="Item Name" />

                            <asp:TemplateField HeaderText="RequestedQuantity">
                                <ItemTemplate>
                                    <asp:Label ID="RequestedQuantity" runat="server" Text='<%# GetReqQty(Eval("ItemName").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="DisbursementQuantity" HeaderText="Disbursement Quantity" />
                        </Columns>
                    </asp:GridView>

                    
                </div>

                <div class="col-md-4">
                    <asp:Button ID="btn_Confirm0" runat="server" OnClick="btn_Confirm_Click" Text="Confirm Disbursement" CausesValidation="False" />
                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_ViewDisbList" runat="server" OnClick="btn_ViewDisbList_Click" Text="View Disbursements List" CausesValidation="False" />
                    <br />
                    <asp:Label ID="lbl_Confirm" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
