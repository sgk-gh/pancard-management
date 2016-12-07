using System;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Model;
using System.Configuration;

public partial class Search : PanCardBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
        if (IsPostBack) return;
        grvPanDetails.BindGridView(GetAllPanCardDetails(), PageSize, 0);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grvPanDetails.BindGridView(GetPanCardDetailsBySearchTerms(), PageSize, 0);
    }
   
    protected IEnumerable<PanCard> GetAllPanCardDetails()
    {
        var query = ConfigurationManager.AppSettings["qryGetAllPanCardDetails"];
        var resultMap = ConfigurationManager.AppSettings["rmapGetAllPanCardDetails"];
        return PanCardRepository.GetAllPanCards(query, resultMap);

    }

    protected IEnumerable<PanCard> GetPanCardDetailsBySearchTerms()
    {
        var query = ConfigurationManager.AppSettings["qryGetAllPanCardDetails"];
        var conditions = new List<string> ();
        
        if (txtApplicationNumber.Text.Trim() != "")
        {            
            conditions.Add("ApplicationNumber='" + txtApplicationNumber.Text + "'");
            
        }
        if (txtName.Text.Trim() != "")
        {
            conditions.Add("CustomerName like '%" + txtName.Text + "%'");
        }
        if (dtDateOfBirth.Text.Trim() != "")
        {
            var dateOfBirth = DateTime.ParseExact(dtDateOfBirth.Text.Trim(), "dd/mm/yyyy", CultureInfo.InvariantCulture);
            conditions.Add("DateOfBirth=#" + dateOfBirth.ToString("yyyy/mm/dd") + "#");
        }
        if (txtFatherName.Text.Trim() != "")
        {            
            conditions.Add("FatherName like '%" + txtFatherName.Text + "%'");
        }
        if (dtPanEntryDate.Text.Trim() != "")
        {
            var panEntryDate = DateTime.ParseExact(dtPanEntryDate.Text.Trim(), "dd/mm/yyyy", CultureInfo.InvariantCulture);
            conditions.Add("PanEntryDate=#" + panEntryDate.ToString("yyyy/mm/dd") + "#");
        }
        query = SqlHandler.AddConditionToQuery(query, conditions);
        return PanCardRepository.GetAllPanCards(query);
    }

    
    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        var index = e.RowIndex;
        var row = grvPanDetails.Rows[index];
        var label = row.FindControl("lblId") as Label;
        if (label != null)
        {
            var id = Convert.ToInt32(label.Text);
            var query = ConfigurationManager.AppSettings["qryDeletePanCardDetails"];
            var conditions = new List<string> {  "ID=" + id  };
            query = SqlHandler.AddConditionToQuery(query, conditions);
            PanCardRepository.UpdatePanCard(query, new PanCard { UpdatedAt = DateTime.Now, UpdatedBy = CurrentUser.Id });
        }
        grvPanDetails.BindGridView(GetPanCardDetailsBySearchTerms(), PageSize, 0);
    }

    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvPanDetails.BindGridView(GetPanCardDetailsBySearchTerms(), PageSize, e.NewPageIndex);
    }
}