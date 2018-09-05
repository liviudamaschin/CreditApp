using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditAppBMG.Entities
{
    [Table("DbVersion")]
    public partial class DbVersion
    {
        [Key]
        public string Product { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int? IntVersion { get; set; }
        public byte[] ReleaseNotes { get; set; }
    }
}
