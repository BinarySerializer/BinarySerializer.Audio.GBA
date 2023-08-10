using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Audio.GBA.GAX {
	public class GAX_Channel : GAX_Entity {
		public GAX_SongInfo SongInfo => Song?.Info ?? Handler?.Children[0]?.Value?.GetData<GAX_SongInfo>();

		public GAX_PatternHeader[] PatternHeaders { get; set; }
		public GAX_Pattern[] Patterns { get; set; }

		public override void SerializeImpl(SerializerObject s) {
			PatternHeaders = s.SerializeObjectArray<GAX_PatternHeader>(PatternHeaders, SongInfo.NumPatternsPerChannel, name: nameof(PatternHeaders));
			if (Patterns == null) Patterns = new GAX_Pattern[PatternHeaders.Length];
			for (int i = 0; i < Patterns.Length; i++) {
				s.DoAt(SongInfo.SequenceDataPointer + PatternHeaders[i].SequenceOffset, () => {
					Patterns[i] = s.SerializeObject<GAX_Pattern>(Patterns[i], onPreSerialize: t => t.Duration = SongInfo.NumRowsPerPattern, name: $"{nameof(Patterns)}[{i}]");
				});
			}
		}
	}
}