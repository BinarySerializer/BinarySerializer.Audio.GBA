using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_SoundListItem : BinarySerializable {
        public ushort ObjectID { get; set; }
        public byte Priority { get; set; }
        public byte MaxVoices { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
			ObjectID = s.Serialize<ushort>(ObjectID, name: nameof(ObjectID));
			Priority = s.Serialize<byte>(Priority, name: nameof(Priority));
			MaxVoices = s.Serialize<byte>(MaxVoices, name: nameof(MaxVoices));
		}
    }
}