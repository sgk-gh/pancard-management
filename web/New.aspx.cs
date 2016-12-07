using System;
using System.Configuration;
using Model;
using System.Globalization;

public partial class New : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
    }
    protected void Submit_Click(object sender, EventArgs e)
    {
        var query = ConfigurationManager.AppSettings["qryInsertPanCardDetails"].ToString();
        var id = PanCardRepository.InsertPanCard(query, new PanCard
        {
            ApplicationNumber = txtApplicationNumber.Text,
            CouponNumber = txtCouponNumber.Text,
            CustomerName = txtName.Text,
            DateOfBirth = DateTime.ParseExact(dtDateOfBirth.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture).Date,
            FatherName = txtFatherName.Text,
            FilePath = UploadAndGetPanCardImageFilePath(),
            PanEntryDate = DateTime.Now,
            CreatedBy=CurrentUser.Id            
        });
        Response.Redirect("Default.aspx");
    }
    string UploadAndGetPanCardImageFilePath()
    {
        if (!filePanImage.HasFile) return null;
        return filePanImage.UploadFile(Server.MapPath(ConfigurationManager.AppSettings["panCardImagePath"].ToString()));
    }    
}