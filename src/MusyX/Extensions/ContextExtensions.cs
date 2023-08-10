namespace BinarySerializer.Audio.GBA.MusyX {
    public static class ContextExtensions {
        public static MusyX_Settings GetMusyXSettings(this SerializerObject s) => s.Context.GetMusyXSettings();
        public static MusyX_Settings GetMusyXSettings(this Context c) {
            return c.GetSettings<MusyX_Settings>() ?? c.AddSettings<MusyX_Settings>(new MusyX_Settings());
        }
        public static MusyX_Settings SetMusyXSettings(this Context c, MusyX_Settings settings) {
            settings = c.AddSettings<MusyX_Settings>(settings);
            return settings;
        }
    }
}