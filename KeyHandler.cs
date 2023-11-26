using System.Text;

namespace Jera
{
    internal class KeyHandler
    {
        private Dictionary <string, string> jsonFile;
        public KeyHandler()
        {
            Console.WriteLine("KeyHandler initialized!");
            jsonFile = LoadJson();
        }

        public Dictionary<string, string> LoadJson()
        {
            String jsonString = File.ReadAllText("../../../config.json", Encoding.UTF8);
            var jsonFile = JSONConverter.FromJson(jsonString);
            return jsonFile;
        }

        public string getOutput(string key)
        {
            Console.WriteLine($"Key provided: {key}");
            if (jsonFile == null) return "";
            return jsonFile[key];
        }
    }
}
