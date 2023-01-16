using System;
using System.ComponentModel.DataAnnotations;
using Models.Db.Common;

namespace Models.Db.MobilePushes
{
    public class MobilePushBase : IdEntity
    {
        [MaxLength(64)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Content { get; set; }

        [MaxLength(40)]
        public string Image { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}