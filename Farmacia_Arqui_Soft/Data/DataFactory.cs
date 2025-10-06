namespace Farmacia_Arqui_Soft.Data
{
    public static class DataFactory
    {
        public static IDataOperations Create(string type)
        {
            return type.ToLower() switch
            {
                "provider" => new ProviderData(),
                "lot" => new LotData(),
                _ => throw new ArgumentException($"Unknown data type: {type}")
            };
        }
    }
}
