using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Farmacia_Arqui_Soft.Pages.Shared
{
    public class SuccessModel : PageModel
    {
        public string Message { get; set; } = string.Empty;

        public void OnGet(string message)
        {
            Message = message ?? "Operación completada exitosamente.";
        }
    }
}
