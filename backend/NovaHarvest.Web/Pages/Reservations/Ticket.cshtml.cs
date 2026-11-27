using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;
namespace NovaHarvest.Web.Pages;

public class TicketModel : PageModel
{
    public Ticket Ticket { get; } = new Ticket()
    {
        Product = "Cereza Picota",
        Kg = 20,
        Place = "Cooperativa del Valle del Jerte",

    };
}