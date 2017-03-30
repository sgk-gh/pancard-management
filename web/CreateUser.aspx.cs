using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using Model;

public partial class CreateUser : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
        divMessage.Visible = false;
        if (IsPostBack) return;
        ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", "var isPostBack = false;", true);
        LoadUserRoles();
        LoadUserGrid(0);
    }
    protected void LoadUserRoles()
    {
        var query = ConfigurationManager.AppSettings["qryGetUserRoles"];
        var roles = UserRepository.GetUserRoles(query);
        foreach (var role in roles)
        {
            ddlRole.Items.Add(new ListItem(role.Role, role.Id.ToString(CultureInfo.InvariantCulture)));
        }
    }
    protected void Submit_Click(object sender, EventArgs e)
    {        
        InsertUserDetails();
        LoadUserGrid(0);
        ResetControls();
    }
    private void ResetControls()
    {
        txtLoginName.Text = "";
        txtPassword.Text = "";
        ddlRole.SelectedIndex = 0;
    }
    private void InsertUserDetails()
    {
        var query = ConfigurationManager.AppSettings["qryInsertUserDetails"];
        var result = UserRepository.InsertUser(query, GetUserDetailsFromControls());
        divMessage.Visible = true;
        if (result != 0)
        {
            divMessage.Attributes["class"] = "bg-success";
            divMessage.InnerHtml = "New client added!";
        }
        else
        {
            divMessage.Attributes["class"] = "bg-warning";
            divMessage.InnerHtml = "Error occurred!";
        }
    }
    private User GetUserDetailsFromControls()
    {
        return new User { LoginName = txtLoginName.Text.Trim(), LoginPassword = txtPassword.Text, UserRole = new UserRole { Id = Convert.ToInt32(ddlRole.SelectedValue) } };
    }
    private void LoadUserGrid(int pageIndex)
    {
        var query = ConfigurationManager.AppSettings["qryGetAllUserDetails"];
        var resultMap = ConfigurationManager.AppSettings["rmapGetUserDetails"];
        grvUsers.BindGridView(UserRepository.GetAllUsers(query, resultMap), PageSize, pageIndex, new List<int>());
    }
    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadUserGrid(e.NewPageIndex);
        ClientScript.RegisterClientScriptBlock(GetType(), "IsGridViewAction", "var isGridViewAction = true;", true);
    }
    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var index = e.RowIndex;
        var row = grvUsers.Rows[index];
        var label = row.FindControl("lblId") as Label;
        if (label != null)
        {
            var id = Convert.ToInt32(label.Text);
            var query = ConfigurationManager.AppSettings["qryDeleteUser"];
            var conditions = new List<string> { "ID=" + id };
            query = SqlHandler.AddConditionToQuery(query, conditions);
            UserRepository.UpdateUser(query, CurrentUser);
        }
        LoadUserGrid(0);
        ClientScript.RegisterClientScriptBlock(GetType(), "IsGridViewAction", "var isGridViewAction = true;", true);

    }
}