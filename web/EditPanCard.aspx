<%@ Page Title="" Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="EditPanCard.aspx.cs" Inherits="EditPanCard" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>Edit Pan Card- PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
    <script type="text/javascript" src="Includes/js/jquery.maskedinput.js"></script>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
    <div class="container">
        <div class="form-group">
            <div class='bg-warning' runat="server" id="divMessage" visible="false">Error occurred!</div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtApplicationNumber">Application number<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" type="text" class="form-control" id="txtApplicationNumber" placeholder="Enter application number"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtDateOfBirth">Date of birth<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" type="date" class="form-control" id="txtDateOfBirth"  placeholder="Select date of birth (dd/mm/yyyy)"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtCustomerName">Customer Name<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" type="text" class="form-control" id="txtCustomerName" placeholder="Enter customer name"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtFatherName">Father Name<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" type="text" class="form-control" id="txtFatherName" placeholder="Enter father name"/>
            </div>
        </div>            
        <div class="form-group">
            <label class="control-label col-sm-4" for="txtCouponNumber">Coupon Number<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:TextBox runat="server" class="form-control" id="txtCouponNumber" placeholder="Enter coupon number"/>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-sm-4" for="hlPanImage">Scanned PAN Document Image<span style="color: #dd4b39;">*</span> :</label>
            <%--<div class="controls">--%>  
            <div class="col-sm-8">                 
                <asp:HyperLink ID="hlPanImage" runat="server" Target="_blank"></asp:HyperLink>
            </div>
            <%--</div>--%>
        </div>          
        <div class="form-group">
            <label class="control-label col-sm-4" for="filePanImage">Replace Scanned PAN Document Image<span style="color: #dd4b39;">*</span> :</label>
            <div class="col-sm-8">
                <asp:FileUpload runat="server" type="file" class="btn btn-default" id="filePanImage"/>
            </div>
        </div>
            
        <div class="form-group">
            <div class="col-sm-offset-4 col-sm-8">
                <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" onclick="Update_Click" OnClientClick="return ValidateInputs();" text="Update"/>                    
                <button type="reset" class="btn btn-default" onclick="ClearValidation();">Clear</button>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#body_txtDateOfBirth").datepicker({ dateFormat: "dd/mm/yy" });
            //$("#body_dtPanEntryDate").datepicker({ dateFormat: "dd/mm/yy" });
            $("#body_txtDateOfBirth").mask("99/99/9999");
            //$("#body_dtPanEntryDate").mask("99/99/9999");
        });
    </script>
</asp:Content>

