﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ZatratyCore.Models;

public partial class Role
{
    public int Id { get; set; }

    public int FillialId { get; set; }

    public int PlansId { get; set; }

    public string UserMod { get; set; }

    public DateTime? DateMod { get; set; }

    public virtual Fillial Fillial { get; set; }

    public virtual Plan Plans { get; set; }
}