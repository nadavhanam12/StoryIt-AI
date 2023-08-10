using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardData
{
    public int Id;
    public Texture2D Picture;
    [HideInInspector]
    public byte[] PictureByteArray;

    internal void GenerateAssets()
    {
        CreateTextureFromByteArray();
    }
    void CreateTextureFromByteArray()
    {
        if (PictureByteArray == null || PictureByteArray.Length == 0)
        {
            Debug.LogError("PictureByteArray is null or empty.");
            return;
        }

        // Create a new Texture2D
        Picture = new Texture2D(2, 2); // You can set the initial dimensions, they will be resized during LoadImage.

        // Load the image data into the Texture2D
        if (Picture.LoadImage(PictureByteArray))
        {
            // If LoadImage returns true, the image data was successfully loaded into the Texture2D.
            return;
        }
        else
        {
            // If LoadImage returns false, there was an error loading the image data.
            Debug.LogError("CardData: Failed to load image data into Picture.");
            return;
        }
    }
}
