using System.Text;

namespace Cobilas {
    [System.Serializable]
    public struct Interrupter {
        private int currentIndex;
        private bool useASwitch;
        private readonly bool[] _switches;

        public int CurrentIndex => currentIndex;
        ///<summary>Esta propriedade permite a troca de unico interruptor para mult interruptores e vise versa.</summary>
        public bool UseASwitch { get => useASwitch; set => useASwitch = value; }

        public bool this[int Index] {
            get => _switches[Index];
            set {
                if (currentIndex != Index && useASwitch) {
                    ChangeValue(Index);
                    currentIndex = Index;
                }
                _switches[Index] = value;
            }
        }

        /// <summary>Só um interruptor especificando o indice sera usado os demais se manteram com o valor falso.</summary>
        /// <param name="Capacity">Quantos interruptores.</param>
        /// <param name="UseASwitch">Permite usar um interruptor por vez.</param>
        public Interrupter(int Capacity, bool UseASwitch) {
            _switches = new bool[Capacity];
            currentIndex = -1;
            useASwitch = UseASwitch;
        }

        /// <summary>Só um interruptor especificando o indice sera usado os demais se manteram com o valor falso.</summary>
        /// <param name="Capacity">Quantos interruptores.</param>
        public Interrupter(int Capacity) : this(Capacity, true) { }

        private void ChangeValue(int index) {
            for (int I = 0; I < _switches.Length; I++)
                if (I != index) _switches[I] = false;
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Switches {");
            for (int I = 0; I < _switches.Length; I++)
                builder.AppendLine($"\tswitch({I})[status:{_switches[I]}]");
            builder.AppendLine("}");
            return builder.ToString();
        }
    }
}
