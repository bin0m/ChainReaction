using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChainReactionBack.Models
{
    [Table("recResults")]
    public class recResult
    {
        public string rownames { get; set; }
        public string idrecom { get; set; }
    }
}