namespace ProductionSystem;

public class KnowledgeBase
{
	public Dictionary<string, Fact> Facts { get; } = new Dictionary<string, Fact>();
	public List<Rule?> Rules { get; } = new List<Rule?>();

	public void LoadFacts(string path)
	{
		foreach (var line in File.ReadLines(path).Skip(1))
		{
			if (string.IsNullOrWhiteSpace(line)) continue;
			if (line.StartsWith("...")) continue;

			var parts = line.Split(';');
			if (parts.Length < 5) continue;

			var fact = new Fact
			(
				parts[0].Trim(),
				parts[1].Trim(),
				FactKindParser.Parse(parts[2]),   
				parts[4].Trim()
			);

			if (!Facts.ContainsKey(fact.Id))
				Facts.Add(fact.Id, fact);
		}
	}


	public void LoadRules(string path)
	{
		foreach (var line in File.ReadLines(path).Skip(1))
		{
			if (string.IsNullOrWhiteSpace(line)) continue;
			if (line.StartsWith("...")) continue;
			if (line.StartsWith(";")) continue;    /

			var parts = line.Split(';');
			if (parts.Length < 5) continue;

			var rule = new Rule
			(
				parts[0].Trim(),
				parts[3].Trim(),
				parts[4].Trim()
			);

			var condPart = parts[2];
			foreach (var c in condPart.Split('&'))
			{
				var trimmed = c.Trim();
				if (!string.IsNullOrEmpty(trimmed))
					rule.Conditions.Add(trimmed);
			}

			Rules.Add(rule);
		}
	}
}

public static class FactKindParser
{
	public static FactKind Parse(string s)
	{
		if (string.IsNullOrWhiteSpace(s))
			return FactKind.Unknown;

		s = s.Trim().ToLowerInvariant();

		return s switch
		{
			"input"        => FactKind.Input,
			"intermediate" => FactKind.Intermediate,
			"target"       => FactKind.Target,
			_              => FactKind.Unknown
		};
	}
}
