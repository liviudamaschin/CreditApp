using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreditAppBMG.Entities
{
    [Table("States")]
    public partial class StatesEntity
    {
        //[Key]
        //public int Id { get; set; }
        [Key]
        public string Abbreviation { get; set; }
        public string State { get; set; }
    }
}
