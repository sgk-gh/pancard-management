using System;
using System.Configuration;
using System.Collections.Generic;
using Model;
using System.Web.Script.Serialization;

public partial class Login : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && CurrentUser != null)
        {         
            ClearCurrentUserCookie();
        }
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        var query = ConfigurationManager.AppSettings["qryGetUserDetails"];
        var aUser = new User
        {
            LoginName = txtUserName.Text.Trim(),
            LoginPassword = txtPassword.Text.Trim()
        };
        query = SqlHandler.AddConditionToQuery(query, new List<string> { "LoginName='" + aUser.LoginName + "'", "LoginPassword='" + aUser.LoginPassword + "'" });
        var user = PanCardRepository.GetUser(query, ConfigurationManager.AppSettings["rmapGetUserDetails"]);
        if (user!=null)
        {            
            var jsonUser = new JavaScriptSerializer().Serialize(user);
            var httpCookie = Response.Cookies["user"];
            if (httpCookie != null) httpCookie.Value = jsonUser;
            Response.Redirect("Default.aspx");
        }
        else
        {
            divMessage.Visible = true;
        }
        
    }
}