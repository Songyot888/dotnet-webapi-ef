using System;
using System.Collections.Generic;

namespace dotnet_webapi_ef.Models;

public partial class Result
{
    public uint Rid { get; set; }

    public decimal PayoutRate { get; set; }

    public string Amount { get; set; } = null!;
}
