namespace Models.Configs
{
    public class ClientAccountServiceConfig
    {
        public string[] QuotaIgnoredLogins { get; set; }
        public int MaxInvalidRequestsPerDay { get; set; }
        public int MaxInvalidAttemptsPerDay { get; set; }

        public string SendType { get; set; }
        public bool UseFakeCode { get; set; }
    }
}