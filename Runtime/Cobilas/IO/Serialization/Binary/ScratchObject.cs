using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cobilas.IO.Serialization.Binary {
    /// <summary>Objeto de rascunho base.</summary>
    [Serializable]
    public abstract class ScratchObject {
        /// <summary>Nome do rascunho.</summary>
        public abstract string Name { get; }

        /// <summary>Descarrega objeto de rascunho para um arquivo.</summary>
        /// <param name="scratch">Objeto de rascunho.</param>
        /// <param name="filePath">Caminho do arquivo onde o objeto vai ser descarregado.</param>
        public static void UnloadScratchObject(ScratchObject scratch, string filePath) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.Create(filePath))
                formatter.Serialize(stream, scratch);
        }

        /// <summary>Descarrega objeto de rascunho para um arquivo.</summary>
        /// <param name="scratch">Objeto de rascunho.</param>
        /// <param name="folderPath">Caminho do diretório onde o arquivo vai ser criado.</param>
        /// <param name="name">Nome do arquivo.</param>
        /// <param name="extension">Extensão do arquivo.</param>
        public static void UnloadScratchObject(ScratchObject scratch, string folderPath, string name, string extension = "sobj")
            => UnloadScratchObject(scratch, Path.ChangeExtension(Path.Combine(folderPath, name), extension));

        /// <summary>Descarrega objeto de rascunho para um arquivo.</summary>
        /// <param name="scratch">Objeto de rascunho.</param>
        /// <param name="folderPath">Caminho do diretório onde o arquivo vai ser criado.</param>
        /// <param name="extension">Extensão do arquivo.</param>
        public static void UnloadScratchObject(ScratchObject scratch, string folderPath, string extension = "sobj")
            => UnloadScratchObject(scratch, folderPath, scratch.Name, extension);

        /// <summary>Carrega um objeto <see cref="ScratchObject"/> de um arquivo.</summary>
        /// <param name="filePath">Caminho do arquivo.</param>
        public static ScratchObject LoadScratchObject(string filePath) {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = File.OpenRead(filePath))
                return (ScratchObject)formatter.Deserialize(stream);
        }
    }
}
