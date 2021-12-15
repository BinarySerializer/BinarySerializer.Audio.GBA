﻿using System;
using System.Linq;

namespace BinarySerializer.GBA.Audio.GAX
{
    public class GAX2_SampleDefinition : BinarySerializable {
        public short Pitch { get; set; }
        public byte Byte_02 { get; set; }
        public bool IsBidirectional { get; set; }
        public int StartPosition { get; set; }
        public uint LoopStart { get; set; }
        public uint LoopEnd { get; set; }
        public int Int_10 { get; set; }
        public ushort UShort_14 { get; set; }
        public ushort UShort_16 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Pitch = s.Serialize<short>(Pitch, name: nameof(Pitch));
			Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
			IsBidirectional = s.Serialize<bool>(IsBidirectional, name: nameof(IsBidirectional));
			StartPosition = s.Serialize<int>(StartPosition, name: nameof(StartPosition));
			LoopStart = s.Serialize<uint>(LoopStart, name: nameof(LoopStart));
			LoopEnd = s.Serialize<uint>(LoopEnd, name: nameof(LoopEnd));
			Int_10 = s.Serialize<int>(Int_10, name: nameof(Int_10));
			UShort_14 = s.Serialize<ushort>(UShort_14, name: nameof(UShort_14));
			UShort_16 = s.Serialize<ushort>(UShort_16, name: nameof(UShort_16));
		}
    }
}