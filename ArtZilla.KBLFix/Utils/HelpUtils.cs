using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ArtZilla.KBLFix.Models;
using Microsoft.Win32;

namespace ArtZilla.KBLFix.Utils {
	public static class HelpUtils {
		[DllImport("user32.dll")]
		public static extern uint GetKeyboardLayoutList(int nBuff, [Out] IntPtr[] lpList);

		[DllImport("user32")]
		public static extern bool UnloadKeyboardLayout(IntPtr handle);

		public static IntPtr[] LoadLayouts() {
			var count = GetKeyboardLayoutList(0, null);
			var items = new IntPtr[count];
			count = GetKeyboardLayoutList(items.Length, items);
			return items;
		}

		public static string GetNameFromReg(IntPtr id)
			=> Registry.GetValue(REGPATH_KEYBOARD_LAYOUT_NAME + Handle2Reg((uint) id).ToString("X8"), REGVAL_KEYBOARD_LAYOUT_NAME, "").ToString();

		private static uint Handle2Reg(IntPtr handle)
			=> Handle2Reg((uint) handle);

		private static uint Handle2Reg(uint handle)
			=> ((handle >> 16 ^ (handle & 0x0000ffff)) == 0)
				? handle & 0x0000ffff
				: handle;

		private const string REGPATH_KEYBOARD_LAYOUT_NAME = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\keyboard layouts\\";
		private const string REGVAL_KEYBOARD_LAYOUT_NAME = "Layout Text";
	}
}
