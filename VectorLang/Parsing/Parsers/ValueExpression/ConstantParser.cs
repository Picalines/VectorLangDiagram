using System;
using System.Diagnostics;
using System.Globalization;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Parsing;

internal static partial class ValueExpressionParser
{
    internal static class ConstantParser
    {
        public static readonly Parser<NumberNode> Number =
            from literalToken in ParseToken.LiteralNumber
            from value in ParseNumberLiteral(literalToken)
            select new NumberNode(value, literalToken);

        public static readonly Parser<StringNode> String =
            from token in ParseToken.LiteralString
            select new StringNode(token.Value[1..^1], token);

        public static readonly Parser<ColorNode> Color =
            from literalToken in ParseToken.LiteralColor
            from color in ParseColorLiteral(literalToken)
            select new ColorNode(color.R, color.G, color.B, literalToken.Selection);

        private static Parser<double> ParseNumberLiteral(Token token)
        {
            Debug.Assert(token.RegexGroups is not null);

            var valueLiteral = token.RegexGroups["value"].ValueSpan;

            if (!double.TryParse(valueLiteral, NumberStyles.Number, CultureInfo.InvariantCulture, out double value))
            {
                return Parse.Throw<double>($"invalid number value '{valueLiteral}'");
            }

            var unit = token.RegexGroups["unit"].Value;

            return unit switch
            {
                "" => Parse.Return(value),
                "deg" => Parse.Return(value * (Math.PI / 180.0)),
                "rad" => Parse.Return(value),
                _ => Parse.Throw<double>($"unknown number unit '{unit}'"),
            };
        }

        private static Parser<(double R, double G, double B)> ParseColorLiteral(Token token)
        {
            Debug.Assert(token.RegexGroups is not null);

            double ParseGroup(string name)
            {
                var parsedPart = int.Parse(token.RegexGroups![name].ValueSpan, NumberStyles.HexNumber);

                return parsedPart / 255.0;
            }

            return Parse.Return((ParseGroup("r"), ParseGroup("g"), ParseGroup("b")));
        }
    }
}
