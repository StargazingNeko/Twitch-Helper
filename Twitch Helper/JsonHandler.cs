using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


public class JsonHandler
{
    public JObject Read(string json, bool isFile)
    {
        if(!isFile)
        {
            var _json = JObject.Parse(json);
            return _json;
        }
        else
        {
            using (StreamReader jsonFile = new StreamReader(json))
            using (JsonReader reader = new JsonTextReader(jsonFile))
            {
                var _json = JObject.Parse(jsonFile.ReadToEnd());
                return _json;
            }
        }
    }

    public void Write(dynamic param1, dynamic param2, string path)
    {
        if(!File.Exists(path))
        {
            File.WriteAllText(path,"{}");
        }

        var json = JObject.Parse(File.ReadAllText(path));
        json[$"{param1}"] = param2;
        File.WriteAllText(path, JsonConvert.SerializeObject(json, Formatting.Indented));
    }
}
