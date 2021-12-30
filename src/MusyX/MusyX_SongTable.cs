using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_SongTable : BinarySerializable {
        // Set in OnPreSerialize
        public Pointer EndOffset { get; set; }

        public uint Length { get; set; }
        public Pointer[] SongPointers { get; set; }

        public MusyX_SongGroup[] Songs { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Length = s.Serialize<uint>(Length, name: nameof(Length));
            SongPointers = s.SerializePointerArray(SongPointers, Length, name: nameof(SongPointers));

            if (Songs == null) {
                Songs = new MusyX_SongGroup[SongPointers.Length];
                for (int i = 0; i < Songs.Length; i++) {
                    Pointer nextOff = (i < SongPointers.Length - 1) ? SongPointers[i + 1] : EndOffset;
                    s.DoAt(SongPointers[i], () => {
						Songs[i] = s.SerializeObject<MusyX_SongGroup>(Songs[i], onPreSerialize: sng => {
                            sng.Length = (uint)(nextOff - SongPointers[i]);
                        }, name: $"{nameof(Songs)}[{i}]");
                    });
                }
            }
        }
    }
}