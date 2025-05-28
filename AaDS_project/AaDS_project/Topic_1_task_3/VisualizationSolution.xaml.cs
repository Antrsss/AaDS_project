using Microsoft.Maui.Graphics; 

namespace AaDS_project.Topic_1_task_3;

public partial class VisualizationSolution : ContentPage
{
    private TreeNode _treeRoot;
    private List<TreeNode> _candidates = new();
    private int _currentStep = 0;
    private List<string> _solutionSteps = new();

    private Color _currentLeafColor = Colors.Green;
    private bool _isDeleting = false;

    public VisualizationSolution()
    {
        InitializeComponent();
    }

    private void OnBuildTreeClicked(object sender, EventArgs e)
    {
        try
        {
            var inputLines = TreeInput.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (inputLines.Length == 0)
            {
                DisplayAlert("������", "������� ������", "OK");
                return;
            }

            var solution = new Solution();
            _treeRoot = solution.BuildTreeFromInput(inputLines);

            OriginalTreeView.Drawable = new TreeDrawable(_treeRoot);
            OriginalTreeView.Invalidate();

            InfoLabel.Text = "������ ������� ���������. ������� '��������������� �������'";
            VisualizeSolutionButton.IsEnabled = true;

            _currentStep = 0;
            SolutionTreeView.Drawable = new TreeDrawable(_treeRoot);
            SolutionTreeView.Invalidate();
            StepDescription.Text = "�������� ������";
        }
        catch (Exception ex)
        {
            DisplayAlert("������", $"�������� ������ �����: {ex.Message}", "OK");
        }
    }

    private void OnVisualizeSolutionClicked(object sender, EventArgs e)
    {
        if (_treeRoot == null)
        {
            DisplayAlert("������", "������� ��������� ������", "OK");
            return;
        }

        var solution = new Solution();
        _candidates = new List<TreeNode>();
        solution.FindNodesWithCondition(_treeRoot, _candidates);

        _solutionSteps = new List<string>();

        if (_candidates.Count == 0)
        {
            _solutionSteps.Add("��� ���������� ������ ��� �������� (��� ������, ��� ������� ���������� ����� � ����������� ����� 1).");
        }
        else
        {
            _solutionSteps.Add($"������� {_candidates.Count} ������, ��������������� ������� (|����� ���������| - |������ ���������| = 1):");
            _solutionSteps.AddRange(_candidates.Select((node, i) => $"{i + 1}. ��������: {node.Value}"));

            _candidates.Sort((a, b) => a.Value.CompareTo(b.Value));
            int middleIndex = _candidates.Count / 2;
            var targetNode = _candidates[middleIndex];

            _solutionSteps.Add($"\n��������������� ������ ����������: {string.Join(", ", _candidates.Select(n => n.Value))}");
            _solutionSteps.Add($"�������� ������� ������� (������ {middleIndex}): �������� {targetNode.Value}");

            // ��������� ���� ��� ������������ ������
            _solutionSteps.Add("�������� ����� ������ ��� ������ �������...");
            _solutionSteps.Add("����� ������ ���������...");
            _solutionSteps.Add("����� ������� ���������...");
            _solutionSteps.Add("������ �������, �������� ����...");

            _solutionSteps.Add($"\n������� ������� {targetNode.Value} �� ������...");
        }

        _currentStep = 0;
        _isDeleting = false;
        UpdateVisualization();

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(1.5); // ��������� �������� ��� ������ ������������
        timer.Tick += async (s, args) =>
        {
            if (_currentStep < _solutionSteps.Count - 1)
            {
                _currentStep++;

                // ������ ���� ������� �� ����� ������
                if (_solutionSteps[_currentStep].Contains("�����") ||
                    _solutionSteps[_currentStep].Contains("�������"))
                {
                    _currentLeafColor = _currentLeafColor == Colors.Green ? Colors.Orange : Colors.Green;
                }

                // �������� ����� ���������
                if (_solutionSteps[_currentStep].Contains("������� �������") && !_isDeleting)
                {
                    _isDeleting = true;
                    await Task.Delay(1000); // �������� ����� ���������

                    var solution = new Solution();
                    var targetNode = _candidates[_candidates.Count / 2];
                    _treeRoot = solution.RightDelete(_treeRoot, targetNode.Value);

                    var traversal = solution.PreorderTraversal(_treeRoot);
                    _solutionSteps.Add($"\n����� ������ ����� �������� (preorder): {string.Join(", ", traversal)}");
                }

                UpdateVisualization();
            }
            else
            {
                timer.Stop();
            }
        };
        timer.Start();
    }

