using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using datuloar.documentation.Config;
using PlasticPipe.Server;

namespace datuloar.documentation.Editor
{
    public class DocumentationEditorWindow : EditorWindow
    {
        private const string Documentation = nameof(Documentation);

        private Vector2 _scrollPosition;
        private static DocumentationEditorWindowConfig _config;
        private static Dictionary<DocumentationCatalogType, List<Type>> _cachedMarkedClasses = new();
        private static DocumentationCatalogType _selectedDocumenationCatalogType;
        private static List<Type> _filteredClasses;

        [MenuItem("Tools/Documentation Viewer")]
        public static void ShowWindow()
        {
            _config = Resources.Load<DocumentationEditorWindowConfig>("DocumentationEditorWindowConfig");

            var window = GetWindow<DocumentationEditorWindow>(Documentation);
            window.minSize = _config.WindowMinSize;
            window.maxSize = _config.WindowMaxSize;

            if (_cachedMarkedClasses.Count == 0)
                UpdateMarkedClasses();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            DrawCatalogButtons();
            DrawDocumentationColumn();
            EditorGUILayout.EndHorizontal();

            DrawSearchButton();
        }

        private void DrawCatalogButtons()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.2f));
            GUILayout.Label("Select Catalog:", EditorStyles.boldLabel);

            foreach (DocumentationCatalogType catalog in Enum.GetValues(typeof(DocumentationCatalogType)))
            {
                GUIStyle buttonStyle = catalog == _selectedDocumenationCatalogType ? EditorStyles.miniButtonMid : EditorStyles.miniButton;
                Color defaultTextColor = GUI.backgroundColor;

                if (catalog == _selectedDocumenationCatalogType)
                {
                    GUI.backgroundColor = _config.ActiveCatalogButtonColor;
                }
                if (GUILayout.Button(catalog.ToString(), buttonStyle))
                {
                    _selectedDocumenationCatalogType = catalog;
                    FilterClasses();
                }

                GUI.backgroundColor = defaultTextColor;
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawDocumentationColumn()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width * 0.8f));
            GUILayout.Label(Documentation, EditorStyles.boldLabel);

            _filteredClasses = _cachedMarkedClasses[_selectedDocumenationCatalogType];

            if (_filteredClasses != null && _filteredClasses.Count > 0)
            {
                foreach (var markedClass in _filteredClasses)
                {
                    var attributes = markedClass.GetCustomAttributes(typeof(DocumentationAttribute), false);

                    if (attributes.Length > 0)
                    {
                        var documentationAttribute = attributes[0] as DocumentationAttribute;

                        Color previousColor = GUI.backgroundColor;

                        GUI.backgroundColor = _config.ClassButtonColor;

                        if (GUILayout.Button(markedClass.Name))
                            ShowClassInProject(markedClass);

                        GUI.backgroundColor = previousColor;

                        EditorGUI.TextArea(EditorGUILayout.GetControlRect(GUILayout.Height(_config.DocumentationTextHeight)),
                            documentationAttribute.Text);

                        EditorGUILayout.Space();
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void ShowClassInProject(Type classType)
        {
            string className = classType.Name;
            string[] allScriptGuids = AssetDatabase.FindAssets(className);

            if (allScriptGuids.Length > 0)
            {
                string classPath = AssetDatabase.GUIDToAssetPath(allScriptGuids[0]);
                UnityEngine.Object classAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(classPath);

                if (classAsset != null)
                {
                    EditorGUIUtility.PingObject(classAsset);
                    Selection.activeObject = classAsset;
                }
            }
        }

        private void DrawSearchButton()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Refresh"))
            {
                UpdateMarkedClasses();
                FilterClasses();
            }
        }

        private static void UpdateMarkedClasses()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => Attribute.IsDefined(t, typeof(DocumentationAttribute)))
                .ToList();

            foreach (DocumentationCatalogType catalog in Enum.GetValues(typeof(DocumentationCatalogType)))
            {
                _cachedMarkedClasses[catalog] = assembly.Where(t =>
                {
                    var attributes = t.GetCustomAttributes(typeof(DocumentationAttribute), false);
                    if (attributes.Length > 0)
                    {
                        var documentationAttribute = attributes[0] as DocumentationAttribute;
                        return documentationAttribute.DocType == catalog;
                    }
                    return false;
                }).ToList();
            }
        }

        private static void FilterClasses()
        {
            if (_cachedMarkedClasses.ContainsKey(_selectedDocumenationCatalogType))
                _filteredClasses = _cachedMarkedClasses[_selectedDocumenationCatalogType];
            else
                _filteredClasses = new List<Type>();
        }
    }
}