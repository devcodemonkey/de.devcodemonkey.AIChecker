using System;
using System.Collections.Generic;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Model : IValue
{
    public Guid ModelId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
