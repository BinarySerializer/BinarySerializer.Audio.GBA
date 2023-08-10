using System.Text;

namespace BinarySerializer.Audio.GBA.MusyX
{
    public class MusyX_SampleTable : BinarySerializable {
        public Pointer<MusyX_Sample>[] Samples { get; set; }


        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            // Hack to get length
            Pointer smpOff1 = null;
            s.DoAt(Offset, () => {
                smpOff1 = s.SerializePointer(smpOff1, name: nameof(smpOff1));
            });
            Samples = s.SerializePointerArray<MusyX_Sample>(Samples, (smpOff1 - Offset) / 4, name: nameof(Samples))
                ?.ResolveObject(s);

            if (s.GetMusyXSettings().EnableErrorChecking) {
                var settings = s.GetMusyXSettings();
                if(Samples == null || Samples.Length == 0) throw new BinarySerializableException(this, $"Sample table had 0 length");
                for (int i = 0; i < Samples.Length; i++) {
                    settings.CheckPointer(Samples[i], this, nameof(Samples), false);
                }
            }
        }
    }
}