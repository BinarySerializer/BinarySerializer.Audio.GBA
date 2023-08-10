using System.Text.RegularExpressions;

namespace BinarySerializer.Audio.GBA.MusyX
{
	public class MusyX_Settings {
		public bool EnableErrorChecking { get; set; } = false;

		public void CheckPointer(Pointer ptr, BinarySerializable b, string name, bool allowNull = false, int maxValue = 0x01000000) {
			if (ptr == null) {
				if (allowNull) return;
				throw new BinarySerializableException(b, $"Pointer {name} was null");
			}
			if (ptr.SerializedOffset >= maxValue)
				throw new BinarySerializableException(b, $"Pointer {name} had value {ptr.SerializedOffset:X8}");
		}
		public void CheckPointer<T>(Pointer<T> ptr, BinarySerializable b, string name, bool allowNull = false, int maxValue = 0x01000000) where T : BinarySerializable, new() {
			CheckPointer(ptr?.PointerValue, b, name, allowNull, maxValue: maxValue);
		}
	}
}
