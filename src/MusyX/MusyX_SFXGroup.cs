using System.Text;

namespace BinarySerializer.Audio.GBA.MusyX
{
    public class MusyX_SFXGroup : BinarySerializable {
        public uint Length { get; set; }
        public SFXDefinition[] SFX { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Length = s.Serialize<uint>(Length, name: nameof(Length));
            SFX = s.SerializeObjectArray<SFXDefinition>(SFX, Length, name: nameof(SFX));
        }

        public class SFXDefinition : BinarySerializable {
            public ushort Macro { get; set; }
            public byte Priority { get; set; }
            public byte Byte_03 { get; set; } // This could be MaxVoicesCount too
            public byte MaxVoicesCount { get; set; } // ??
            public byte DefaultVelocity { get; set; } // Volume - usually 0x7F
            public byte DefaultPanning { get; set; }
            public byte DefaultKey { get; set; } // The default pitch - usually 0x3C (MIDI C4)

			public override void SerializeImpl(SerializerObject s) {
				Macro = s.Serialize<ushort>(Macro, name: nameof(Macro));
				Priority = s.Serialize<byte>(Priority, name: nameof(Priority));
				Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
				MaxVoicesCount = s.Serialize<byte>(MaxVoicesCount, name: nameof(MaxVoicesCount));
				DefaultVelocity = s.Serialize<byte>(DefaultVelocity, name: nameof(DefaultVelocity));
				DefaultPanning = s.Serialize<byte>(DefaultPanning, name: nameof(DefaultPanning));
				DefaultKey = s.Serialize<byte>(DefaultKey, name: nameof(DefaultKey));
			}
		}
    }
}