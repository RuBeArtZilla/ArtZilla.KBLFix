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

		public bool IsAutorun {
			get => _autorun.IsEnabled();
			set {
				if (IsAutorun == value)
					return;
				_autorun.SetEnabled(value);
				RaisePropertyChanged(nameof(IsAutorun));
			}
		}

		public ObservableCollection<KeyboardLayoutViewModel> Layouts
			=> new ObservableCollection<KeyboardLayoutViewModel>(_layouts);

		public bool IsVisible {
			get => _isVisible;
			set {
				if (IsVisible == value)
					return;
				_isVisible = value;
				_repeater.Enabled(value);
				RaisePropertyChanged(nameof(IsVisible));
			}
		}

		public MainWindowViewModel() {
			_layouts.AddRange(KeyboardLayout.Load().Select(l => new KeyboardLayoutViewModel(l)));
			_repeater = new BackgroundRepeater(Update, TimeSpan.FromSeconds(3D), true);
		}

		private void Update() {
			if (!IsVisible)
				return;

			if (HelpUtils.LoadLayouts().Distinct().Count() != _layouts.Count) {
				_layouts.Clear();
				_layouts.AddRange(KeyboardLayout.Load().Select(l => new KeyboardLayoutViewModel(l)));
				RaisePropertyChanged(nameof(Layouts));
			}
		}

		private bool _isVisible;
		private readonly BackgroundRepeater _repeater;
		private readonly AutorunManager _autorun = new AutorunManager("KBLFix", args: "-hide");
		private readonly List<KeyboardLayoutViewModel> _layouts = new List<KeyboardLayoutViewModel>();
	}
}
