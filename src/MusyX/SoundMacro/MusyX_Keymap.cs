using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_Keymap : BinarySerializable {
        public ushort Length { get; set; }
        public Entry[] Entries { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Length = s.Serialize<ushort>(Length, name: nameof(Length));
            Entries = s.SerializeObjectArray<Entry>(Entries, Length, name: nameof(Entries));
        }

        public class Entry : BinarySerializable {
            public ushort SampleIndex { get; set; }
            public short Short_02 { get; set; }
            public byte Byte_04 { get; set; }
            public byte Byte_05 { get; set; }
            public byte Byte_06 { get; set; }
            public byte Byte_07 { get; set; }

			public override void SerializeImpl(SerializerObject s) {
				SampleIndex = s.Serialize<ushort>(SampleIndex, name: nameof(SampleIndex));
				Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
				Byte_04 = s.Serialize<byte>(Byte_04, name: nameof(Byte_04));
				Byte_05 = s.Serialize<byte>(Byte_05, name: nameof(Byte_05));
				Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
				Byte_07 = s.Serialize<byte>(Byte_07, name: nameof(Byte_07));
			}
		}
    }
}