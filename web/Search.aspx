<%@ Page Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>Search - PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
    <script type="text/javascript" src="Includes/js/jquery.maskedinput.js"></script>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
        <div class="container">
            <div class="form-group">
                <label class="control-label col-sm-2" >Search by<span style="color: #dd4b39;">*</span> :</label>                
                <div class="col-sm-8">
                    <label class="control-label col-sm-4" >Application Number : </label>
                    <div class="col-sm-8">
                        <span class="col-sm-11"><asp:TextBox runat="server" type="text" class="form-control" id="txtApplicationNumber" placeholder="Enter application number"/></span>                        
                        <span class="col-sm-1"><asp:CheckBox runat="server" ID="chkApplicationNumber"/></span>
                    </div>               
                    <label class="control-label col-sm-4" >Date of birth : </label>
                    <div class="col-sm-8">
                        <span class="col-sm-11"><asp:TextBox runat="server" type="date" class="form-control" id="dtDateOfBirth"  placeholder="Select date of birth"/></span>
                        <span class="col-sm-1"><asp:CheckBox runat="server" ID="chkDateOfBirth"/></span>
                    </div>                
                    <label class="control-label col-sm-4" >Customer Name : </label>
                    <div class="col-sm-8">
                        <span class="col-sm-11"><asp:TextBox runat="server" type="text" class="form-control" id="txtName" placeholder="Enter customer name"/></span>
                        <span class="col-sm-1"><asp:CheckBox runat="server" ID="chkName"/></span>
                    </div>
                    <label class="control-label col-sm-4" >Father Name : </label>
                    <div class="col-sm-8">
                        <span class="col-sm-11"><asp:TextBox runat="server" type="text" class="form-control" id="txtFatherName" placeholder="Enter father name"/></span>
                        <span class="col-sm-1"><asp:CheckBox runat="server" ID="chkFatherName"/></span>
                    </div>
                    <label class="control-label col-sm-4" >PAN Entry Date : </label>
                    <div class="col-sm-8">
                        <span class="col-sm-11"><asp:TextBox runat="server" type="date" class="form-control" id="dtPanEntryDate" placeholder="Select PAN entry date"/></span>
                        <span class="col-sm-1"><asp:CheckBox runat="server" ID="chkPanEntryDate"/></span>
                    </div>
                    <% if (CurrentUser.UserRole.Role.ToLower() == "admin")
                       { %>
                    <label class="control-label col-sm-4" >Client : </label>
                    <div class="col-sm-8">
                        <span class="col-sm-11"><asp:DropDownList runat ="server" ID="ddlClient" class="form-control"></asp:DropDownList></span>
                        <span class="col-sm-1"><asp:CheckBox runat="server" ID="chkClient"/></span>
                    </div>
                    <% } %>
                </div>
                <div class="col-sm-2">
                    <button type="button" class="btn btn-default" onclick="Search();">
                        <span class="glyphicon glyphicon-search"></span> Search
                    </button>
                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" type="button" style="display:none;" Text="Search"/>
                </div>
            </div>  
            <div class="col-sm-12"> </div>
            <div class="well col-sm-12">
                <div class="table-responsive">
                    <asp:GridView ID="grvPanDetails" runat="server" AllowPaging="true" AutoGenerateColumns="false"
                        EmptyDataText="<div class='bg-warning'>No records found</div>" CssClass="table" OnPageIndexChanging="PageIndexChanging" OnRowDeleting="OnRowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Application Number" ItemStyle-Width="10">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' CssClass="hide"></asp:Label>
                                    <asp:Label ID="lblApplicationNumber" runat="server" Text='<%#Eval("ApplicationNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                          
                            <%--<asp:BoundField DataField="ApplicationNumber" HeaderText="Application Number" />--%>
                            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                            <asp:BoundField DataField="FatherName" HeaderText="Father Name" />                            
                            <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" DataFormatString="{0:dd/MM/yyyy}" ApplyFormatInEditMode="true" />
                            <asp:BoundField DataField="CouponNumber" HeaderText="Coupon Number" />
                            <asp:BoundField DataField="PanEntryDate" HeaderText="Pan Entry Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" ApplyFormatInEditMode="true"/>                            
                            
                            <asp:TemplateField HeaderText="Client">
                                <ItemTemplate>
                                    <asp:Label ID="lblRole" runat="server" Text='<%#Eval("User.UserRole.Role") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField DataNavigateUrlFields="FilePath" DataNavigateUrlFormatString="PanCardImages/{0}" DataTextField="FilePath" HeaderText="Scanned Form" Target="_blank"/>                                                        
                            <%--<asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="Edit.aspx?q=Id:{0}" Text="Edit" HeaderText="Edit"/>--%>
                            <asp:TemplateField HeaderText="Edit/Delete">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" NavigateUrl='<%#"EditPanCard.aspx?q=Id:"+Eval("Id") %>' Text="Edit" Target="_blank"></asp:HyperLink><br />
                                    <asp:LinkButton Text="Delete" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')" />
                                </ItemTemplate>
                                <%--<EditItemTemplate>
                                    <asp:LinkButton Text="Update" runat="server" CommandName="Update" />
                                    <asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" />
                                </EditItemTemplate>--%>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                </div>
            </div>
        </div>
    
    <script type="text/javascript">
        $(function () {
            $("#ctl00_body_dtDateOfBirth").datepicker({ dateFormat: "dd/mm/yy" });
            $("#ctl00_body_dtPanEntryDate").datepicker({ dateFormat: "dd/mm/yy" });
            $("#ctl00_body_dtDateOfBirth").mask("99/99/9999");
            $("#ctl00_body_dtPanEntryDate").mask("99/99/9999");
        });
        function Search() {
            $("#ctl00_body_btnSearch").click();
        }
        var logout = function (name) {
            document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
            window.location.href = 'Login.aspx';
        };
        function AssignDatePicker(element) {
            $(element).datepicker({ dateFormat: "dd/mm/yy" });
        }
    </script>
</asp:Content>
