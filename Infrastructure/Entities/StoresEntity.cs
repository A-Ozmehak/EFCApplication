using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class StoresEntity
{
    public int Id { get; set; }

    public string StoreName { get; set; } = null!;

    public virtual ICollection<ProductsEntity> ProductsEntities { get; set; } = new List<ProductsEntity>();
}
