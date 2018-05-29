using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hont.PostProcessing
{
    public static class HontPostProcessingGUIStyle
    {
        #region --- GUI Style ---
        public readonly static GUIStyle tickStyleRight;
        public readonly static GUIStyle tickStyleLeft;
        public readonly static GUIStyle tickStyleCenter;

        public readonly static GUIStyle preSlider;
        public readonly static GUIStyle preSliderThumb;
        public readonly static GUIStyle preButton;
        public readonly static GUIStyle preDropdown;

        public readonly static GUIStyle preLabel;
        public readonly static GUIStyle hueCenterCursor;
        public readonly static GUIStyle hueRangeCursor;

        public readonly static GUIStyle centeredBoldLabel;
        public readonly static GUIStyle wheelThumb;
        public readonly static Vector2 wheelThumbSize;

        public readonly static GUIStyle header;
        public readonly static GUIStyle headerCheckbox;
        public readonly static GUIStyle headerFoldout;

        public readonly static Texture2D playIcon;
        public readonly static Texture2D checkerIcon;
        public readonly static Texture2D paneOptionsIcon;

        public static GUIStyle centeredMiniLabel;


        static HontPostProcessingGUIStyle()
        {
            tickStyleRight = new GUIStyle("Label")
            {
                alignment = TextAnchor.MiddleRight,
                fontSize = 9
            };

            tickStyleLeft = new GUIStyle("Label")
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 9
            };

            tickStyleCenter = new GUIStyle("Label")
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 9
            };

            preSlider = new GUIStyle("PreSlider");
            preSliderThumb = new GUIStyle("PreSliderThumb");
            preButton = new GUIStyle("PreButton");
            preDropdown = new GUIStyle("preDropdown");

            preLabel = new GUIStyle("ShurikenLabel");

            hueCenterCursor = new GUIStyle("ColorPicker2DThumb")
            {
                normal = { background = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/DarkSkin/Images/ShurikenPlus.png") },
                fixedWidth = 6,
                fixedHeight = 6
            };

            hueRangeCursor = new GUIStyle(hueCenterCursor)
            {
                normal = { background = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/DarkSkin/Images/CircularToggle_ON.png") }
            };

            wheelThumb = new GUIStyle("ColorPicker2DThumb");

            centeredBoldLabel = new GUIStyle(GUI.skin.GetStyle("Label"))
            {
                alignment = TextAnchor.UpperCenter,
                fontStyle = FontStyle.Bold
            };

            centeredMiniLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.UpperCenter
            };

            wheelThumbSize = new Vector2(
                    !Mathf.Approximately(wheelThumb.fixedWidth, 0f) ? wheelThumb.fixedWidth : wheelThumb.padding.horizontal,
                    !Mathf.Approximately(wheelThumb.fixedHeight, 0f) ? wheelThumb.fixedHeight : wheelThumb.padding.vertical
                    );

            header = new GUIStyle("ShurikenModuleTitle")
            {
                font = (new GUIStyle("Label")).font,
                border = new RectOffset(15, 7, 4, 4),
                fixedHeight = 22,
                contentOffset = new Vector2(20f, -2f)
            };

            headerCheckbox = new GUIStyle("ShurikenCheckMark");
            headerFoldout = new GUIStyle("Foldout");

            playIcon = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/DarkSkin/Images/IN foldout act.png");
            checkerIcon = (Texture2D)EditorGUIUtility.LoadRequired("Icons/CheckerFloor.png");

            if (EditorGUIUtility.isProSkin)
                paneOptionsIcon = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/DarkSkin/Images/pane options.png");
            else
                paneOptionsIcon = (Texture2D)EditorGUIUtility.LoadRequired("Builtin Skins/LightSkin/Images/pane options.png");
        }
        #endregion
    }
}
