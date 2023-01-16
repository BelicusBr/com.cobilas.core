using Cobilas.Collections;

namespace System.Xml {
    public class ElementTag : ElementAttribute, ICloneable {
        protected ElementBase[] elementos;
        protected ElementTag parent;

        public bool ValueIsEmpty => value == null;
        public ElementTag Parent { get => parent; set => parent = value; }
        public ElementBase[] Elementos { get => elementos; set => elementos = value; }
        public int ElementCount => ArrayManipulation.ArrayLength(elementos);
        public int TagElementCount => TagCount();
        public int AttributeCount => m_AttributeCount();

        //=========Construct(ElementTag, string, (ElementValue, object, string), ElementAttribute[], ElementTag[])
        public ElementTag(ElementTag parent, string name, ElementValue value, params ElementBase[] elementos) :
            base(name, value) {
            this.parent = parent;
            for (int I = 0; I < ArrayManipulation.ArrayLength(elementos); I++)
                Add(elementos[I]);
        }

        public ElementTag(ElementTag parent, string name, object value, params ElementBase[] elementos) :
            this(parent, name, new ElementValue(value), elementos) { }

        public ElementTag(ElementTag parent, string name, string value, params ElementBase[] elementos) :
            this(parent, name, new ElementValue(value), elementos) { }

        public ElementTag(string name, ElementValue value, params ElementBase[] elementos) :
            this(null, name, value, elementos) { }

        public ElementTag(string name, object value, params ElementBase[] elementos) :
            this(null, name, new ElementValue(value), elementos) { }

        public ElementTag(string name, string value, params ElementBase[] elementos) :
            this(null, name, new ElementValue(value), elementos) { }

        public ElementTag(string name, ElementValue value) :
            this(null, name, value, null) { }

        public ElementTag(string name, object value) :
            this(null, name, new ElementValue(value), null) { }

        public ElementTag(string name, string value) :
            this(null, name, new ElementValue(value), null) { }

        public ElementTag(ElementTag parent, string name, params ElementBase[] elementos) :
            this(parent, name, (ElementValue)null, elementos) { }

        public ElementTag(string name, params ElementBase[] elementos) :
            this(null, name, (ElementValue)null, elementos) { }

        public ElementTag(ElementTag parent, string name) :
            this(parent, name, (ElementValue)null, null) { }

        public ElementTag(string name) :
            this(null, name, (ElementValue)null, null) { }

        public ElementTag() : this(null) { }

        ~ElementTag()
            => Dispose(false);

        public void Add(ElementBase item) {
            if (item.CompareType<ElementTag>())
                (item as ElementTag).parent = this;
            ArrayManipulation.Add<ElementBase>(item, ref elementos);
        }

        public void Remover(int index)
            => ArrayManipulation.Remove<ElementBase>(index, ref elementos);

        public void Remover(ElementBase elemento)
            => ArrayManipulation.Remove<ElementBase>(elemento, ref elementos);

        public ElementAttribute GetElementAttribute(string name) {
            for (int I = 0; I < ElementCount; I++)
                if (elementos[I].CompareType<ElementAttribute>())
                    if (elementos[I].Name == name)
                        return elementos[I] as ElementAttribute;
            return null;
        }

        public ElementTag GetElementTag(string name) {
            for (int I = 0; I < ElementCount; I++) 
                if (elementos[I].CompareType<ElementTag>()) { 
                    ElementTag Item = elementos[I] as ElementTag;
                    if (Item.name == name) return Item;
                    else {
                        Item = Item.GetElementTag(name);
                        if (Item != null)
                            if (Item.name == name)
                                return Item;
                    }
                }
            
            return null;
        }

        public void ForEach(Action<ElementTag> action) {
            ElementTag[] Itens = GetElementTags();
            for (int I = 0; I < ArrayManipulation.ArrayLength(Itens); I++)
                action(Itens[I]);
        }

        public void ForEach(Action<ElementAttribute> action) {
            ElementAttribute[] Itens = GetElementAttributes();
            for (int I = 0; I < ArrayManipulation.ArrayLength(Itens); I++)
                action(Itens[I]);
        }

        public ElementTag[] GetElementTags() {
            ElementTag[] Res = null;
            for (int I = 0; I < ElementCount; I++)
                if (elementos[I].CompareType<ElementTag>())
                    ArrayManipulation.Add<ElementTag>((ElementTag)elementos[I], ref Res);
            return Res;
        }

        public ElementAttribute[] GetElementAttributes() {
            ElementAttribute[] Res = null;
            for (int I = 0; I < ElementCount; I++)
                if (elementos[I].CompareType<ElementAttribute>())
                    ArrayManipulation.Add<ElementAttribute>((ElementAttribute)elementos[I], ref Res);
            return Res;
        }

        public bool Contains(string name, bool retroativo) {
            for (int I = 0; I < TagElementCount; I++)
                if (elementos[I].CompareType<ElementTag>()) {
                    if (retroativo) {
                        if (elementos[I].Name == name) return true;
                        else {
                            if ((elementos[I] as ElementTag).Contains(name, retroativo))
                                return true;
                        }
                    } else {
                        if (elementos[I].Name == name)
                            return true;
                    }
                }
            return false;
        }

        public object Clone() {
            ElementTag Res = new ElementTag(parent, name, ValueIsEmpty ? (ElementValue)null : new ElementValue(value.ValueToString));
            for (int I = 0; I < ElementCount; I++)
                if (elementos[I].CompareType<ElementAttribute>()) {
                    ElementAttribute Atri = elementos[I] as ElementAttribute;
                    Res.Add(new ElementAttribute(Atri.Name, Atri.Value));
                } else Res.Add((ElementTag)(elementos[I] as ElementTag).Clone());
            return Res;
        }

        private int TagCount() {
            int Indice = 0;
            for (int I = 0; I < ElementCount; I++)
                if (elementos[I].CompareType<ElementTag>())
                    Indice++;
            return Indice;
        }

        private int m_AttributeCount() {
            int Indice = 0;
            for (int I = 0; I < ElementCount; I++)
                if (elementos[I].CompareType<ElementAttribute>())
                    Indice++;
            return Indice;
        }

        protected override void Dispose(bool disposing)
            => base.Dispose(disposing);

        protected override void Garbage() {
            base.Garbage();
            for (int I = 0; I < ElementCount; I++)
                elementos[I].Dispose();
            parent = null;
            ArrayManipulation.ClearArraySafe(ref elementos);
        }
    }
}
