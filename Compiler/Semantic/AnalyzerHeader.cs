namespace Smc.Semantic
{
    internal class AnalyzerHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsEmpty => Name == null;
    }
}