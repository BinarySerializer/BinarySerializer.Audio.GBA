namespace BinarySerializer.GBA.Audio.GAX {
    public static class ContextExtensions {
        public static GAX_Settings GetGAXSettings(this SerializerObject s) => s.Context.GetGAXSettings();
        public static GAX_Settings GetGAXSettings(this Context c) {
            var settings = c.GetStoredObject<GAX_Settings>(GAX_Settings.ContextID);
            if (settings == null) {
                settings = c.StoreObject<GAX_Settings>(GAX_Settings.ContextID, new GAX_Settings() {
                    MajorVersion = 3,
                    MinorVersion = 0
                });
            }
            return settings;
        }
    }
}