using Jera.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Jera
{
    internal class InputConstructor
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();
        public InputConstructor() { }

        public Input[] GetInputs(UInt16 wScan, bool shouldUseBackspace = false)
        {
            int type = (int)InputType.Keyboard;

            Input[] backspaceInputs = [
                new Input
                {
                    type = type,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 8,
                            wScan = 0,
                            dwFlags = (uint)(KeyEventF.KeyDown),
                            dwExtraInfo = GetMessageExtraInfo(),
                        }

                    }
                },
                new Input
                {
                    type = type,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 8,
                            wScan = 0,
                            dwFlags = (uint)(KeyEventF.KeyUp),
                            dwExtraInfo = GetMessageExtraInfo(),
                        }

                    }
                },
            ];
            Input[] inputs =
            [
                new Input
                {
                    type = (int)InputType.Keyboard,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 0,
                            wScan = wScan,
                            dwFlags = (uint)(KeyEventF.KeyDown | KeyEventF.Unicode),
                            dwExtraInfo = GetMessageExtraInfo(),
                        }
                    }
                },
                new Input
                {
                    type = (int)InputType.Keyboard,
                    u = new InputUnion
                    {
                        ki = new KeyboardInput
                        {
                            wVk = 0,
                            wScan = wScan,
                            dwFlags = (uint)(KeyEventF.KeyUp | KeyEventF.Unicode),
                            dwExtraInfo = GetMessageExtraInfo(),
                        }
                    }
                }
            ];
            if (!shouldUseBackspace) return inputs;
            Input[] combinedArray = new Input[inputs.Length + backspaceInputs.Length];
            Array.Copy(backspaceInputs, combinedArray, backspaceInputs.Length);
            Array.Copy(inputs, 0, combinedArray, inputs.Length, backspaceInputs.Length);
            return combinedArray;
        }
    }
}
