using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtZilla.Config;
using ArtZilla.KBLFix.Models;
using ArtZilla.KBLFix.Utils;
using ArtZilla.Net.Core;
using GalaSoft.MvvmLight;

namespace ArtZilla.KBLFix.ViewModels {
	public class ViewModel : ViewModelBase { }

	public class KeyboardLayoutViewModel: ViewModel {
		public KeyboardLayout Model { get; }

		public string Name => Model.Name;

		public bool FixedLayout {
			get => Model.FixedLayout;
			set {
				if (Model.FixedLayout == value)
					return;
				Model.FixedLayout = value;
				RaisePropertyChanged(nameof(FixedLayout));
			}
		}

		public KeyboardLayoutViewModel(KeyboardLayout model) => Model = model;
	}

	public class MainWindowViewModel : ViewModel {
		public IAppConfiguration Config { get; } = ConfigManager.GetRealtime<IAppConfiguration>();

		public bool IsEnabled {
			get => Config.IsEnabled;
			set {
				if (IsEnabled == value)
					return;

				Config.IsEnabled = value;
				RaisePropertyChanged(nameof(IsEnabled));
			}
		}

		public ObservableCollection<KeyboardLayoutViewModel> Layouts
			=> new ObservableCollection<KeyboardLayoutViewModel>(_layouts);

		public MainWindowViewModel() {
			_layouts.AddRange(KeyboardLayout.Load().Select(l => new KeyboardLayoutViewModel(l)));
			_repeater = new BackgroundRepeater(Update);
		}

		private void Update() {
			if (HelpUtils.LoadLayouts().Distinct().Count() != _layouts.Count) {
				_layouts.AddRange(KeyboardLayout.Load().Select(l => new KeyboardLayoutViewModel(l)));
				RaisePropertyChanged(nameof(Layouts));
			}
		}

		private readonly BackgroundRepeater _repeater;
		private readonly List<KeyboardLayoutViewModel> _layouts = new List<KeyboardLayoutViewModel>();
	}
}
