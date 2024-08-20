using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class SystemPromt : IValue
{
    public Guid SystemPromtId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
