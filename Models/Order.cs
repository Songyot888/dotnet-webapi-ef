using System;
using System.Collections.Generic;

namespace dotnet_webapi_ef.Models;

public partial class Order
{
    public ulong Oid { get; set; }

    public uint Uid { get; set; }

    public uint Lid { get; set; }

    public DateTime Date { get; set; }

    public bool? Status { get; set; }

    public virtual Lottery LidNavigation { get; set; } = null!;

    public virtual User UidNavigation { get; set; } = null!;
}
