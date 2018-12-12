using System.Collections.Generic;

namespace Smc.Generator
{
    public class SwitchCaseNode : INscNode
    {
        public string VariableName { get; }
        public IEnumerable<INscNode> CaseNodes { get; }

        public SwitchCaseNode(string variableName, IEnumerable<INscNode> caseNodes)
        {
            VariableName = variableName;
            CaseNodes = caseNodes;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}