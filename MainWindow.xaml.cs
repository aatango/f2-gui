using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace f2_gui;
/// <summary>
/// Proof of concept for f2 GUI.
/// 
/// Lessons can be taken from this endeavour, and this might work as a test bed for further prototyping, but
/// THIS IS NOT INTENDED FOR PRODUCTION, NOR FOR REFACTORING INTO A PRODUCTION READY PRODUCT.
/// </summary>
public partial class MainWindow : Window {
	public MainWindow() { InitializeComponent(); }


	// In more developed version, both of the following data structures would be their own objects.
	public List<Point> PointList = new List<Point>();
	public List<Point[]> LineList = new List<Point[]>();    // Not using List of Drawing.Line because that uses starting and end coordinates only, not start and end points.

	// Reset canvas
	private void ResetCanvas() {
		LineList.Clear();
		PointList.Clear();
		Canvas.Children.Clear();

		// Re-add axis
		var AxisX = new Line {
			X1 = 0,
			Y1 = 225,
			X2 = 1000,
			Y2 = 225,

			Stroke = Brushes.LightSteelBlue,
			StrokeDashArray = new DoubleCollection { 1 },
			StrokeThickness = 1
		};
		var AxisY = new Line {
			X1 = 400,
			Y1 = 0,
			X2 = 400,
			Y2 = 1000,

			Stroke = Brushes.LightSteelBlue,
			StrokeDashArray = new DoubleCollection { 1 },
			StrokeThickness = 1
		};

		Canvas.Children.Add(AxisX);
		Canvas.Children.Add(AxisY);
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


	//Menu File
	private void NewCanvas(object sender, RoutedEventArgs e) {
		// Should ask for user confirmation before executing command.
		ResetCanvas();
	}
	private void OpenFile(object sender, RoutedEventArgs e) {
		ResetCanvas();

		var OpenFileDialog = new Microsoft.Win32.OpenFileDialog();

		OpenFileDialog.FileName = "Document1";
		OpenFileDialog.DefaultExt = ".f2g";
		OpenFileDialog.Filter = "f2 Geometry (*.f2g) | *.f2g";

		OpenFileDialog.ShowDialog();

		var FileStream = OpenFileDialog.OpenFile();


		using (System.IO.StreamReader reader = new(FileStream)) {
			while (reader.Peek() >= 0) {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				string FileContent = reader.ReadLine(); // Code will not be null, as we're testing EOF at the while loop level.
				string[] Line = FileContent.Split(' '); // See above.


				if (Line[0].ToUpper() == "PT") {
					byte PointCount = Convert.ToByte(Line[1]);

					for (byte i = 0; i < PointCount; ++i) {
						string PointLine = reader.ReadLine();
						string[] PointCoordinates = PointLine.Split(' ');

						Point Point = new(Convert.ToDouble(PointCoordinates[0]), Convert.ToDouble(PointCoordinates[1]));
						PointList.Add(Point);
					}
				} else if (Line[0].ToUpper() == "EL") {
					byte LineCount = Convert.ToByte(Line[1]);

					for (byte i = 0; i < LineCount; ++i) {
						string? LineLine = reader.ReadLine();

						if (LineLine == null) { return; }

						string[] LinePoints = LineLine.Split(' ');
						byte StartPoint = Convert.ToByte(LinePoints[0]);
						byte EndPoint = Convert.ToByte(LinePoints[1]);
						AddLine(PointList[StartPoint - 1], PointList[EndPoint - 1]);
					}
				}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			}
		}
	}
	private void SaveFile(object sender, RoutedEventArgs e) {
		var SaveFileDialog = new Microsoft.Win32.SaveFileDialog();

		SaveFileDialog.FileName = "Document1";
		SaveFileDialog.DefaultExt = ".f2g";
		SaveFileDialog.Filter = "f2 Geometry (*.f2g) | *.f2g";

		Nullable<bool> SaveFile = SaveFileDialog.ShowDialog();

		if (SaveFile == true) {
			string FileContent = "PT" + ' ' + PointList.Count.ToString() + '\n';

			// Save Points
			foreach (var Point in PointList) {
				FileContent += Point.X.ToString() + ' ' + Point.Y.ToString() + '\n';
			}

			FileContent += "EL" + ' ' + LineList.Count.ToString() + '\n';

			foreach (var Line in LineList) {
				FileContent += (PointList.IndexOf(Line[0]) + 1).ToString() + ' ' + (PointList.IndexOf(Line[1]) + 1).ToString() + '\n';
			}
			// save the fle
			System.IO.File.WriteAllTextAsync(SaveFileDialog.FileName, FileContent);
		}
	}
	private void ExitProgram(object sender, RoutedEventArgs e) { Close(); }

	// Menu Geometry
	private void Click_AddLine(object sender, RoutedEventArgs e) {
		var AddWindow = new AddLineWindow();
		AddWindow.Show();
	}

	// Menu Help
	private void AboutPage(object sender, RoutedEventArgs e) {
		System.Diagnostics.Process.Start(
			new System.Diagnostics.ProcessStartInfo {
				FileName = "https://github.com/aatango/f2-gui",
				UseShellExecute = true
			}
		);
	}
}



