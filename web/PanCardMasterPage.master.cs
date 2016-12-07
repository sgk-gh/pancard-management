using System;
using System.Web.Script.Serialization;
using Model;
public partial class PanCardMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        userName.InnerHtml = "Welcome";
        if (PanCardBasePage.CurrentUser != null)
        {
            userName.InnerHtml = PanCardBasePage.CurrentUser.LoginName;
        }
        else if(System.IO.Path.GetFileName(Request.Url.AbsolutePath).Contains("Login.aspx"))
        {            
            divUserMenu.Visible = false;
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
}
