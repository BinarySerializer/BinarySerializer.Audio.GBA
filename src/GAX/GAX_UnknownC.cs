using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.GBA.Audio.GAX {
	public class GAX_UnknownC : GAX_Entity {
		public uint UInt_A0 { get; set; }
		public uint[] UInts_A4 { get; set; }
		public uint[] UInts_B0 { get; set; }
		public uint UInt_BC { get; set; }
		public uint UInt_C0 { get; set; }
		public uint UInt_C4 { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			UInt_A0 = s.Serialize<uint>(UInt_A0, name: nameof(UInt_A0));
			if (s.GetGAXSettings().MajorVersion >= 3) {
				UInts_A4 = s.SerializeArray<uint>(UInts_A4, 3, name: nameof(UInts_A4));
				UInts_B0 = s.SerializeArray<uint>(UInts_B0, 3, name: nameof(UInts_B0));
				UInt_BC = s.Serialize<uint>(UInt_BC, name: nameof(UInt_BC));
				UInt_C0 = s.Serialize<uint>(UInt_C0, name: nameof(UInt_C0));
				UInt_C4 = s.Serialize<uint>(UInt_C4, name: nameof(UInt_C4));
			} else {
				if (UInts_A4 == null) UInts_A4 = new uint[3];
				if (UInts_B0 == null) UInts_B0 = new uint[3];
				for (int i = 0; i < 3; i++) {
					UInts_A4[i] = s.Serialize<uint>(UInts_A4[i], name: $"{nameof(UInts_A4)}[{i}]");
					UInts_B0[i] = s.Serialize<uint>(UInts_B0[i], name: $"{nameof(UInts_B0)}[{i}]");
				}
			}
		}
	}
}