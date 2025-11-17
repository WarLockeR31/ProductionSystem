using ProductionSystem.Knowledge;

namespace ProductionSystem.Reasoners;

public class ForwardStep(Rule? rule, string newFactId)
{
	public Rule?			Rule		{ get; set; } = rule;
	public List<string>		UsedFacts	{ get; set; } = new List<string>();
	public string			NewFactId	{ get; set; } = newFactId;
	
	public ForwardStep(Rule? rule, string newFactId, IEnumerable<string> usedFacts) 
		: this(rule, newFactId)
	{
		UsedFacts.AddRange(usedFacts);
	}
}

public class ForwardResult
{
	public HashSet<string>		FinalFacts		{ get; } = new HashSet<string>();
	public List<ForwardStep>	Steps			{ get; } = new List<ForwardStep>();
	public string?				TargetFactId	{ get; set; }
}


public static class ForwardReasoner
{
	public static ForwardResult Run(
		KnowledgeBase kb, 
		IEnumerable<string> initialFacts,
		string? targetFactId = null)
	{
		var result = new ForwardResult
		{
			TargetFactId = targetFactId
		};

		foreach (var f in initialFacts)
			result.FinalFacts.Add(f);

		bool added;
		int safety = 0;

		do
		{
			added = false;
			foreach (var rule in kb.Rules)
			{
				if (rule is null)
					continue;

				
				if (result.FinalFacts.Contains(rule.Conclusion))
					continue;

				bool allConditionsKnown = true;
				foreach (var cond in rule.Conditions)
				{
					if (!result.FinalFacts.Contains(cond))
					{
						allConditionsKnown = false;
						break;
					}
				}

				if (!allConditionsKnown)
					continue;

				result.FinalFacts.Add(rule.Conclusion);

				result.Steps.Add(new ForwardStep
				(
					rule,
					rule.Conclusion,
					[..rule.Conditions]
				));

				added = true;
				
				if (targetFactId != null && rule.Conclusion == targetFactId)
				{
					return result;
				}
			}

			safety++;
			if (safety > 1000)
				break; 
		} while (added);

		return result;
	}
}
