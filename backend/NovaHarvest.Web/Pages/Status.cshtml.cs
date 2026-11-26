using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;
namespace NovaHarvest.Web.Pages;

public class StatusModel : PageModel
{
    public DateTime ServerDateTime { get; private set; }
    public int Visits { get; private set; }
    public string ServerName { get; private set; }
    public void OnGet()
    {
        ServerDateTime = DateTime.Now;
        SiteStats.Visits++;
        Visits = SiteStats.Visits;

        ServerName = Environment.MachineName;

    }
}