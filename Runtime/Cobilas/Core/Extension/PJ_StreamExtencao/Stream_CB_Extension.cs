using System.Text;

namespace System.IO {
    public static class Stream_CB_Extension {
        public static byte[] Read(this Stream F) {
            byte[] Res = new byte[F.Length];
            int numDeBytesPraLer = Res.Length;
            int numDeBytesLidos = 0;
            while (numDeBytesPraLer > 0) {
                int n = F.Read(Res, numDeBytesLidos, numDeBytesPraLer);

                if (n == 0) break;
                numDeBytesLidos += n;
                numDeBytesPraLer -= n;
            }
            return Res;
        }

        public static void Write(this Stream F, string text, Encoding encoding)
            => Write(F, text.ToCharArray(), encoding);

        public static void Write(this Stream F, char[] chars, Encoding encoding)
            => Write(F, encoding.GetBytes(chars));

        public static void Write(this Stream F, byte[] bytes)
            => F.Write(bytes, 0, bytes.Length);

        public static char[] GetChars(this Stream F, Encoding encoding)
            => encoding.GetChars(Read(F));

        public static string GetString(this Stream F, Encoding encoding)
            => new string(GetChars(F, encoding));

        public static Guid GenerateGuid(this Stream F) {
            byte[] guid = new byte[16];
            byte[] content = Read(F);
            for (int I = 0, g = 0; I < content.Length; I++, g++)
                guid[(g >= 16 ? g = 0 : g)] ^= content[I];
            return new Guid(guid);
        }
    }
}
