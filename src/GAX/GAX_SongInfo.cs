using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BinarySerializer.GBA.Audio.GAX {
    public class GAX_SongInfo : GAX_Entity {
        public ushort NumChannels { get; set; } // Actually a byte?
        public ushort NumRowsPerPattern { get; set; }
        public ushort NumPatternsPerChannel { get; set; }
        public ushort LoopPoint { get; set; } // Pattern index starting from 0
        public ushort Volume { get; set; }
        public ushort UShort_0A { get; set; }
        public Pointer SequenceDataPointer { get; set; }
        public Pointer InstrumentSetPointer { get; set; }
        public Pointer SampleSetPointer { get; set; }
        public ushort SampleRate { get; set; } // 0x3D99
        public ushort FXSampleRate { get; set; } // 0 for same as music
        public byte NumFXChannels { get; set; }
        public byte Byte_1D { get; set; }
        public ushort UShort_1E { get; set; }

        // Parsed
        public Pointer<GAX_Instrument>[] InstrumentSet { get; set; }
        public int[] UsedInstrumentIndices { get; set; }
        public GAX_Sample[] Samples { get; set; }

        public string Name { get; set; }
        public string ParsedName { get; set; }
        public string ParsedArtist { get; set; }

        public override void SerializeImpl(SerializerObject s) {
            NumChannels = s.Serialize<ushort>(NumChannels, name: nameof(NumChannels));
            NumRowsPerPattern = s.Serialize<ushort>(NumRowsPerPattern, name: nameof(NumRowsPerPattern));
            NumPatternsPerChannel = s.Serialize<ushort>(NumPatternsPerChannel, name: nameof(NumPatternsPerChannel));
            if (s.GetGAXSettings().EnableErrorChecking) {
                if (NumChannels > 32) throw new BinarySerializableException(this, $"Incorrect {nameof(NumChannels)}: {NumChannels}");
                if (NumRowsPerPattern >= 0x200) throw new BinarySerializableException(this, $"Incorrect {nameof(NumRowsPerPattern)}: {NumRowsPerPattern}");
                if (NumPatternsPerChannel >= 0x100) throw new BinarySerializableException(this, $"Incorrect {nameof(NumPatternsPerChannel)}: {NumPatternsPerChannel}");
            }
            LoopPoint = s.Serialize<ushort>(LoopPoint, name: nameof(LoopPoint));
            Volume = s.Serialize<ushort>(Volume, name: nameof(Volume));
            UShort_0A = s.Serialize<ushort>(UShort_0A, name: nameof(UShort_0A));
            SequenceDataPointer = s.SerializePointer(SequenceDataPointer, name: nameof(SequenceDataPointer));
            InstrumentSetPointer = s.SerializePointer(InstrumentSetPointer, name: nameof(InstrumentSetPointer));
            SampleSetPointer = s.SerializePointer(SampleSetPointer, name: nameof(SampleSetPointer));
            SampleRate = s.Serialize<ushort>(SampleRate, name: nameof(SampleRate));
            if (s.GetGAXSettings().MajorVersion >= 3) {
                FXSampleRate = s.Serialize<ushort>(FXSampleRate, name: nameof(FXSampleRate));
            }
            NumFXChannels = s.Serialize<byte>(NumFXChannels, name: nameof(NumFXChannels));
            Byte_1D = s.Serialize<byte>(Byte_1D, name: nameof(Byte_1D));

            if (s.GetGAXSettings().MajorVersion >= 3) {
                UShort_1E = s.Serialize<ushort>(UShort_1E, name: nameof(UShort_1E));
            }
        }

        public void ParseInstrumentsAndChannels(SerializerObject s, IEnumerable<GAX_Channel> Channels,
            long? predefinedInstrumentCount = null, long? predefinedSamplesCount = null) {
            List<int> instruments = new List<int>();
            if (Channels != null) {
                int instrumentCount = 0;
                Pointer endOffset = null;
                foreach (var ch in Channels) {
                    if (ch?.Patterns != null) {
                        foreach (var pat in ch.Patterns) {
                            if (endOffset == null || endOffset.AbsoluteOffset < pat.EndOffset.AbsoluteOffset) endOffset = pat.EndOffset;
                            if ((pat?.Rows?.Length ?? 0) > 0) {
                                instrumentCount = Math.Max(instrumentCount, pat.Rows
                                    .Max(cmd => (cmd.Command == GAX_PatternRow.Cmd.Note || cmd.Command == GAX_PatternRow.Cmd.NoteOnly) ? cmd.Instrument + 1 : 0));
                                instruments.AddRange(pat.Rows
                                    .Where(cmd => cmd.Command == GAX_PatternRow.Cmd.Note || cmd.Command == GAX_PatternRow.Cmd.NoteOnly)
                                    .Select(cmd => (int)cmd.Instrument));
                            }
                        }
                    }
                }
                UsedInstrumentIndices = instruments.Distinct().ToArray();
                s.Log("Instrument Count: " + UsedInstrumentIndices.Length);

                // Parse name
                if (endOffset != null) {
                    s.DoAt(endOffset, () => {
                        Name = s.Serialize<string>(Name, name: nameof(Name));
                        const string GAXNamePattern = @"^""(?<title>[^""]*)"" © (?<artist>[A-Za-z0-9_\-\s]*)";
                        var m = Regex.Match(Name, GAXNamePattern);
                        if (m.Success) {
                            ParsedName = m.Groups["title"].Value;
                            ParsedArtist = m.Groups["artist"].Value;
                            s.Log($"{ParsedName} - {ParsedArtist}");
                        } else {
                            if (s.GetGAXSettings().EnableErrorChecking) {
                                throw new BinarySerializableException(this, $"GAX name check failed for {nameof(Name)}: {Name}");
                            }
                        }
                    });
                }
                s.DoAt(InstrumentSetPointer, () => {
                    InstrumentSet = s.SerializePointerArray<GAX_Instrument>(InstrumentSet, predefinedInstrumentCount ?? instrumentCount, resolve: true, name: nameof(InstrumentSet));
                });
                /*Samples = new GAX_Sample[PredefinedSampleCount ?? InstrumentIndices.Length];
                for (int i = 0; i < Samples.Length; i++) {
                    int ind = PredefinedSampleCount.HasValue ? i : InstrumentIndices[i];
                    var instr = InstrumentSet[ind].Value;
                    if (instr != null) {
                        s.DoAt(Info.SampleSetPointer + (instr.SampleIndices[0]) * 8, () => {
                            Samples[i] = s.SerializeObject<GAX_Sample>(Samples[i], name: $"{nameof(Samples)}[{i}]");
                        });
                    }
                }*/
                int samplesLength = InstrumentSet.Max(i => i.Value?.SampleIndices.Max() ?? -1) + 1;
                Samples = new GAX_Sample[predefinedSamplesCount ?? samplesLength];
                s.DoAt(SampleSetPointer, () => {
                    Samples = s.SerializeObjectArray<GAX_Sample>(Samples, Samples.Length, name: nameof(Samples));
                });
            }
        }
    }
}