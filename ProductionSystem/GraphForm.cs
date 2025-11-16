using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProductionSystem;
using ProductionSystem.Reasoners;

namespace ProductionSystem
{
    public partial class GraphForm : Form
    {
        private readonly KnowledgeBase _kb;
        private readonly HashSet<string> _forwardFacts;
        private readonly HashSet<string> _backwardFacts;
		private readonly HashSet<string> _forwardRules;
		private readonly HashSet<string> _backwardRules;
		
		private Rectangle _worldBounds = Rectangle.Empty;
		private float _zoom = 1.0f;
		private const float MinZoom = 0.2f;
		private const float MaxZoom = 5f;
		
		private const int RuleNodeSize = 14;

		private Dictionary<string, int> _factLevels = new Dictionary<string, int>();
		
        private readonly Dictionary<string, Rectangle> _factRects = new Dictionary<string, Rectangle>();

        public GraphForm(
            KnowledgeBase kb,
            ForwardResult? forwardResult,
            BackwardResult? backwardResult)
        {
            _kb = kb;
            _forwardFacts = forwardResult != null
                ? new HashSet<string>(forwardResult.FinalFacts)
                : new HashSet<string>();

            _backwardFacts = backwardResult != null
                ? new HashSet<string>(backwardResult.UsedFacts)
                : new HashSet<string>();
			
			var fRules = new HashSet<string>();
			if (forwardResult != null)
			{
				foreach (var step in forwardResult.Steps)
				{
					if (step.Rule != null)
						fRules.Add(step.Rule.Id);
				}
			}
			_forwardRules = fRules;

			var bRules = new HashSet<string>();
			if (backwardResult != null)
			{
				foreach (var app in backwardResult.RuleSequence)
				{
					if (app.Rule != null)
						bRules.Add(app.Rule.Id);
				}
			}
			_backwardRules = bRules;

            InitializeComponent();
			
			ComputeFactLevels();
			LayoutFactNodes();                  
			panelGraph.Resize += (_, __) =>     
			{
				LayoutFactNodes();
				panelGraph.Invalidate();
			};
			
			panelGraph.MouseWheel += panelGraph_MouseWheel;
			this.Shown += (_, __) => panelGraph.Focus(); 
        }

		private void panelGraph_Paint(object? sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.Clear(panelGraph.BackColor);

			var scroll = panelGraph.AutoScrollPosition;
			e.Graphics.TranslateTransform(scroll.X, scroll.Y);
			e.Graphics.ScaleTransform(_zoom, _zoom);

			DrawRuleEdges(e.Graphics);
			DrawFactNodes(e.Graphics);
		}



		private void LayoutFactNodes()
		{
			_factRects.Clear();

			const int margin = 20;
			const int nodeWidth = 220;
			const int nodeHeight = 40;
			const int vSpacing = 45;
			const int hSpacing = 200;

			if (_kb.Facts.Count == 0)
			{
				_worldBounds = new Rectangle(0, 0, panelGraph.Width, panelGraph.Height);
				UpdateScrollArea();
				return;
			}
			
			var factsByLevel = _kb.Facts.Values
								  .GroupBy(f => _factLevels.TryGetValue(f.Id, out var lvl) ? lvl : 0)
								  .OrderBy(g => g.Key);

			var levelX = new Dictionary<int, int>();

			int currentX = margin;

			foreach (var group in factsByLevel)
			{
				int level = group.Key;
				levelX[level] = currentX;
				currentX += nodeWidth + hSpacing;
			}

			var levelY = new Dictionary<int, int>();
			foreach (var group in factsByLevel)
				levelY[group.Key] = margin;

			foreach (var group in factsByLevel)
			{
				int level = group.Key;

				foreach (var fact in group.OrderBy(f => f.Id))
				{
					int x = levelX[level];
					int y = levelY[level];

					var rect = new Rectangle(x, y, nodeWidth, nodeHeight);
					_factRects[fact.Id] = rect;

					levelY[level] = y + nodeHeight + vSpacing;
				}
			}

			int right = _factRects.Values.Max(r => r.Right);
			int bottom = _factRects.Values.Max(r => r.Bottom);
			_worldBounds = new Rectangle(0, 0, right + margin, bottom + margin);

			UpdateScrollArea();
		}


