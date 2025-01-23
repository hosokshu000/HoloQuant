using UnityEditor;
using UnityEngine;

public class AnimationGeneratorWindow : EditorWindow
{
    private string spritePath = "Path/To/Sprites"; // Default animation frame path 
    private string animationName = "NewAnimation"; // Default animation name
    private int frameRate = 60;

    [MenuItem("Window/Animation Generator")]
    public static void ShowWindow()
    {
        GetWindow<AnimationGeneratorWindow>("Animation Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generate Animation", EditorStyles.boldLabel);
        
        spritePath = EditorGUILayout.TextField("Sprite Path", spritePath);
        animationName = EditorGUILayout.TextField("Animation Name", animationName);
        frameRate = EditorGUILayout.IntField("Frame Rate", frameRate);

        if (GUILayout.Button("Generate Animation"))
        {
            GenerateAnimation();
        }
    }

    private void GenerateAnimation()
    {
        // Create an instance of AnimationGenerator to call the method
        AnimationGenerator generator = new AnimationGenerator
        {
            spritePath = spritePath,
            animationName = animationName,
            frameRate = frameRate
        };

        generator.GenerateAnimation();
    }
}