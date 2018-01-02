using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ArtZilla.KBLFix.ViewModels;

namespace ArtZilla.KBLFix.Views {
	public partial class MainWindow: Window {
		public MainWindowViewModel ViewModel {
			get => DataContext as MainWindowViewModel;
			set => DataContext = value;
		}

		public MainWindow() {
			InitializeComponent();
			ViewModel = new MainWindowViewModel();
		}

		protected override void OnInitialized(EventArgs e) {
			base.OnInitialized(e);
			if (Environment.GetCommandLineArgs().Contains("-hide")) {
				WindowState = WindowState.Minimized;
				Hide();
			}
		}

		protected override void OnStateChanged(EventArgs e) {
			if (WindowState == WindowState.Minimized) {
				Hide();
			} else {
				_previous = WindowState;
			}

			ShowInTaskbar = WindowState == WindowState.Minimized;
		}

		private void TrayDoubleClick(object sender, RoutedEventArgs e) {
			if (IsVisible) {
				WindowState = WindowState.Minimized;
			} else {
				Show();
				Activate();
				WindowState = _previous;
			}
		}

		private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
			=> ViewModel.IsVisible = WindowState == WindowState.Minimized;

		private WindowState _previous = WindowState.Normal;
	}
}
