using UnityEngine;

[System.Serializable]
public class ImageData
{
    public string imageName;   // 이미지 이름
    public bool isUploaded;    // 업로드 여부
    public Sprite imageSprite; // 이미지 Sprite
    public int exp;            // 경험치
    public int reputation;     // 평판
    public int friends;        //친구 수
    public int money;          // 돈
    public int atenaGrowth;    // 성장도

    public ImageData(string imageName, Sprite sprite, int exp, int reputation, int friends, int money, int atenaGrowth)
    {
        this.imageName = imageName;
        this.isUploaded = false; // 기본값: 업로드되지 않음
        this.imageSprite = sprite; // 특정 Sprite 저장
        this.exp = exp;
        this.reputation = reputation;
        this.friends = friends;
        this.money = money;
        this.atenaGrowth = atenaGrowth;
    }
}
