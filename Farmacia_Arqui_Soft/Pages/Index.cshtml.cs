using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/Auth/Login");
            }
            return Page();
        }
    }
}
