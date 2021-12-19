namespace BinarySerializer.GBA.Audio.GAX
{
    public class GAX_SampleTable : BinarySerializable
    {
        public uint Length { get; set; }
        public Pointer Silence { get; set; }
        public uint Unknown { get; set; }
        public GAX_Sample[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Silence = s.SerializePointer(Silence, name: nameof(Silence));
            Unknown = s.Serialize<uint>(Unknown, name: nameof(Unknown));
            Entries = s.SerializeObjectArray<GAX_Sample>(Entries, Length, name: nameof(Entries));
        }
    }
}