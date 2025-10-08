namespace Farmacia_Arqui_Soft.Repository
{
    public class RepositoryBase
    {
        protected static int ToIntId(object id)
        {
            switch (id)
            {
                case int i: return i;
                case long l: return checked((int)l);
                case short s: return s;
                case byte b: return b;
                case uint ui: return checked((int)ui);
                case ulong ul: return checked((int)ul);
                case string str when int.TryParse(str, out var parsed): return parsed;
                default: throw new ArgumentException($"El id '{id}' no es convertible a int.", nameof(id));
            }
        }
    }
}
