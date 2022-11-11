using System.IO;
using System.Text;
using Cobilas.Collections;

namespace System.Security.Cryptography {
    public static class HashAlgorithm_CB_Extension {

        public static bool CompareComprestFileHash(this HashAlgorithm H, string FileName1, string FileName2)
            => ComprestComputehashToString(H, FileName1) == ComprestComputehashToString(H, FileName2);

        public static bool CompareComprestFileHash(this HashAlgorithm H, FileStream file1, FileStream file2)
            => ComprestComputehashToString(H, file1) == ComprestComputehashToString(H, file2);

        public static bool CompareFileHash(this HashAlgorithm H, string FileName1, string FileName2)
            => ComputeHashToString(H, FileName1) == ComputeHashToString(H, FileName2);

        public static bool CompareFileHash(this HashAlgorithm H, FileStream file1, FileStream file2)
            => ComputeHashToString(H, file1) == ComputeHashToString(H, file2);

        public static byte[] ComputeHash(this HashAlgorithm H, string FileName)
            => H.ComputeHash(GetFileBytes(FileName));

        public static string ComputeHashToString(this HashAlgorithm H, FileStream File)
            => ComputeHashToString(H, File.Read());

        public static string ComputeHashToString(this HashAlgorithm H, string FileName)
            => ComputeHashToString(H, GetFileBytes(FileName));

        public static string ComputeHashToString(this HashAlgorithm H, byte[] bytes)
            => I_ComputehashToString(H.ComputeHash(bytes));

        public static string ComprestComputehashFileNameToString(this HashAlgorithm H, string FileName)
            => I_ComprestComputehashToString(ComputehashFileName(H, FileName));

        public static string ComputehashFileNameToString(this HashAlgorithm H, string FileName)
            => I_ComputehashToString(ComputehashFileName(H, FileName));

        public static byte[] ComputehashFileName(this HashAlgorithm H, string FileName)
            => H.ComputeHash(
                ArrayManipulation.Add<byte>(
                    GetFileBytes(FileName),
                    FilePathToByteArray(FileName)));

        public static string ComprestComputehashToString(this HashAlgorithm H, string FileName) {
            using (FileStream File = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                return ComprestComputehashToString(H, File);
        }

        public static string ComprestComputehashToString(this HashAlgorithm H, FileStream File)
            => ComprestComputehashToString(H, File.Read());

        public static string ComprestComputehashToString(this HashAlgorithm H, byte[] bytes)
            => I_ComprestComputehashToString(H.ComputeHash(bytes));

        public static string ComputehashDirectoryToString(this HashAlgorithm H, string DirectoryPath)
            => I_ComputehashToString(ComputehashDirectory(H, DirectoryPath));

        public static string ComprestComputehashDirectoryToString(this HashAlgorithm H, string DirectoryPath)
            => I_ComprestComputehashToString(ComputehashDirectory(H, DirectoryPath));

        public static string ComputehashDirectoryNameToString(this HashAlgorithm H, string DirectoryPath)
            => I_ComputehashToString(ArrayManipulation.Add<byte>(
                ComputehashDirectory(H, DirectoryPath),
                FilePathToByteArray(DirectoryPath)
                ));

        public static string ComprestComputehashDirectoryNameToString(this HashAlgorithm H, string DirectoryPath)
            => I_ComprestComputehashToString(ArrayManipulation.Add<byte>(
                ComputehashDirectory(H, DirectoryPath),
                FilePathToByteArray(DirectoryPath)
                ));

        public static byte[] ComputehashDirectoryName(this HashAlgorithm H, string DirectoryPath)
            => H.ComputeHash(
                ArrayManipulation.Add<byte>(
                    GetDirectoryBytes(DirectoryPath),
                    FilePathToByteArray(DirectoryPath)));

        public static byte[] ComputehashDirectory(this HashAlgorithm H, string DirectoryPath)
            => H.ComputeHash(GetDirectoryBytes(DirectoryPath));

        private static string I_ComputehashToString(byte[] bytes) {
            StringBuilder builder = new StringBuilder();
            for (int I = 0; I < ArrayManipulation.ArrayLength(bytes); I++)
                builder.Insert(builder.Length, bytes[I]);
            return builder.Length > 0 ? builder.ToString() : "0";
        }

        private static byte[] FilePathToByteArray(string path)
            => Array.ConvertAll<char, byte>(path.ToCharArray(), (c) => (byte)c);

        private static string I_ComprestComputehashToString(byte[] bytes) {
            StringBuilder builder = new StringBuilder();
            int Count = ArrayManipulation.ArrayLength(bytes) / 4;
            int S_Bytes = 0;
            for (int I = 0, C = 1; I < ArrayManipulation.ArrayLength(bytes); I++, C++) {
                if (C >= Count) {
                    C = 0;
                    builder.Insert(builder.Length, S_Bytes);
                    S_Bytes = 0;
                }
                S_Bytes += bytes[I];
            }
            builder.Insert(builder.Length, S_Bytes);
            return builder.ToString();
        }

        private static byte[] GetDirectoryBytes(string DirectoryPath) {
            byte[] Res = null;
            string[] Files = Directory.GetFiles(DirectoryPath);
            for (int I = 0; I < ArrayManipulation.ArrayLength(Files); I++)
                ArrayManipulation.Add<byte>(GetFileBytes(Files[I]), ref Res);

            ArrayManipulation.ClearArraySafe<string>(ref Files);
            if (ArrayManipulation.EmpytArray(Res))
                Res = new byte[] { 0 };
            return Res;
        }

        private static byte[] GetFileBytes(string FileName) {
            using (FileStream F = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                return GetFileBytes(F);
        }

        private static byte[] GetFileBytes(FileStream file)
            => file.Read();
    }
}
