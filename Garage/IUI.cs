namespace Garage
{
    public interface IUI
    {
        bool CursorVisible { set; }
        int LabelWidth { get; }
        int CommandRow { get; }
        int InfoRow { get; }
        void Clear();
        public void ClearCommand();
        void ClearInfo(int numberOfRows);
        public string PadLeft(string str, int textFieldWidth);
        public string PadRight(string str, int textFieldWidth);
        void WriteInfo(string info);
        public void WriteEnterToConfirm();
        public void WriteRegistrationLabel(int x, int y);
        ConsoleKey GetKey();
        string GetValue(int numberOfDigits);
        string GetDigit(int numberOfDigits);
        string GetFileName(int numberOfCharacters);
        string GetInput(int numberOfCharacters);
        string GetRegNum();
        string ReadLine();
        void SetCursorPosition(int x, int y);
        void Write(string message);
        void WriteColoredFirstLetter(string word);
        void WriteColoredFirstLetter(List<string> list);

        void WriteLine(string message);
    }
}