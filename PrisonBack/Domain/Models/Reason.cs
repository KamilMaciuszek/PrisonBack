﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrisonBack.Domain.Models
{
    [Table("Reason")]
    public class Reason
    {
        [Key]
        public int Id { get; set; }
        public string ReasonName { get; set; }
        public virtual ICollection<Punishment> Punishments { get; set; }
    }
}
