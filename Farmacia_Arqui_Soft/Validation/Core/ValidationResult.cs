using System.Collections.Generic;

namespace Farmacia_Arqui_Soft.Validations
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;


        public Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        public void AddError(string property, string message)
        {
            if (!Errors.ContainsKey(property))
                Errors.Add(property, message);
        }
    }
}
