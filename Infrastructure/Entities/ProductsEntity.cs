using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ProductsEntity
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal? Price { get; set; }

    public int? StoreId { get; set; }

    public virtual StoresEntity? Store { get; set; }
}
