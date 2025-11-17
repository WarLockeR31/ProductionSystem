using ProductionSystem.Knowledge;
using ProductionSystem.Reasoners;

namespace ProductionSystem;

public partial class MainForm : Form
{
	private KnowledgeBase _kb = new KnowledgeBase();
	
	private string _factsPath = "";
	private string _rulesPath = "";
	private ForwardResult? _lastForwardResult;
	private BackwardResult? _lastBackwardResult;

	public MainForm()
	{
		InitializeComponent();
	}

	private void InitUi()
	{
		checkedListBoxFacts.DisplayMember = "Description";
		checkedListBoxFacts.ValueMember = "Id";
		checkedListBoxFacts.DataSource = _kb.Facts.Values
											.Where(f => f.Kind == FactKind.Input)
											.OrderBy(f => f.Id)
											.ToList();

		comboBoxGoal.DisplayMember = "Description";
		comboBoxGoal.ValueMember = "Id";
		comboBoxGoal.DataSource = _kb.Facts.Values
									 .Where(f => f.Kind == FactKind.Target)
									 .OrderBy(f => f.Id)
									 .ToList();
	}
	
	private void buttonForward_Click(object sender, EventArgs e)
	{
		var initialFacts = new List<string>();

		foreach (var item in checkedListBoxFacts.CheckedItems)
		{
			var fact = (Fact)item;
			initialFacts.Add(fact.Id);
		}

		string? goalFactId = null;
		if (comboBoxGoal.SelectedItem is Fact goalFact)
			goalFactId = goalFact.Id;

		var forwardResult = ForwardReasoner.Run(_kb, initialFacts, goalFactId);
		var explanation = ExplanationBuilder.BuildForwardExplanation(_kb, forwardResult);
		
		_lastForwardResult = forwardResult;

		textBoxOutput.Text = explanation;
	}

	
	private void buttonBackward_Click(object sender, EventArgs e)
	{
		var initialFacts = new List<string>();

		foreach (var item in checkedListBoxFacts.CheckedItems)
		{
			var fact = (Fact)item;
			initialFacts.Add(fact.Id);
		}

		if (comboBoxGoal.SelectedItem == null)
			return;

		var goalFact = (Fact)comboBoxGoal.SelectedItem;
		var backwardResult = BackwardReasoner.Prove(_kb, goalFact.Id, initialFacts);
		var explanation = ExplanationBuilder.BuildBackwardExplanation(_kb, goalFact.Id, backwardResult);
		
		_lastBackwardResult = backwardResult;

		textBoxOutput.Text = explanation;
	}
	
