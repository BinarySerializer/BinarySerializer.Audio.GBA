using System.Text;

namespace BinarySerializer.Audio.GBA.MusyX
{
    public class MusyX_SoundMacroTable : BinarySerializable {
        // Set in OnPreSerialize
        public Pointer EndOffset { get; set; }

        public Pointer<MusyX_SoundMacro>[] Macros { get; set; }


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
            Macros = s.SerializePointerArray<MusyX_SoundMacro>(Macros, (instrOff1 - Offset) / 4, resolve: false, name: nameof(Macros));
            for (int i = 0; i < Macros.Length; i++) {
                Pointer nextOff = (i < Macros.Length - 1) ? Macros[i + 1].PointerValue : EndOffset;
                Macros[i].Resolve(s, onPreSerialize: m => m.EndOffset = nextOff);
            }
        }
    }
}