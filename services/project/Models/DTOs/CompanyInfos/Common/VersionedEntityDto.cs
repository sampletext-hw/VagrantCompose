using System;
using Models.DTOs.Misc;

namespace Models.DTOs.CompanyInfos.Common
{
    public class VersionedEntityDto : IDto
    {
        public uint Version { get; set; }

        public DateTime DateTime { get; set; }
    }
}