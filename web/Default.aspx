<%@ Page Language="C#" MasterPageFile="~/PanCardMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="cphHead" ContentPlaceHolderID="head" Runat="Server">
    <title>PAN Details</title>
</asp:Content>
<asp:Content ID="cphBody" ContentPlaceHolderID="body" Runat="Server">
    
        <div class="container">
            <div id="divNew" class="col-lg-3 col-md-4 col-xs-6 thumb">
                <a class="thumbnail" href="#" onclick="open_page('New.aspx')">
                    <img class="img-responsive" src="Includes/images/addNew.png" alt="">
                </a>
            </div>
            <div id="divSearch" class="col-lg-3 col-md-4 col-xs-6 thumb">
                <a class="thumbnail" href="#" onclick="open_page('Search.aspx')">
                    <img class="img-responsive" src="Includes/images/search.png" alt="">
                </a>
            </div>
        </div>    
</asp:Content>
