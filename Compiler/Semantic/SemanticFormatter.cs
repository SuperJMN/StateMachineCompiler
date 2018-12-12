using System;
using System.Collections.Generic;
using System.Text;

namespace Smc.Semantic
{
    public class SemanticFormatter : ISemanticVisitor
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();

        public void Visit(SemanticStateMachine semanticStateMachine)
        {
            stringBuilder.AppendLine("Actions: " + semanticStateMachine.ActionClass);
            stringBuilder.AppendLine("FSM: " + semanticStateMachine.FsmName);
            stringBuilder.AppendLine("Initial: " + semanticStateMachine.InitialState.Name);
            
            semanticStateMachine.States.Accept(this);
        }

        public void Visit(State state)
        {
            var stateName = state.IsAbstract ? "(" + state.Name + ")" : state.Name;
            
            stringBuilder.Append("\t" + stateName);

            foreach (var superState in state.SuperStates)
            {
                stringBuilder.Append(" :" + superState.Name);
            }

            foreach (var action in state.EntryActions)
            {
                stringBuilder.Append(" <" + action);
            }

            foreach (var action in state.ExitActions)
            {
                stringBuilder.Append(" >" + action);
            }

            stringBuilder.AppendLine(" {");

            FormatTranstitions(state.Transitions);

            stringBuilder.AppendLine("\t}");
        }

        public void Visit(SemanticTransition transition)
        {
            var actions = string.Join(" ", transition.Actions);
            stringBuilder.AppendLine($"\t\t{transition.Event ?? "null"} {transition.NextState.Name} {{{actions}}}");
        }

        public void Visit(SemanticStates states)
        {
            stringBuilder.AppendLine("{");
            foreach (var state in states)
            {
                state.Value.Accept(this);
            }
            stringBuilder.AppendLine("}");
        }

        private void FormatTranstitions(IEnumerable<SemanticTransition> stateTransitions)
        {
            foreach (var transition in stateTransitions)
            {
                transition.Accept(this);
            }
        }

        public string GetResult()
        {
            return stringBuilder.ToString();
        }
    }
}