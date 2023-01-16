using System.ComponentModel.DataAnnotations;
using Models.Db.Common;

namespace Models.Db.CompanyInfo
{
    public class ApplicationStartupImageData : VersionedEntity
    {
        [MaxLength(40)]
        public string BackgroundImage { get; set; }

        [MaxLength(40)]
        public string ForegroundImage { get; set; }
    }
}