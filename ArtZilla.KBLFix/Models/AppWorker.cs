using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtZilla.Config;
using System.ComponentModel;
using ArtZilla.Net.Core;

namespace ArtZilla.KBLFix.Models {
	public class MainWorker {
		public MainWorker() {
			_repeater = new BackgroundRepeater(MainWork, TimeSpan.FromSeconds(_config.CheckCooldownSec), _config.IsEnabled);
			((IRealtimeConfiguration)_config).PropertyChanged += MainWorker_PropertyChanged;
		}

		private void MainWorker_PropertyChanged(object sender, PropertyChangedEventArgs e) {
			switch (e.PropertyName) {
				case nameof(IAppConfiguration.CheckCooldownSec):
					_repeater.Cooldown = TimeSpan.FromSeconds(_config.CheckCooldownSec);
					break;

				case nameof(IAppConfiguration.IsEnabled):
					_repeater.Enabled(_config.IsEnabled);
					break;
			}
		}

		private void MainWork() {
			var list = KeyboardLayout.Load().ToArray();
			if (list.Length == 0)
				return;

			foreach (var item in list) {
				if (!item.FixedLayout)
					item.Unload();
			}
		}

		private readonly BackgroundRepeater _repeater;
		private readonly IAppConfiguration _config = ConfigManager.GetRealtime<IAppConfiguration>();
	}
}
