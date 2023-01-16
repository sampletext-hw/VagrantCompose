using System.ComponentModel.DataAnnotations;
using Models.Db.Common;

namespace Models.Db.CompanyInfo
{
    public class InstagramUrlData : VersionedEntity
    {
        [MaxLength(4096)]
        public string Content { get; set; }
    }
}