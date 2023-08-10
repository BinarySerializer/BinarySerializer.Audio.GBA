using System.Linq;
using System.Text;

namespace BinarySerializer.Audio.GBA.MusyX
{
    public class MusyX_SoundMacro : BinarySerializable {
        // Set in OnPreSerialize
        public Pointer EndOffset { get; set; }
        public byte[] InstrumentBytes { get; set; } // SMaL (Sound Macro Language)
        public sbyte Voice { get; set; }
        public byte Flags { get; set; }
        public bool IsKeymap { get; set; }
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
			IsKeymap = s.Serialize<bool>(IsKeymap, name: nameof(IsKeymap));
			Unknown = s.Serialize<byte>(Unknown, name: nameof(Unknown));

            if (!IsKeymap) {
                Commands = s.SerializeObjectArrayUntil<MusyX_SMaL>(Commands, c => c.Command == MusyX_SMaL.CommandType.END, name: nameof(Commands));
                var command = Commands.FirstOrDefault(c =>
                    c.Command == MusyX_SMaL.CommandType.STARTSAMPLE ||
                    c.Command == MusyX_SMaL.CommandType.STARTSAMPLE_KEYMAP ||
                    c.Command == MusyX_SMaL.CommandType.VOICE_ON ||
                    c.Command == MusyX_SMaL.CommandType.SETNOISE);
                if (command == null) {
                    s.Log("No STARTSAMPLE command!");
                } else {
                    s.Log($"STARTSAMPLE command: {command.Command}");
                }
            } else {
                Keymap = s.SerializeObject<MusyX_Keymap>(Keymap, name: nameof(Keymap));
            }
		}
    }
}