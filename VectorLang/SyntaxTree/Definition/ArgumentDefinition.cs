namespace VectorLang.SyntaxTree;

internal sealed record ArgumentDefinition(TypeNode Type, string Name) : Definition;
