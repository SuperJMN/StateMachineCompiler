using System.Linq;
using Superpower;
using Superpower.Parsers;

namespace Smc.Syntax
{
    public static class Parsers
    {
        public static readonly TokenListParser<SmcToken, string> Name = Token.EqualTo(SmcToken.Name).Select(x => x.ToStringValue());

        public static readonly TokenListParser<SmcToken, Header> Header =
            from name in Name
            from colon in Token.EqualTo(SmcToken.Colon)
            from value in Name
            select new Header(name, value);

        public static readonly TokenListParser<SmcToken, string> Event = 
            from name in Token.EqualTo(SmcToken.Hyphen).Value((string)null).Or(Name)
            select name;

        public static readonly TokenListParser<SmcToken, string> NextState = 
            from name in Token.EqualTo(SmcToken.Hyphen).Value((string)null).Or(Name)
            select name;

        public static readonly TokenListParser<SmcToken, Subtransition> Subtransition =
            from ev in Event
            from nextState in NextState
            from actions in Actions
            select new Subtransition(ev, nextState, actions);

        public static readonly TokenListParser<SmcToken, ModifierKind> ModifierKind =
            Token.EqualTo(SmcToken.Colon).Value(Syntax.ModifierKind.SuperState)
                .Or(Token.EqualTo(SmcToken.Gt).Value(Syntax.ModifierKind.EntryAction))
                .Or(Token.EqualTo(SmcToken.Lt).Value(Syntax.ModifierKind.ExitAction));

        public static readonly TokenListParser<SmcToken, StateModifier> StateModifier =
            from kind in ModifierKind
            from name in Name
            select new StateModifier(kind, name);
            
        public static readonly TokenListParser<SmcToken, StateModifier[]> StateModifiers = StateModifier.Many();

        public static readonly TokenListParser<SmcToken, StateSpec> StateSpec =
            from state in Name.Between(SmcToken.Lparen, SmcToken.Rparen).Select(s => new{ Name = s, IsSuper = true})
                .Or(Name.Select(s => new{ Name = s, IsSuper = false}))           
            from modifiers in StateModifiers
            select new StateSpec(state.Name, modifiers, state.IsSuper);

        public static readonly TokenListParser<SmcToken, Transition> Transition =
            from stateSpec in StateSpec
            from subTransitions in Subtransitions
            select new Transition(stateSpec, subTransitions);

        public static readonly TokenListParser<SmcToken, Subtransitions> Subtransitions =
            from subbs in Subtransition.Select(x => new[] {x}).Or(Subtransition.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace))
            select new Subtransitions(subbs.ToList());

        public static readonly TokenListParser<SmcToken, string> Action = 
            from name in Token.EqualTo(SmcToken.Hyphen).Value((string)null).Or(Name)
            select name;

        public static readonly TokenListParser<SmcToken, string[]> Actions = Action.Select(x => new[] {x})
            .Or(Action.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace));
        
        public static readonly TokenListParser<SmcToken, FsmSyntax> Fsm =
            from headers in Header.Many()
            from transitions in Transition.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace)
            select new FsmSyntax(new Headers(headers.ToList()), new Logic(transitions.ToList()));        
    }
}