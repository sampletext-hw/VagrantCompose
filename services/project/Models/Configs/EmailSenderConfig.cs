namespace Models.Configs
{
    public class EmailSenderConfig
    {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool UseSSL { get; set; }
        public string SmtpLogin { get; set; }
        public string SmtpPassword { get; set; }
        public string SenderEmail { get; set; }
        public string SenderVisibleName { get; set; }
    }
}