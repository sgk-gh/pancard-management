<%@ Page Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>Login - PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
    <div class="container">
        <div class="form-group">
            <div class='bg-warning' runat="server" id="divMessage" visible="false">Login failed. Please check your login details</div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-2" for="txtUserName">User Name<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-10">
                <asp:TextBox runat="server" type="text" class="form-control" id="txtUserName" placeholder="User Name"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-2" for="txtPassword">Password<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-10">
                <asp:TextBox TextMode="Password" runat="server" type="text" class="form-control" id="txtPassword" placeholder="Password"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-10">
                <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" onclick="Submit_Click" OnClientClick="return ValidateInputs();" text="Login"/>                                        
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ValidateInputs() {
            var elements = [
                { id: "ctl00_body_txtUserName", type: "text", isRequired: true, reqErrorMessage: "Please Enter Username" },
                { id: "ctl00_body_txtPassword", type: "text", isRequired: true, reqErrorMessage: "Please Enter your Password" }
            ];
            return jsvalidator.validate(elements);             
        }
    </script> 
</asp:Content>
