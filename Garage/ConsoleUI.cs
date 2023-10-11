using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Garage
{
    public class ConsoleUI : IUI
    {
        private (int x, int y) pos = (0, 0);
        public bool CursorVisible { set => Console.CursorVisible = value; }

        public void Clear()
        {
            Console.Clear();
        }

        public string ReadLine() 
        {
            CursorVisible = true;
            string? input = Console.ReadLine();
            CursorVisible = false;
            return input!;
        } 
        
        public void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }

        public void WriteColoredFirstLetter(string word)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(word.Substring(0, 1));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{word.Substring(1)} ");
        }
        public void WriteColoredFirstLetter(List<string> list)
        {
            foreach (string item in list) WriteColoredFirstLetter(item);
            Console.Write("\n");
        }
        public void Write(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
        
        public string GetValue(int numberOfDigits)
        {
            string result = default!;
            string str = "";
            int index = 0;

            pos = Console.GetCursorPosition();
            ConsoleKeyInfo getKey = Console.ReadKey(intercept: true);
            while (getKey.Key != ConsoleKey.Enter)
            {
                if (getKey.Key == ConsoleKey.Backspace)
                {
                    if (index > 0)
                    {
                        str = str.Substring(0, index - 1);
                        index--;
                        Console.SetCursorPosition(pos.x + index, pos.y);
                        Console.Write(" ");
                    }
                }
                else if (index < numberOfDigits)
                {
                    char c;
                    if (char.IsDigit(getKey.KeyChar) || getKey.KeyChar == ',' || getKey.KeyChar == '.')
                    {
                        c = getKey.KeyChar;
                        if (c == '.') c = ',';
                        str += c;
                        index++;
                    }
                }
                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(str);
                getKey = Console.ReadKey(intercept: true);
            }
            if (getKey.Key == ConsoleKey.Enter)
            {
                result = str;
            }
            return result;
        }
        public string GetDigit(int numberOfDigits)
        {
            string result = default!;
            string str = "";
            int index = 0;

            pos = Console.GetCursorPosition();
            ConsoleKeyInfo getKey = Console.ReadKey(intercept: true);
            while (getKey.Key != ConsoleKey.Enter)
            {
                if (getKey.Key == ConsoleKey.Backspace)
                {
                    if (index > 0)
                    {
                        str = str.Substring(0, index - 1);
                        index--;
                        Console.SetCursorPosition(pos.x + index, pos.y);
                        Console.Write(" ");
                    }
                }
                else if (index < numberOfDigits)
                {
                    if (char.IsDigit(getKey.KeyChar))
                    {
                        str += getKey.KeyChar;
                        index++;
                    }
                }

                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(str);
                getKey = Console.ReadKey(intercept: true);
            }
            if (getKey.Key == ConsoleKey.Enter)
            {
                result = str;
            }
            return result;
        }
        public string GetInput(int numberOfCharacters)
        {
            string result = default!;
            string str = "";
            int index = 0;
            CursorVisible = true;
            pos = Console.GetCursorPosition();
            ConsoleKeyInfo getKey = Console.ReadKey(intercept: true);
            while (getKey.Key != ConsoleKey.Enter)
            {
                if (getKey.Key == ConsoleKey.Backspace)
                {
                    if (index > 0)
                    {
                        str = str.Substring(0, index - 1);
                        index--;
                        Console.SetCursorPosition(pos.x + index, pos.y);
                        Console.Write(" ");
                    }
                }
                else if (index < numberOfCharacters)
                {
                    if (!char.IsControl(getKey.KeyChar))
                    {
                        str += getKey.KeyChar;
                        index++;
                    }
                }
                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(str);
                getKey = Console.ReadKey(intercept: true);
            }
            if (getKey.Key == ConsoleKey.Enter)
            {
                result = str;
                CursorVisible = false;
            }
            return result;
        }
        public string GetRegistrationNumber()
        {
            string result = default!;
            string str = "";
            int index = 0;

            pos = Console.GetCursorPosition();
            ConsoleKeyInfo getKey = Console.ReadKey(intercept: true);
            while (getKey.Key != ConsoleKey.Enter)
            {
                //int length = res.Length;
                if (getKey.Key == ConsoleKey.Backspace)
                {
                    if (index > 0)
                    {
                        str = str.Substring(0, index - 1);
                        index--;
                        Console.SetCursorPosition(pos.x + index, pos.y);
                        Console.Write(" ");
                    }
                }
                else if (index < 6)
                {
                    if (index < 3 && getKey.KeyChar >= 'a' && getKey.KeyChar <= 'z' ||
                        index < 3 && getKey.KeyChar >= 'A' && getKey.KeyChar <= 'Z')
                    {
                        str += getKey.KeyChar;
                        index++;
                    }
                    if
                        (index > 2 && getKey.KeyChar >= '0' && getKey.KeyChar <= '9')
                    {
                        str += getKey.KeyChar;
                        index++;
                    }
                }

                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(str);
                getKey = Console.ReadKey(intercept: true);
            }

            if (getKey.Key == ConsoleKey.Enter)
            {
                result = str;
            }

            return result;
        }
        public string GetFileName(int numberOfCharacters)
        {
            string result = default!;
            string str = "";
            int index = 0;

            pos = Console.GetCursorPosition();
            ConsoleKeyInfo getKey = Console.ReadKey(intercept: true);
            while (getKey.Key != ConsoleKey.Enter && getKey.Key != ConsoleKey.Escape)
            {
                //int length = res.Length;
                if (getKey.Key == ConsoleKey.Backspace)
                {
                    if (index > 0)
                    {
                        str = str.Substring(0, index - 1);
                        index--;
                        Console.SetCursorPosition(pos.x + index, pos.y);
                        Console.Write(" ");
                    }
                }
                else if (index < numberOfCharacters)
                {
                    if (getKey.KeyChar >= 'a' && getKey.KeyChar <= 'z' ||
                        getKey.KeyChar >= 'A' && getKey.KeyChar <= 'Z' ||
                        getKey.KeyChar >= '0' && getKey.KeyChar <= '9' ||
                        getKey.KeyChar == '_' || getKey.KeyChar == '-' || 
                        getKey.KeyChar == '.')
                    
                    {
                        str += getKey.KeyChar;
                        index++;
                    }
                }

                Console.SetCursorPosition(pos.x, pos.y);
                Console.Write(str);
                getKey = Console.ReadKey(intercept: true);
            }

            if (getKey.Key == ConsoleKey.Enter)
            {
                result = str;
            }

            return result;
        }
        public ConsoleKey GetKey() => Console.ReadKey(intercept: true).Key;
    }
}


