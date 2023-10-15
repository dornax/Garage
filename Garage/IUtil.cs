namespace Garage
{
    internal interface IUtil
    {
        int CommandRow { get; }
        int InfoRow { get; }
        int LabelWidth { get; }
        void ClearCommand();
        void ClearInfo(int numberOfRows);
        void WriteColoredFirstLetter(IEnumerable<string> list);
        void WriteColoredFirstLetter(string word);
        void WriteEnterToConfirm();
        void WriteInfo(string info);
        void WriteRegistrationLabel(int x, int y);
    }
}