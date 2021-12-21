using System.Text.RegularExpressions;

namespace BinarySerializer.GBA.Audio.GAX
{
	public class GAX_Settings {
		public int MajorVersion { get; set; }
		public int MinorVersion { get; set; }
		public string MinorVersionAdd { get; set; }
		public string VersionString { get; set; }
		public string DateString { get; set; }
		public string FullVersionString { get; set; }

		public bool EnableErrorChecking { get; set; } = false;

		private const string VersionCheckString = "GAX Sound Engine ";
		public bool SerializeVersion(SerializerObject s) {
			var str = s.SerializeString(default, VersionCheckString.Length);
			if (str != VersionCheckString) return false;

			s.Goto(s.CurrentPointer-VersionCheckString.Length);
			FullVersionString = s.SerializeString(FullVersionString, name: nameof(FullVersionString));

			const string GAXNamePattern = @"^GAX Sound Engine [Vv]?(?<major>[0-9]*).(?<minor>[0-9]*)(?<minoradd>[A-Za-z_\-][0-9A-Za-z_\-]*)?(?<dategroup> \((?<date>[^\)]*)\))? © .*";
			var m = Regex.Match(FullVersionString, GAXNamePattern);
			if (m.Success) {
				var major = m.Groups["major"].Value;
				var minor = m.Groups["minor"].Value.TrimStart('0');
				MinorVersionAdd = m.Groups["minoradd"].Success ? m.Groups["minoradd"].Value : null;
				if(int.TryParse(major, out int res_major)) MajorVersion = res_major;
				else return false;
				if(int.TryParse(minor, out int res_minor)) MinorVersion = res_minor;
				else return false;
				VersionString = $"{major}.{minor}{MinorVersionAdd ?? ""}";
				DateString = m.Groups["date"].Success ? m.Groups["date"].Value : null;
				return true;
			} else {
				return false;
			}
		}
	}
}
