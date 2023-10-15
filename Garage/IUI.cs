namespace Garage
{
    public interface IUI
    {
        bool CursorVisible { set; }
        void Clear();
        string GetDigit(int numberOfDigits);
        string GetFileName(int numberOfCharacters);
        string GetInput(int numberOfCharacters);
        ConsoleKey GetKey();
        string GetRegNum();
        string GetValue(int numberOfDigits);
        int GetWindowWidth();
        string PadLeft(string str, int textFieldWidth);
        string PadRight(string str, int textFieldWidth);
        void SetCursorPosition(int x, int y);
        void SetForegroundColor(ConsoleColor color);
        void Write(string message, ConsoleColor color = ConsoleColor.White);
        void WriteLine(string message, ConsoleColor color = ConsoleColor.White);


    }
}