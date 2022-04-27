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

	// Does not sanitise input before attempting to add line. 
	private void NewLineButton(object sender, RoutedEventArgs e) {
		var StartPoint = new Point(Convert.ToDouble(XCoordStart.Text), Convert.ToDouble(YCoordStart.Text));
		var EndPoint = new Point(Convert.ToDouble(XCoordEnd.Text), Convert.ToDouble(YCoordEnd.Text));

		MainWindow.AddLine(StartPoint, EndPoint);
	}
}
