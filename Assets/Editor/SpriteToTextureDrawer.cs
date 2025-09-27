// Place this file under an "Editor" folder: Assets/Editor/SpriteToTextureDrawer.cs
using UnityEngine;
using UnityEditor;

public class SpriteToTextureDrawer : MaterialPropertyDrawer
{
    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
    {
        // Row 1: Sprite field
        Rect r0 = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.BeginChangeCheck();
        var sprite = EditorGUI.ObjectField(r0, label, null, typeof(Sprite), false) as Sprite;
        if (EditorGUI.EndChangeCheck() && sprite != null)
        {
            prop.textureValue = sprite.texture;
        }

        // Row 2: Texture mini thumbnail (still allow direct Texture2D drag)
        Rect r1 = new Rect(position.x, r0.yMax + 2, position.width, EditorGUIUtility.singleLineHeight);
        editor.TexturePropertyMiniThumbnail(r1, prop, "Texture2D", null);
    }

    public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
    {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
}
