using Superpower;
using Superpower.Parsers;

namespace Smc.Syntax
{
    public static class ParserMixin
    {
        public static TokenListParser<TToken, TResult> Between<TToken, TResult>(this TokenListParser<TToken, TResult> self, TToken left, TToken right)
        {
            return self.Between(Token.EqualTo(left), Token.EqualTo(right));
        }
    }
}