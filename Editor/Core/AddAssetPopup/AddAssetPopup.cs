using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ConfigMe.EditorCM
{
    public class AddAssetPopup<T> : PopupWindowContent where T : UnityEngine.Object
    {
        public event Action<T> OnSelect;

        private List<T> assetsToDisplay;
        public IEnumerable<T> AssetsToDisplay
        {
            set
            {
                this.assetsToDisplay = new List<T>(value ?? Array.Empty<T>());
                this.assetsToDisplay.RemoveAll(asset => asset == null);

                if (root != null)
                { 
                    RebuildList(); 
                }
            }
        }


        private VisualElement root = null;
        private ScrollView scroll = null;
        private TextField search = null;

        public AddAssetPopup(IEnumerable<T> assetsToDisplay)
        {
            AssetsToDisplay = assetsToDisplay;
        }

        public override VisualElement CreateGUI()
        {
            /* --- ROOT --- */
            root = new VisualElement();
            root.style.width = 320;
            root.style.height = 360;
            root.style.paddingLeft = 6;
            root.style.paddingRight = 6;
            root.style.paddingTop = 6;

            /* --- SEARCH FIELD --- */
            search = new TextField();
            search.RegisterValueChangedCallback(evt => RebuildList());
            root.Add(search);

            /* --- SCROLL VIEW --- */
            scroll = new ScrollView();
            scroll.style.flexGrow = 1;
            scroll.contentContainer.style.flexDirection = FlexDirection.Column;
            root.Add(scroll);

            RebuildList();

            return root;
        }

        private void RebuildList()
        {
            string filter = search.text;

            if (scroll == null)
                return;

            scroll.Clear();

            var filtered = assetsToDisplay
                .Where(asset => string.IsNullOrEmpty(filter) || asset.name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(asset => asset.name);

            foreach (var asset in filtered)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.alignItems = Align.Center;
                row.style.height = 22;
                row.style.minHeight = 22;
                row.style.marginBottom = 2;
                row.style.paddingLeft = 4;
                row.style.paddingRight = 4;
                row.style.width = Length.Percent(100);

                var assetNameLabel = new Label(asset.name);
                assetNameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
                assetNameLabel.style.unityFontStyleAndWeight = FontStyle.Normal;
                assetNameLabel.style.flexGrow = 1;
                row.Add(assetNameLabel);

                var description = GetDescription(asset);

                if (!string.IsNullOrEmpty(description))
                {
                    var desc = new Label(description) { name = "desc" };
                    desc.style.fontSize = 10;
                    desc.style.opacity = 0.7f;
                    desc.style.marginLeft = 6;
                    desc.style.alignSelf = Align.Center;
                    row.Add(desc);
                }


                row.userData = asset;

                row.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent callback) =>
                {
                    callback.menu.AppendAction("Add Parameter", (x) =>
                    {
                        OnSelect?.Invoke(row.userData as T);
                    });

                    callback.menu.AppendAction("Select Asset", (x) =>
                    {
                        EditorGUIUtility.PingObject(asset);                        
                    });
                }));

                row.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    row.style.backgroundColor = new StyleColor(new Color(0.18f, 0.18f, 0.18f, 1f));
                });

                row.RegisterCallback<MouseLeaveEvent>(evt =>
                {
                    row.style.backgroundColor = new StyleColor(Color.clear);
                });

                row.RegisterCallback<MouseDownEvent>(evt =>
                {
                    if (evt.button != 0)
                        return;

                    var item = row.userData as T;

                    if (item != null)
                    {
                        OnSelect?.Invoke(item);
                    }

                    evt.StopPropagation();
                });

                row.focusable = true;

                row.tooltip = AssetDatabase.GetAssetPath(asset);

                scroll.Add(row);
            }

            if (!filtered.Any())
            {
                var noAssetLabel = new Label("No asset available.");
                noAssetLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                noAssetLabel.style.marginTop = 8;
                scroll.Add(noAssetLabel);
            }
        }

        private string GetDescription(T p)
        {
            return "TO-DO";
        }
    }

}
