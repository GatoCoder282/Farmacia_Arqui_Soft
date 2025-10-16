using System;
using System.Text.RegularExpressions;
using FluentResults;
using Farmacia_Arqui_Soft.Domain.Models;
using Farmacia_Arqui_Soft.Validations.Interfaces;
using Farmacia_Arqui_Soft.Validations.Core;

namespace Farmacia_Arqui_Soft.Validations.Lots
{
    public class LotValidator : IValidator<Lot>
    {
        public Result Validate(Lot lot)
        {
            var result = Result.Ok();

            // MedicineId: requerido y > 0
            if (lot.MedicineId <= 0)
                result = result.WithFieldError("MedicineId", "Debes seleccionar un medicamento válido.");

            // BatchNumber: requerido, 3-30, alfanumérico con guiones opcionales
            if (string.IsNullOrWhiteSpace(lot.BatchNumber))
            {
                result = result.WithFieldError("BatchNumber", "El número de lote es obligatorio.");
            }
            else
            {
                var bn = lot.BatchNumber.Trim();
                if (bn.Length < 3 || bn.Length > 30)
                    result = result.WithFieldError("BatchNumber", "El número de lote debe tener entre 3 y 30 caracteres.");
                else if (!Regex.IsMatch(bn, @"^[A-Za-z0-9\-]+$"))
                    result = result.WithFieldError("BatchNumber", "El número de lote solo admite letras, números y guiones.");
            }

            // ExpirationDate: debe ser posterior a hoy (sin hora)
            var today = DateTime.Today;
            if (lot.ExpirationDate <= today)
                result = result.WithFieldError("ExpirationDate", "La fecha de vencimiento debe ser posterior a hoy.");

            // Quantity: > 0
            if (lot.Quantity <= 0)
                result = result.WithFieldError("Quantity", "La cantidad debe ser mayor a 0.");

            // UnitCost: >= 0.01
            if (lot.UnitCost < 0.01m)
                result = result.WithFieldError("UnitCost", "El costo unitario debe ser al menos 0.01.");

            return result;
        }
    }
}
