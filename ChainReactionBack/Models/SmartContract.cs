using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChainReactionBack.Models
{
    public class SmartContract
    {
        [Key]
        public int SmartContractId { get; set; }

        [StringLength(256)]
        public string Adress { get; set; }
     
        [StringLength(128)]
        public string Pattern { get; set; }

        // Relation many-to-one
        public int UserId { get; set; }
        public User User { get; set; }
    }
}