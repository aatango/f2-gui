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

	private void NewLineButton(object sender, RoutedEventArgs e) {
		// Does not sanitise input before attempting to add line. Does not verify if line is a duplicate, or uses existing points. etc.
		var NewLine = new Line {
			X1 = Convert.ToDouble(XCoordStart.Text),
			Y1 = Convert.ToDouble(YCoordStart.Text),
			X2 = Convert.ToDouble(XCoordEnd.Text),
			Y2 = Convert.ToDouble(YCoordEnd.Text),

			Stroke = Brushes.DarkOliveGreen,
			StrokeThickness = 2
		};
		MainWindow.LineList.Add(NewLine);

		// Transforming from real space to screen space. In production should be done through an actual transformation that is dependent on the windows size, position, and scale.
		// Since this is just a mock-up, it's hard-coded.
		var DrawLine = new Line {
			X1 = 800 / 2 + 10 * Convert.ToDouble(XCoordStart.Text),
			Y1 = 450 / 2 - 10 * Convert.ToDouble(YCoordStart.Text),
			X2 = 800 / 2 + 10 * Convert.ToDouble(XCoordEnd.Text),
			Y2 = 450 / 2 - 10 * Convert.ToDouble(YCoordEnd.Text),

			Stroke = Brushes.DarkOliveGreen,
			StrokeThickness = 2
		};

		MainWindow.Canvas.Children.Add(DrawLine);
	}
}
