using System.IO;
using System.Reflection;
using ArtZilla.Net.Core.Extensions;
using Microsoft.Win32;

namespace ArtZilla.KBLFix.Utils {
	public class AutorunManager {
		public AutorunManager() : this(null) { }
		public AutorunManager(string appName, string appPath = null, string args = null) {
			var a = Assembly.GetExecutingAssembly();
			_appName = appName ?? Path.GetFileName(a.Location);
			_appPath = appPath ?? a.Location;
			_args = args ?? string.Empty;
		}

		public void Enable()
			=> GetKey(true).SetValue(GetName(), GetValue());

		public void Disable()
			=> GetKey(true).DeleteValue(GetName(), false);

		public void SetEnabled(bool value) {
			if (value)
				Enable();
			else
				Disable();
		}

		public bool IsEnabled() 
			=> GetKey(false).GetValue(GetName()) != null;

		private RegistryKey GetKey(bool writable)
			=> Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable);

		private string GetName() => _appName;

		private string GetValue() => _appPath + (_args.IsGood() ? " " + _args : "");

		private readonly string _appName;
		private readonly string _appPath;
		private readonly string _args;
	}
}
