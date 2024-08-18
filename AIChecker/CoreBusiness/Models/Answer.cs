﻿using System;
using System.Collections.Generic;

namespace de.devcodemonkey.AIChecker.CoreBusiness.Models;

public partial class Answer
{
    public Guid AnswerId { get; set; }

    public Guid? QuestionId { get; set; }

    public string Value { get; set; } = null!;

    public virtual Question? Question { get; set; }
}
