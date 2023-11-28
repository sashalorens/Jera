using System.Collections.Generic;
using System.Text;
using Jera.helpers;

namespace Jera
{
    internal class KeyHandler
    {
        private Dictionary<string, Dictionary<string, string>> jsonFile;
        Sequence sequence;
        InputConstructor inputConstructor;
        string lastSimpleChar = "";
        bool isLastCharWasCombined = false;
        long prevTimestamp = 0;

        public KeyHandler()
        {
            Console.WriteLine("KeyHandler initialized!");
            jsonFile = LoadJson();
            sequence = new Sequence();
            inputConstructor = new InputConstructor();
        }

        public Dictionary<string, Dictionary<string, string>> LoadJson()
        {
            String jsonString = File.ReadAllText("../../../config.json", Encoding.UTF8);
            var jsonFile = JSONConverter.FromJson(jsonString);
            return jsonFile;
        }

        UInt16 GetValue(string key, bool isShiftPressed)
        {
            string str = isShiftPressed ? key.ToUpper() : key;
            char[] chars = key.ToCharArray();
            UInt16 unicode = chars[0];
            return unicode;
        }

        public string getOutput(string key, bool isShiftPressed)
        {
            if (jsonFile == null || !jsonFile["keys"].ContainsKey(key)) return "";
            string value = jsonFile["keys"][key];
            string result = isShiftPressed ? value.ToUpper() : value;
            return result;
        }

        public Input[] getOutput2(string key, bool isShiftPressed, List<Int32> seq)
        {
            if (jsonFile == null || !jsonFile["keys"].ContainsKey(key)) return [];
            string value = jsonFile["keys"][key];
            string combinedValue = "";
            List<string> maybeCombined = new List<string>();

            UInt16 regularUnicode = GetValue(value, isShiftPressed);

            string temp = "";

            for (var i = seq.Count - 1; i >= 0; i--)
            {
                if (temp != "")
                {
                    temp = seq[i] + "&" + temp;
                    maybeCombined.Add(temp);
                }
                else
                {
                    temp = seq[i].ToString();
                }
            }

            foreach(string combination in maybeCombined)
            {
                if (jsonFile["keys"].ContainsKey(combination)) {
                    combinedValue = jsonFile["keys"][combination];
                }
            }

            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            long diff = timestamp - prevTimestamp;

            if (combinedValue != "" && prevTimestamp != 0 && diff < 555)
            {
                UInt16 combinedUnicode = GetValue(combinedValue, isShiftPressed);
                isLastCharWasCombined = true;
                prevTimestamp = timestamp;
                return inputConstructor.GetInputs(combinedUnicode, true);
            }
            prevTimestamp = timestamp;
            return inputConstructor.GetInputs(regularUnicode);
        }
    }
}
