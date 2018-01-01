using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ArtZilla.Config;
using ArtZilla.Config.Configurators;
using ArtZilla.KBLFix.Models;
using ArtZilla.KBLFix.Utils;

namespace ArtZilla.KBLFix {
	public partial class App: Application {
		public App() {
			ConfigManager.AppName = "KBLFix";
			ConfigManager.CompanyName = "AZDEV";
			ConfigManager.SetDefaultConfigurator<FileConfigurator>();
			Worker = new MainWorker();
		}

		public static MainWorker Worker;
	}
}
