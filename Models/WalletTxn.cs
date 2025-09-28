using System;
using System.Collections.Generic;

namespace dotnet_webapi_ef.Models;

public partial class WalletTxn
{
    public ulong Wid { get; set; }

    public uint Uid { get; set; }

    public decimal? TopUp { get; set; }

    public decimal? Withdraw { get; set; }

    public bool Status { get; set; }

    public DateTime Date { get; set; }

    public virtual User UidNavigation { get; set; } = null!;
}