		private void UpdateScrollArea()
		{
			var size = new Size(
				(int)(_worldBounds.Width * _zoom),
				(int)(_worldBounds.Height * _zoom));

			panelGraph.AutoScrollMinSize = size;
		}



        private void DrawFactNodes(Graphics g)
        {
            foreach (var fact in _kb.Facts.Values)
            {
                if (!_factRects.TryGetValue(fact.Id, out var rect))
                    continue;

                Color backColor;
                switch (fact.Kind)
                {
                    case FactKind.Input:
                        backColor = Color.LightGreen;
                        break;
                    case FactKind.Target:
                        backColor = Color.LightSkyBlue;
                        break;
                    case FactKind.Intermediate:
                        backColor = Color.LightYellow;
                        break;
                    default:
                        backColor = Color.White;
                        break;
                }

                bool usedForward = _forwardFacts.Contains(fact.Id);
                bool usedBackward = _backwardFacts.Contains(fact.Id);

                if (usedForward && usedBackward)
                    backColor = Color.Orange;        
                else if (usedBackward)
                    backColor = Color.LightCoral;    

                using (var brush = new SolidBrush(backColor))
                using (var pen = new Pen(Color.Black, usedBackward ? 2f : 1f))
                {
                    g.FillRectangle(brush, rect);
                    g.DrawRectangle(pen, rect);
                }

                string text = $"{fact.Id}: {fact.Description}";
                using (var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    g.DrawString(text, this.Font, Brushes.Black, rect, sf);
                }
            }
        }

