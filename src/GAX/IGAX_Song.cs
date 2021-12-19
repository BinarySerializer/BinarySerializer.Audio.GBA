using System;
using System.Collections.Generic;
using System.Text;

namespace BinarySerializer.GBA.Audio.GAX {
	public interface IGAX_Song {
		public Pointer Offset { get; }
		public GAX_SongInfo Info { get; }
		public GAX_UnknownC UnknownC { get; }

		public GAX_Channel GetChannel(int i);
	}
}
