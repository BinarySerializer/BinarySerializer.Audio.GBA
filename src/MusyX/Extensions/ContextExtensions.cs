namespace BinarySerializer.GBA.Audio.MusyX {
    public static class ContextExtensions {
        public static MusyX_Settings GetMusyXSettings(this SerializerObject s) => s.Context.GetMusyXSettings();
        public static MusyX_Settings GetMusyXSettings(this Context c) {
            var settings = c.GetSettings<MusyX_Settings>(throwIfNotFound: false);
            if (settings == null) {
                settings = c.AddSettings<MusyX_Settings>(new MusyX_Settings() {
                });
            }
            return settings;
        }
        public static MusyX_Settings SetMusyXSettings(this Context c, MusyX_Settings settings) {
            settings = c.AddSettings<MusyX_Settings>(settings);
            return settings;
        }
    }
}