using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models;

public partial class ResultSet
{
    public Guid ResultSetId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
