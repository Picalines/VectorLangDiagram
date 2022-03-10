﻿using VectorLang.SyntaxTree;

namespace VectorLang.Parsing;

internal static class ParseType
{
    public static readonly Parser<TypeNode> Type =
        from identifier in ParseToken.Identifier.WithError("type expected")
        select new TypeNode(identifier.Value);
}
