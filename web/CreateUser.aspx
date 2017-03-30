<%@ Page Title="" Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="CreateUser" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>Create User - PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
    <script type="text/javascript" src="Includes/js/common.js"></script>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
    <div class="container">
        <ul id="clientTabs" class="nav nav-tabs">
          <li class="active" id="liCreate"><a data-toggle="tab" href="#Create">Create Client</a></li>
          <li><a data-toggle="tab" href="#AllUsers">View All Clients</a></li>          
        </ul>
        <div class="tab-content">
            <div id="Create" class="tab-pane fade in active">
                <h3></h3>
                <div class="form-group">
                    <div class='bg-warning' runat="server" id="divMessage" visible="false">Error occurred!</div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="txtLoginName">User Name<span style="color: #dd4b39;">*</span> :</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" type="text" class="form-control" id="txtLoginName" placeholder="Enter user name"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="txtPassword">Password<span style="color: #dd4b39;">*</span> :</label>
                    <div class="col-sm-10">
                        <asp:TextBox TextMode="Password" runat="server" type="text" class="form-control" id="txtPassword" placeholder="Enter password"/>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-2" for="ddlRole">User Role<span style="color: #dd4b39;">*</span> :</label>
                    <div class="col-sm-10">
                        <%--<asp:TextBox runat="server" type="text" class="form-control" id="TextBox1" placeholder="Enter user name"/>--%>
                        <asp:DropDownList runat ="server" ID="ddlRole" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" onclick="Submit_Click" OnClientClick="return ValidateInputs();" text="Submit"/>                    
                        <button type="reset" class="btn btn-default" onclick="ClearValidation();ClearMessage();">Clear</button>
                    </div>
                </div>
            </div>
            <div id="AllUsers" class="tab-pane fade">
                <h3></h3>
                <asp:GridView ID="grvUsers" runat="server" EmptyDataText="<div class='bg-warning'>No users found</div>" AllowPaging="true" AutoGenerateColumns="false" CssClass="table" OnPageIndexChanging="PageIndexChanging" OnRowDeleting="OnRowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' CssClass="hide"></asp:Label>
                                <asp:Label ID="lblLoginName" runat="server" Text='<%#Eval("LoginName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="User Type">
                            <ItemTemplate>
                                <asp:Label ID="lblRole" Text='<%#Eval("UserRole.Role") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit/Delete">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" NavigateUrl='<%#"EditUser.aspx?q=Id:"+Eval("Id") %>' Text="Edit" Target="_blank"></asp:HyperLink><br />
                                <asp:LinkButton Text="Delete" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')" />
                            </ItemTemplate>                               
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        if ((location.search.substring(1).length && typeof isPostBack != "undefined" && !isPostBack) || (typeof isGridViewAction != "undefined" && isGridViewAction)) {
            $('#clientTabs a[href="#AllUsers"]').tab('show');
        }
        function ValidateInputs() {
            var elements = [
                { id: "ctl00_body_txtLoginName", type: "text", isRequired: true, reqErrorMessage: "Please Enter Username" },
                { id: "ctl00_body_txtPassword", type: "text", isRequired: true, reqErrorMessage: "Please Enter Password" }
            ];
            return jsvalidator.validate(elements);
        }        
    </script>
</asp:Content>

