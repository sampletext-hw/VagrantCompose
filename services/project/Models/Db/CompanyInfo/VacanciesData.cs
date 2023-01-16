using System.ComponentModel.DataAnnotations;
using Models.Db.Common;

namespace Models.Db.CompanyInfo
{
    public class VacanciesData : VersionedEntity
    {
        [MaxLength(4096)]
        public string Content { get; set; }
    }
}