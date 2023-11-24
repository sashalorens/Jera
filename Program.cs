using System.Diagnostics;

namespace Jera
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var kh = new KeyboardHook(true);
            kh.KeyDown += Kh_KeyDown;
            Application.Run();
        }

        private static void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)
        {
            Console.WriteLine("The Key: " + key);
            if (key == Keys.F) key = Keys.H;
        }
    }
}
