namespace Garage
{
    internal interface IHelperUI
    {
        public int CommandRow { get; }
        public int InfoRow { get; }
        public int LabelWidth { get; }
        void ClearCommand();
        void ClearInfo(int numberOfRows);
        string PadLeft(string str, int textFieldWidth);
        string PadRight(string str, int textFieldWidth);
        void WriteInfo(string info);
        void WriteEnterToConfirm();
        void WriteRegistrationLabel(int x, int y);
    }
}