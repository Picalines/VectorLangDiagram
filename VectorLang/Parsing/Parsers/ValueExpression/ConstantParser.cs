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

        public static readonly Parser<BooleanNode> Boolean =
            from literalToken in ParseToken.LiteralBoolean
            from boolean in ParseBooleanLiteral(literalToken)
            select new BooleanNode(boolean, literalToken);

        public static readonly Parser<ColorNode> Color =
            from literalToken in ParseToken.LiteralColor
            from color in ParseColorLiteral(literalToken)
            select new ColorNode(color.R, color.G, color.B, literalToken.Selection);

        public static readonly Parser<VoidNode> Void =
            from literalToken in ParseToken.LiteralVoid
            select new VoidNode(literalToken);

        private static Parser<double> ParseNumberLiteral(Token token)
        {
            Debug.Assert(token.RegexGroups is not null);

            var valueLiteral = token.RegexGroups["value"].ValueSpan;

            if (!double.TryParse(valueLiteral, NumberStyles.Number, CultureInfo.InvariantCulture, out double value))
            {
                return Parse.Throw<double>($"invalid number value '{valueLiteral}'", new[] { "number literal" });
            }

            var unit = token.RegexGroups["unit"].Value;

            return unit switch
            {
                "" => Parse.Return(value),
                "deg" => Parse.Return(value * (Math.PI / 180.0)),
                "rad" => Parse.Return(value),
                _ => Parse.Throw<double>($"unknown number unit '{unit}'", new[] { "no unit", "deg", "rad" }),
            };
        }

        private static Parser<bool> ParseBooleanLiteral(Token token)
        {
            Debug.Assert(token.RegexGroups is not null);

            if (token.RegexGroups["true"].Success)
            {
                return Parse.Return(true);
            }

            Debug.Assert(token.RegexGroups["false"].Success);

            return Parse.Return(false);
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
