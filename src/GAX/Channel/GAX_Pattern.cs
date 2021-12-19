using System.Collections.Generic;

namespace BinarySerializer.GBA.Audio.GAX
{
    public class GAX_Pattern : BinarySerializable {
        public ushort Duration { get; set; }
        public bool IsEmptyTrack { get; set; }
        public GAX_PatternRow[] Rows { get; set; }
        public Pointer EndOffset { get; set; }

        public override void SerializeImpl(SerializerObject s) {
            IsEmptyTrack = s.Serialize<bool>(IsEmptyTrack, name: nameof(IsEmptyTrack));
            if(IsEmptyTrack) EndOffset = s.CurrentPointer;
            if (Rows == null) {
                List<GAX_PatternRow> rows = new List<GAX_PatternRow>();
                bool isEndOfTrack = IsEmptyTrack;
                int curDuration = 0;
                while (!isEndOfTrack) {
                    GAX_PatternRow row = s.SerializeObject<GAX_PatternRow>(default, name: $"{nameof(Rows)}[{rows.Count}]");
                    rows.Add(row);
                    curDuration += row.Duration;
                    if (curDuration >= Duration) {
                        isEndOfTrack = true;
                        EndOffset = s.CurrentPointer;
                        s.Log($"GAX2 Track Duration: {curDuration} - Last Command: {row.Command}");
                    }
                }
                Rows = rows.ToArray();
            } else {
				Rows = s.SerializeObjectArray<GAX_PatternRow>(Rows, Rows.Length, name: nameof(Rows));
			}
        }
    }
}