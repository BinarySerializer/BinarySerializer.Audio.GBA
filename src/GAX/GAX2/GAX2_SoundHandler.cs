namespace BinarySerializer.Audio.GBA.GAX {
	// See https://github.com/loveemu/gaxtapper/blob/main/src/gaxtapper/gax_sound_handler_v2.cpp
	public class GAX2_SoundHandler : BinarySerializable {
		public Pointer InitFunction { get; set; }
		public Pointer UnknownFunction { get; set; }
		public Pointer PlayFunction { get; set; }
		public uint NumChildren { get; set; }
		public Pointer ChildrenPointer { get; set; }
		public uint TypeFlags { get; set; } // 0x4C: channel, 0x1C: song. 0xC, 0x10 = same thing?
		public Pointer DataPointer { get; set; }

		// Parsed
		public Pointer<GAX2_SoundHandler>[] Children { get; set; }
		public GAX_Entity Data { get; set; }
		public T GetData<T>() where T : GAX_Entity {
			return (T)Data;
		}
		
		public EntityType? Pre_Type { get; set; }
		public enum EntityType {
			UnknownC,
			Channel,
			SongInfo
		}

		public override void SerializeImpl(SerializerObject s) {
			InitFunction = s.SerializePointer(InitFunction, name: nameof(InitFunction));
			UnknownFunction = s.SerializePointer(UnknownFunction, name: nameof(UnknownFunction));
			PlayFunction = s.SerializePointer(PlayFunction, name: nameof(PlayFunction));
			NumChildren = s.Serialize<uint>(NumChildren, name: nameof(NumChildren));
			ChildrenPointer = s.SerializePointer(ChildrenPointer, name: nameof(ChildrenPointer));
			TypeFlags = s.Serialize<uint>(TypeFlags, name: nameof(TypeFlags));
			DataPointer = s.SerializePointer(DataPointer, name: nameof(DataPointer));

			if (s.GetGAXSettings().EnableErrorChecking) {
				if (InitFunction == null || PlayFunction == null || UnknownFunction == null)
					throw new BinarySerializableException(this, $"One of the functions was null");
				if (DataPointer == null)
					throw new BinarySerializableException(this, $"Data pointer was null");
			}

			s.DoAt(ChildrenPointer, () => {
				Children = s.SerializePointerArray<GAX2_SoundHandler>(Children, NumChildren, name: nameof(Children));
			});


			switch (Pre_Type) {
				case EntityType.UnknownC:
					SerializeValues<GAX_UnknownC>(s, EntityType.Channel);
					break;
				case EntityType.Channel:
					SerializeValues<GAX_Channel>(s, EntityType.SongInfo);
					break;
				case EntityType.SongInfo:
					SerializeValues<GAX_SongInfo>(s, null);
					break;
				default:
					break;
			}
		}

		public void SerializeValues<T>(SerializerObject s, EntityType? childType) where T : GAX_Entity, new() {
			if (Children != null) {
				foreach (var c in Children) {
					c.Resolve(s, onPreSerialize: c => c.Pre_Type = childType);
				}
			}
			s.DoAt(DataPointer, () => {
				Data = s.SerializeObject<T>((T)Data, onPreSerialize: d => d.Handler = this, name: nameof(Data));
			});
		}
	}
}