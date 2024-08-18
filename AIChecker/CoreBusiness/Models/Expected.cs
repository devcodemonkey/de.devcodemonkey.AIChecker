using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models;

public partial class Expected
{
    public Guid ExpectedId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ICollection<ExpectedsResult> ExpectedsResults { get; set; } = new List<ExpectedsResult>();
}
