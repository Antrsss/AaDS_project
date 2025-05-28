using AaDS_project.Topic_3_task_4;
using System.Diagnostics;

namespace AaDS_project.Topic_4_Task_9;

public partial class LeadTimeSolutions : ContentPage
{
	public LeadTimeSolutions()
	{
		InitializeComponent();
	}

    private void RunButton_1_Clicked(object sender, EventArgs e)
    {
        int[,] sparseMatrix = new int[,]
        {
            { 0, 1, 0, 0, 0 },
            { 1, 0, 1, 0, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 1, 0, 1 },
            { 0, 0, 0, 1, 0 }
        };

        int[,] denseMatrix = new int[,]
        {
            { 0, 1, 1, 1, 1 },
            { 1, 0, 1, 1, 1 },
            { 1, 1, 0, 1, 1 },
            { 1, 1, 1, 0, 1 },
            { 1, 1, 1, 1, 0 }
        };

        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        var (isConnected, componentSize) = GraphAnalyzer.Solution1(sparseMatrix);
        stopwatch.Stop();

        double elapsedSeconds_sparse = stopwatch.Elapsed.TotalSeconds;

        stopwatch.Start();
        (isConnected, componentSize) = GraphAnalyzer.Solution1(denseMatrix);
        stopwatch.Stop();

        double elapsedSeconds_dense = stopwatch.Elapsed.TotalSeconds;

        ResultLabel_1.Text = $"����� ���������� (���� �����): {elapsedSeconds_sparse:F6} ������";
        ResultLabel_2.Text = $"����� ���������� (����� �����): {elapsedSeconds_dense:F6} ������";
    }

    private void RunButton_2_Clicked(object sender, EventArgs e)
    {
        int[,] sparseMatrix = new int[,]
        {
            { 0, 1, 0, 0, 0 },
            { 1, 0, 1, 0, 0 },
            { 0, 1, 0, 1, 0 },
            { 0, 0, 1, 0, 1 },
            { 0, 0, 0, 1, 0 }
        };

        int[,] denseMatrix = new int[,]
        {
            { 0, 1, 1, 1, 1 },
            { 1, 0, 1, 1, 1 },
            { 1, 1, 0, 1, 1 },
            { 1, 1, 1, 0, 1 },
            { 1, 1, 1, 1, 0 }
        };

        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();
        var (isConnected, componentSize) = GraphAnalyzer.Solution2(sparseMatrix);
        stopwatch.Stop();

        double elapsedSeconds_sparse = stopwatch.Elapsed.TotalSeconds;

        stopwatch.Start();
        (isConnected, componentSize) = GraphAnalyzer.Solution2(denseMatrix);
        stopwatch.Stop();

        double elapsedSeconds_dense = stopwatch.Elapsed.TotalSeconds;

        ResultLabel_3.Text = $"����� ���������� (���� �����): {elapsedSeconds_sparse:F6} ������";
        ResultLabel_4.Text = $"����� ���������� (����� �����): {elapsedSeconds_dense:F6} ������";
    }
}