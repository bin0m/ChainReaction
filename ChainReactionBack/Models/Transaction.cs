using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChainReactionBack.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public decimal Value { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        
        // Relation many-to-one
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Relation many-to-one
        public int FromUserId { get; set; }
        public User FromUser { get; set; }
        public int ToUserId { get; set; }
        public User ToUser { get; set; }


    }
}