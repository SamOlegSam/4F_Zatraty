﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ZatratyCore.Models;

public partial class Expense
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? ChildId { get; set; }

    public string UserMod { get; set; }

    public DateTime? DateMod { get; set; }

    public string Primechanie { get; set; }

    public int? Sort { get; set; }

    public virtual ICollection<Form4F> Form4Fs { get; set; } = new List<Form4F>();

    public virtual ICollection<TableFillialsExpense> TableFillialsExpenses { get; set; } = new List<TableFillialsExpense>();
}