using System.Text;
using ProductionSystem.Knowledge;

namespace ProductionSystem.Reasoners;

public static class ExplanationBuilder
{
	public static string BuildForwardExplanation(KnowledgeBase kb, ForwardResult result)
	{
		var sb = new StringBuilder();
		int stepNo = 1;

		foreach (var step in result.Steps)
		{
			sb.AppendLine($"Шаг {stepNo}. Сработало правило {step.Rule?.Id}: {step.Rule?.Explanation}");

			sb.Append("  Условия известны: ");
			sb.AppendLine(string.Join(", ",
				step.UsedFacts.Select(id => kb.Facts.TryGetValue(id, out var f)
					? $"{id} ({f.Description})"
					: id)));

			string newFactText = kb.Facts.TryGetValue(step.NewFactId, out var nf)
				? $"{step.NewFactId} ({nf.Description})"
				: step.NewFactId;

			sb.AppendLine($"  Добавлен факт: {newFactText}");
			sb.AppendLine();

			stepNo++;
		}
		
		if (!string.IsNullOrEmpty(result.TargetFactId))
		{
			var goalId = result.TargetFactId!;
			bool reached = result.FinalFacts.Contains(goalId);

			if (kb.Facts.TryGetValue(goalId, out var goalFact))
			{
				sb.AppendLine(reached
					? $"Целевой факт достигнут: {goalId} ({goalFact.Description})"
					: $"Целевой факт НЕ достигнут: {goalId} ({goalFact.Description})");
			}
			else
			{
				sb.AppendLine(reached
					? $"Целевой факт достигнут: {goalId}"
					: $"Целевой факт НЕ достигнут: {goalId}");
			}

			sb.AppendLine();
		}
		
		sb.AppendLine("Все достигнутые факты:");
		foreach (var factId in result.FinalFacts)
		{
			if (kb.Facts.TryGetValue(factId, out var fact))
				sb.AppendLine($"- {factId} ({fact.Description})");
			else
				sb.AppendLine($"- {factId}");
		}

		return sb.ToString();
	}

	
	public static string BuildBackwardExplanation(
		KnowledgeBase kb,
		string goalFactId,
		BackwardResult result)
	{
		var sb = new StringBuilder();

		if (!result.Success)
		{
			sb.AppendLine($"Целевой факт {goalFactId} не удалось вывести.");
			return sb.ToString();
		}

		if (kb.Facts.TryGetValue(goalFactId, out var goal))
			sb.AppendLine($"Цель: {goalFactId} ({goal.Description})");
		else
			sb.AppendLine($"Цель: {goalFactId}");

		sb.AppendLine();

		int stepNo = 1;
		foreach (var app in result.RuleSequence)
		{
			string conclText = kb.Facts.TryGetValue(app.DerivedFactId, out var concl)
				? $"{app.DerivedFactId} ({concl.Description})"
				: app.DerivedFactId;

			sb.AppendLine($"Шаг {stepNo}. Уровень {app.Depth}.");
			sb.AppendLine($"  Правило {app.Rule?.Id}: {app.Rule?.Explanation}");
			sb.Append("  Посылки: ");
			sb.AppendLine(string.Join(", ",
				app.Premises.Select(id => kb.Facts.TryGetValue(id, out var f)
					? $"{id} ({f.Description})"
					: id)));
			sb.AppendLine($"  Вывод: {conclText}");
			sb.AppendLine();

			stepNo++;
		}

		return sb.ToString();
	}

}
