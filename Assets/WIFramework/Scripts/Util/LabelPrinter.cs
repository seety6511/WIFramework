using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace WIFramework.Util
{
    public enum LabelGroup
    {
        Place,
        Sensor,
        SensorPart,
    }

    public struct LabelData
    {
        public LabelGroup group;
        public string personalName;

        public LabelGroup rootGroup;
        public string rootName;
    }

    public class LabelPrinter : MonoBehaviour
    {
        [SerializeField]
        public SDictionary<LabelGroup, List<Label>> labelGroupTable = new SDictionary<LabelGroup, List<Label>>();
        public void LabelingObjects()
        {
            var labels = FindObjectsOfType<Label>();
            
            int groupCount = typeof(LabelGroup).GetEnumNames().Length;
            labelGroupTable.Clear();
            for (int i = 0; i < groupCount; ++i)
            {
                labelGroupTable.Add((LabelGroup)i, new List<Label>());
            }

            foreach(var l in labels)
            {
                var labelGroup = l.group;
                List<Label> currentList = labelGroupTable[labelGroup];
                if (currentList.Contains(l))
                {
                    continue;
                }
                l.index = currentList.Count;
                currentList.Add(l);
            }
        }

        public void FindPreLabelObjects()
        {
            var labelingObjects = FindObjectsOfType<GameObject>();

            List<string> preLabelNameList = new List<string>();
            Dictionary<GameObject,string> prelabelGroupTable = new Dictionary<GameObject, string>();
            foreach (var preLabel in labelingObjects)
            {
                var preName = preLabel.name.Split(':');
                if (preName.Length < 3)
                    continue;
                
                preLabelNameList.Add(preName[1]);
                prelabelGroupTable.Add(preLabel, preName[1]);
                preLabel.TryAddComponent<Label>().personalName = preName[2];
            }

            WriteLabelGroup(preLabelNameList);
            foreach(var pair in prelabelGroupTable)
            {
                var obj = pair.Key;
                var groupName = pair.Value;

                var label = obj.GetComponent<Label>();
                
            }
        }

        public string labelGroupFilePath;
        void WriteLabelGroup(List<string> labelGroupNames)
        {
            if (string.IsNullOrEmpty(labelGroupFilePath))
            {
                var printerPath = GetPrinterPath(typeof(LabelPrinter));
                var printerFileLength = printerPath.Split('\\').Last().Length;
                var newPath = printerPath.Substring(0, printerPath.Length - printerFileLength);
                newPath += $"LabelGroup.cs";
                labelGroupFilePath = newPath;
            }
            using (StreamWriter sw = new StreamWriter(labelGroupFilePath))
            {
                sw.WriteLine("public enum LabelGroup");
                sw.WriteLine("{");
                for(int i = 0; i < labelGroupNames.Count; ++i)
                {
                    sw.WriteLine($"\t_{labelGroupNames[i]},");
                }
                sw.WriteLine("}");
            }
        }

        string GetPrinterPath(Type t, [CallerFilePath]string file="")
        {
            return file;
        }
        public void ExtractLabel()
        {
        }
    }
}

