﻿using System.ComponentModel;

namespace VectorLang.Model;

public enum UnaryOperator
{
    [Description("+"), Format("+{0}")] Plus,
    [Description("-"), Format("-{0}")] Minus,
    [Description("not"), Format("not {0}")] Not,
}
