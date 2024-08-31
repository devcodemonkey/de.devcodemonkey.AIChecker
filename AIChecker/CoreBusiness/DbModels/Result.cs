﻿using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.DbModels;

public partial class Result
{
    public Guid ResultId { get; set; }

    public Guid? QuestionId { get; set; }

    public Guid ResultSetId { get; set; }

    public Guid RequestObjectId { get; set; }

    public Guid RequestReasonId { get; set; }

    public string RequestId { get; set; } = null!;

    public string Asked { get; set; } = null!;

    public string Message { get; set; } = null!;

    public Guid ModelId { get; set; }

    public Guid SystemPromtId { get; set; }

    public double Temperature { get; set; }

    public int MaxTokens { get; set; }

    public int PromtTokens { get; set; }

    public int CompletionTokens { get; set; }

    public int TotalTokens { get; set; }

    public DateTime RequestCreated { get; set; }

    public DateTime RequestStart { get; set; }

    public DateTime RequestEnd { get; set; }

    public virtual ICollection<ExpectedsResult> ExpectedsResults { get; set; } = new List<ExpectedsResult>();

    public virtual Model Model { get; set; } = null!;

    public virtual Question? Question { get; set; }

    public virtual RequestObject RequestObject { get; set; } = null!;

    public virtual RequestReason RequestReason { get; set; } = null!;

    public virtual ResultSet ResultSet { get; set; } = null!;

    public virtual SystemPromt SystemPromt { get; set; } = null!;
}
