using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model;
using Persistence;

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
        SetPanCardDetailsToControls(GetPancardDetailsById());        
    }

    protected void Update_Click(object sender, EventArgs e)
    {
        UpdatePanCard();
    }

    string GetPanCardIdFromQueryString()
    {
        if (string.IsNullOrEmpty(Request.QueryString["q"])) return null;
        return Request.QueryString["q"].Split(':')[1];
    }

    PanCard GetPancardDetailsById()
    {
        var panCardId = GetPanCardIdFromQueryString();
        if (string.IsNullOrEmpty(panCardId)) return null;
        var query = ConfigurationManager.AppSettings["qryGetAllPanCardDetails"].ToString();
        var conditions = new List<string> { { "ID=" + panCardId } };
        query = SqlHandler.AddConditionToQuery(query, conditions);
        return PanCardRepository.GetPanCard(query);
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
            UpdatedAt = DateTime.Now,
            UpdatedBy = CurrentUser.Id
        };
    }

    void UpdatePanCard()
    {
        var panCardId = GetPanCardIdFromQueryString();
        if (string.IsNullOrEmpty(panCardId)) return;
        var query = ConfigurationManager.AppSettings["qryUpdatePanCardDetails"].ToString();
        var conditions = new List<string> { { "ID=" + panCardId } };
        query = SqlHandler.AddConditionToQuery(query, conditions);
        var panCard = GetPanCardValuesFromControls();
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
        if (!filePanImage.HasFile) return GetPancardDetailsById().FilePath;
        return SavePanCardImageFile();
    }

    protected string SavePanCardImageFile()
    {        
        var fileName = txtApplicationNumber.Text + "_" + txtCustomerName.Text.Replace(" ", "_").Replace(".", "_") + "_" + filePanImage.FileName;        
        return filePanImage.UploadFile(Server.MapPath(ConfigurationManager.AppSettings["panCardImagePath"].ToString()), fileName);        
    }
}