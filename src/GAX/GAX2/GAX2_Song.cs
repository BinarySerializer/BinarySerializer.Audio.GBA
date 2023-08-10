using System;
using System.Linq;

namespace BinarySerializer.Audio.GBA.GAX
{
    public class GAX2_Song : BinarySerializable, IGAX_Song
    {
        public uint NumItems { get; set; }
        public Pointer<GAX2_SoundHandler> UnknownCHandler { get; set; }
        public Pointer<GAX2_SoundHandler> InfoHandler { get; set; }
        public Pointer Unk { get; set; }
        public Pointer<GAX2_SoundHandler>[] ChannelHandlers { get; set; }

		public GAX_SongInfo Info => InfoHandler?.Value?.GetData<GAX_SongInfo>();

        public GAX_UnknownC UnknownC => UnknownCHandler?.Value?.GetData<GAX_UnknownC>();

		public GAX_Channel GetChannel(int i) {
			return UnknownCHandler?.Value?.Children[i]?.Value.GetData<GAX_Channel>();
		}

		public override void SerializeImpl(SerializerObject s)
        {
			NumItems = s.Serialize<uint>(NumItems, name: nameof(NumItems));
            if (s.GetGAXSettings().EnableErrorChecking) {
                if(NumItems < 4 || NumItems > 32+3)
                    throw new BinarySerializableException(this, $"Incorrect {nameof(NumItems)} value: {NumItems}");
            }
            if (NumItems > 0) UnknownCHandler = s.SerializePointer<GAX2_SoundHandler>(UnknownCHandler, name: nameof(UnknownCHandler));
			if (NumItems > 1) InfoHandler = s.SerializePointer<GAX2_SoundHandler>(InfoHandler, name: nameof(InfoHandler));
			if (NumItems > 2) Unk = s.SerializePointer(Unk, name: nameof(Unk));
            int channelsCount = Math.Max((int)NumItems-3, 0);
			ChannelHandlers = s.SerializePointerArray<GAX2_SoundHandler>(ChannelHandlers, channelsCount, name: nameof(ChannelHandlers));

            if (s.GetGAXSettings().EnableErrorChecking) {
                if (UnknownCHandler?.PointerValue == null
                    || InfoHandler?.PointerValue == null
                    || UnknownCHandler?.PointerValue == InfoHandler?.PointerValue)
                    throw new BinarySerializableException(this, $"{nameof(UnknownCHandler)} or {nameof(InfoHandler)} was null or they were equal");
            }

            InfoHandler?.ResolveObject(s, onPreSerialize: h => h.Pre_Type = GAX2_SoundHandler.EntityType.SongInfo);
            UnknownCHandler?.ResolveObject(s, onPreSerialize: h => h.Pre_Type = GAX2_SoundHandler.EntityType.UnknownC);
            foreach (var ch in ChannelHandlers) {
                ch?.ResolveObject(s, onPreSerialize: h => h.Pre_Type = GAX2_SoundHandler.EntityType.Channel);
            }

            Info?.ParseInstrumentsAndChannels(s, UnknownCHandler?.Value?.Children.Select(c => c.Value?.GetData<GAX_Channel>()));
        }
    }
}