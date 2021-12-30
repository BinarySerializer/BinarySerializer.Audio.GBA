using System.Linq;
using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_Pattern : BinarySerializable {
        public bool Pre_IsControlPattern { get; set; }
        public MusyX_Message[] Messages { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            Messages = s.SerializeObjectArrayUntil<MusyX_Message>(Messages, m => m.IsEnd, getLastObjFunc: () => new MusyX_Message() {
                AsInt = -1
            }, onPreSerialize: m => m.Pre_IsControlMessage = Pre_IsControlPattern, name: nameof(Messages));
        }
    }
}