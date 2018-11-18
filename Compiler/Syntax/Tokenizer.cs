using Superpower;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace Smc.Syntax
{
    public class Tokenizer : Tokenizer<SmcToken>
    {
        public static Tokenizer<SmcToken> Create()
        {
            return new TokenizerBuilder<SmcToken>()
                .Ignore(Span.WhiteSpace)
                .Match(Character.EqualTo('('), SmcToken.Lparen)
                .Match(Character.EqualTo(')'), SmcToken.Rparen)
                .Match(Character.EqualTo('{'), SmcToken.Lbrace)
                .Match(Character.EqualTo('}'), SmcToken.Rbrace)
                .Match(Character.EqualTo(':'), SmcToken.Colon)
                .Match(Character.EqualTo('>'), SmcToken.Gt)
                .Match(Character.EqualTo('<'), SmcToken.Lt)
                .Match(Character.EqualTo('-'), SmcToken.Hyphen)
                .Match(Span.Regex("\\w*"), SmcToken.Name, true)
                .Build();
        }
    }
}
