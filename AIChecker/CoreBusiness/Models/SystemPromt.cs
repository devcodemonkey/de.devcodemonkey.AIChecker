﻿using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models;

public partial class SystemPromt
{
    public Guid SystemPromtId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
