using System;
using System.Linq;
using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX {
	public class MusyX_Song : BinarySerializable {
		// Set in OnPreSerialize
		public Pointer TracksPointer { get; set; }
		public Pointer PatternsPointer { get; set; }
		public ushort BPM { get; set; }

		public Pointer<MusyX_Track>[] Tracks { get; set; } // 17. 16 normal padded with 0, 1 at the very end
		public Pointer<MusyX_Pattern>[] Patterns { get; set; }

		/// <summary>
		/// Handles the data serialization
		/// </summary>
		/// <param name="s">The serializer object</param>
		public override void SerializeImpl(SerializerObject s) {
			s.DoWithDefaults(new SerializerDefaults() {
				PointerAnchor = Offset,
				PointerNullValue = 0,
			}, () => {
				TracksPointer = s.SerializePointer(TracksPointer, name: nameof(TracksPointer));
				PatternsPointer = s.SerializePointer(PatternsPointer, name: nameof(PatternsPointer));
				BPM = s.Serialize<ushort>(BPM, name: nameof(BPM));

				s.DoAt(TracksPointer, () => {
					Tracks = s.SerializePointerArray<MusyX_Track>(Tracks, 17, resolve: true, name: nameof(Tracks));
				});
				if (Tracks != null) {
					int patternsCount = Tracks.Max(t => t.Value?.Entries.Max(te => te.PatternIndex + 1) ?? 0);
					if (patternsCount > 0) {
						s.DoAt(PatternsPointer, () => {
							Patterns = s.SerializePointerArray<MusyX_Pattern>(Patterns, patternsCount, resolve: false, name: nameof(Patterns));
						});
						for (int i = 0; i < Tracks.Length; i++) {
							if(Tracks[i].Value == null) continue;
							bool isControlTrack = i == 16;
							foreach (var e in Tracks[i].Value.Entries) {
								if (e.PatternIndex >= 0) {
									Patterns[e.PatternIndex].Resolve(s, onPreSerialize: t => t.Pre_IsControlPattern = isControlTrack);
								}
							}
						}
					}
				}
			});
		}
	}
}