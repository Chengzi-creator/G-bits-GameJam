using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DataTableEditor
{
    /// <summary>
    /// 实体与界面代码生成器
    /// </summary>
    public class DataTableEditingCodeGenWindow : EditorWindow
    {
        public static DataTableEditingCodeGenWindow Instance { get; private set; }


        #region Id

        private string IdCodeGenPath;
        private string IdCodeGenNamespace;
        private string IdCodeName;

        private int IdentifierRowIndex;
        private int[] IdentifierPopUpIndices;
        private string[] IdentifierPopUpNames;

        private int CommentRowIndex = -1;
        private int[] CommentPopUpIndices;
        private string[] CommentPopUpNames;

        #endregion

        #region DataTable

        private string DRCodeGenPath;
        private string DRCodeGenNamespace;
        private string DRCodeName;

        #endregion

        public string FileName;
        public List<DataTableRowData> RowDatas;

        
        public static void OpenCodeGeneratorWindow()
        {
            DataTableEditingCodeGenWindow window =
                GetWindowWithRect<DataTableEditingCodeGenWindow>(new Rect(500, 500, 600, 300), true);
            window.minSize = new Vector2(500, 100);
            window.titleContent = new GUIContent("数据表代码生成");
            Instance = window;
        }

        private void OnEnable()
        {
            IdCodeGenPath =
                EditorPrefs.GetString(Application.productName + "_DataTableIdCodeGenPath", Application.dataPath);
            IdCodeGenNamespace =
                EditorPrefs.GetString(Application.productName + "_DataTableIdCodeGenNamespace", "GameMain");

            DRCodeGenPath =
                EditorPrefs.GetString(Application.productName + "_DataTableDRCodeGenPath", Application.dataPath);
            DRCodeGenNamespace =
                EditorPrefs.GetString(Application.productName + "_DataTableDRCodeGenNamespace", "GameMain");
        }

        private void OnGUI()
        {
            Init();
            DrawIdGenerate();
            EditorGUILayout.LabelField("");
            DrawDRGenerate();
        }

        private void Init()
        {
            if (string.IsNullOrEmpty(IdCodeName))
            {
                IdCodeName = FileName + "Id";
            }

            if (string.IsNullOrEmpty(DRCodeName))
            {
                DRCodeName = "DR" + FileName;
            }

            if (IdentifierPopUpIndices is null)
            {
                IdentifierPopUpIndices = new int[RowDatas[0].Data.Count];
                IdentifierPopUpNames = new string[RowDatas[0].Data.Count];

                for (int i = 0; i < IdentifierPopUpIndices.Length; i++)
                {
                    IdentifierPopUpIndices[i] = i;
                    IdentifierPopUpNames[i] = $"{i}:{RowDatas[3].Data[i]}";
                }
            }

            if (CommentPopUpIndices is null)
            {
                CommentPopUpIndices = new int[RowDatas[0].Data.Count + 1];
                CommentPopUpIndices[0] = -1;
                CommentPopUpNames = new string[RowDatas[0].Data.Count + 1];
                CommentPopUpNames[0] = "无注释";

                for (int i = 0; i < RowDatas[0].Data.Count; i++)
                {
                    CommentPopUpIndices[i + 1] = i;
                    CommentPopUpNames[i + 1] = $"{i}:{RowDatas[3].Data[i]}";
                }
            }

            CommentRowIndex = Mathf.Min(2, CommentPopUpIndices.Length-1);
            IdentifierRowIndex = Mathf.Min(3, IdentifierPopUpIndices.Length - 1);
        }

        private void DrawIdGenerate()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("生成Id代码路径：", GUILayout.Width(100));


            EditorGUILayout.LabelField(IdCodeGenPath);
            if (GUILayout.Button("选择", GUILayout.Width(60)))
            {
                ChooseIdCodeGenAddress();
            }

            EditorGUILayout.EndHorizontal();

            //命名空间
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("命名空间：", GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();
            IdCodeGenNamespace = EditorGUILayout.TextField(IdCodeGenNamespace);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(Application.productName + "_DataTableIdCodeGenNamespace",
                    IdCodeGenNamespace);
            }

            EditorGUILayout.EndHorizontal();
            //文件名称
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("文件名称：", GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();
            IdCodeName = EditorGUILayout.TextField(IdCodeName);
            EditorGUILayout.EndHorizontal();

            //Identifier列数
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("标识符列数：", GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();
            IdentifierRowIndex =
                EditorGUILayout.IntPopup(IdentifierRowIndex, IdentifierPopUpNames, IdentifierPopUpIndices);
            EditorGUILayout.EndHorizontal();

            //注释列数
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("注释列数：", GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();
            CommentRowIndex =
                EditorGUILayout.IntPopup(CommentRowIndex, CommentPopUpNames, CommentPopUpIndices);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("生成Id代码", GUILayout.Width(400), GUILayout.Height(40), GUILayout.ExpandWidth(true)))
            {
                GenerateIdCode(IdCodeGenPath, IdCodeGenNamespace, IdCodeName, RowDatas);
            }
        }

        private void DrawDRGenerate()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("生成DR代码路径：", GUILayout.Width(100));


            EditorGUILayout.LabelField(DRCodeGenPath);
            if (GUILayout.Button("选择", GUILayout.Width(60)))
            {
                ChooseDRCodeGenAddress();
            }

            EditorGUILayout.EndHorizontal();

            //命名空间
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("命名空间：", GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();
            DRCodeGenNamespace = EditorGUILayout.TextField(DRCodeGenNamespace);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString(Application.productName + "_DataTableDRCodeGenNamespace",
                    DRCodeGenNamespace);
            }

            EditorGUILayout.EndHorizontal();
            //文件名称
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("文件名称：", GUILayout.Width(100));
            EditorGUI.BeginChangeCheck();
            DRCodeName = EditorGUILayout.TextField(DRCodeName);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("生成DR代码", GUILayout.Width(400), GUILayout.Height(40), GUILayout.ExpandWidth(true)))
            {
                GenerateDRCode(DRCodeGenPath, DRCodeGenNamespace, DRCodeName, RowDatas);
            }
        }


        private void ChooseIdCodeGenAddress()
        {
            string filePath = EditorUtility.OpenFolderPanel("选择DataTableId代码生成位置", IdCodeGenPath, "");
            if (filePath.Contains("Assets/"))
            {
                int index = filePath.IndexOf("Assets/", StringComparison.Ordinal);
                filePath = filePath.Substring(index);
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                EditorPrefs.SetString(Application.productName + "_DataTableIdCodeGenPath", filePath);
                IdCodeGenPath = filePath;
            }
        }

        private void ChooseDRCodeGenAddress()
        {
            string filePath = EditorUtility.OpenFolderPanel("选择DataTableDR代码生成位置", DRCodeGenPath, "");
            if (filePath.Contains("Assets/"))
            {
                int index = filePath.IndexOf("Assets/", StringComparison.Ordinal);
                filePath = filePath.Substring(index);
            }

            if (!string.IsNullOrEmpty(filePath))
            {
                EditorPrefs.SetString(Application.productName + "_DataTableDRCodeGenPath", filePath);
                DRCodeGenPath = filePath;
            }
        }

        #region 代码生成

        private void GenerateIdCode(string codePath, string nameSpace, string codeName, List<DataTableRowData> rowDatas)
        {
            if (!Directory.Exists($"{codePath}"))
            {
                Directory.CreateDirectory($"{codePath}");
            }

            string scriptPath = $"{codePath}/{codeName}.cs";
            using (StreamWriter sw = new StreamWriter(scriptPath))
            {
                sw.WriteLine("//------------------------------------------------------------");
                sw.WriteLine("// 此文件由工具自动生成，请勿直接修改。");
                sw.WriteLine("// 生成时间：" + DateTime.Now);
                sw.WriteLine("//------------------------------------------------------------");
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                //类名
                sw.WriteLine($"\tpublic static class {codeName}");
                sw.WriteLine("\t{");
                for (int i = 0; i < rowDatas.Count; i++)
                {
                    if (rowDatas[i].Data[0].StartsWith("#")) continue;
                    if (CommentRowIndex >= 0)
                    {
                        sw.WriteLine("\t\t/// <summary>");
                        sw.WriteLine($"\t\t/// {rowDatas[i].Data[CommentRowIndex]}");
                        sw.WriteLine("\t\t/// </summary>");
                    }

                    sw.WriteLine(
                        $"\t\tpublic const int {rowDatas[i].Data[IdentifierRowIndex]} = {rowDatas[i].Data[1]};");
                    sw.WriteLine("");
                }

                sw.WriteLine("\t}");

                sw.WriteLine("}");
            }
        }

        private void GenerateDRCode(string codePath, string nameSpace, string codeName, List<DataTableRowData> rowDatas)
        {
            if (!Directory.Exists($"{codePath}"))
            {
                Directory.CreateDirectory($"{codePath}");
            }

            string scriptPath = $"{codePath}/{codeName}.cs";
            using (StreamWriter sw = new StreamWriter(scriptPath))
            {
                HashSet<string> usings = new()
                {
                    "UnityGameFramework.Runtime",
                    "System.IO",
                    "GameFramework",
                    "System",
                    "System.Collections.Generic",
                    "System.Text",
                    "UnityEngine"
                };

                sw.WriteLine("//------------------------------------------------------------");
                sw.WriteLine("// 此文件由工具自动生成，请勿直接修改。");
                sw.WriteLine("// 生成时间：" + DateTime.Now);
                sw.WriteLine("//------------------------------------------------------------");

                foreach (var u in usings)
                {
                    sw.WriteLine($"using {u};");
                }

                sw.WriteLine();
                
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                //类名
                GenerateSummary(sw, 1, rowDatas[0].Data[1]);
                sw.WriteLine($"\tpublic class {codeName} : DataRowBase");
                sw.WriteLine("\t{");
                sw.WriteLine("\t\t private int m_Id = 0;");
                sw.WriteLine();
                //gen props
                for (int i = 1; i < rowDatas[0].Data.Count; i++)
                {
                    string propName = rowDatas[1].Data[i];
                    string propType = rowDatas[2].Data[i];
                    string propComment = rowDatas[3].Data[i];
                    
                    if(string.IsNullOrEmpty(propType)) continue;
                    
                    if (propName == "Id" || propName == "ID")
                    {
                        GenerateSummary(sw,2,"获取编号");
                        sw.WriteLine($"\t\tpublic override {propType} {propName}");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\tget");
                        sw.WriteLine("\t\t\t{");
                        sw.WriteLine("\t\t\t\treturn m_Id;");
                        sw.WriteLine("\t\t\t}");
                        sw.WriteLine("\t\t}");
                    }
                    else
                    {
                        if(!string.IsNullOrEmpty(propComment))
                            GenerateSummary(sw,2,$"获取{propComment}");
                        sw.WriteLine($"\t\tpublic {propType} {propName}");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\tget;");
                        sw.WriteLine("\t\t\tprivate set;");
                        sw.WriteLine("\t\t}");
                    }
                }
                //gen parse func
                sw.WriteLine("\t\tpublic override bool ParseDataRow(string dataRowString, object userData)");
                sw.WriteLine("\t\t{");
                
                sw.WriteLine("\t\t\tstring[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);\n" +
                             "\t\t\tfor (int i = 0; i < columnStrings.Length; i++)\n" +
                             "\t\t\t{\n" +
                             "\t\t\t\tcolumnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);\n" +
                             "\t\t\t}");
                sw.WriteLine();
                
                sw.WriteLine("\t\t\tint index = 0;\n" +
                             "\t\t\tindex++;\n" +
                             "\t\t\tm_Id = int.Parse(columnStrings[index++]);");
                for (int i = 2; i < rowDatas[0].Data.Count; i++)
                {
                    string propName = rowDatas[1].Data[i];
                    string propType = rowDatas[2].Data[i];

                    if (string.IsNullOrEmpty(propType))
                    {
                        sw.WriteLine("\t\t\tindex++;");
                        continue;
                    }
                    if(propType=="string")
                    {
                        sw.WriteLine($"\t\t\t{propName} = columnStrings[index++];");
                    }
                    else
                    {
                        sw.WriteLine($"\t\t\t{propName} = {propType}.Parse(columnStrings[index++]);");
                    }
                }
                sw.WriteLine();
                sw.WriteLine("\t\t\tGeneratePropertyArray();");
                sw.WriteLine("\t\t\treturn true;");
                
                sw.WriteLine("\t\t}");
                sw.WriteLine();
                
                sw.WriteLine("\t\tprivate void GeneratePropertyArray()");
                sw.WriteLine("\t\t{");
                sw.WriteLine();
                sw.WriteLine("\t\t}");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }

        private static void GenerateSummary(StreamWriter sw, int tab, string content)
        {
            string tabs = "";
            for (int i = 1; i <= tab; i++)
                tabs += "\t";
            sw.WriteLine($"{tabs}/// <summary>");
            sw.WriteLine($"{tabs}/// {content}");
            sw.WriteLine($"{tabs}/// </summary>");
        }

        #endregion
    }
}