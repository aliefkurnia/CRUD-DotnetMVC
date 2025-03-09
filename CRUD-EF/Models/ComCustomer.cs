using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUD_EF.Models;

[Table("COM_CUSTOMER")]
public partial class COM_CUSTOMER
{
    [Key]
    [Column("COM_CUSTOMER_ID")]
    public int ComCustomerId { get; set; }

    [Column("CUSTOMER_NAME")]
    [StringLength(100)]
    [Unicode(false)]
    public string? CustomerName { get; set; }

    public virtual ICollection<SO_ORDER> Orders { get; set; } = new List<SO_ORDER>();

}
