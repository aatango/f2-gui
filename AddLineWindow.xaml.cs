using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace f2_gui;
/// <summary>
/// Interaction logic for AddLineWindow.xaml
/// </summary>
public partial class AddLineWindow : Window {
	MainWindow MainWindow = (MainWindow)Application.Current.MainWindow;

	public AddLineWindow() => InitializeComponent();
	private void ExitWindow(object sender, RoutedEventArgs e) => this.Close();

	// Does not sanitise input before attempting to add line. Does not verify if line is a duplicate, or uses existing points. etc.
	private void NewLineButton(object sender, RoutedEventArgs e) {
		var StartPoint = new Point(Convert.ToDouble(XCoordStart.Text), Convert.ToDouble(YCoordStart.Text));
		var EndPoint = new Point(Convert.ToDouble(XCoordEnd.Text), Convert.ToDouble(YCoordEnd.Text));

		if (!MainWindow.PointList.Contains(StartPoint)) { MainWindow.PointList.Add(StartPoint); }
		if (!MainWindow.PointList.Contains(EndPoint)) { MainWindow.PointList.Add(EndPoint); }

		Point[] NewLine = { StartPoint, EndPoint };
		MainWindow.LineList.Add(NewLine);

		// Transforming from real space to screen space. In production should be done through an actual transformation that is dependent on the windows size, position, and scale.
		// Since this is just a mock-up, it's hard-coded.
		var DrawLine = new Line {
			X1 = 800 / 2 + 10 * StartPoint.X,
			Y1 = 450 / 2 - 10 * StartPoint.Y,
			X2 = 800 / 2 + 10 * EndPoint.X,
			Y2 = 450 / 2 - 10 * EndPoint.Y,

			Stroke = Brushes.DarkOliveGreen,
			StrokeThickness = 2
		};

		MainWindow.Canvas.Children.Add(DrawLine);
	}
}
