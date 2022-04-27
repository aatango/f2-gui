using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace f2_gui;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
	public MainWindow() {
		InitializeComponent();
	}

	private void ExitProgram(object sender, RoutedEventArgs e) { Close(); }

	private void AboutPage(object sender, RoutedEventArgs e) {
		System.Diagnostics.Process.Start(
			new System.Diagnostics.ProcessStartInfo {
				FileName = "https://github.com/aatango/f2-gui",
				UseShellExecute = true
			}
		);
	}

	public List<Point> PointList = new List<Point>();

	// In more developed version, Line would be it's own object.
	// Not using Drawing.Line because that uses starting and end coordinates only, not start and end points.
	public List<Point[]> LineList = new List<Point[]>();

	private void AddLine(object sender, RoutedEventArgs e) {
		var AddWindow = new AddLineWindow();
		AddWindow.Show();
	}

	private void LeftClickOnCanvas(object sender, MouseButtonEventArgs e) {

	}

	private void NewCanvas(object sender, RoutedEventArgs e) {
		LineList.Clear();
		PointList.Clear();
		Canvas.Children.Clear();

		// Re-add axis
		var AxisX = new Line {
			X1 = 0,
			Y1 = 225,
			X2 = 8000,
			Y2 = 225,

			Stroke = Brushes.LightSteelBlue,
			StrokeDashArray = new DoubleCollection { 1 },
			StrokeThickness = 1
		};
		var AxisY = new Line {
			X1 = 400,
			Y1 = 0,
			X2 = 400,
			Y2 = 8000,

			Stroke = Brushes.LightSteelBlue,
			StrokeDashArray = new DoubleCollection { 1 },
			StrokeThickness = 1
		};

		Canvas.Children.Add(AxisX);
		Canvas.Children.Add(AxisY);
	}


	private void SaveFile(object sender, RoutedEventArgs e) {
		var SaveFileDialog = new Microsoft.Win32.SaveFileDialog();

		SaveFileDialog.FileName = "Document1";
		SaveFileDialog.DefaultExt = ".f2g";
		SaveFileDialog.Filter = "f2 Geometry (*.f2g) | *.f2g";

		Nullable<bool> SaveFile = SaveFileDialog.ShowDialog();

		if (SaveFile == true) {
			string FileContent = "PT" + ' ' + PointList.Count.ToString() + '\n';
			uint PointNo = 1;
			uint LineNo = 1;

			// Save Points
			foreach (var Point in PointList) {
				FileContent += PointNo.ToString() + '\t' + Point.X + ' ' + Point.Y + '\n';
				++PointNo;
				}

			FileContent += "EL" + ' ' + LineList.Count.ToString() + '\n';

			foreach (var Line in LineList) {
				FileContent += LineNo.ToString() + '\t' + (PointList.IndexOf(Line[0]) + 1) + ' ' + (PointList.IndexOf(Line[1]) + 1) + '\n';
				++LineNo;
			}
			// save the fle
			System.IO.File.WriteAllTextAsync(SaveFileDialog.FileName, FileContent);
		}
	}

	private void OpenFile(object sender, RoutedEventArgs e) {
		var OpenFileDialog = new Microsoft.Win32.OpenFileDialog();

		OpenFileDialog.FileName = "Document1";
		OpenFileDialog.DefaultExt = ".f2g";
		OpenFileDialog.Filter = "f2 Geometry (*.f2g) | *.f2g";

		Nullable<bool> OpenFile = OpenFileDialog.ShowDialog();


	}

	// Does not verify if line is a duplicate, or uses existing points.
	public void AddLine(Point _StartPoint, Point _EndPoint) {
		if (!PointList.Contains(_StartPoint)) { PointList.Add(_StartPoint); }
		if (!PointList.Contains(_EndPoint)) { PointList.Add(_EndPoint); }

		Point[] NewLine = { _StartPoint, _EndPoint };
		LineList.Add(NewLine);

		// Transforming from real space to screen space. In production should be done through an actual transformation that is dependent on the windows size, position, and scale.
		// Since this is just a mock-up, it's hard-coded.
		var DrawLine = new Line {
			X1 = 800 / 2 + 10 * _StartPoint.X,
			Y1 = 450 / 2 - 10 * _StartPoint.Y,
			X2 = 800 / 2 + 10 * _EndPoint.X,
			Y2 = 450 / 2 - 10 * _EndPoint.Y,

			Stroke = Brushes.DarkOliveGreen,
			StrokeThickness = 2
		};

		Canvas.Children.Add(DrawLine);
	}
}
