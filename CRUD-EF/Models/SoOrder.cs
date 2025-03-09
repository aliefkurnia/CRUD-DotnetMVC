using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUD_EF.Models;

[Table("SO_ORDER")]
public partial class SO_ORDER
{
    [Key]
    [Column("SO_ORDER_ID")]
    public long SoOrderId { get; set; }

    [Column("ORDER_NO")]
    [StringLength(20)]
    [Unicode(false)]
    public string OrderNo { get; set; } = null!;

    [Column("ORDER_DATE",TypeName = "datetime")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Column("COM_CUSTOMER_ID")]
    public int ComCustomerId { get; set; }

    [Column("ADDRESS")]
    [StringLength(100)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [ForeignKey("ComCustomerId")]
    public virtual COM_CUSTOMER? Customer { get; set; }

    public List<SO_ITEM> Items { get; set; } = new List<SO_ITEM>(); 
}
