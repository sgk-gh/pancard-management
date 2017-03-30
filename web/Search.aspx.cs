using System;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Model;
using System.Configuration;

public partial class Search : PanCardBasePage
{
    readonly IList<int> _columnIndexesToHide = new List<int>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (CurrentUser == null) return;
        if (CurrentUser.UserRole.Role.ToLower() != "admin")
        {
            _columnIndexesToHide.Add(6);
        }
        if (IsPostBack) return;
        LoadClients();       
        grvPanDetails.BindGridView(GetAllPanCardDetails(), PageSize, 0, _columnIndexesToHide);
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grvPanDetails.BindGridView(GetPanCardDetailsBySearchTerms(), PageSize, 0, _columnIndexesToHide);
    }

    protected IEnumerable<PanCard> GetAllPanCardDetails()
    {
        var query = ConfigurationManager.AppSettings["qryGetAllPanCardDetails"];
        if (CurrentUser.UserRole.Role.ToLower() != "admin")
        {
            query = SqlHandler.AddConditionToQuery(query, new List<string> { "p.CreatedById=" + CurrentUser.Id + " OR clientId=" + CurrentUser.Id });

        }
        return PanCardRepository.GetAllPanCards(query, ConfigurationManager.AppSettings["rmapGetAllPanCardDetails"]);
    }

    protected IEnumerable<PanCard> GetPanCardDetailsBySearchTerms()
    {
        var query = ConfigurationManager.AppSettings["qryGetAllPanCardDetails"];
        var conditions = new List<string> ();
        
        if (chkApplicationNumber.Checked && txtApplicationNumber.Text.Trim() != "")
        {            
            conditions.Add("ApplicationNumber='" + txtApplicationNumber.Text + "'");            
        }
        if (chkName.Checked && txtName.Text.Trim() != "")
        {
            conditions.Add("CustomerName like '%" + txtName.Text + "%'");
        }
        if (chkDateOfBirth.Checked && dtDateOfBirth.Text.Trim() != "")
        {
            var dateOfBirth = DateTime.ParseExact(dtDateOfBirth.Text.Trim(), "dd/mm/yyyy", CultureInfo.InvariantCulture);
            conditions.Add("DateOfBirth=#" + dateOfBirth.ToString("yyyy/mm/dd") + "#");
        }
        if (chkFatherName.Checked && txtFatherName.Text.Trim() != "")
        {            
            conditions.Add("FatherName like '%" + txtFatherName.Text + "%'");
        }
        if (chkPanEntryDate.Checked && dtPanEntryDate.Text.Trim() != "")
        {
            var panEntryDate = DateTime.ParseExact(dtPanEntryDate.Text.Trim(), "dd/mm/yyyy", CultureInfo.InvariantCulture);
            conditions.Add("datevalue(PanEntryDate)=#" + panEntryDate.ToString("yyyy/mm/dd") + "#");
        }
        if (CurrentUser.UserRole.Role.ToLower() == "admin" && chkClient.Checked)
        {
            conditions.Add("p.ClientId=" + ddlClient.SelectedItem.Value);
        }
        if(CurrentUser.UserRole.Role.ToLower() != "admin")
        {
            conditions.Add("(p.CreatedById=" + CurrentUser.Id + "OR clientId=" + CurrentUser.Id + ")");
        }
        query = SqlHandler.AddConditionToQuery(query, conditions);
        return PanCardRepository.GetAllPanCards(query, ConfigurationManager.AppSettings["rmapGetAllPanCardDetails"]);
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
            PanCardRepository.UpdatePanCard(query, new PanCard { UpdatedById = CurrentUser.Id });
        }
        grvPanDetails.BindGridView(GetPanCardDetailsBySearchTerms(), PageSize, 0, _columnIndexesToHide);
    }

    protected void PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvPanDetails.BindGridView(GetPanCardDetailsBySearchTerms(), PageSize, e.NewPageIndex, _columnIndexesToHide);
    }
}