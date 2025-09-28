using System;
using System.Collections.Generic;

namespace dotnet_webapi_ef.Models;

public partial class Lottery
{
    public uint Lid { get; set; }

    public uint Uid { get; set; }

    public string? Number { get; set; }

    public decimal Price { get; set; }

    public int? Total { get; set; }

    public DateTime Date { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User UidNavigation { get; set; } = null!;
}
