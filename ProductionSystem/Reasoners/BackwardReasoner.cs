namespace ProductionSystem.Reasoners;

class GoalFrame(string factId, Rule? currentRule)
{
	public string		FactId			{ get; set; } = factId;
	public List<Rule?>	Candidates		{ get; set; } = new List<Rule?>();
	public Rule?		CurrentRule		{ get; set; } = currentRule;
	public int			PremiseIndex	{ get; set; } = -1;

	public GoalFrame(string factId, Rule? currentRule, int premiseIndex)
		: this(factId, currentRule)
	{
		PremiseIndex = premiseIndex;
	}
	
	public GoalFrame(string factId, Rule? currentRule, int premiseIndex, List<Rule?> candidates)
		: this(factId, currentRule, premiseIndex)
	{
		Candidates		= candidates;
	}
}

class ProvenFact(string factId, Rule? rule, List<string> premises)
{
	public string		FactId		{ get; set; } = factId;
	public Rule?		Rule		{ get; set; } = rule;
	public List<string> Premises	{ get; set; } = premises;
}

public class RuleApplication(Rule? rule, string derivedFactId, List<string> premises, int depth)
{
	public Rule?		Rule			{ get; set; } = rule;
	public string		DerivedFactId	{ get; set; } = derivedFactId;
	public List<string> Premises		{ get; set; } = premises;
	public int			Depth			{ get; set; } = depth;
}

public class BackwardResult(bool success)
{
	public bool						Success			{ get; set; }			= success;
	public HashSet<string>			UsedFacts		{ get; private set; }	= new HashSet<string>();
	public List<RuleApplication>	RuleSequence	{ get; private set; }	= new List<RuleApplication>();
	
	public BackwardResult(bool success, HashSet<string> usedFacts)
		: this(success)
	{
		UsedFacts = usedFacts;
	}
	
	public BackwardResult(bool success, HashSet<string> usedFacts, List<RuleApplication> ruleSequence)
		: this(success, usedFacts)
	{
		RuleSequence = ruleSequence;
	}
}


public static class BackwardReasoner
{
    public static BackwardResult Prove(
        KnowledgeBase kb,
        string goalFactId,
        IEnumerable<string> initialFacts)
    {
        var known = new HashSet<string>(initialFacts);
        var proven = new Dictionary<string, ProvenFact>();
		List<RuleApplication> ruleSequence = [];

        if (known.Contains(goalFactId))
        {
            return new BackwardResult(true, [goalFactId]);
        }

        var frames = new List<GoalFrame>();

        var goalCandidates = kb.Rules
							   .Where(r => r?.Conclusion == goalFactId)
							   .ToList();

        frames.Add(new GoalFrame
		(
            goalFactId,
            null,
            -1,
			goalCandidates
        ));

        while (frames.Count > 0)
        {
            var frame = frames[^1];

            if (known.Contains(frame.FactId))
            {
                frames.RemoveAt(frames.Count - 1);
                continue;
            }

            if (frame.CurrentRule == null)
            {
                if (frame.Candidates.Count == 0)
                {
                    if (frames.Count == 1)
                    {
						return new BackwardResult(false, [goalFactId]);
                    }

                    var parent = frames[^2];
                    parent.CurrentRule = null;
                    parent.PremiseIndex = -1;

                    frames.RemoveAt(frames.Count - 1);
                    continue;
                }
                else
                {
                    frame.CurrentRule = frame.Candidates[0];
                    frame.Candidates.RemoveAt(0);
                    frame.PremiseIndex = 0;
                    continue;
                }
            }

            var premises = frame.CurrentRule.Conditions;

            if (frame.PremiseIndex >= premises.Count)
            {
                known.Add(frame.FactId);
                proven[frame.FactId] = new ProvenFact
				(
                    frame.FactId,
                    frame.CurrentRule,
					[..premises]
				);
                frames.RemoveAt(frames.Count - 1);
                continue;
            }

            var premiseId = premises[frame.PremiseIndex];

            if (known.Contains(premiseId))
            {
                frame.PremiseIndex++;
                continue;
            }

            bool cycle = frames.Any(t => t.FactId == premiseId);

			if (cycle)
            {
                frame.CurrentRule = null;
                frame.PremiseIndex = -1;
                continue;
            }

            var premiseCandidates = kb.Rules
									  .Where(r => r?.Conclusion == premiseId)
									  .ToList();

            if (premiseCandidates.Count == 0)
            {
                frame.CurrentRule = null;
                frame.PremiseIndex = -1;
                continue;
            }

            frames.Add(new GoalFrame
            (
                premiseId,
                null,
                -1,
				premiseCandidates
            ));
        }

        if (!known.Contains(goalFactId))
        {
            return new BackwardResult(false);
        }

        var explanationFacts = new HashSet<string>();
        var queue = new Queue<string>();
        explanationFacts.Add(goalFactId);
        queue.Enqueue(goalFactId);

        while (queue.Count > 0)
        {
            var f = queue.Dequeue();
			if (!proven.TryGetValue(f, out var pf)) continue;

            foreach (var p in pf.Premises.Where(p => explanationFacts.Add(p)))
			{
				queue.Enqueue(p);
			}
        }

        var depth = new Dictionary<string, int>();
        foreach (var f in explanationFacts)
            depth[f] = -1;

        foreach (var f in explanationFacts)
            if (!proven.ContainsKey(f))
                depth[f] = 0;

        bool changed;
        do
        {
            changed = false;
            foreach (var f in explanationFacts)
            {
                if (depth[f] != -1) continue;
				if (!proven.TryGetValue(f, out var pf)) continue;

                bool allPremisesHaveDepth = true;
                int maxPremDepth = 0;

                foreach (var p in pf.Premises)
                {
					if (!depth.TryGetValue(p, out var dp) || dp == -1)
                    {
                        allPremisesHaveDepth = false;
                        break;
                    }
                    if (dp > maxPremDepth) maxPremDepth = dp;
                }

                if (allPremisesHaveDepth)
                {
                    depth[f] = maxPremDepth + 1;
                    changed = true;
                }
            }
        } while (changed);

        foreach (var pair in proven)
        {
            var factId = pair.Key;
            if (!explanationFacts.Contains(factId)) continue;

            var pf = pair.Value;
			var d = depth.GetValueOrDefault(factId, 0);

            ruleSequence.Add(new RuleApplication
            (
                pf.Rule,
                factId,
				[..pf.Premises],
				d
            ));
        }

		ruleSequence.Sort((a, b) => a.Depth.CompareTo(b.Depth));

        return new BackwardResult(true, explanationFacts, ruleSequence);
    }
}
