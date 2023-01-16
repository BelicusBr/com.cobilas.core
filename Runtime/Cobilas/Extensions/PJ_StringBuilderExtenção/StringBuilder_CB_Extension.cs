namespace System.Text {
    public static class StringBuilder_CB_Extension {
        public static StringBuilder Clear(this StringBuilder S)
            => S.Remove(0, S.Length);
    }
}
