using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtZilla.Config;

namespace ArtZilla.KBLFix.Models {
	public interface IAppConfiguration : IConfiguration {
		[DefaultValue(10D)]
		double CheckCooldownSec { get; set; }

		uint[] Handles { get; set; }

		[DefaultValue(false)]
		bool IsEnabled { get; set; }
	}
}
