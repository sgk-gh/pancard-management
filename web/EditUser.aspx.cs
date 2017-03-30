using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

public partial class EditUser : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
        if (IsPostBack) return;
        if (string.IsNullOrEmpty(Request.QueryString["q"]))
        {
            divMessage.InnerHtml = "Error occurred!";
            divMessage.Attributes["class"] = "bg-warning";
            divMessage.Visible = true;
            return;
        }
        LoadUserRoles();
        SetUserDetailsToControls(GetUserById());
    }
    string GetUserIdFromQueryString()
    {
        return string.IsNullOrEmpty(Request.QueryString["q"]) ? null : Request.QueryString["q"].Split(':')[1];
    }

    User GetUserById()
    {
        var userId = GetUserIdFromQueryString();
        if (string.IsNullOrEmpty(userId)) return null;
        var query = ConfigurationManager.AppSettings["qryGetUserDetails"];
        query = SqlHandler.AddConditionToQuery(query, new string[] {"Users.ID=" + userId});
        return UserRepository.GetUser(query, ConfigurationManager.AppSettings["rmapGetUserDetails"]);
    }

    private void SetUserDetailsToControls(User aUser)
    {
        txtLoginName.Text = aUser.LoginName;
        txtPassword.Text = aUser.LoginPassword;
        ddlRole.SelectedValue = aUser.UserRole.Id.ToString(CultureInfo.InvariantCulture);
    }

    protected void LoadUserRoles()
    {
        var query = ConfigurationManager.AppSettings["qryGetUserRoles"];
        var roles = UserRepository.GetUserRoles(query);
        foreach(var role in roles)
        {
            ddlRole.Items.Add(new ListItem(role.Role, role.Id.ToString()));
        }
    }

    User GetUserDetailsFromControls()
    {
        return new User
            {
                LoginName = txtLoginName.Text.Trim(),
                LoginPassword = txtPassword.Text,
                UserRole = new UserRole { Id = Convert.ToInt32(ddlRole.SelectedValue) },
                UpdatedById = CurrentUser.Id
            };
    }

    protected void Update_Click(object sender, EventArgs e)
    {
        UpdateUser();
    }

    void UpdateUser()
    {
        var userId = GetUserIdFromQueryString();
        if (string.IsNullOrEmpty(userId)) return;
        var query = ConfigurationManager.AppSettings["qryUpdateUserDetails"];
        query = SqlHandler.AddConditionToQuery(query, new List<string> { "ID=" + userId });
        var user = GetUserDetailsFromControls();
        var result = UserRepository.UpdateUser(query, user);
        divMessage.Visible = true;
        if (result != 0)
        {
            divMessage.Attributes["class"] = "bg-success";
            divMessage.InnerHtml = "Updated!";
        }
        else
        {
            divMessage.Attributes["class"] = "bg-warning";
            divMessage.InnerHtml = "Updated failed";
        }
    }
}