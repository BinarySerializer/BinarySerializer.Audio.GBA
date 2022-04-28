using System.Text;

namespace BinarySerializer.Audio.GBA.MusyX
{
    public class MusyX_SoundMacro : BinarySerializable {
        // Set in OnPreSerialize
        public Pointer EndOffset { get; set; }
        public byte[] InstrumentBytes { get; set; } // SMaL (Sound Macro Language)
        public sbyte Voice { get; set; }
        public byte Flags { get; set; }
        public byte Type { get; set; }
        public byte Unknown { get; set; }
        public MusyX_SMaL[] Commands { get; set; }
        public MusyX_Keymap Keymap { get; set; }


        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoAt(Offset, () => {
                InstrumentBytes = s.SerializeArray<byte>(InstrumentBytes, EndOffset - Offset, name: nameof(InstrumentBytes));
            });

			Voice = s.Serialize<sbyte>(Voice, name: nameof(Voice));
			Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
			Type = s.Serialize<byte>(Type, name: nameof(Type));
			Unknown = s.Serialize<byte>(Unknown, name: nameof(Unknown));

            if (Type == 0)
                Commands = s.SerializeObjectArrayUntil<MusyX_SMaL>(Commands, c => c.Command == MusyX_SMaL.CommandType.END, name: nameof(Commands));
            else
				Keymap = s.SerializeObject<MusyX_Keymap>(Keymap, name: nameof(Keymap));
		}
    }
}