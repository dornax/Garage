namespace Garage
{
    internal class Util : IUtil
    {
        private IUI ui;
        public int LabelWidth { get; } = 20;
        public int CommandRow { get; } = 14;
        public int InfoRow { get; } = 16;
        public Util(IUI ui) => this.ui = ui;
        public void ClearCommand()
        {
            ui.SetCursorPosition(0, CommandRow);
            ui.Write(ui.PadRight("", ui.GetWindowWidth()));
        }
        public void ClearInfo(int numberOfRows)
        {
            string output = "";
            string row = ui.PadRight("", ui.GetWindowWidth());
            ui.SetCursorPosition(0, InfoRow);
            for (int i = 0; i < numberOfRows; i++)
            {
                output += row + '\n';
            }
            ui.Write(output);
        }
        public void WriteEnterToConfirm() => WriteInfo("Press Enter to confirm.");
        public void WriteInfo(string info)
        {
            ui.SetCursorPosition(0, InfoRow);
            ui.WriteLine(info);
        }
        public void WriteRegistrationLabel(int x, int y)
        {
            ui.SetCursorPosition(x, y);
            ui.Write($"{ui.PadLeft("RegistrationNumber:", LabelWidth)} ");
        }
        public void WriteColoredFirstLetter(string word)
        {
            ui.Write(word.Substring(0, 1), ConsoleColor.Green);
            ui.Write($"{word.Substring(1)} ");
        }
        public void WriteColoredFirstLetter(IEnumerable<string> list)
        {
            foreach (string item in list) WriteColoredFirstLetter(item);
            ui.Write("\n");
        }
    }
}
