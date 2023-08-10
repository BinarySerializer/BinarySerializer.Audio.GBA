using System;
using System.Linq;

namespace BinarySerializer.Audio.GBA.GAX
{
    public class GAX_Sample : BinarySerializable {
        public Pointer SampleOffset { get; set; }
        public uint Length { get; set; }

        public byte[] SampleUnsigned { get; set; }
        public sbyte[] SampleSigned { get; set; }

        public override void SerializeImpl(SerializerObject s) {
            SampleOffset = s.SerializePointer(SampleOffset, name: nameof(SampleOffset));
            Length = s.Serialize<uint>(Length, name: nameof(Length));

            s.DoAt(SampleOffset, () => {
                if (s.GetGAXSettings().MajorVersion < 3) {
					SampleSigned = s.SerializeArray<sbyte>(SampleSigned, Length, name: nameof(SampleSigned));
				} else {
                    SampleUnsigned = s.SerializeArray<byte>(SampleUnsigned, Length, name: nameof(SampleUnsigned));
                }
            });
        }
    }
}