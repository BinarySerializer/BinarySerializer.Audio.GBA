namespace BinarySerializer.Audio.GBA.GAX
{
    public class GAX3_File : BinarySerializable
    {
        public long SongsCount { get; set; } // Set before serializing
        public long? SamplesCount { get; set; } // Set before serializing

        public string Magic { get; set; } // "GAX!"
        public Pointer[] SongPointers { get; set; }

        // Serialized from pointers
        public GAX3_Song[] Songs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Magic = s.SerializeString(Magic, 4, name: nameof(Magic));
            SongPointers = s.SerializePointerArray(SongPointers, SongsCount, name: nameof(SongPointers));

            if (Songs == null)
                Songs = new GAX3_Song[SongPointers.Length];

            for (int i = 0; i < Songs.Length; i++)
                Songs[i] = s.DoAt(SongPointers[i], () => s.SerializeObject(Songs[i], onPreSerialize: sng => {
                    sng.Pre_SamplesCount = SamplesCount;
                    sng.Pre_InstrumentsCount = SamplesCount;
                }, name: $"{nameof(Songs)}[{i}]"));

            s.Goto(Offset + s.CurrentLength);
        }
    }
}