using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using VectorLang.Compilation;
using VectorLang.Model;

namespace VectorLangDocs.Shared.DocumentationModel;

public sealed class VectorLangDocumentation
{
    public DocumentationRepository<InstanceTypeDocumentation> Types { get; } = new();

    public DocumentationRepository<ConstantDocumentation> Constants { get; } = new();

    public DocumentationRepository<FunctionDocumentation> Functions { get; } = new();

    private VectorLangDocumentation()
    {
    }

    public static VectorLangDocumentation FromXml(string xmlString)
    {
        return FromXml(XDocument.Parse(xmlString));
    }

    public static VectorLangDocumentation FromXml(XDocument xDocument)
    {
        var langAssembly = Assembly.GetAssembly(typeof(ProgramCompiler))!;
        ProgramCompiler.DefineInstanceTypesAndLibraries();

        var documentation = new VectorLangDocumentation();

        var reflectionInstanceTypes = ProgramCompiler.InstanceTypes.OfType<ReflectionInstanceType>().ToArray();

        foreach (var reflectionInstanceType in reflectionInstanceTypes)
        {
            var vlDoc = XmlLangDocByMemberName(xDocument, XmlMemberNameOf(reflectionInstanceType));

            if (vlDoc is null)
            {
                continue;
            }

            documentation.Types.Add(new InstanceTypeDocumentation(reflectionInstanceType.Name)
            {
                Summary = vlDoc.Element("summary")?.Value,
                UsageExample = vlDoc.Element("example")?.Value,
            });
        }

        foreach (var reflectionInstanceType in reflectionInstanceTypes)
        {
            documentation.Types.TryGet(reflectionInstanceType.Name, out var instanceTypeDocumentation);
            Debug.Assert(instanceTypeDocumentation is not null);

            var reflectedProperties = reflectionInstanceType.InstanceReflectionType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(property => new { property.GetCustomAttribute<InstanceFieldAttribute>()?.FieldName, property })
                .Where(pair => pair.FieldName is not null)
                .ToDictionary(pair => pair.FieldName!, pair => pair.property);

            foreach (var (fieldName, fieldType) in reflectionInstanceType.Fields)
            {
                var vlDoc = XmlLangDocByMemberName(xDocument, XmlMemberNameOfProperty(reflectedProperties[fieldName]));
                if (vlDoc is null)
                {
                    continue;
                }

                documentation.Types.TryGet(fieldType.Name, out var fieldTypeDoc);
                Debug.Assert(fieldTypeDoc is not null);

                instanceTypeDocumentation.Fields.Add(new InstanceFieldDocumentation(instanceTypeDocumentation, fieldName, fieldTypeDoc)
                {
                    Summary = vlDoc.Element("summary")?.Value,
                    UsageExample = vlDoc.Element("example")?.Value,
                });
            }

            var reflectedMethods = reflectionInstanceType.InstanceReflectionType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Select(method => new { method.GetCustomAttribute<InstanceMethodAttribute>()?.MethodName, method })
                .Where(pair => pair.MethodName is not null)
                .ToDictionary(pair => pair.MethodName!, pair => pair.method);

            foreach (var (methodName, instanceMethod) in reflectionInstanceType.Methods)
            {
                var vlDoc = XmlLangDocByMemberName(xDocument, XmlMemberNameOfMethod(reflectedMethods[methodName]));
                if (vlDoc is null)
                {
                    continue;
                }

                var (returnTypeDoc, parameterDocs) = SignatureDocumentation(documentation, vlDoc, instanceMethod.Signature);

                var methodDocumentation = new InstanceMethodDocumentation(instanceTypeDocumentation, methodName, returnTypeDoc)
                {
                    Summary = vlDoc.Element("summary")?.Value,
                    UsageExample = vlDoc.Element("example")?.Value,
                    ReturnValueInfo = vlDoc.Element("returns")?.Value,
                };

                foreach (var parameterDoc in parameterDocs)
                {
                    methodDocumentation.Parameters.Add(parameterDoc);
                }

                instanceTypeDocumentation.Methods.Add(methodDocumentation);
            }

            var staticMethods = reflectionInstanceType.InstanceReflectionType.GetMethods(BindingFlags.Public | BindingFlags.Static);

            var reflectedUnaryOperators = staticMethods
                .Select(method => new { UnaryOperator = MethodNameToUnaryOperator(method.Name), method })
                .Where(pair => pair.UnaryOperator is not null)
                .ToDictionary(pair => pair.UnaryOperator.Value, pair => pair.method);

            foreach (var (unaryOperator, instanceUnaryOperator) in reflectionInstanceType.UnaryOperators)
            {
                var vlDoc = XmlLangDocByMemberName(xDocument, XmlMemberNameOfMethod(reflectedUnaryOperators[unaryOperator]));
                if (vlDoc is null)
                {
                    continue;
                }

                documentation.Types.TryGet(instanceUnaryOperator.ReturnType.Name, out var returnTypeDoc);
                Debug.Assert(returnTypeDoc is not null);

                instanceTypeDocumentation.UnaryOperators.Add(new InstanceUnaryOperatorDocumentation(unaryOperator, instanceTypeDocumentation, returnTypeDoc)
                {
                    Summary = vlDoc.Element("summary")?.Value,
                    UsageExample = vlDoc.Element("example")?.Value,
                    ReturnValueInfo = vlDoc.Element("returns")?.Value,
                });
            }

            var reflectedBinaryOperators = staticMethods
                .Where(method => method.GetParameters().Length is 2)
                .Select(method => new { BinaryOperator = MethodNameToBinaryOperator(method.Name), RightType = ReflectionInstanceType.From(method.GetParameters()[1].ParameterType) as InstanceType, method })
                .Where(pair => pair.BinaryOperator is not null)
                .ToDictionary(pair => (pair.BinaryOperator.Value, pair.RightType), pair => pair.method);

            foreach (var ((binaryOperator, rightType), instanceBinaryOperator) in reflectionInstanceType.BinaryOperators)
            {
                var vlDoc = XmlLangDocByMemberName(xDocument, XmlMemberNameOfMethod(reflectedBinaryOperators[(binaryOperator, rightType)]));
                if (vlDoc is null)
                {
                    continue;
                }

                documentation.Types.TryGet(instanceBinaryOperator.RightType.Name, out var rightTypeDoc);
                Debug.Assert(rightTypeDoc is not null);

                documentation.Types.TryGet(instanceBinaryOperator.ReturnType.Name, out var returnTypeDoc);
                Debug.Assert(returnTypeDoc is not null);

                instanceTypeDocumentation.BinaryOperators.Add(new InstanceBinaryOperatorDocumentation(binaryOperator, instanceTypeDocumentation, rightTypeDoc, returnTypeDoc)
                {
                    Summary = vlDoc.Element("summary")?.Value,
                    UsageExample = vlDoc.Element("example")?.Value,
                    ReturnValueInfo = vlDoc.Element("returns")?.Value,
                });
            }
        }

        var plotLibrary = new PlotLibrary();
        plotLibrary.DefineItems();
        var reflectionLibraries = ProgramCompiler.Libraries.OfType<ReflectionLibrary>().Append(plotLibrary);

        foreach (var library in reflectionLibraries)
        {
            var libraryName = XmlLangDocByMemberName(xDocument, XmlMemberNameOfLibrary(library))?.Element("name")?.Value;

            var libraryType = library.GetType();

            var reflectedFunctions = libraryType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Select(method => new { method.GetCustomAttribute<ReflectionFunctionAttribute>()?.FunctionName, method })
                .Where(pair => pair.FunctionName is not null)
                .ToDictionary(pair => pair.FunctionName!, pair => pair.method);

            foreach (var function in library.Functions)
            {
                var vlDoc = XmlLangDocByMemberName(xDocument, XmlMemberNameOfMethod(reflectedFunctions[function.Name]));
                if (vlDoc is null)
                {
                    continue;
                }

                var (returnTypeDoc, parameterDocs) = SignatureDocumentation(documentation, vlDoc, function.Signature);

                var functionDocumentation = new FunctionDocumentation(function.Name, returnTypeDoc)
                {
                    LibraryName = libraryName,
                    Summary = vlDoc.Element("summary")?.Value,
                    UsageExample = vlDoc.Element("example")?.Value,
                    ReturnValueInfo = vlDoc.Element("returns")?.Value,
                };

                foreach (var parameterDoc in parameterDocs)
                {
                    functionDocumentation.Parameters.Add(parameterDoc);
                }

                documentation.Functions.Add(functionDocumentation);
            }

            var reflectedConstants = libraryType.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(field => new { field.GetCustomAttribute<ReflectionConstantAttribute>()?.ConstantName, field })
                .Where(pair => pair.ConstantName is not null)
                .ToDictionary(pair => pair.ConstantName!, pair => pair.field);

            foreach (var (constantName, consantInstance) in library.Constants)
            {
                var path = XmlMemberNameOfField(reflectedConstants[constantName]);
                var vlDoc = XmlLangDocByMemberName(xDocument, path);
                if (vlDoc is null)
                {
                    continue;
                }

                documentation.Types.TryGet(consantInstance.Type.Name, out var constantTypeDoc);
                Debug.Assert(constantTypeDoc is not null);

                documentation.Constants.Add(new ConstantDocumentation(constantName, constantTypeDoc)
                {
                    LibraryName = libraryName,
                    Summary = vlDoc.Element("summary")?.Value,
                    UsageExample = vlDoc.Element("example")?.Value,
                });
            }
        }

        return documentation;

        static XElement? XmlDocMemberByName(XDocument xDocument, string memberName)
        {
            return (from member in xDocument.Descendants("member")
                    where member.Attribute("name")?.Value == memberName
                    select member).FirstOrDefault();
        }

        static XElement? XmlLangDocByMemberName(XDocument xDocument, string xmlMemberName)
        {
            return XmlDocMemberByName(xDocument, xmlMemberName)?.Element("vl-doc");
        }

        static (InstanceTypeDocumentation ReturnTypeDoc, ParameterDocumentation[] ParameterDocs) SignatureDocumentation(VectorLangDocumentation documentation, XElement vlDocElement, CallSignature signature)
        {
            documentation.Types.TryGet(signature.ReturnType.Name, out var returnTypeDocumentation);
            Debug.Assert(returnTypeDocumentation is not null);

            var parameterDocs = new ParameterDocumentation[signature.Arguments.Count];
            for (int i = 0; i < signature.Arguments.Count; i++)
            {
                var parameter = signature.Arguments[i];
                documentation.Types.TryGet(parameter.Type.Name, out var parameterTypeDocumentation);
                Debug.Assert(parameterTypeDocumentation is not null);
                parameterDocs[i] = new ParameterDocumentation(parameter.Name, parameterTypeDocumentation)
                {
                    Summary = (from paramElement in vlDocElement.Elements("param")
                               where paramElement.Attribute("name")?.Value == parameter.Name
                               select paramElement.Value).FirstOrDefault(),
                };
            }

            return (returnTypeDocumentation, parameterDocs);
        }

        static string XmlMemberNameOf(ReflectionInstanceType instanceType)
        {
            return $"T:{instanceType.InstanceReflectionType.FullName}";
        }

        static string XmlMemberNameOfField(FieldInfo fieldInfo)
        {
            return $"F:{fieldInfo.DeclaringType.FullName}.{fieldInfo.Name}";
        }

        static string XmlMemberNameOfProperty(PropertyInfo propertyInfo)
        {
            return $"P:{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}";
        }

        static string XmlMemberNameOfMethod(MethodInfo methodInfo)
        {
            return $"M:{methodInfo.DeclaringType.FullName}.{methodInfo.Name}({string.Join(",", methodInfo.GetParameters().Select(p => p.ParameterType.FullName))})";
        }

        static string XmlMemberNameOfLibrary(Library library)
        {
            return $"T:{library.GetType().FullName}";
        }

        static UnaryOperator? MethodNameToUnaryOperator(string methodName) => methodName switch
        {
            "op_UnaryPlus" => UnaryOperator.Plus,
            "op_UnaryNegation" => UnaryOperator.Minus,
            "op_LogicalNot" => UnaryOperator.Not,
            _ => null,
        };

        static BinaryOperator? MethodNameToBinaryOperator(string methodName) => methodName switch
        {
            "op_Addition" => BinaryOperator.Plus,
            "op_Subtraction" => BinaryOperator.Minus,
            "op_Multiply" => BinaryOperator.Multiply,
            "op_Division" => BinaryOperator.Divide,
            "op_Modulus" => BinaryOperator.Modulo,
            "op_LessThan" => BinaryOperator.Less,
            "op_LessThanOrEqual" => BinaryOperator.LessOrEqual,
            "op_GreaterThan" => BinaryOperator.Greater,
            "op_GreaterThanOrEqual" => BinaryOperator.GreaterOrEqual,
            "op_Equality" => BinaryOperator.Equals,
            "op_Inequality" => BinaryOperator.NotEquals,
            _ => null,
        };
    }
}
