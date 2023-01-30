using System.Text;

namespace System.Xml {
    public static class XMLWriter_CB_Extension {
        public static void WriteElementTag(this XmlWriter W, ElementTag tag) {
            W.WriteStartDocument();
            MountElementTagEngraving(W, tag, 0);
            W.WriteEndDocument();
        }

        private static void MountElementTagEngraving(XmlWriter W, ElementTag tag, int layer) {
            if (W.Settings.Indent) {
                W.WriteWhitespace(W.Settings.IndentChars);
                W.WriteWhitespace(GenerateTabulation(layer));
            }
            W.WriteStartElement(tag.Name);
            tag.ForEach((EA) => { W.WriteAttributeString(EA.Name, EA.Value.ValueToString); });

            bool ContainsElements = tag.TagElementCount > 0;

            if (!tag.ValueIsEmpty)
                W.WriteValue(tag.ValueOBJ);

            tag.ForEach((ET) => { MountElementTagEngraving(W, ET, layer + 1); });

            if (W.Settings.Indent && ContainsElements) { 
                W.WriteWhitespace(W.Settings.IndentChars);
                W.WriteWhitespace(GenerateTabulation(layer));
            }
            W.WriteEndElement();
        }

        private static string GenerateTabulation(int tabulation) {
            StringBuilder Res = new StringBuilder("");
            for (int I = 0; I < tabulation; I++)
                Res.Insert(Res.Length, '\t');
            return Res.ToString();
        }
    }
}
