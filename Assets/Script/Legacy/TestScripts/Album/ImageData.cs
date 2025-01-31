using UnityEngine;

public class ImageData
{
    public int exp;
    public int friends;
    public int reputation;
    public int atenaGrowth;
    public bool isUploaded;
    public Sprite imageSprite;

    public ImageData(int exp, int friends, int reputation, int atenaGrowth, bool isUploaded, Sprite imageSprite = null)
    {
        this.exp = exp;
        this.friends = friends;
        this.reputation = reputation;
        this.atenaGrowth = atenaGrowth;
        this.isUploaded = isUploaded;
        this.imageSprite = imageSprite;
    }
}
