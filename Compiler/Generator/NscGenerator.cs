using System.Collections.Generic;
using System.Linq;
using Smc.Optimize;

namespace Smc.Generator
{
    public class NscGenerator
    {
        public INscNode Generate(OptimizedStateMachine machine)
        {
            return new FsmNode
            {
                ClassName = machine.Header.Fsm,
                ActionsName = machine.Header.Actions,
                Events = new EnumNode("Event", machine.Events),
                States = new EnumNode("StateEnum", machine.States),
                Delegators = new EventDelegatorsNode(machine.Events),
                HandleEvent = new HandleEventNode(StateSwitch(machine)),
                Initial = new StatePropertyNode(machine.Header.Initial),
            };
        }

        private SwitchCaseNode StateSwitch(OptimizedStateMachine machine)
        {
            return new SwitchCaseNode("State", GetStateSwitchNodes(machine));
        }

        private IEnumerable<INscNode> GetStateSwitchNodes(OptimizedStateMachine machine)
        {
            var query = from t in machine.Transitions
                        select new CaseNode("StateEnum", t.CurrentState, EventSwitch(t));

            return query;
        }

        private INscNode EventSwitch(OptimizedStateMachine.OptimizedTransition transition)
        {
            return new SwitchCaseNode("ev", GetEventSwitchNodes(transition));
        }

        private IEnumerable<INscNode> GetEventSwitchNodes(OptimizedStateMachine.OptimizedTransition transition)
        {
            var defaultNode = new DefaultCaseNode(transition.CurrentState);
            return transition.Subtransitions.Select(GetEventSwitchNode).Concat(new[] { defaultNode });
        }

        private INscNode GetEventSwitchNode(OptimizedStateMachine.OptimizedSubtransition subtransition)
        {
            return new CaseNode("Event", subtransition.Event, ActionNode(subtransition));
        }

        private INscNode ActionNode(OptimizedStateMachine.OptimizedSubtransition optimizedSubtransition)
        {
            var setStateNode = GetSetStateNode(optimizedSubtransition.NextState);
            var nodes = optimizedSubtransition.Actions.Select(x => new FunctionCallNode(x));

            return new CompositeNode(new[] { setStateNode }.Concat(nodes));
        }

        private INscNode GetSetStateNode(string stateName)
        {
            var n = new EnumeratorNode("StateEnum", stateName);
            var fc = new SetterNode("State", n);

            return fc;
        }
    }

    public class FunctionCallNode : INscNode
    {
        public string Name { get; }
        public INscNode Node { get; }

        public FunctionCallNode(string name)
        {
            Name = name;
        }

        public FunctionCallNode(string name, INscNode node)
        {
            Name = name;
            Node = node;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class EnumeratorNode : INscNode
    {
        public string Enumeration { get; }
        public string Enumerator { get; }

        public EnumeratorNode(string enumeration, string enumerator)
        {
            Enumeration = enumeration;
            Enumerator = enumerator;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class CaseNode : INscNode
    {
        public string SwitchName { get; }
        public string CaseName { get; }
        public INscNode CaseActionNode { get; }

        public CaseNode(string switchName, string caseName, INscNode caseActionNode)
        {
            SwitchName = switchName;
            CaseName = caseName;
            CaseActionNode = caseActionNode;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}