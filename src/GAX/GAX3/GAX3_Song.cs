using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Audio.GBA.GAX
{
    public class GAX3_Song : BinarySerializable, IGAX_Song
    {
        public GAX_SongInfo Info { get; set; }
        public Pointer<GAX_Channel>[] Channels { get; set; }
        public GAX_UnknownC UnknownC { get; set; }

		public GAX_Channel GetChannel(int i) => Channels[i]?.Value;

        public long? Pre_InstrumentsCount { get; set; }
        public long? Pre_SamplesCount { get; set; }

		public override void SerializeImpl(SerializerObject s)
        {
			Info = s.SerializeObject<GAX_SongInfo>(Info, onPreSerialize: info => info.Song = this, name: nameof(Info));

            Channels = s.SerializePointerArray(Channels, 32, name: nameof(Channels));
            if (s.GetGAXSettings().EnableErrorChecking) {
                for (int i = 0; i < Info.NumChannels; i++) {
                    if (Channels[i].PointerValue == null) throw new BinarySerializableException(this, $"{nameof(Channels)}[{i}] is null");
                }
                for (int i = Info.NumChannels; i < 32; i++) {
                    if (Channels[i].PointerValue != null) throw new BinarySerializableException(this, $"{nameof(Channels)}[{i}] is not null");
                }
            }
            Channels?.ResolveObject(s, onPreSerialize: (c, _) => c.Song = this);

			UnknownC = s.SerializeObject<GAX_UnknownC>(UnknownC, onPreSerialize: c => c.Song = this, name: nameof(UnknownC));

            Info.ParseInstrumentsAndChannels(s, Channels.Select(c => c?.Value),
                predefinedInstrumentCount: Pre_InstrumentsCount,
                predefinedSamplesCount: Pre_SamplesCount);
        }
    }
}