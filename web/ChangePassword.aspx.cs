using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Change_Click(object sender, EventArgs e)
    {
        var query = ConfigurationManager.AppSettings["qryChangePassword"].ToString();
        query = SqlHandler.AddConditionToQuery(query, new List<string> { "Id=" + CurrentUser.Id });
        var result = UserRepository.UpdateUser(query, new Model.User { LoginPassword = txtNewPassword.Text });
        divMessage.Visible = true;
        if (result != 0)
        {
            divMessage.Attributes["class"] = "bg-success";
            divMessage.InnerHtml = "Password has been changed!";
            ClearControls();
        }
        else
        {
            divMessage.Attributes["class"] = "bg-warning";
            divMessage.InnerHtml = "Error occurred!";
        }
    }
    void ClearControls()
    {
        txtNewPassword.Text = txtConfirmPassword.Text = string.Empty;
    }
}