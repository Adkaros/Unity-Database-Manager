using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace M1.Utilities
{
    [XmlRoot("Root")]
    public class Container
    {
        [XmlArray("Container"), XmlArrayItem("Entry")]
        public List<ContainerData> data = new List<ContainerData>();

        public static string ClientAssetPath(string name)
        {
#if UNITY_EDITOR
            return Application.dataPath + "/ClientAssets/" + name;
#else
        return Application.dataPath + "/../ClientAssets/" + name;
#endif
        }

        public static Container Load(string name)
        {
            var serializer = new XmlSerializer(typeof(Container));
            using (var stream = new StreamReader(ClientAssetPath(name)))
            {
                return serializer.Deserialize(stream) as Container;
            }
        }
    }
}