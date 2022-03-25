namespace VectorLang.Model;

internal sealed class NumberInstanceType : InstanceType
{
    public static readonly NumberInstanceType Instance = new();

    private NumberInstanceType() : base("number")
    {
    }

    protected override void DefineMembersInternal()
    {
        DefineOperator(UnaryOperator.Plus, this, (NumberInstance thisInstance) => +thisInstance);
        DefineOperator(UnaryOperator.Minus, this, (NumberInstance thisInstance) => -thisInstance);

        DefineOperator(BinaryOperator.Plus, this, this, (NumberInstance a, NumberInstance b) => a + b);
        DefineOperator(BinaryOperator.Minus, this, this, (NumberInstance a, NumberInstance b) => a - b);
        DefineOperator(BinaryOperator.Multiply, this, this, (NumberInstance a, NumberInstance b) => a * b);
        DefineOperator(BinaryOperator.Divide, this, this, (NumberInstance a, NumberInstance b) => a / b);
        DefineOperator(BinaryOperator.Modulo, this, this, (NumberInstance a, NumberInstance b) => a % b);

        var vectorType = VectorInstanceType.Instance;

        DefineOperator(BinaryOperator.Multiply, vectorType, vectorType, (NumberInstance number, VectorInstance vector) => number * vector);
        DefineOperator(BinaryOperator.Divide, vectorType, vectorType, (NumberInstance number, VectorInstance vector) => number / vector);
    }
}
