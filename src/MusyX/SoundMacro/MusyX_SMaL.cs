using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_SMaL : BinarySerializable {
        public CommandType Command { get; set; }
        public CommandData Data { get; set; }


        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s) {
            Command = s.Serialize<CommandType>(Command, name: nameof(Command));
            if (s.GetMusyXSettings().EnableErrorChecking) {
                if ((int)Command > 53)
                    throw new BinarySerializableException(this, $"Invalid SMaL command");
            }

            CommandData SerializeData<T>() where T : CommandData, new() {
                return s.SerializeObject<T>((T)Data, onPreSerialize: d => d.SMaL = this, name: nameof(Data));
            }

            Data = Command switch
            {
                CommandType.END => null,
                CommandType.STOP => null,
                CommandType.Command02 => SerializeData<Command02>(),
                CommandType.Command03 => SerializeData<Command03>(),
                CommandType.WAIT_TICKS => SerializeData<WAIT_TICKS>(),
                CommandType.LOOP => SerializeData<LOOP>(),
                CommandType.Command06 => SerializeData<Command06>(),
                CommandType.Command07 => SerializeData<Command07>(),
                CommandType.Command08 => SerializeData<Command08>(),
                CommandType.Command09 => SerializeData<Command09>(),
                CommandType.Command10 => SerializeData<Command10>(),
                CommandType.Command11 => SerializeData<Command11>(),
                CommandType.Command12 => SerializeData<Command12>(),
                CommandType.Command13 => SerializeData<Command13>(),
                CommandType.Command14 => SerializeData<Command14>(),
                CommandType.STARTSAMPLE => SerializeData<STARTSAMPLE>(),
                CommandType.TRAP_KEYOFF => SerializeData<TRAP_KEYOFF>(),
                CommandType.UNTRAP_KEYOFF => SerializeData<UNTRAP_KEYOFF>(),
                CommandType.Command18 => SerializeData<Command18>(),
                CommandType.Command19 => SerializeData<Command19>(),
                CommandType.Command20 => SerializeData<Command20>(),
                CommandType.Command21 => SerializeData<Command21>(),
                CommandType.Command22 => SerializeData<Command22>(),
                CommandType.SPLITKEY => SerializeData<SPLITKEY>(),
                CommandType.SPLITVEL => SerializeData<SPLITVEL>(),
                CommandType.SPLITRND => SerializeData<SPLITRND>(),
                CommandType.Command26 => SerializeData<Command26>(),
                CommandType.Command27 => SerializeData<Command27>(),
                CommandType.Command28 => SerializeData<Command28>(),
                CommandType.Command29 => SerializeData<Command29>(),
                CommandType.Command30 => SerializeData<Command30>(),
                CommandType.Command31 => SerializeData<Command31>(),
                CommandType.VOICE_ON => SerializeData<VOICE_ON>(),
                CommandType.SETNOISE => SerializeData<SETNOISE>(),
                CommandType.KEYOFF => SerializeData<KEYOFF>(),
                CommandType.Command35 => SerializeData<Command35>(),
                CommandType.HARDENVELOPE => SerializeData<HARDENVELOPE>(),
                CommandType.HARDENVELOPE_OFF => SerializeData<HARDENVELOPE_OFF>(),
                CommandType.Command38 => SerializeData<Command38>(),
                CommandType.Command39 => SerializeData<Command39>(),
                CommandType.Command40 => SerializeData<Command40>(),
                CommandType.Command41 => SerializeData<Command41>(),
                CommandType.Command42 => SerializeData<Command42>(),
                CommandType.Command43 => SerializeData<Command43>(),
                CommandType.Command44 => SerializeData<Command44>(),
                CommandType.Command45 => SerializeData<Command45>(),
                CommandType.Command46 => SerializeData<Command46>(),
                CommandType.Command47 => SerializeData<Command47>(),
                CommandType.Command48 => SerializeData<Command48>(),
                CommandType.Command49 => SerializeData<Command49>(),
                CommandType.Command50 => SerializeData<Command50>(),
                CommandType.Command51 => SerializeData<Command51>(),
                CommandType.Command52 => SerializeData<Command52>(),
                CommandType.Command53 => SerializeData<Command53>(),


                _ => throw new BinarySerializableException(this, $"Invalid or unparsed command: {Command}"),
            };
        }

        public enum CommandType : byte {
            END = 0, // Terminates the SoundMacro
            STOP = 1, // Similar to END but can be used as return command (can be placed anywhere in macro)
            Command02 = 2,
            Command03 = 3,
            WAIT_TICKS = 4,
            LOOP = 5,
            Command06 = 6,
            Command07 = 7,
            Command08 = 8,
            Command09 = 9,
            Command10 = 10,
            Command11 = 11,
            Command12 = 12,
            Command13 = 13, // Directly jump to command with offset Offset + OffsetInBytes
            Command14 = 14,
            STARTSAMPLE = 15,
            TRAP_KEYOFF = 16, // Register jump destination with offset Offset + OffsetInBytes
            UNTRAP_KEYOFF = 17, // Register jump destination with offset Offset + OffsetInBytes
            Command18 = 18,
            Command19 = 19,
            Command20 = 20,
            Command21 = 21,
            Command22 = 22,
            SPLITKEY = 23,
            SPLITVEL = 24,
            SPLITRND = 25,
            Command26 = 26,
            Command27 = 27,
            Command28 = 28,
            Command29 = 29,
            Command30 = 30,
            Command31 = 31,
            VOICE_ON = 32,
            SETNOISE = 33,
            KEYOFF = 34,
            Command35 = 35,
            HARDENVELOPE = 36,
            HARDENVELOPE_OFF = 37,
            Command38 = 38,
            Command39 = 39,
            Command40 = 40,
            Command41 = 41,
            Command42 = 42,
            Command43 = 43,
            Command44 = 44,
            Command45 = 45,
            Command46 = 46,
            Command47 = 47,
            Command48 = 48,
            Command49 = 49,
            Command50 = 50,
            Command51 = 51,
            Command52 = 52,
            Command53 = 53,

        }

		#region Data
		public abstract class CommandData : BinarySerializable {
            public MusyX_SMaL SMaL { get; set; }
        }

        public class Command02 : CommandData { // 2
            public byte Byte1 { get; set; }
            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }

        public class Command03 : CommandData { // 3
            public byte Byte1 { get; set; }
            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }

        public class WAIT_TICKS : CommandData { // 4
            public byte Flags { get; set; }
            public ushort Time { get; set; }
			public override void SerializeImpl(SerializerObject s) {
				Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
				Time = s.Serialize<ushort>(Time, name: nameof(Time));
			}
        }

        public class LOOP : CommandData { // 5
            public bool Bool1 { get; set; } // Either key release or sample end
            public override void SerializeImpl(SerializerObject s) {
                Bool1 = s.Serialize<bool>(Bool1, name: nameof(Bool1));
            }
        }
        public class Command06 : CommandData { // 6
            public byte Flags { get; set; }
            public ushort Time { get; set; }
            public ushort Unknown { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
                Time = s.Serialize<ushort>(Time, name: nameof(Time));
				Unknown = s.Serialize<ushort>(Unknown, name: nameof(Unknown));
			}
        }
        public class Command07 : CommandData { // 7
            public byte Byte1 { get; set; }
            public byte Byte2 { get; set; }
            public byte Byte3 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				Byte2 = s.Serialize<byte>(Byte2, name: nameof(Byte2));
				Byte3 = s.Serialize<byte>(Byte3, name: nameof(Byte3));
			}
        }
        public class Command08 : CommandData { // 8
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
			}
        }
        public class Command09 : CommandData { // 9
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
            }
        }
        public class Command10 : CommandData { // 10
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
            }
        }
        public class Command11 : CommandData { // 11
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }
            public short Short4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
                Short4 = s.Serialize<short>(Short4, name: nameof(Short4));
            }
        }
        public class Command12 : CommandData { // 12
            public byte ADSRType { get; set; } // Only 2 possible values: 0 = MusyX linear ADSR, 1 = exp. DLS type ADSR
            public short Short2 { get; set; }
            public short Short4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                ADSRType = s.Serialize<byte>(ADSRType, name: nameof(ADSRType));
                Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
                Short4 = s.Serialize<short>(Short4, name: nameof(Short4));
            }
        }
        public class Command13 : CommandData { // 13
            public byte Byte1 { get; set; }
            public int OffsetInBytes { get; set; } // Directly jump to command with offset Offset + OffsetInBytes

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                OffsetInBytes = s.Serialize<int>(OffsetInBytes, name: nameof(OffsetInBytes));
            }
        }
        public class Command14 : CommandData { // 14
            public byte Byte1 { get; set; }
            public ushort MusyXFileInt04Index { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                MusyXFileInt04Index = s.Serialize<ushort>(MusyXFileInt04Index, name: nameof(MusyXFileInt04Index));
            }
        }
        public class STARTSAMPLE : CommandData { // 15
            public byte Flags { get; set; }
            public ushort SampleIndex { get; set; }
            public uint SampleStartOffset { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
                SampleIndex = s.Serialize<ushort>(SampleIndex, name: nameof(SampleIndex));
				SampleStartOffset = s.Serialize<uint>(SampleStartOffset, name: nameof(SampleStartOffset));
			}
        }
        public class TRAP_KEYOFF : CommandData { // 16
            public byte Byte1 { get; set; }
            public int OffsetInBytes { get; set; } // Register jump destination with offset Offset + OffsetInBytes

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                OffsetInBytes = s.Serialize<int>(OffsetInBytes, name: nameof(OffsetInBytes));
            }
        }
        public class UNTRAP_KEYOFF : CommandData { // 17
            public byte Byte1 { get; set; } // Removes previously registered jump destination

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class Command18 : CommandData { // 18
            public byte Flags { get; set; }
            public uint StartOffset { get; set; } // Offset in a sample
            public int Length { get; set; } // Length counting from StartOffset

            public override void SerializeImpl(SerializerObject s) {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
				StartOffset = s.Serialize<uint>(StartOffset, name: nameof(StartOffset));
				Length = s.Serialize<int>(Length, name: nameof(Length));
			}
        }
        public class Command19 : CommandData { // 19
            public byte Byte1 { get; set; }
            public byte Byte2 { get; set; }
            public byte Byte3 { get; set; }
            public short Short4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Byte2 = s.Serialize<byte>(Byte2, name: nameof(Byte2));
                Byte3 = s.Serialize<byte>(Byte3, name: nameof(Byte3));
				Short4 = s.Serialize<short>(Short4, name: nameof(Short4));
			}
        }
        public class Command20 : CommandData { // 20
            public byte Byte1 { get; set; }
            public byte Byte2 { get; set; }
            public byte Byte3 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Byte2 = s.Serialize<byte>(Byte2, name: nameof(Byte2));
                Byte3 = s.Serialize<byte>(Byte3, name: nameof(Byte3));
            }
        }
        public class Command21 : CommandData { // 21
            public byte Byte1 { get; set; }
            public ushort UShort2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                UShort2 = s.Serialize<ushort>(UShort2, name: nameof(UShort2));
            }
        }
        public class Command22 : CommandData { // 22
            public byte Flags { get; set; }
            public ushort UShort2 { get; set; }
            public ushort UShort4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
				UShort2 = s.Serialize<ushort>(UShort2, name: nameof(UShort2));
				UShort4 = s.Serialize<ushort>(UShort4, name: nameof(UShort4));
			}
        }
        public class SPLITKEY : CommandData { // 23
            public byte Key { get; set; }
            public int OffsetInBytes { get; set; } // If key >= current key, jump to (parent object.)Offset + OffsetInBytes

            public override void SerializeImpl(SerializerObject s) {
                Key = s.Serialize<byte>(Key, name: nameof(Key));
				OffsetInBytes = s.Serialize<int>(OffsetInBytes, name: nameof(OffsetInBytes));
			}
        }
        public class SPLITVEL : CommandData { // 24
            public byte Velocity { get; set; }
            public int OffsetInBytes { get; set; } // If velocity >= current velocity, jump to (parent object.)Offset + OffsetInBytes

            public override void SerializeImpl(SerializerObject s) {
                Velocity = s.Serialize<byte>(Velocity, name: nameof(Velocity));
                OffsetInBytes = s.Serialize<int>(OffsetInBytes, name: nameof(OffsetInBytes));
            }
        }
        public class SPLITRND : CommandData { // 25
            public byte RandomNumber { get; set; }
            public int OffsetInBytes { get; set; } // If random number >= roll, jump to (parent object.)Offset + OffsetInBytes

            public override void SerializeImpl(SerializerObject s) {
                RandomNumber = s.Serialize<byte>(RandomNumber, name: nameof(RandomNumber));
                OffsetInBytes = s.Serialize<int>(OffsetInBytes, name: nameof(OffsetInBytes));
            }
        }
        public class Command26 : CommandData { // 26
            public byte Flags { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
            }
        }
        public class Command27 : CommandData { // 27
            public byte Flags { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
            }
        }
        public class Command28 : CommandData { // 28
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
			}
        }
        public class Command29 : CommandData { // 29
            public byte Byte1 { get; set; }
            public short MacroIndex { get; set; }
            public short Key { get; set; } // Not sure if correct

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                MacroIndex = s.Serialize<short>(MacroIndex, name: nameof(MacroIndex));
				Key = s.Serialize<short>(Key, name: nameof(Key));
			}
        }
        public class Command30 : CommandData { // 30
            public byte Byte1 { get; set; }
            public byte Flags { get; set; }
            public byte Key { get; set; }
            public byte Byte4 { get; set; }
            public byte Byte5 { get; set; }
            public short MacroIndex { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
				Key = s.Serialize<byte>(Key, name: nameof(Key));
				Byte4 = s.Serialize<byte>(Byte4, name: nameof(Byte4));
				Byte5 = s.Serialize<byte>(Byte5, name: nameof(Byte5));
				MacroIndex = s.Serialize<short>(MacroIndex, name: nameof(MacroIndex));
            }
        }
        public class Command31 : CommandData { // 31 - Doesn't do anything on GBA
            public byte Byte1 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class VOICE_ON : CommandData { // 32
            // see http://problemkaputt.de/gbatek-gba-sound-channel-1-tone-sweep.htm
            // Voice 0: Sets Duty cycle in SOUND1CNT_H
            // Voice 1: Sets Duty cycle in SOUND2CNT_L
            /*
             * Specify the duty cycle to use (see above). Enter 255 to have the
                velocity modify the duty cycle (0-31=12.5%, 32-63=25%,
                64-95=50%, 96-127=75%)
             * */
            public byte DutyCycle { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                DutyCycle = s.Serialize<byte>(DutyCycle, name: nameof(DutyCycle));
            }
        }
        public class SETNOISE : CommandData { // 33
            // See http://problemkaputt.de/gbatek-gba-sound-channel-4-noise.htm
            public byte Noise { get; set; } // Sets low byte of SOUND4CNT_H

            public override void SerializeImpl(SerializerObject s) {
                Noise = s.Serialize<byte>(Noise, name: nameof(Noise));
            }
        }
        public class KEYOFF : CommandData { // 34
            // Sends a keyoff to the specified voice. Specify 255 to send a keyoff to the current voice.
            public byte Voice { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Voice = s.Serialize<byte>(Voice, name: nameof(Voice));
            }
        }
        public class Command35 : CommandData { // 35
            // Sends a keyoff to any active macro with the specified index
            public byte Byte1 { get; set; }
            public ushort MacroIndex { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				MacroIndex = s.Serialize<ushort>(MacroIndex, name: nameof(MacroIndex));
			}
        }
        public class HARDENVELOPE : CommandData { // 36
            /* Only works on voice 0, 1 and 3.
             * 
             * Byte consists of these bits:
                8-10  R/W  Envelope Step-Time; units of n/64s  (1-7, 0=No Envelope)
                11    R/W  Envelope Direction                  (0=Decrease, 1=Increase)
                12-15 R/W  Initial Volume of envelope          (1-15, 0=No Sound)
            */
            public byte Envelope { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Envelope = s.Serialize<byte>(Envelope, name: nameof(Envelope));
            }
        }
        public class HARDENVELOPE_OFF : CommandData { // 37
            // Same as HardEnvelope with Envelope = 0. Byte1 isn't checked
            public byte Byte1 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class Command38 : CommandData { // 38
            public byte Byte1 { get; set; } // Seems to set frequency for voices 0-3, so maybe a pitch?

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class Command39 : CommandData { // 39
            public byte Byte1 { get; set; } // Variation of the previous command (38)

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class Command40 : CommandData { // 40
            public byte Byte1 { get; set; }
            public ushort UShort2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				UShort2 = s.Serialize<ushort>(UShort2, name: nameof(UShort2));
			}
        }
        public class Command41 : CommandData { // 41
            // Same as previous command, however whereas that is an absolute value, this uses Short2 as a relative value.
            // It's added and then the total value is clamped between 0 and 0xFFFF.
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
            }
        }
        public class Command42 : CommandData { // 42
            public byte Byte1 { get; set; } // Sets a flag with index FlagIndex to value FlagValue.
            public bool FlagValue { get; set; }
            public sbyte FlagIndex { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				FlagValue = s.Serialize<bool>(FlagValue, name: nameof(FlagValue));
				FlagIndex = s.Serialize<sbyte>(FlagIndex, name: nameof(FlagIndex));
			}
        }
        public class Command43 : CommandData { // 43
            public byte FlagIndex { get; set; } // Flags set by Command42
            public int OffsetInBytes { get; set; } // If flagIndex == 1, go to Offset + OffsetInBytes. Otherwise, continue.

            public override void SerializeImpl(SerializerObject s) {
                FlagIndex = s.Serialize<byte>(FlagIndex, name: nameof(FlagIndex));
                OffsetInBytes = s.Serialize<int>(OffsetInBytes, name: nameof(OffsetInBytes));
            }
        }
        public class Command44 : CommandData { // 44
            public byte Byte1 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class Command45 : CommandData { // 45
            public byte Byte1 { get; set; }
            public byte Byte2 { get; set; }
            public byte Byte3 { get; set; }
            public ushort UShort4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				Byte2 = s.Serialize<byte>(Byte2, name: nameof(Byte2));
				Byte3 = s.Serialize<byte>(Byte3, name: nameof(Byte3));
				UShort4 = s.Serialize<ushort>(UShort4, name: nameof(UShort4));
			}
        }
        public class Command46 : CommandData { // 46
            public byte Byte1 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
            }
        }
        public class Command47 : CommandData { // 47
            public bool Bool1 { get; set; }
            public byte Byte2 { get; set; }
            public byte Byte3 { get; set; }
            public ushort UShort4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
				Bool1 = s.Serialize<bool>(Bool1, name: nameof(Bool1));
				Byte2 = s.Serialize<byte>(Byte2, name: nameof(Byte2));
				Byte3 = s.Serialize<byte>(Byte3, name: nameof(Byte3));
				UShort4 = s.Serialize<ushort>(UShort4, name: nameof(UShort4));
			}
        }
        public class Command48 : CommandData { // 48
            public byte Byte1 { get; set; } // Not used
            public ushort UShort2 { get; set; }
            public ushort UShort4 { get; set; }
            public byte Byte6 { get; set; }
            public byte Byte7 { get; set; } // Not used

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				UShort2 = s.Serialize<ushort>(UShort2, name: nameof(UShort2));
				UShort4 = s.Serialize<ushort>(UShort4, name: nameof(UShort4));
				Byte6 = s.Serialize<byte>(Byte6, name: nameof(Byte6));
				Byte7 = s.Serialize<byte>(Byte7, name: nameof(Byte7));
			}
        }
        public class Command49 : CommandData { // 49
            public byte Byte1 { get; set; }
            public ushort SampleIndex { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                SampleIndex = s.Serialize<ushort>(SampleIndex, name: nameof(SampleIndex));
            }
        }
        public class Command50 : CommandData { // 50
            public byte Byte1 { get; set; }
            public short Short2 { get; set; }
            public ushort UShort4 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                Short2 = s.Serialize<short>(Short2, name: nameof(Short2));
                UShort4 = s.Serialize<ushort>(UShort4, name: nameof(UShort4));
            }
        }
        public class Command51 : CommandData { // 51
            // Changes loop length of current sample. RelativeLoopLength is added to current loop length
            public byte Byte1 { get; set; } // Not used
            public short RelativeLoopLength { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
                RelativeLoopLength = s.Serialize<short>(RelativeLoopLength, name: nameof(RelativeLoopLength));
            }
        }
        public class Command52 : CommandData { // 52
            public byte Byte1 { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
			}
        }
        public class Command53 : CommandData { // 53
            // Changes loop length of current sample. LoopLength is set as absolute value.
            public byte Byte1 { get; set; } // Not used
            public uint LoopLength { get; set; }

            public override void SerializeImpl(SerializerObject s) {
                Byte1 = s.Serialize<byte>(Byte1, name: nameof(Byte1));
				LoopLength = s.Serialize<uint>(LoopLength, name: nameof(LoopLength));
			}
        }
        #endregion
    }
}