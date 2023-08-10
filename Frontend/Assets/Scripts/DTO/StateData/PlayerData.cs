
using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Id;
    public string Name;
    public Color Color;
    [HideInInspector]
    public string ColorString;
    public Texture2D Avatar;
    [HideInInspector]
    public byte[] AvatarByteArray;

    internal void GenerateAssets()
    {
        CreateTextureFromByteArray();
        CreateColorFromHexString();
    }

    void CreateTextureFromByteArray()
    {
        if (AvatarByteArray == null || AvatarByteArray.Length == 0)
        {
            Debug.LogError("AvatarByteArray is null or empty.");
            return;
        }

        // Create a new Texture2D
        Avatar = new Texture2D(2, 2); // You can set the initial dimensions, they will be resized during LoadImage.

        // Load the image data into the Texture2D
        if (Avatar.LoadImage(AvatarByteArray))
        {
            // If LoadImage returns true, the image data was successfully loaded into the Texture2D.
            return;
        }
        else
        {
            // If LoadImage returns false, there was an error loading the image data.
            Debug.LogError("Failed to load image data into Avatar.");
            return;
        }
    }
    void CreateColorFromHexString()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(ColorString, out color))
        {
            Color = color;
        }
        else
        {
            Debug.LogError("Invalid hex color string: " + ColorString);
            Color = Color.white;
        }
    }
}
