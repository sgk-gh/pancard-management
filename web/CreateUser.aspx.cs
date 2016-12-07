using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
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
        var query = ConfigurationManager.AppSettings["qryGetUserRoles"].ToString();
        ddlRole.DataSource = PanCardRepository.GetUserRoles(query).Select(r => r.Role);
        ddlRole.DataBind();
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
        var query = ConfigurationManager.AppSettings["qryInsertUserDetails"].ToString();
        var result = PanCardRepository.InsertUser(query, GetUserDetailsFromControls());
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
        return new User { LoginName = txtLoginName.Text.Trim(), LoginPassword = txtPassword.Text, UserRole = new UserRole { Role = ddlRole.SelectedValue } };
    }
    private void LoadUserGrid(int pageIndex)
    {
        var query = ConfigurationManager.AppSettings["qryGetUserDetails"].ToString();
        var resultMap = ConfigurationManager.AppSettings["rmapGetUserDetails"].ToString();
        grvUsers.BindGridView(PanCardRepository.GetAllUsers(query, resultMap), PageSize, pageIndex);
    }
    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadUserGrid(e.NewPageIndex);
        ClientScript.RegisterClientScriptBlock(GetType(), "IsGridViewAction", "var isGridViewAction = true;", true);
    }
    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = e.RowIndex;
        var row = grvUsers.Rows[index];
        int id = Convert.ToInt32((row.FindControl("lblId") as Label).Text);
        ClientScript.RegisterClientScriptBlock(GetType(), "IsGridViewAction", "var isGridViewAction = true;", true);
    }
}