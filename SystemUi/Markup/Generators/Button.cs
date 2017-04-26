// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2017 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using LeopotamGroup.Common;
using LeopotamGroup.Math;
using LeopotamGroup.Serialization;
using LeopotamGroup.SystemUi.Widgets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LeopotamGroup.SystemUi.Markup.Generators {
    static class ButtonNode {
        static readonly int HashedBlend = "blend".GetStableHashCode ();

        static readonly int HashedDisabled = "disabled".GetStableHashCode ();

        /// <summary>
        /// Create "button" node. If children supported - GameObject container for them should be returned.
        /// </summary>
        /// <param name="widget">Ui widget.</param>
        /// <param name="node">Xml node.</param>
        /// <param name="container">Markup container.</param>
        public static RectTransform Create (RectTransform widget, XmlNode node, MarkupContainer container) {
#if UNITY_EDITOR
            widget.name = "button";
#endif
            var btn = widget.gameObject.AddComponent<Button> ();
            var img = widget.gameObject.AddComponent<Image> ();
            string attrValue;
            var transition = Button.Transition.ColorTint;

            attrValue = node.GetAttribute (HashedBlend);
            switch (attrValue) {
                case "sprites":
                    transition = Button.Transition.SpriteSwap;
                    break;
                case "none":
                    transition = Button.Transition.None;
                    break;
            }

            var theme = MarkupUtils.GetTheme (node, container);

            switch (transition) {
                case Button.Transition.ColorTint:
                    var colors = btn.colors;
                    colors.normalColor = theme.GetButtonColor (MarkupTheme.ButtonState.Normal);
                    colors.pressedColor = theme.GetButtonColor (MarkupTheme.ButtonState.Pressed);
                    colors.highlightedColor = theme.GetButtonColor (MarkupTheme.ButtonState.Highlighted);
                    colors.disabledColor = theme.GetButtonColor (MarkupTheme.ButtonState.Disabled);
                    btn.colors = colors;
                    break;
                case Button.Transition.SpriteSwap:
                    var sprites = btn.spriteState;
                    sprites.pressedSprite = theme.GetButtonSprite (MarkupTheme.ButtonState.Pressed);
                    sprites.highlightedSprite = theme.GetButtonSprite (MarkupTheme.ButtonState.Highlighted);
                    sprites.disabledSprite = theme.GetButtonSprite (MarkupTheme.ButtonState.Disabled);
                    btn.spriteState = sprites;
                    break;
            }

            img.sprite = theme.GetButtonSprite (MarkupTheme.ButtonState.Normal);
            img.type = Image.Type.Sliced;
            btn.targetGraphic = img;
            btn.transition = transition;

            MarkupUtils.SetSize (widget, node);
            MarkupUtils.SetRotation (widget, node);
            MarkupUtils.SetOffset (widget, node);
            MarkupUtils.SetHidden (widget, node);

            attrValue = node.GetAttribute (HashedDisabled);
            var disabled = string.CompareOrdinal (attrValue, "true") == 0;

            btn.interactable = !disabled && MarkupUtils.ValidateInteractive (widget, node);

            return widget;
        }
    }
}