using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    internal class HelperUI : IHelperUI
    {
        private IUI ui;
        public int LabelWidth { get; } = 20;
        public int CommandRow { get; } = 14;
        public int InfoRow { get; } = 16;

        private int infoRow = 16;
        private int commandRow = 14;
        private int labelWidth = 20;
        public HelperUI(IUI ui)
        {
            this.ui = ui;
        }

        public void ClearCommand()
        {
            ui.SetCursorPosition(0, commandRow);
            ui.Write(PadRight("", Console.WindowWidth));
        }
        public void ClearInfo(int numberOfRows)
        {
            string output = "";
            string line = PadRight("", Console.WindowWidth);
            ui.SetCursorPosition(0, infoRow);
            for (int i = 0; i < numberOfRows; i++)
            {
                output += line + '\n';
            }
            ui.Write(output);
        }
        public void WriteInfo(string info)
        {
            ui.SetCursorPosition(0, infoRow);
            ui.WriteLine(info);
        }
        public void WriteEnterToConfirm() => WriteInfo("Press Enter to confirm.");
        public void WriteRegistrationLabel(int x, int y)
        {
            string label = PadLeft("RegistrationNumber:", labelWidth) + " ";
            ui.SetCursorPosition(x, y);
            ui.Write(label);
        }

        public string PadRight(string str, int textFieldWidth) => str.PadRight(textFieldWidth);
        public string PadLeft(string str, int textFieldWidth) => str.PadLeft(textFieldWidth);
    }
}
