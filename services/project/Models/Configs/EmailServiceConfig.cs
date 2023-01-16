namespace Models.Configs
{
    public class EmailServiceConfig
    {
        public string[] Emails { get; set; }

        public int DelayBetweenEmailsMs { get; set; }

        public bool SaveFile { get; set; }
    }
}