using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal class HelperUI : IHelperUI
    {
        IUI ui = default!;
        public int LabelWidth { get; } = 20;
        public int CommandRow { get; } = 14;
        public int InfoRow { get; } = 16;

        public HelperUI(IUI ui)
        {
            this.ui = ui;
        }

        public void ClearCommand()
        {
            ui.SetCursorPosition(0, CommandRow);
            ui.Write(PadRight("", Console.WindowWidth));
        }
        public void ClearInfo(int numberOfRows)
        {
            string output = "";
            string line = PadRight("", Console.WindowWidth);
            ui.SetCursorPosition(0, InfoRow);
            for (int i = 0; i < numberOfRows; i++)
            {
                output += line + '\n';
            }
            ui.Write(output);
        }
        public void WriteInfo(string info)
        {
            ui.SetCursorPosition(0, InfoRow);
            ui.WriteLine(info);
        }
        public void WriteEnterToConfirm() => WriteInfo("Press Enter to confirm.");
        public void WriteRegistrationLabel(int x, int y)
        {
            string label = PadLeft("RegistrationNumber:", LabelWidth) + " ";
            ui.SetCursorPosition(x, y);
            ui.Write(label);
        }

        public string PadRight(string str, int textFieldWidth) => str.PadRight(textFieldWidth);
        public string PadLeft(string str, int textFieldWidth) => str.PadLeft(textFieldWidth);
    }
}
