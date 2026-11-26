using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;
namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    private const decimal SurplusDiscount = 0.30m;

    private const int BoxKg = 5;

    private const int ShelfLifeDays = 7;

    private const int LowStockKg = 50;

    public Surplus Surplus { get; } = new Surplus()
    {
        Product = "Cereza Picota",
        Producer = "Huerta Los Castaños",
        Village = "Cabezuela del Valle",
        HarvestDate = DateTime.Today.AddDays(-2),
        AvailableKg = 343,
        ReservedKg = 120,
        PricePerKg = 3.20m,
        Note = "<b>Recoger antes de las 14:00</b>",

    };

    public int DaysToConsume { get; private set; }

    public DateTime ConsumeDate { get; private set; }

    public decimal SurplusPrice { get; private set; }

    public int NewAvailableKg { get; private set; }

    public int Boxes { get; private set; }

    public int RestKg { get; private set; }

    public decimal SurplusValue { get; private set; }

    public string Status { get; private set; }

    public bool SoldOut { get; private set; }

    public void OnGet()
    {
        NewAvailableKg = Surplus.AvailableKg - Surplus.ReservedKg;

        ConsumeDate = Surplus.HarvestDate.AddDays(ShelfLifeDays);

        DaysToConsume = (ConsumeDate - DateTime.Today).Days;

        SurplusPrice = Surplus.PricePerKg * (1 - SurplusDiscount);

        Boxes = NewAvailableKg / BoxKg;

        RestKg = NewAvailableKg % BoxKg;

        SurplusValue = NewAvailableKg * SurplusPrice;

        if (NewAvailableKg == 0)
        {
            Status = "Agotado";
            SoldOut = true;
        }
        else if (NewAvailableKg < LowStockKg)
        {
            Status = "¡Últimos kilos!";
        }
        else
        {
            Status = "Disponible";
        }

        if (Surplus.Note == null)
        {
            Surplus.Note = "Sin observaciones";
        }

    }
}