using System.Configuration;
using System.Web;
using Model.Repository;
using Persistence;
/// <summary>
/// Summary description for ApplicationState
/// </summary>
public class ApplicationState
{
    private readonly SqlHandler _handler;
    public SqlHandler Handler { get; set; }    
    public IPanCardRepository PanCardRepository { get { return new PanCardRepository(_handler); } }
    public IUserRepository UserRepository { get { return new UserRepository(_handler); } }
    public ApplicationState()
    {
        //
        // TODO: Add constructor logic here
        //
        var connectionString = ConfigurationManager.ConnectionStrings["msAccessConnectionString"];
        Handler = _handler = new SqlHandler(connectionString.ProviderName, connectionString.ConnectionString);
    }
    private static HttpApplicationState State
    {
        get { return HttpContext.Current.Application; }
    }
    public static ApplicationState Instance
    {
        get
        {
            if (State["ApplicationState"] == null)
            {

                State.Lock();
                if (State["ApplicationState"] == null)
                    State["ApplicationState"] = new ApplicationState();
                State.UnLock();
            }
            return State["ApplicationState"] as ApplicationState;
        }
    }
}