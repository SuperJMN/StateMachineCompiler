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

        private static readonly TokenListParser<SmcToken, string> OptionalName = from name in Token.EqualTo(SmcToken.Hyphen).Value((string)null).Or(Name)
            select name;

        public static readonly TokenListParser<SmcToken, Subtransition> Subtransition =
            from ev in OptionalName
            from nextState in OptionalName
            from actions in Actions
            select new Subtransition(ev, nextState, actions);

        public static readonly TokenListParser<SmcToken, ModifierKind> ModifierKind =
            Token.EqualTo(SmcToken.Colon).Value(Syntax.ModifierKind.SuperState)
                .Or(Token.EqualTo(SmcToken.Lt).Value(Syntax.ModifierKind.EntryAction))
                .Or(Token.EqualTo(SmcToken.Gt).Value(Syntax.ModifierKind.ExitAction));

        public static readonly TokenListParser<SmcToken, StateModifier> StateModifier =
            from kind in ModifierKind
            from actions in Actions
            select new StateModifier(kind, actions);

        public static readonly TokenListParser<SmcToken, StateModifier[]> StateModifiers = StateModifier.Many();

        public static readonly TokenListParser<SmcToken, StateSpec> StateSpec =
            from state in Name.Between(SmcToken.Lparen, SmcToken.Rparen).Select(x => new { Name = x, IsAbstract = true })
                .Or(Name.Select(s => new { Name = s, IsAbstract = false }))
            from modifiers in StateModifiers
            select new StateSpec(state.Name, modifiers, state.IsAbstract);

        public static readonly TokenListParser<SmcToken, Transition> Transition =
            from stateSpec in StateSpec
            from subTransitions in Subtransitions
            select new Transition(stateSpec, subTransitions);
        
        private static readonly TokenListParser<SmcToken, Subtransition[]> MultipleSubtransition = 
            Subtransition.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace);

        private static readonly TokenListParser<SmcToken, Subtransition[]> SingleSubtransition = Subtransition
            .Select(x => new[] { x });

        public static readonly TokenListParser<SmcToken, Subtransitions> Subtransitions =
            from subt in MultipleSubtransition.Or(SingleSubtransition)
            select new Subtransitions(subt.ToList());

        public static readonly TokenListParser<SmcToken, string[]> Actions = 
            OptionalName
            .Select(x => new[] { x })
            .Or(OptionalName.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace))
            .Select(s => s.Where(x => x != null).ToArray());

        public static readonly TokenListParser<SmcToken, Logic> Logic = from ta in Transition.Many().Between(SmcToken.Lbrace, SmcToken.Rbrace)
                                                                        select new Logic(ta.ToList());

        public static readonly TokenListParser<SmcToken, Headers> Headers = from headers in Header.Many()
            select new Headers(headers.ToList());

        public static readonly TokenListParser<SmcToken, FsmSyntax> Fsm =
            from headers in Headers
            from logic in Logic
            select new FsmSyntax(headers, logic);
    }
}