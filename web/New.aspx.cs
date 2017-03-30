using System;
using System.Configuration;
using System.Web.UI.WebControls;
using Model;
using System.Globalization;

public partial class New : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
        if (IsPostBack) return;
        LoadClients();
    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        var query = ConfigurationManager.AppSettings["qryInsertPanCardDetails"];
        var id = PanCardRepository.InsertPanCard(query, new PanCard
        {
            ApplicationNumber = txtApplicationNumber.Text,
            CouponNumber = txtCouponNumber.Text,
            CustomerName = txtName.Text,
            DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date,
            FatherName = txtFatherName.Text,
            FilePath = UploadAndGetPanCardImageFilePath(),
            PanEntryDate = DateTime.Now,
            CreatedById = CurrentUser.Id,
            ClientId = CurrentUser.UserRole.Role.ToLower() == "admin" ? Convert.ToInt32(ddlClient.SelectedValue) : CurrentUser.Id
        });
        //Response.Redirect("Default.aspx");
        divMessage.Visible = true;
        if (id != 0)
        {
            divMessage.Attributes["class"] = "bg-success";
            divMessage.InnerHtml = "New Pan Card added!";
            ClearControls();
        }
        else
        {
            divMessage.Attributes["class"] = "bg-warning";
            divMessage.InnerHtml = "Error occurred!";
        }
    }

    protected void LoadClients()
    {
        if (CurrentUser.UserRole.Role.ToLower() != "admin") return;
        var query = ConfigurationManager.AppSettings["qryGetAllClients"];
        var clients = UserRepository.GetAllUsers(query);
        foreach (var client in clients)
        {
            ddlClient.Items.Add(new ListItem(client.LoginName, client.Id.ToString(CultureInfo.InvariantCulture)));
        }
        ddlClient.SelectedValue = CurrentUser.Id.ToString(CultureInfo.InvariantCulture);
    }

    void ClearControls()
    {
        txtApplicationNumber.Text = txtCouponNumber.Text = txtFatherName.Text = txtName.Text = txtDateOfBirth.Text = string.Empty;        //filePanImage.Attributes.Clear();
    }

    string UploadAndGetPanCardImageFilePath()
    {
        return !filePanImage.HasFile ? null : filePanImage.UploadFile(Server.MapPath(ConfigurationManager.AppSettings["panCardImagePath"]));
    }
}