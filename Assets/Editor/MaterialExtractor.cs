using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class MaterialExtractor
{
    [MenuItem("Tools/MaterialExtract #P")]
    public static void MaterialExtract()
    {
        return;
        //Debug.Log(Selection.activeObject);
        GameObject select = Selection.activeGameObject;
        var selectMeshs = select.GetComponentsInChildren<MeshRenderer>();
        var basePath = $"{Application.dataPath}/MaterialExtract";
        var dir = $"{basePath}/{select.name}";
        if (Directory.Exists(dir)){
            Directory.CreateDirectory(dir);
            Debug.Log($"Create Directory({dir})");
        }

        foreach(var m in selectMeshs)
        {
            object material = m.sharedMaterial;
            if (material == null)
            {
                Debug.Log($"Missing={m.gameObject.name}");
                continue;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, material);
                var buffer = ms.ToArray();
                foreach (var b in buffer)
                {
                    Debug.Log(b.ToString());
                }
            }
           
            //Debug.Log(AssetDatabase.GetAssetPath(m.material));
            //Debug.Log(material.name);
        }
    }

}
