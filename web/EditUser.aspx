<%@ Page Title="" Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="EditUser.aspx.cs" Inherits="EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title>Edit Pan Card- PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
    <script type="text/javascript" src="Includes/js/common.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">
    <div class="container">
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
                <asp:TextBox runat="server" type="text" class="form-control" id="txtPassword" placeholder="Enter password"/>
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
                <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" onclick="Update_Click" OnClientClick="return ValidateInputs();" text="Update"/>
                <button type="reset" class="btn btn-default" onclick="ClearValidation();ClearMessage();">Reset</button>
            </div>
        </div>
    </div>
</asp:Content>

