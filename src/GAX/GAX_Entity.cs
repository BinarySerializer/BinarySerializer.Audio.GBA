using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Audio.GBA.GAX {
    public abstract class GAX_Entity : BinarySerializable {
        public GAX2_SoundHandler Handler { get; set; }
        public GAX3_Song Song { get; set; }
    }
}