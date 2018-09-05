using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CreditAppBMG.DAL.Entities
{
    [Table("States")]
    public class StatesEntity
    {
        [Column("Id")]
        public int Id { get; set; } // Id

        [Column("Abbreviation")]
        [Key]
        public string Abbreviation { get; set; } // (Primary key)

        [Column("State")]
        public string State { get; set; } // (Primary key)
    }
}
