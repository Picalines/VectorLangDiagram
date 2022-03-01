namespace VectorLang.Parsing;

internal delegate IParseResult<T> Parser<out T>(ParseInput input);
