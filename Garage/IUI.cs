namespace Garage
{
    public interface IUI
    {
        bool CursorVisible { set; }
        void Clear();
        ConsoleKey GetKey();
        string GetValue(int numberOfDigits);
        string GetDigit(int numberOfDigits);
        string GetFileName(int numberOfCharacters);
        string GetInput(int numberOfCharacters);
        string GetRegistrationNumber();
        string ReadLine();
        void SetCursorPosition(int x, int y);
        void Write(string message);
        void WriteColoredFirstLetter(string word);
        void WriteColoredFirstLetter(List<string> list);

        void WriteLine(string message);
    }
}