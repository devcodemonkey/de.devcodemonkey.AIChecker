using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class RequestObject : IValue
{
    public Guid RequestObjectId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
