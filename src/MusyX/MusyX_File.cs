using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    /// <summary>
    /// Base file for GBA MusyX
    /// </summary>
    public class MusyX_File : BinarySerializable
    {
        public Pointer<MusyX_InstrumentTable> InstrumentTable { get; set; } // Sounds?
        public Pointer<MusyX_SFXGroup> SFXGroup1 { get; set; } // Layers? in Rayman Advance this points to a list. in RHR, this points to a pointer (or a list of pointers with only 1 entry), which points to 1 8 byte struct
        public Pointer<MusyX_SFXGroup> SFXGroup2 { get; set; } // Keymaps?
        public Pointer<MusyX_SFXGroup> SFXGroup3 { get; set; } // Soundmacros?
        public uint UInt_10 { get; set; }
        public uint UInt_14 { get; set; }
        public Pointer<MusyX_SongTable> SongTable { get; set; }
        public Pointer<MusyX_SampleTable> SampleTable { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoWithDefaults(new SerializerDefaults() {
                PointerAnchor = Offset,
                PointerNullValue = 0
            }, () => {
                InstrumentTable = s.SerializePointer<MusyX_InstrumentTable>(InstrumentTable, name: nameof(InstrumentTable));
                SFXGroup1 = s.SerializePointer<MusyX_SFXGroup>(SFXGroup1, resolve: false, name: nameof(SFXGroup1)); // Don't resolve for now, this isn't parsed correctly
                SFXGroup2 = s.SerializePointer<MusyX_SFXGroup>(SFXGroup2, resolve: false, name: nameof(SFXGroup2));
                SFXGroup3 = s.SerializePointer<MusyX_SFXGroup>(SFXGroup3, resolve: false, name: nameof(SFXGroup3));
                UInt_10 = s.Serialize<uint>(UInt_10, name: nameof(UInt_10));
                UInt_14 = s.Serialize<uint>(UInt_14, name: nameof(UInt_14));
                SongTable = s.SerializePointer<MusyX_SongTable>(SongTable, name: nameof(SongTable));
                SampleTable = s.SerializePointer<MusyX_SampleTable>(SampleTable, name: nameof(SampleTable));

                if (s.GetMusyXSettings().EnableErrorChecking) {
                    var settings = s.GetMusyXSettings();
                    settings.CheckPointer(InstrumentTable, this, nameof(InstrumentTable), false);
                    settings.CheckPointer(SFXGroup1, this, nameof(SFXGroup1), true);
                    settings.CheckPointer(SFXGroup2, this, nameof(SFXGroup2), true);
                    settings.CheckPointer(SFXGroup3, this, nameof(SFXGroup3), true);
                    settings.CheckPointer(SongTable, this, nameof(SongTable), true);
                    settings.CheckPointer(SampleTable, this, nameof(SampleTable), false);
                }

                // Read sample table first as errors will usually occur here if it's not a valid MusyX file
                SampleTable.Resolve(s, onPreSerialize: st => {
                    //st.EndOffset = SampleTable.pointer;
                });

                // Read SFXGroup3
                SFXGroup3.Resolve(s);

                // Read instrument table
                InstrumentTable.Resolve(s, onPreSerialize: st => {
                    st.EndOffset = SFXGroup1.PointerValue;
                });

                // Read song table
                SongTable.Resolve(s, onPreSerialize: st => {
                    st.EndOffset = SampleTable.PointerValue;
                });
            });
        }
    }
}