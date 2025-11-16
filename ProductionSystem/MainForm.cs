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
}


