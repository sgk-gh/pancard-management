<%@ Page Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="New.aspx.cs" Inherits="New" %>

<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>New Entry - PAN Details</title>
    <script type="text/javascript" src="Includes/js/jsvalidator.js"></script>
    <script type="text/javascript" src="Includes/js/jquery.maskedinput.js"></script>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
    <div class="container">
            <div class="form-group">
                <label class="control-label col-sm-2" for="txtApplicationNumber">Application number<span style="color: #dd4b39;">*</span> :</label>
                <div class="col-sm-10">
                    <asp:TextBox runat="server" type="text" class="form-control" id="txtApplicationNumber" placeholder="Enter application number"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="dtDateOfBirth">Date of birth<span style="color: #dd4b39;">*</span> :</label>
                <div class="col-sm-10">
                    <asp:TextBox runat="server" type="date" class="form-control" id="dtDateOfBirth"  placeholder="Select date of birth (dd-mm-yyyy)"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="txtName">Name<span style="color: #dd4b39;">*</span> :</label>
                <div class="col-sm-10">
                    <asp:TextBox runat="server" type="text" class="form-control" id="txtName" placeholder="Enter name"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="txtFatherName">Father Name<span style="color: #dd4b39;">*</span> :</label>
                <div class="col-sm-10">
                    <asp:TextBox runat="server" type="text" class="form-control" id="txtFatherName" placeholder="Enter father name"/>
                </div>
            </div>            
            <div class="form-group">
                <label class="control-label col-sm-2" for="txtCouponNumber">Coupon Number<span style="color: #dd4b39;">*</span> :</label>
                <div class="col-sm-10">
                    <asp:TextBox runat="server" class="form-control" id="txtCouponNumber" placeholder="Enter coupon number"/>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2" for="filePanImage">Select Scanned PAN Document Image<span style="color: #dd4b39;">*</span> :</label>
                <div class="col-sm-10">
                    <asp:FileUpload runat="server" type="file" class="btn btn-default" id="filePanImage"/>
                </div>
            </div>
            
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <asp:Button ID="btnSubmit" class="btn btn-default" runat="server" onclick="Submit_Click" OnClientClick="return ValidateInputs();" text="Submit"/>                    
                    <button type="reset" class="btn btn-default" onclick="ClearValidation();">Clear</button>
                </div>
            </div>
             <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    
                </div>
            </div>
        </div>

        <script type="text/javascript">
            $(function () {
                $("#ctl00_body_dtDateOfBirth").datepicker({ dateFormat: "dd/mm/yy" });
                //$("#body_dtPanEntryDate").datepicker({ dateFormat: "dd/mm/yy" });
                $("#ctl00_body_dtDateOfBirth").mask("99/99/9999");
                //$("#body_dtPanEntryDate").mask("99/99/9999");
            });

            function ValidateInputs() {
                var elements = [
                { id: "ctl00_body_txtApplicationNumber", type: "text", isRequired: true, reqErrorMessage: "Please Enter application number" },
                { id: "ctl00_body_dtDateOfBirth", type: "text", isRequired: true, reqErrorMessage: "Please Select date of birth" },
                { id: "ctl00_body_txtName", type: "text", isRequired: true, reqErrorMessage: "Please Enter name" },
                { id: "ctl00_body_txtFatherName", type: "text", isRequired: true, reqErrorMessage: "Please Enter father name" },
                { id: "ctl00_body_txtCouponNumber", type: "text", isRequired: true, reqErrorMessage: "Please Enter coupon number" },
                { id: "ctl00_body_filePanImage", type: "file", isRequired: true, reqErrorMessage: "Please Select scanned PAN document image" }
            ];
                return jsvalidator.validate(elements);
            }
            function ClearValidation() {
                jsvalidator.clear();
            }
            var logout = function (name) {
                document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                window.location.href = 'Login.aspx';
            };
    </script>
</asp:Content>