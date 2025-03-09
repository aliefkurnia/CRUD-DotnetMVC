using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_EF.Models;

[Table("SO_ITEM")]
public partial class SO_ITEM
{
    [Key]
    [Column("SO_ITEM_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SoItemId { get; set; }

    [Column("SO_ORDER_ID")]
    public long SoOrderId { get; set; }

    [Column("ITEM_NAME")]
    [StringLength(100)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column("QUANTITY")]
    public int Quantity { get; set; }

    [Column("PRICE")]
    public double Price { get; set; }

    [ForeignKey("SoOrderId")]
    public virtual SO_ORDER? SoOrder { get; set; }
}
