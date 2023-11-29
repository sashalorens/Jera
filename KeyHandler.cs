using System.Collections.Generic;
using System.Text;
using Jera.helpers;
using Newtonsoft.Json.Linq;

namespace Jera
{
    internal class KeyHandler
    {
        private JObject? jsonFile;
        Sequence sequence;
        InputConstructor inputConstructor;
        string lastCombination = string.Empty;
        long prevTimestamp = 0;
        const string BACKSPACE_CODE = "8";

        public KeyHandler()
        {
            Console.WriteLine("KeyHandler initialized!");
            jsonFile = LoadJson();
            sequence = new Sequence();
            inputConstructor = new InputConstructor();
        }

        public JObject? LoadJson()
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

        int ParseDelay(string value) {
            int delay = 0;
            try
            {
                delay = Int32.Parse(value);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse delay '{value}'");
            }
            return delay;
        }

        public Input[] GetOutput(string key, bool isShiftPressed, List<Int32> seq)
        {
            Dictionary<string, string> keys = jsonFile!["keys"]!.ToObject<Dictionary<string, string>>()!; ;
            if (jsonFile == null || keys == null || !keys.ContainsKey(key)) return [];
            string value = keys[key];
            int delayTime = ParseDelay(jsonFile["delayTime"]!.ToString());
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

            maybeCombined = maybeCombined.OrderByDescending(x => x.Length).ToList();

            string maybeLastCombination = string.Empty;

            foreach (string combination in maybeCombined)
            {
                // handling triple combinations where first two chars are an another combination
                string tempCombination = combination;

                if (lastCombination != string.Empty && tempCombination.Contains(lastCombination))
                {
                    string[] parts = tempCombination.Split('&').Where(part => part != BACKSPACE_CODE).ToArray();
                    tempCombination = string.Join("&", parts);
                }

                if (keys.ContainsKey(tempCombination)) {
                    combinedValue = keys[tempCombination];
                    maybeLastCombination = tempCombination;
                }
            }

            long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            long diff = timestamp - prevTimestamp;

            if (combinedValue != "" && prevTimestamp != 0 && diff < delayTime)
            {
                UInt16 combinedUnicode = GetValue(combinedValue, isShiftPressed);
                lastCombination = maybeLastCombination;
                prevTimestamp = timestamp;
                return inputConstructor.GetInputs(combinedUnicode, true);
            }
            prevTimestamp = timestamp;
            return inputConstructor.GetInputs(regularUnicode);
        }
    }
}
