using System.Text;
using Cobilas.IO.Alf.Components;

namespace Cobilas.IO.Alf.Alfbt.Components.Compiler {
    internal static class ALFBTCompiler {

        internal static string version => GetALFBTVersion(ALFBTVersion.alfbt_1_5);

        internal static void Read(string[] inputs, ALFWriter writer) {
            ALFBTVersion aLFBTVersion = DetectAlfbtVersion(inputs);
            
            switch (aLFBTVersion) {
                case ALFBTVersion.alfbt_1_0:
                    ALFBTCompiler_1_0.Read(inputs, writer);
                    break;
                case ALFBTVersion.alfbt_1_5:
                    ALFBTCompiler_1_5.Read(inputs, writer);
                    break;
                default:
                    break;
            }
        }

        internal static void Writer(ALFItem root, StringBuilder builder)
            => ALFBTCompiler_1_5.Writer(root, builder);

        private static ALFBTVersion DetectAlfbtVersion(string[] lines) {
            ALFBTVersion res = ALFBTVersion.UnknownVersion;
            foreach (var item in lines) {
                if (string.IsNullOrEmpty(item)) continue;
                if (ALFBTCompiler_1_0.DetectAlfbtVersion(item, out res))
                    return res;
                if (ALFBTCompiler_1_5.DetectAlfbtVersion(item, out res))
                    return res;
            }

            return res;
        }

        private static string GetALFBTVersion(ALFBTVersion v) {
            switch (v) {
                case ALFBTVersion.alfbt_1_0: return "1.0";
                case ALFBTVersion.alfbt_1_5: return "1.5";
                default: return "x.x";
            }
        }
    }
}