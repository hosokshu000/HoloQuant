using UnityEngine;
using UnityEditor;
using System.IO;

public class AnimationGenerator : MonoBehaviour
{
    public string spritePath; // Path to PNG frames
    public string animationName; // Name of the animation
    public int frameRate = 60; // Animation frame rate (default 60)

    [ContextMenu("Generate Animation")]
    public void GenerateAnimation()
    {
        // Load all PNGs from the specified path
        string[] spriteFiles = Directory.GetFiles(spritePath, "*.png");

        // Check if any PNGs are found
        if (spriteFiles.Length == 0)
        {
            Debug.LogError("No PNG files found in the specified path: " + spritePath);
            return;
        }

        System.Array.Sort(spriteFiles);

        // Create "Animations" folder if it doesn't exist
        string animationsFolder = "Assets/Animations";
        if (!AssetDatabase.IsValidFolder(animationsFolder))
        {
            AssetDatabase.CreateFolder("Assets", "Animations");
        }

        // Create a new animation clip
        AnimationClip animationClip = new AnimationClip { frameRate = frameRate };

        Sprite[] sprites = new Sprite[spriteFiles.Length];

        // Load sprites into the array
        for (int i = 0; i < spriteFiles.Length; i++)
        {
            // Load sprite as an asset
            sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(spriteFiles[i]);
            
            if (sprites[i] == null)
            {
                Debug.LogError($"Failed to load sprite at path: {spriteFiles[i]}");
                return;
            }
        }

        // Create keyframes for the animation
        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i / (float)frameRate, // Time in seconds
                value = sprites[i]
            };
        }

        EditorCurveBinding curveBinding = new EditorCurveBinding
        {
            path = "", // Use empty string for root object
            type = typeof(SpriteRenderer),
            propertyName = "m_Sprite" // Property to animate
        };

        // Apply keyframes to the animation clip
        AnimationUtility.SetObjectReferenceCurve(animationClip, curveBinding, keyframes);

        string animationPath = $"{animationsFolder}/{animationName}.anim";
        AssetDatabase.CreateAsset(animationClip, animationPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Animation generated and saved: " + animationPath);
    }

    [MenuItem("GameObject/Animation/Generate Animation", false, 10)]
    private static void GenerateAnimationFromMenu()
    {
        // Get the selected GameObject
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
        {
            Debug.LogError("No GameObject selected to generate animation on.");
            return;
        }

        // Get the AnimationGenerator component
        AnimationGenerator generator = selectedObject.GetComponent<AnimationGenerator>();
        if (generator == null)
        {
            Debug.LogError("The selected GameObject does not have an AnimationGenerator component.");
            return;
        }

        // Call the GenerateAnimation method
        generator.GenerateAnimation();
    }
}