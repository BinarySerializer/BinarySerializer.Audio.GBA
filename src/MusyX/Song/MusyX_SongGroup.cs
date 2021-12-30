using System.Text;

namespace BinarySerializer.GBA.Audio.MusyX
{
    public class MusyX_SongGroup : BinarySerializable {
        // Set in OnPreSerialize
        public uint Length { get; set; }

        public byte[] SongBytes { get; set; }

        public int[] Header { get; set; }
        public MusyX_SoundListItem[] SoundList { get; set; }
        public MusyX_SoundListItem[] DrumChannel { get; set; }
        public MusyX_Song Song { get; set; }

        /// <summary>
        /// Handles the data serialization
        /// </summary>
        /// <param name="s">The serializer object</param>
        public override void SerializeImpl(SerializerObject s)
        {
            s.DoAt(Offset, () => {
                SongBytes = s.SerializeArray<byte>(SongBytes, Length, name: nameof(SongBytes));
            });
			Header = s.SerializeArray<int>(Header, 4, name: nameof(Header));
			SoundList = s.SerializeObjectArray<MusyX_SoundListItem>(SoundList, 128, name: nameof(SoundList));
			DrumChannel = s.SerializeObjectArray<MusyX_SoundListItem>(DrumChannel, 128, name: nameof(DrumChannel));
			Song = s.SerializeObject<MusyX_Song>(Song, name: nameof(Song));

		}
    }
}