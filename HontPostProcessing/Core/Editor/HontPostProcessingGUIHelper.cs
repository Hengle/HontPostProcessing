using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hont.PostProcessing
{
    public static class HontPostProcessingGUIHelper
    {
        public static void Header(string title, bool isEnabled, out bool headerIsClicked, out bool enabledButtonClicked)
        {
            headerIsClicked = false;
            enabledButtonClicked = false;

            var rect = GUILayoutUtility.GetRect(16f, 22f, HontPostProcessingGUIStyle.header);
            GUI.Box(rect, title, HontPostProcessingGUIStyle.header);

            var toggleRect = new Rect(rect.x + 4f, rect.y + 4f, 13f, 13f);
            var e = Event.current;

            if (e.type == EventType.Repaint)
                HontPostProcessingGUIStyle.headerCheckbox.Draw(toggleRect, false, false, isEnabled, false);

            if (e.type == EventType.MouseDown)
            {
                var flag = false;

                if (toggleRect.Contains(e.mousePosition))
                {
                    enabledButtonClicked = !enabledButtonClicked;
                    flag = true;
                }

                else if (rect.Contains(e.mousePosition))
                {
                    headerIsClicked = !headerIsClicked;
                    flag = true;
                }

                if (flag)
                    e.Use();
            }
        }
    }
}
