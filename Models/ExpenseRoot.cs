#nullable disable
using System;
using System.Collections.Generic;

namespace ZatratyCore.Models;

public partial class ExpenseRoot
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Expense> Childs { get; set; }// = new List<Expense>();

    public List<ExpenseRoot> Childspis { get; set; }

    public Expense? Parent { get; set; } = null;
}