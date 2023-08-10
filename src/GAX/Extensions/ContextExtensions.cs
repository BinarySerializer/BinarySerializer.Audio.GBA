namespace BinarySerializer.Audio.GBA.GAX {
    public static class ContextExtensions {
        public static GAX_Settings GetGAXSettings(this SerializerObject s) => s.Context.GetGAXSettings();
        public static GAX_Settings GetGAXSettings(this Context c) {
            return c.GetSettings<GAX_Settings>() ?? c.AddSettings<GAX_Settings>(new GAX_Settings() {
                MajorVersion = 3,
                MinorVersion = 0
            });
        }
        public static GAX_Settings SetGAXSettings(this Context c, GAX_Settings settings) {
            settings = c.AddSettings<GAX_Settings>(settings);
            return settings;
        }
    }
}