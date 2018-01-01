using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using ArtZilla.KBLFix.Utils;
using ArtZilla.Config;
using ArtZilla.Net.Core.Extensions;

namespace ArtZilla.KBLFix.Models {
	public class KeyboardLayout {
		public uint Id { get; }

		public IntPtr Handle { get; }

		public string Name { get; }

		public bool FixedLayout {
			get => ConfigManager.GetReadonly<IAppConfiguration>().Handles?.Contains(Id) ?? false;
			set {
				var cfg = ConfigManager.GetRealtime<IAppConfiguration>();
				if (value) {
					cfg.Handles = (cfg.Handles ?? Enumerable.Empty<uint>()).Append(Id).Distinct().ToArray();
				} else {
					cfg.Handles = (cfg.Handles ?? Enumerable.Empty<uint>()).Where(i => i != Id).Distinct().ToArray();
				}
			}
		}

		public KeyboardLayout(IntPtr handle): this(handle, HelpUtils.GetNameFromReg(handle)) { }

		public KeyboardLayout(IntPtr handle, string name) {
			Handle = handle;
			Name = name;
			Id = (uint) Handle;
		}

		public override string ToString() => "[0x" + Id.ToString("X8") + "] " + Name;

		public void Unload() => HelpUtils.UnloadKeyboardLayout(Handle);

		public static IEnumerable<KeyboardLayout> Load()
			=> HelpUtils
				.LoadLayouts()
				.Distinct()
				.Select(handle => new KeyboardLayout(handle));
	}
}
