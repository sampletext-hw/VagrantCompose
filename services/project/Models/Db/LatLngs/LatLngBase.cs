using System.ComponentModel.DataAnnotations;
using Models.Db.Common;

namespace Models.Db.LatLngs
{
    public class LatLngBase : IdEntity
    {
        [Range(-90, 90)]
        public float Lat { get; set; }

        [Range(-180, 180)]
        public float Lng { get; set; }
    }
}