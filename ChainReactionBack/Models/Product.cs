using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChainReactionBack.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [DisplayName("Name")]
        [StringLength(128)]
        public string Name { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        // Relation one-to-many
        public ICollection<Transaction> Transactions { get; set; }

        // Relation many-to-one
        public int UserId { get; set; }
        public User User { get; set; }
    }
}