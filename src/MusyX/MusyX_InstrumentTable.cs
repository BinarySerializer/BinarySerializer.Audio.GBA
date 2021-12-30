using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_InstrumentTable : BinarySerializable {
        // Set in OnPreSerialize
        public Pointer EndOffset { get; set; }

        public Pointer[] Instruments { get; set; }
        public byte[][] InstrumentBytes { get; set; }


        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Hack to get length of instrument table & instrument bytes
            Pointer instrOff1 = null;
            s.DoAt(Offset, () => {
                instrOff1 = s.SerializePointer(instrOff1, name: nameof(instrOff1));
            });
            Instruments = s.SerializePointerArray(Instruments, (instrOff1 - Offset) / 4, name: nameof(Instruments));
            if (InstrumentBytes == null) {
                InstrumentBytes = new byte[Instruments.Length][];
                for (int i = 0; i < Instruments.Length; i++) {
                    Pointer nextOff = (i < Instruments.Length - 1) ? Instruments[i + 1] : EndOffset;
                    s.DoAt(Instruments[i], () => {
                        InstrumentBytes[i] = s.SerializeArray<byte>(InstrumentBytes[i], nextOff - Instruments[i], name: $"{nameof(InstrumentBytes)}[{i}]");
                    });
                }
            }
        }
    }
}