    private void UpdateVisualization()
    {
        StepDescription.Text = _solutionSteps[_currentStep];

        // ��������� Drawable � ������� ������ �������
        SolutionTreeView.Drawable = new TreeDrawable(_treeRoot, _currentLeafColor);

        if (_currentStep == _solutionSteps.Count - 1 && _candidates.Count > 0)
        {
            ResultLabel.Text = $"������� ������� �� ���������: {_candidates[_candidates.Count / 2].Value}";

            var solution = new Solution();
            var traversal = solution.PreorderTraversal(_treeRoot);
            TraversalResult.Text = string.Join(", ", traversal);
        }

        SolutionTreeView.Invalidate();
    }
}

public class TreeDrawable : IDrawable
{
    private readonly TreeNode _root;
    private readonly Dictionary<TreeNode, (PointF Center, RectF Bounds)> _nodePositions = new();
    private float _scale = 1.0f;
    private Color _leafColor;

    public TreeDrawable(TreeNode root, Color leafColor = null)
    {
        _root = root;
        _leafColor = leafColor ?? Colors.Green;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_root == null) return;

        canvas.FontColor = Colors.Black;
        canvas.FontSize = 14;

        CalculateScale(dirtyRect);
        DrawConnections(canvas, _root);
        DrawNodes(canvas);
    }

    private void DrawNodes(ICanvas canvas)
    {
        foreach (var kvp in _nodePositions)
        {
            var node = kvp.Key;
            var pos = kvp.Value.Center;
            var bounds = kvp.Value.Bounds;

            // ���������� ���� ����
            Color nodeColor;
            if (node.Left == null && node.Right == null) // ��� ����
            {
                nodeColor = _leafColor;
            }
            else if (node == _root) // ��� ������
            {
                nodeColor = Color.FromArgb("#512BD4");
            }
            else // ������� ����
            {
                nodeColor = Color.FromArgb("#2B0B98");
            }

            // ������ ����
            canvas.FillColor = nodeColor;
            canvas.FillCircle(pos.X * _scale, pos.Y * _scale, 15 * _scale);

            // ������ �����
            canvas.FontColor = Colors.White;
            canvas.DrawString(node.Value.ToString(),
                new RectF(bounds.X * _scale, bounds.Y * _scale, bounds.Width * _scale, bounds.Height * _scale),
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
        }
    }

    private void CalculateScale(RectF dirtyRect)
    {
        // ������� ������������ ������� ��� ���������������
        CalculateNodePositions(_root, 300, 30, 150);

        // ������� ������������ ����������
        float maxX = _nodePositions.Values.Max(p => p.Center.X);
        float maxY = _nodePositions.Values.Max(p => p.Center.Y);

        // ������������ �������
        float requiredWidth = maxX + 50;
        float requiredHeight = maxY + 50;

        _scale = Math.Min(
            dirtyRect.Width / requiredWidth,
            dirtyRect.Height / requiredHeight
        );

        // ������������ �������
        _scale = Math.Min(Math.Max(_scale, 0.5f), 2.0f);
    }

    private void CalculateNodePositions(TreeNode node, float x, float y, float xOffset)
    {
        if (node == null) return;

        _nodePositions[node] = (new PointF(x, y), new RectF(x - 15, y - 15, 30, 30));

        if (node.Left != null)
        {
            CalculateNodePositions(node.Left, x - xOffset, y + 60, xOffset / 1.8f);
        }

        if (node.Right != null)
        {
            CalculateNodePositions(node.Right, x + xOffset, y + 60, xOffset / 1.8f);
        }
    }

    private void DrawConnections(ICanvas canvas, TreeNode node)
    {
        if (node == null) return;

        if (node.Left != null && _nodePositions.TryGetValue(node.Left, out var leftPos))
        {
            var start = _nodePositions[node].Center;
            var end = leftPos.Center;
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2 * _scale;
            canvas.DrawLine(start.X * _scale, start.Y * _scale, end.X * _scale, end.Y * _scale);
            DrawConnections(canvas, node.Left);
        }

        if (node.Right != null && _nodePositions.TryGetValue(node.Right, out var rightPos))
        {
            var start = _nodePositions[node].Center;
            var end = rightPos.Center;
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2 * _scale;
            canvas.DrawLine(start.X * _scale, start.Y * _scale, end.X * _scale, end.Y * _scale);
            DrawConnections(canvas, node.Right);
        }
    }
}
