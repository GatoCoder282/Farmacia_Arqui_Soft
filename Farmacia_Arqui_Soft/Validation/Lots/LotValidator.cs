using System;
using System.Text.RegularExpressions;
using Farmacia_Arqui_Soft.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;

namespace Farmacia_Arqui_Soft.Validations.Lots
{
    public class LotValidator : IValidator<Lot>
    {
        public ValidationResult Validate(Lot lot)
        {
            var result = new ValidationResult();

            // MedicineId: requerido y > 0
            if (lot.MedicineId <= 0)
                result.AddError("MedicineId", "Debes seleccionar un medicamento válido.");

            // BatchNumber: requerido, 3-30, alfanumérico con guiones opcionales
            if (string.IsNullOrWhiteSpace(lot.BatchNumber))
            {
                result.AddError("BatchNumber", "El número de lote es obligatorio.");
            }
            else
            {
                var bn = lot.BatchNumber.Trim();
                if (bn.Length < 3 || bn.Length > 30)
                    result.AddError("BatchNumber", "El número de lote debe tener entre 3 y 30 caracteres.");
                else if (!Regex.IsMatch(bn, @"^[A-Za-z0-9\-]+$"))
                    result.AddError("BatchNumber", "El número de lote solo admite letras, números y guiones.");
            }

            // ExpirationDate: debe ser futura (después de hoy)
            var today = DateTime.Today;
            if (lot.ExpirationDate <= today)
                result.AddError("ExpirationDate", "La fecha de vencimiento debe ser posterior a hoy.");

            // Quantity: > 0
            if (lot.Quantity <= 0)
                result.AddError("Quantity", "La cantidad debe ser mayor a 0.");

            // UnitCost: >= 0.01
            if (lot.UnitCost < 0.01m)
                result.AddError("UnitCost", "El costo unitario debe ser al menos 0.01.");

            return result;
        }
    }
}