	private void TryLoadKnowledgeBase()
	{
		if (string.IsNullOrWhiteSpace(_factsPath) ||
			string.IsNullOrWhiteSpace(_rulesPath))
		{
			return;
		}

		try
		{
			_kb = new KnowledgeBase();
			_kb.LoadFacts(_factsPath);
			_kb.LoadRules(_rulesPath);
			
			_lastForwardResult = null;
			_lastBackwardResult = null;

			InitUi();

			MessageBox.Show("База знаний успешно загружена.",
				"Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Ошибка при загрузке базы знаний:\n" + ex.Message,
				"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	private void buttonSelectFacts_Click(object sender, EventArgs e)
	{
		using var ofd = new OpenFileDialog();
		ofd.Title = "Выберите файл facts.csv";
		ofd.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";

		if (ofd.ShowDialog() == DialogResult.OK)
		{
			_factsPath = ofd.FileName;

			TryLoadKnowledgeBase();
		}
	}


	private void buttonSelectRules_Click(object sender, EventArgs e)
	{
		using var ofd = new OpenFileDialog();
		ofd.Title = "Выберите файл rules.csv";
		ofd.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";

		if (ofd.ShowDialog() == DialogResult.OK)
		{
			_rulesPath = ofd.FileName;

			TryLoadKnowledgeBase();
		}
	}

	private void buttonClearSelection_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < checkedListBoxFacts.Items.Count; i++)
		{
			checkedListBoxFacts.SetItemChecked(i, false);
		}
	}
	
	private void buttonGraph_Click(object sender, EventArgs e)
	{
		if (_kb == null || _kb.Facts.Count == 0)
		{
			MessageBox.Show("Сначала загрузите базу знаний.",
				"Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return;
		}

		var form = new GraphForm(
			_kb,
			_lastForwardResult,
			_lastBackwardResult);

		form.Show();  
	}
	
	private void buttonLoadPreset_Click(object sender, EventArgs e)
	{
		if (_kb == null || _kb.Facts.Count == 0)
		{
			MessageBox.Show("Сначала загрузите базу знаний (facts.csv и rules.csv).",
				"Нет базы знаний", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
	
		using var ofd = new OpenFileDialog();
		ofd.Title = "Выберите файл с преднастройками (аксиомы и цель)";
		ofd.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
	
		if (ofd.ShowDialog() != DialogResult.OK)
			return;
	
		string[] allLines;
		try
		{
			allLines = File.ReadAllLines(ofd.FileName);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Не удалось прочитать файл преднастроек:\n" + ex.Message,
				"Ошибка чтения файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}
	
		var lines = allLines
			.Where(l => !string.IsNullOrWhiteSpace(l))
			.ToArray();
	
		if (lines.Length < 2)
		{
			MessageBox.Show("Ожидаются минимум две непустые строки:\n" +
							"1) F***; F***; ... (аксиомы)\n" +
							"2) F*** (цель)",
				"Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
	
		var axiomsLine = lines[0];
		var separators = new[] { ';', ',', ' ' };
	
		var axiomIds = axiomsLine
			.Split(separators, StringSplitOptions.RemoveEmptyEntries)
			.Select(s => s.Trim())
			.Where(s => !string.IsNullOrEmpty(s))
			.ToList();
	
		var goalLine = lines[1];
		var goalId = goalLine.Trim();
	
		if (axiomIds.Count == 0)
		{
			MessageBox.Show("В первой строке не найдено ни одного идентификатора факта.",
				"Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
	
		if (string.IsNullOrEmpty(goalId))
		{
			MessageBox.Show("Во второй строке не найден идентификатор целевого факта.",
				"Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
	
		var unknownAxioms = axiomIds
			.Where(id => !_kb.Facts.ContainsKey(id))
			.ToList();
	
		if (!_kb.Facts.ContainsKey(goalId))
		{
			MessageBox.Show($"Целевой факт '{goalId}' отсутствует в базе знаний.",
				"Неизвестная цель", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}
	
		if (unknownAxioms.Count > 0)
		{
			var msg = "Следующие аксиомы отсутствуют в базе знаний:\n" +
					  string.Join(", ", unknownAxioms);
			MessageBox.Show(msg, "Неизвестные факты", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	
		for (int i = 0; i < checkedListBoxFacts.Items.Count; i++)
		{
			checkedListBoxFacts.SetItemChecked(i, false);
		}
	
		for (int i = 0; i < checkedListBoxFacts.Items.Count; i++)
		{
			if (checkedListBoxFacts.Items[i] is Fact fact &&
				axiomIds.Contains(fact.Id))
			{
				checkedListBoxFacts.SetItemChecked(i, true);
			}
		}
	
		bool goalFoundInCombo = false;
		for (int i = 0; i < comboBoxGoal.Items.Count; i++)
		{
			if (comboBoxGoal.Items[i] is Fact f && f.Id == goalId)
			{
				comboBoxGoal.SelectedIndex = i;
				goalFoundInCombo = true;
				break;
			}
		}
	
		if (!goalFoundInCombo)
		{
			// Такое возможно, если цель не помечена как target в facts.csv
			MessageBox.Show($"Целевой факт '{goalId}' есть в базе, " +
							"но не попадает в список целей (Kind != Target).",
				"Цель не в списке", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	
		MessageBox.Show("Преднастройки успешно применены.",
			"Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
	}
}


