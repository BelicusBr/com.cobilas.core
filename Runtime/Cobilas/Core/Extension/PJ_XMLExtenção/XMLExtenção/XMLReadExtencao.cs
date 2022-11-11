namespace System.Xml {
    public static class XMLRead_CB_Extension {
        public static ElementTag GetElementTag(this XmlReader R) {
            ElementTag Res = null;
            ElementTag Atual = null;
            while (R.Read()) {
                switch (R.NodeType) {
                    case XmlNodeType.Element:
                        if (Res == null) Atual = Res = new ElementTag(R.Name);
                        else {
                            ElementTag Novo = new ElementTag(R.Name);
                            Atual.Add(Novo);
                            Atual = Novo;
                        }
                        if (R.AttributeCount > 0) { 
                            for (int I = 0; I < R.AttributeCount; I++) {
                                R.MoveToAttribute(I);
                                Atual.Add(new ElementAttribute(R.Name, R.Value));
                            }
                            R.MoveToElement();
                        }
                        if (R.IsEmptyElement)
                            Atual = Atual.Parent;
                        break;
                    case XmlNodeType.Text:
                        Atual.Value = new ElementValue(R.Value);
                        break;
                    case XmlNodeType.EndElement:
                        Atual = Atual.Parent;
                        break;
                }
            }
            return Res;
        }
    }
}
