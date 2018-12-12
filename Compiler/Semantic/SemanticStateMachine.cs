using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Smc.Semantic
{
    public class SemanticStateMachine : ISemanticNode
    {
        public void Accept(ISemanticVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ICollection<AnalysisError> Errors { get; } = new Collection<AnalysisError>();
        public SemanticStates States { get; set; } = new SemanticStates();
        public ISet<string> Events { get; set; } = new HashSet<string>();
        public ISet<string> Actions { get; set; } = new HashSet<string>();
        public State InitialState { get; set; }
        public string ActionClass { get; set; }
        public string FsmName { get; set; }

        public void AddError(AnalysisError analysisError)
        {
            Errors.Add(analysisError);
        }
    }
}   