using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SolvupSDK
{
    public class Response
    {        
        public static dynamic GetResponse(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            XDocument doc = XDocument.Parse(source);
            string jsonText = JsonConvert.SerializeXNode(doc);
            return JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
        }
    }

}
