using System.Linq;
using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_Message : BinarySerializable {
        public ushort Time { get; set; }
        public byte Velocity { get; set; }
        public byte Note { get; set; }
        public ushort SustainTime { get; set; }

        public bool HasExtraData { get; set; }
        public byte ExtraByte0 { get; set; }
        public byte ExtraByte1 { get; set; }

        public ushort BPM { get; set; }

        public int AsInt { get; set; }
        public bool IsEnd => AsInt == -1;

        public bool Pre_IsControlMessage { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoAt(Offset, () => {
				AsInt = s.Serialize<int>(AsInt, name: nameof(AsInt));
			});
            if(IsEnd) return;

            Time = s.Serialize<ushort>(Time, name: nameof(Time));
            if (Pre_IsControlMessage) {
				BPM = s.Serialize<ushort>(BPM, name: nameof(BPM));
			} else {
                Velocity = s.Serialize<byte>(Velocity, name: nameof(Velocity));
                s.DoBits<byte>(b => {
                    Note = b.SerializeBits<byte>(Note, 7, name: nameof(Note));
                    HasExtraData = b.SerializeBits<bool>(HasExtraData, 1, name: nameof(HasExtraData));
                });
                if (HasExtraData) {
                    ExtraByte0 = s.Serialize<byte>(ExtraByte0, name: nameof(ExtraByte0));
                    ExtraByte1 = s.Serialize<byte>(ExtraByte1, name: nameof(ExtraByte1));
                }
                SustainTime = s.Serialize<ushort>(SustainTime, name: nameof(SustainTime));
            }
		}
    }
}