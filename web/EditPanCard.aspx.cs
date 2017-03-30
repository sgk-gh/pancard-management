using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using Model;

public partial class EditPanCard : PanCardBasePage
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
        LoadClients();
        SetPanCardDetailsToControls(GetPancardDetailsById());        
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
    }
    protected void Update_Click(object sender, EventArgs e)
    {
        UpdatePanCard();
    }

    string GetPanCardIdFromQueryString()
    {
        return string.IsNullOrEmpty(Request.QueryString["q"]) ? null : Request.QueryString["q"].Split(':')[1];
    }

    PanCard GetPancardDetailsById()
    {
        var panCardId = GetPanCardIdFromQueryString();
        if (string.IsNullOrEmpty(panCardId)) return null;
        var query = string.Format(ConfigurationManager.AppSettings["qryGetPanCardDetails"], panCardId);
        return PanCardRepository.GetPanCard(query, ConfigurationManager.AppSettings["rmapGetAllPanCardDetails"]);
    }

    void SetPanCardDetailsToControls(PanCard aPancard)
    {
        if (aPancard == null) return;
        txtApplicationNumber.Text = aPancard.ApplicationNumber;
        txtCouponNumber.Text = aPancard.CouponNumber;
        txtDateOfBirth.Text = aPancard.DateOfBirth.ToString("dd/MM/yyyy");
        txtFatherName.Text = aPancard.FatherName;
        txtCustomerName.Text = aPancard.CustomerName;
        hlPanImage.Text = aPancard.FilePath;
        hlPanImage.NavigateUrl = "PanCardImages/" + aPancard.FilePath;
        ddlClient.SelectedValue = CurrentUser.UserRole.Role.ToLower() != "admin" ? CurrentUser.Id.ToString(CultureInfo.InvariantCulture) : aPancard.ClientId.ToString(CultureInfo.InvariantCulture);
    }

    PanCard GetPanCardValuesFromControls()
    {
        return new PanCard
        {
            ApplicationNumber = txtApplicationNumber.Text,
            CouponNumber = txtCouponNumber.Text,
            CustomerName = txtCustomerName.Text,
            DateOfBirth = DateTime.ParseExact(txtDateOfBirth.Text.Trim(), "dd/mm/yyyy", CultureInfo.InvariantCulture).Date,
            FatherName = txtFatherName.Text,
            FilePath= GetPanCardImageFile(),
            //UpdatedAt = DateTime.Now,
            UpdatedById = CurrentUser.Id
        };
    }

    void UpdatePanCard()
    {
        var panCardId = GetPanCardIdFromQueryString();
        if (string.IsNullOrEmpty(panCardId)) return;
        var query = ConfigurationManager.AppSettings["qryUpdatePanCardDetails"];
        query = SqlHandler.AddConditionToQuery(query, new List<string> {"ID=" + panCardId});
        var panCard = GetPanCardValuesFromControls();
        panCard.ClientId = CurrentUser.UserRole.Role.ToLower() == "admin" ? Convert.ToInt32(ddlClient.SelectedValue) : GetPancardDetailsById().ClientId;
        var result = PanCardRepository.UpdatePanCard(query, panCard);
        divMessage.Visible = true;
        if (result != 0)
        {
            divMessage.Attributes["class"] = "bg-success";
            divMessage.InnerHtml = "Updated!";
            hlPanImage.Text = panCard.FilePath;
            hlPanImage.NavigateUrl= "PanCardImages/" + panCard.FilePath;
        }
        else
        {
            divMessage.Attributes["class"] = "bg-warning";
            divMessage.InnerHtml = "Updated failed";
        }
    }

    string GetPanCardImageFile()
    {
        return !filePanImage.HasFile ? GetPancardDetailsById().FilePath : SavePanCardImageFile();
    }

    protected string SavePanCardImageFile()
    {        
        var fileName = txtApplicationNumber.Text + "_" + txtCustomerName.Text.Replace(" ", "_").Replace(".", "_") + "_" + filePanImage.FileName;        
        return filePanImage.UploadFile(Server.MapPath(ConfigurationManager.AppSettings["panCardImagePath"]), fileName);
    }
}