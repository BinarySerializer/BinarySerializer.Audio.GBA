using System.Linq;
using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_Track : BinarySerializable {
        public Entry[] Entries { get; set; }

        public int StartLoopEntryOffset { get; set; }
        public int StartLoopTime { get; set; }
        public int StartLoopEntryIndex => StartLoopEntryOffset / 8;

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) {
            Entries = s.SerializeObjectArrayUntil<Entry>(Entries, e => e.PatternIndex < 0, name: nameof(Entries));
            StartLoopEntryOffset = s.Serialize<int>(StartLoopEntryOffset, name: nameof(StartLoopEntryOffset));
			StartLoopTime = s.Serialize<int>(StartLoopTime, name: nameof(StartLoopTime));

            // Error checking
            if (StartLoopEntryOffset % 8 != 0 || StartLoopEntryIndex < 0 || StartLoopEntryIndex >= Entries.Length) {
                throw new BinarySerializableException(this, $"{nameof(StartLoopEntryOffset)} had incorrect value: {StartLoopEntryOffset}");
            }
            var lastEntry = Entries[Entries.Length-1];
            if (lastEntry.PatternIndex != -1 && lastEntry.PatternIndex != -2) {
                throw new BinarySerializableException(this, $"Last Track Entry did not have index -1 or -2: {lastEntry.PatternIndex}");
            }
        }


        public class Entry : BinarySerializable {
            public int Time { get; set; } // Start time after previous track, in ticks
            public int PatternIndex { get; set; }
            // Last entry has pattern index -1 (no loop, or startloop == endloop) or -2: Loop

            public override void SerializeImpl(SerializerObject s) {
                Time = s.Serialize<int>(Time, name: nameof(Time));
                PatternIndex = s.Serialize<int>(PatternIndex, name: nameof(PatternIndex));
            }
        }
    }
}