namespace Cobilas.IO.Alf.Components {
    public abstract class ALFReadSettings : ALFSettings {
        public abstract bool RemoveEscapeOnSpecialCharacters { get; set; }

        public abstract char[] Read();
    }
}
