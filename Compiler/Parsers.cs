using Superpower;
using Superpower.Parsers;

namespace Compiler
{
    public static class Parsers
    {
        public static readonly TokenListParser<SmcToken, string> Name = Token.EqualTo(SmcToken.Name).Select(x => x.ToStringValue());

        public static readonly TokenListParser<SmcToken, Header> Header =
            from name in Name
            from colon in Token.EqualTo(SmcToken.Colon)
            from value in Name
            select new Header(name, value);

        public static readonly TokenListParser<SmcToken, Event> Event = from name in Token.EqualTo(SmcToken.Hyphen).Value("-").Or(Name)
            select new Event(name);

        public static readonly TokenListParser<SmcToken, NextState> NextState = from name in Token.EqualTo(SmcToken.Hyphen).Value("-").Or(Name)
            select new NextState(name);

        public static readonly TokenListParser<SmcToken, Subtransition> Subtransition =
            from ev in Event
            from nextState in NextState
            from actions in Actions
            select new Subtransition(ev, nextState, actions);

        public static readonly TokenListParser<SmcToken, string> StateModifier = 
            Token.EqualTo(SmcToken.Gt).Value(">").Then(s => Name.Select(x => x +  " " + s))
                .Or(Token.EqualTo(SmcToken.Lt).Value("<")).Then(s => Name.Select(x => x +  " " + s))
                .Or(Token.EqualTo(SmcToken.Colon).Value(":")).Then(s => Name.Select(x => x +  " " + s));

        public static readonly TokenListParser<SmcToken, string[]> StateModifiers = StateModifier.Many();

        public static readonly TokenListParser<SmcToken, State> State =
            from name in Name.Or(Name.Between(SmcToken.Lparen, SmcToken.Rparen))
            select new State(name);

        public static readonly TokenListParser<SmcToken, StateSpec> StateSpec =
            from state in State
            from modifiers in StateModifiers
            select new StateSpec(state, modifiers);

        public static readonly TokenListParser<SmcToken, Transition> Transition =
            from stateSpec in StateSpec
            from subTransitions in Subtransitions
            select new Transition(stateSpec, subTransitions);

        public static readonly TokenListParser<SmcToken, Subtransition[]> Subtransitions =
            Subtransition.Select(x => new[] {x}).Or(Subtransition.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace));

        public static readonly TokenListParser<SmcToken, MyAction> Action = 
            from name in Token.EqualTo(SmcToken.Hyphen).Value("-").Or(Name)
            select new MyAction(name);

        public static readonly TokenListParser<SmcToken, MyAction[]> Actions = Action.Select(x => new[] {x})
            .Or(Action.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace));
        
        public static readonly TokenListParser<SmcToken, Fsm> Fsm =
            from headers in Header.Many()
            from logic in Transition.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace)
            select new Fsm(headers, logic);        
    }
}