using System;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Web.UI;
using Model;
using Model.Repository;
using Persistence;
/// <summary>
/// Summary description for PanCardBasePage
/// </summary>
public class PanCardBasePage : Page
{
    public PanCardBasePage()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    protected int PageSize { get { return Convert.ToInt32(ConfigurationManager.AppSettings["grvPageSize"]); } }
    protected IPanCardRepository PanCardRepository { get { return ApplicationState.Instance.PanCardRepository; } }
    protected IUserRepository UserRepository { get { return ApplicationState.Instance.UserRepository; } }
    protected SqlHandler SqlHandler
    {
        get { return ApplicationState.Instance.Handler; }
    }
    public static User CurrentUser
    {

        get
        {
            var currentRequest = System.Web.HttpContext.Current.Request;
            if (currentRequest.Cookies["user"] != null && !string.IsNullOrEmpty(currentRequest.Cookies["user"].Value))
            {
                return new JavaScriptSerializer().Deserialize<User>(currentRequest.Cookies["user"].Value);
            }
            else
            {
                return null;
            }
        }
    }
    protected void ClearCurrentUserCookie()
    {
        Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
    }
}