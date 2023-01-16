using System.ComponentModel.DataAnnotations;
using Models.Db.Common;

namespace Models.Db.CompanyInfo
{
    public class AboutData : VersionedEntity
    {
        [MaxLength(40)]
        public string Image { get; set; }
        
        [MaxLength(128)]
        public string Title { get; set; }
        
        [MaxLength(4096)]
        public string Content { get; set; }
    }
}