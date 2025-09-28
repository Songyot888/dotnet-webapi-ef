using System;
using System.Collections.Generic;

namespace dotnet_webapi_ef.Models;

public partial class User
{
    public uint Uid { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Role { get; set; }

    public string? BankName { get; set; }

    public string? BankNumber { get; set; }

    public decimal Balance { get; set; }

    public DateTime Date { get; set; }

    public virtual ICollection<Lottery> Lotteries { get; set; } = new List<Lottery>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<WalletTxn> WalletTxns { get; set; } = new List<WalletTxn>();
}