		private void DrawRuleEdges(Graphics g)
		{
		    foreach (var rule in _kb.Rules)
		    {
		        if (rule == null)
		            continue;
		        
		        if (!_factRects.TryGetValue(rule.Conclusion, out var conclusionRect))
		            continue;
		
		        var targetPoint = new Point(
		            conclusionRect.Left,
		            conclusionRect.Top + conclusionRect.Height / 2);
		
		        var condPoints = new List<Point>();
		
		        foreach (var condId in rule.Conditions)
		        {
		            if (!_factRects.TryGetValue(condId, out var condRect))
		                continue;
		
		            var sourcePoint = new Point(
		                condRect.Right,
		                condRect.Top + condRect.Height / 2);
		
		            condPoints.Add(sourcePoint);
		        }
		
		        if (condPoints.Count == 0)
		            continue;
		
		        int maxX = condPoints.Max(p => p.X);
		        int sumY = condPoints.Sum(p => p.Y) / condPoints.Count + targetPoint.Y;
		
		        int ruleCenterX = (maxX + targetPoint.X) / 2;
		        int ruleCenterY = targetPoint.Y + (sumY - targetPoint.Y) / 3;
		
		        var ruleCenter = new Point(ruleCenterX, ruleCenterY);
				
		        bool usedForward = _forwardRules.Contains(rule.Id);
		        bool usedBackward = _backwardRules.Contains(rule.Id);
		
		        Color edgeColor = Color.LightGray;
		        float edgeWidth = 1f;
		
		        if (usedForward && usedBackward)
		        {
		            edgeColor = Color.OrangeRed;
		            edgeWidth = 2.5f;
		        }
		        else if (usedBackward)
		        {
		            edgeColor = Color.Red;
		            edgeWidth = 2.0f;
		        }
		        else if (usedForward)
		        {
		            edgeColor = Color.SeaGreen;
		            edgeWidth = 2.0f;
		        }
				
		        using (var pen = new Pen(edgeColor, edgeWidth))
		        {
		            foreach (var sp in condPoints)
		            {
		                g.DrawLine(pen, sp, ruleCenter);
		            }
		
		            pen.EndCap = LineCap.ArrowAnchor;
		            g.DrawLine(pen, ruleCenter, targetPoint);
		        }
		
		        int half = RuleNodeSize / 2;
		        var ruleRect = new Rectangle(
		            ruleCenter.X - half,
		            ruleCenter.Y - half,
		            RuleNodeSize,
		            RuleNodeSize);
		
		        var baseColor = rule.Conditions.Count > 1
		            ? Color.LightSalmon
		            : Color.LightGray;
		
		        Color fillColor = baseColor;
		        Color borderColor = Color.DimGray;
		        float borderWidth = 1f;
		
		        if (usedForward && usedBackward)
		        {
		            fillColor = Color.Orange;
		            borderColor = Color.OrangeRed;
		            borderWidth = 2f;
		        }
		        else if (usedBackward)
		        {
		            fillColor = Color.LightCoral;
		            borderColor = Color.Red;
		            borderWidth = 2f;
		        }
		        else if (usedForward)
		        {
		            fillColor = Color.LightGreen;
		            borderColor = Color.SeaGreen;
		            borderWidth = 2f;
		        }
		
		        using (var brush = new SolidBrush(fillColor))
		        using (var penNode = new Pen(borderColor, borderWidth))
		        {
		            g.FillEllipse(brush, ruleRect);
		            g.DrawEllipse(penNode, ruleRect);
		        }
		
		        var idBrush = (usedForward || usedBackward) ? Brushes.Black : Brushes.DimGray;
		        var idPoint = new Point(ruleRect.Right + 2, ruleRect.Top - 2);
		        g.DrawString(rule.Id, this.Font, idBrush, idPoint);
		    }
		}




		
		private void panelGraph_MouseWheel(object? sender, MouseEventArgs e)
		{
			if (!ModifierKeys.HasFlag(Keys.Control))
				return;
			
			float oldZoom = _zoom;

			if (e.Delta > 0)
				_zoom *= 1.1f;   
			else if (e.Delta < 0)
				_zoom /= 1.1f;   

			if (_zoom < MinZoom) _zoom = MinZoom;
			if (_zoom > MaxZoom) _zoom = MaxZoom;

			if (Math.Abs(_zoom - oldZoom) > 0.001f)
			{
				UpdateScrollArea();
				panelGraph.Invalidate();
			}
		}

		private void ComputeFactLevels()
		{
			_factLevels.Clear();

			foreach (var fact in _kb.Facts.Values)
			{
				if (fact.Kind == FactKind.Input)
					_factLevels[fact.Id] = 0;
			}
			
			bool changed;
			int maxIterations = 1000; 

			do
			{
				changed = false;
				maxIterations--;

				foreach (var rule in _kb.Rules)
				{
					bool allKnown = true;
					int maxLevel = 0;

					foreach (var cond in rule.Conditions)
					{
						if (!_factLevels.TryGetValue(cond, out int lvl))
						{
							allKnown = false;
							break;
						}
						if (lvl > maxLevel) maxLevel = lvl;
					}

					if (!allKnown)
						continue;

					int conclusionLevel = maxLevel + 1;

					if (!_factLevels.TryGetValue(rule.Conclusion, out int currentLevel) ||
						conclusionLevel > currentLevel)
					{
						_factLevels[rule.Conclusion] = conclusionLevel;
						changed = true;
					}
				}

			} while (changed && maxIterations > 0);

			int fallbackLevel = 0;
			if (_factLevels.Count > 0)
				fallbackLevel = _factLevels.Values.Max() + 1;

			foreach (var fact in _kb.Facts.Values)
			{
				if (!_factLevels.ContainsKey(fact.Id))
					_factLevels[fact.Id] = fallbackLevel;
			}
		}
	}
}
