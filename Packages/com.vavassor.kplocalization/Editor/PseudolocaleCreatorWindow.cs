#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Data;
using System.IO;
using System.Threading;

namespace KPLocalization.Editor
{
    public class PseudolocaleCreatorWindow : EditorWindow
    {
        private bool isGenerateDone;
        private bool isProcessing;
        private string sourceJson;
        private TextAsset sourceJsonFile;
        private string targetJson;

        [MenuItem("Window/KP Localization/Pseudolocale Creator")]
        public static void Init()
        {
            PseudolocaleCreatorWindow window = GetWindow<PseudolocaleCreatorWindow>(false, "Pseudolocale Creator");
            window.Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUIUtility.labelWidth = 125f;
            EditorGUIUtility.fieldWidth = 5f;

            GUILayout.Space(10.0f);

            GUILayout.Label("A pseudolocale is a locale that's designed to help find issues with UI and layouts. It can be used during development to make adjustments early before you have real translations.\n\nThis pseudolocale works best when the source language uses latin script. If your source translation is in another script, prefer using machine translations for testing.", new GUIStyle(EditorStyles.label) { wordWrap = true });

            GUILayout.Space(10.0f);

            EditorGUI.BeginDisabledGroup(isProcessing);
            {
                sourceJsonFile = EditorGUILayout.ObjectField("Source JSON File", sourceJsonFile, typeof(TextAsset), false) as TextAsset;
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10.0f);

            if (GUILayout.Button("Generate Pseudolocale Resource") && sourceJsonFile != null && !isProcessing)
            {
                isProcessing = true;

                sourceJson = sourceJsonFile.text;

                ThreadPool.QueueUserWorkItem(GenerateResource =>
                {
                    isGenerateDone = false;

                    if (VRCJson.TryDeserializeFromJson(sourceJson, out DataToken sourceToken))
                    {
                        var pseudolocaleToken = GenerateToken(sourceToken);

                        if (VRCJson.TrySerializeToJson(pseudolocaleToken, JsonExportType.Beautify, out DataToken targetToken))
                        {
                            targetJson = targetToken.String;
                        }
                    }

                    isGenerateDone = true;

                    return;
                });
            }

            EditorGUILayout.EndVertical();
        }

        public void Update()
        {
            if (isGenerateDone)
            {
                SaveFile();

                isGenerateDone = false;
                isProcessing = false;
            }
        }

        private void SaveFile()
        {
            var assetPath = AssetDatabase.GetAssetPath(sourceJsonFile);
            var targetPath = Path.Combine(Path.GetDirectoryName(assetPath), "xa.asset");

            var targetJsonFile = new TextAsset(targetJson);
            AssetDatabase.CreateAsset(targetJsonFile, targetPath);
            AssetDatabase.SaveAssets();
        }

        private DataToken GenerateToken(DataToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.DataDictionary:
                {
                    var dictionary = token.DataDictionary;
                    var keys = dictionary.GetKeys();
                    var valueTokens = dictionary.GetValues();

                    var resultDictionary = new DataDictionary();

                    for (var i = 0; i < keys.Count; i++)
                    {
                        resultDictionary.Add(keys[i], GenerateToken(valueTokens[i]));
                    }

                    return resultDictionary;
                }
                case TokenType.String:
                    return TranslateString(token.String);
                default:
                    return token;
            }
        }

        private string TranslateString(string value)
        {
            var translation = "";

            for (var i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '{':
                        if (i < value.Length - 2 && value[i + 1] == '{')
                        {
                            int codeStart = i + 2;
                            int codeEnd = value.IndexOf("}}", codeStart);

                            if (codeEnd == -1)
                            {
                                Debug.LogError("Failed to get value. Invalid interpolation code.");
                                return value;
                            }

                            translation += value.Substring(i, codeEnd + 2 - i);
                            i = codeEnd + 1;
                        }
                        break;
                    default:
                        translation += TranslateChar(value[i]);
                        break;
                }
            }

            return "[!!! " + translation + " !!!]";
        }

        private char TranslateChar(char value)
        {
            switch (value)
            {
                case 'a':
                    return 'á';
                case 'b':
                    return 'β';
                case 'c':
                    return 'ç';
                case 'd':
                    return 'δ';
                case 'e':
                    return 'è';
                case 'f':
                    return 'ƒ';
                case 'g':
                    return 'ϱ';
                case 'h':
                    return 'λ';
                case 'i':
                    return 'ï';
                case 'j':
                    return 'J';
                case 'k':
                    return 'ƙ';
                case 'l':
                    return 'ℓ';
                case 'm':
                    return '₥';
                case 'n':
                    return 'ñ';
                case 'o':
                    return 'ô';
                case 'p':
                    return 'ƥ';
                case 'q':
                    return '9';
                case 'r':
                    return 'ř';
                case 's':
                    return 'ƨ';
                case 't':
                    return 'ƭ';
                case 'u':
                    return 'ú';
                case 'v':
                    return 'Ʋ';
                case 'w':
                    return 'ω';
                case 'x':
                    return 'ж';
                case 'y':
                    return '¥';
                case 'z':
                    return 'ƺ';
                case 'A':
                    return 'Â';
                case 'B':
                    return 'ß';
                case 'C':
                    return 'Ç';
                case 'D':
                    return 'Ð';
                case 'E':
                    return 'É';
                case 'F':
                    return 'F';
                case 'G':
                    return 'G';
                case 'H':
                    return 'H';
                case 'I':
                    return 'Ì';
                case 'J':
                    return 'J';
                case 'K':
                    return 'K';
                case 'L':
                    return '£';
                case 'M':
                    return 'M';
                case 'N':
                    return 'N';
                case 'O':
                    return 'Ó';
                case 'P':
                    return 'Þ';
                case 'Q':
                    return 'Q';
                case 'R':
                    return 'R';
                case 'S':
                    return '§';
                case 'T':
                    return 'T';
                case 'U':
                    return 'Û';
                case 'V':
                    return 'V';
                case 'W':
                    return 'W';
                case 'X':
                    return 'X';
                case 'Y':
                    return 'Ý';
                case 'Z':
                    return 'Z';
                default:
                    return value;
            }
        }
    }
}
#endif // !COMPILER_UDONSHARP && UNITY_EDITOR
