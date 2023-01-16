using Models.Db.Common;

namespace Models.Db.CompanyInfo
{
    public class ApplicationTerminationData : VersionedEntity
    {
        public bool Terminated { get; set; }
    }
}