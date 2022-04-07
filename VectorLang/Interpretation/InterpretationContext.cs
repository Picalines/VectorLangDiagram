using System.Collections.Generic;
using VectorLang.Compilation;
using VectorLang.Model;

namespace VectorLang.Interpretation;

internal static partial class Interpreter
{
    private sealed class InterpretationContext
    {
        public IReadOnlyList<Instruction> Instructions { get; }

        private int _CurrentInstructionIndex = 0;

        private readonly Stack<Instance> _Stack = new();

        private readonly Dictionary<int, Instance> _StoredValues = new();

        public InterpretationContext(IReadOnlyList<Instruction> instructions)
        {
            Instructions = instructions;
        }

        public Instruction CurrentInstruction => Instructions[_CurrentInstructionIndex];

        public bool AtEnd => _CurrentInstructionIndex >= Instructions.Count;

        public void MoveNext()
        {
            _CurrentInstructionIndex++;
        }

        public void Jump(int delta)
        {
            _CurrentInstructionIndex += delta;
        }

        // TODO: exceptions?

        public Instance PeekStack()
        {
            return _Stack.Peek();
        }

        public void PushToStack(Instance instance)
        {
            _Stack.Push(instance);
        }

        public Instance PopFromStack()
        {
            return _Stack.Pop();
        }

        public void StoreValue(int address, Instance value)
        {
            _StoredValues[address] = value;
        }

        public Instance LoadValue(int address)
        {
            return _StoredValues[address];
        }
    }
}
