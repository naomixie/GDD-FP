using UnityEngine;
using UnityEditor;
using System;
public class CustomShaderGUI : ShaderGUI
{
    MaterialEditor editor;
    MaterialProperty[] properties;
    Material target;

    enum RenderChoice
    {
        NormalVisualized, BlinnPhong, Bump
    }



    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        this.editor = editor;
        this.properties = properties;
        this.target = editor.target as Material;

        RenderChoice renderChoice = RenderChoice.NormalVisualized;
        if (target.IsKeywordEnabled("USE_NORMAL"))
            renderChoice = RenderChoice.NormalVisualized;

        else if (target.IsKeywordEnabled("USE_BLINNPHONG"))
            renderChoice = RenderChoice.BlinnPhong;
        else if (target.IsKeywordEnabled("USE_BUMP"))
            renderChoice = RenderChoice.Bump;

        EditorGUI.BeginChangeCheck();
        renderChoice = (RenderChoice)EditorGUILayout.EnumPopup(new GUIContent("Choose Render Type: "), renderChoice);
        if (EditorGUI.EndChangeCheck())
        {
            if (renderChoice == RenderChoice.NormalVisualized)
                target.EnableKeyword("USE_NORMAL");
            else
                target.DisableKeyword("USE_NORMAL");
            if (renderChoice == RenderChoice.BlinnPhong)
                target.EnableKeyword("USE_BLINNPHONG");
            else
                target.DisableKeyword("USE_BLINNPHONG");
            if (renderChoice == RenderChoice.Bump)
                target.EnableKeyword("USE_BUMP");
            else
                target.DisableKeyword("USE_BUMP");
        }

        if (renderChoice == RenderChoice.NormalVisualized)
        {
            MaterialProperty color = FindProperty("_Color", properties);
            GUIContent colorLabel = new GUIContent(color.displayName);
            editor.ColorProperty(color, "Color");
        }

        else if (renderChoice == RenderChoice.BlinnPhong)
        {
            MaterialProperty texture = FindProperty("_MainTex", properties);
            GUIContent textureLabel = new GUIContent(texture.displayName);
            editor.TextureProperty(texture, "Texture");

            MaterialProperty gloss = FindProperty("_Gloss", properties);
            GUIContent glossLabel = new GUIContent(gloss.displayName);
            editor.FloatProperty(gloss, "Gloss");
        }

        else if (renderChoice == RenderChoice.Bump)
        {
            MaterialProperty texture = FindProperty("_MainTex", properties);
            GUIContent textureLabel = new GUIContent(texture.displayName);
            editor.TextureProperty(texture, "Texture");

            MaterialProperty bumpMap = FindProperty("_BumpMap", properties);
            GUIContent bumpMapLabel = new GUIContent(bumpMap.displayName);
            editor.TextureProperty(bumpMap, "Bump Map");


            MaterialProperty bumpScale = FindProperty("_BumpScale", properties);
            GUIContent bumpScaleLabel = new GUIContent(bumpScale.displayName);
            editor.FloatProperty(bumpScale, "Bump Scale");

            MaterialProperty color = FindProperty("_Color", properties);
            GUIContent colorLabel = new GUIContent(color.displayName);
            editor.ColorProperty(color, "Color");

            MaterialProperty gloss = FindProperty("_Gloss", properties);
            GUIContent glossLabel = new GUIContent(gloss.displayName);
            editor.FloatProperty(gloss, "Gloss");


            MaterialProperty specular = FindProperty("_Specular", properties);
            GUIContent specularLabel = new GUIContent(specular.displayName);
            editor.ColorProperty(specular, "Specular");

        }

    }
}
