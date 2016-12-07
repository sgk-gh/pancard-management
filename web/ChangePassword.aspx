<%@ Page Title="" Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>Change Password - PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
    <div class="container">
        <div class="form-group">
            <div class='bg-warning' runat="server" id="divMessage" visible="false">Error occurred!</div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtNewPassword">New Password<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" type="text" TextMode="Password" class="form-control" id="txtNewPassword" placeholder="Enter new password"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtConfirmPassword">Retype Password :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" type="text" TextMode="Password" class="form-control" id="txtConfirmPassword" placeholder="Retype new password"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" onclick="Change_Click" OnClientClick="return ValidateInputs();" text="Change"/>                    
                <button type="reset" class="btn btn-default" onclick="ClearValidation();">Clear</button>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ValidateInputs() {
            var elements = [
                { id: "ctl00_body_txtNewPassword", type: "text", isRequired: true, reqErrorMessage: "Enter new password." },
                { id: "ctl00_body_txtConfirmPassword", type: "text", compareWith: "ctl00_body_txtNewPassword", compErrorMessage: "Passwords don't match." }
            ];
            return jsvalidator.validate(elements);
        }
        function ClearValidation() {
            jsvalidator.clear();
        }
    </script>
</asp:Content>

