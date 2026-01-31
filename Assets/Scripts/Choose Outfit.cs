using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;

public class ChooseOutfit : MonoBehaviour
{

    bool UniqueFound;
    
    GameObject outfit;
    GameObject head;
    GameObject body;
    SpriteRenderer mask;

    int gender;
    SpriteRenderer bodyType;
    SpriteRenderer clothesStyle;
    SpriteRenderer hairStyle;

    [Header("Gender Nuetral")]
    [SerializeField] List<Sprite> maskSprites;
    [SerializeField] List<Sprite> bodyTypeList;
    [SerializeField] List<Color> clothesColorList;
    [SerializeField] List<Color> hairColorList;

    [Header("Male Specific")]
    [SerializeField] List<Sprite> maleClothesList;
    [SerializeField] List<Sprite> maleHairStyles;

    [Header("Female Specific")]
    [SerializeField] List<Sprite> femaleClothesList;
    [SerializeField] List<Sprite> femaleHairStyles;


    string outfitHash;
    public static List<string> usedOutfitHashes = new List<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        outfit = transform.Find("Outfit").gameObject;
        head = outfit.transform.Find("Head").gameObject;
        body = outfit.transform.Find("Body").gameObject;
        mask = head.transform.Find("Mask").GetComponent<SpriteRenderer>();
        bodyType = body.GetComponent<SpriteRenderer>();
        clothesStyle = body.transform.Find("Clothes").GetComponent<SpriteRenderer>();
        hairStyle = head.transform.Find("Hair").GetComponent<SpriteRenderer>();

        ChooseRandomOutfit();
    }

    void ChooseRandomOutfit()
    {
        
        while(!UniqueFound)
        {
            int hairStyleIndex;
            int clothesIndex;
            gender = Random.Range(0, 2);
            if(gender == 0)//female
            {
                head.transform.localPosition = Vector3.up * 1;
                hairStyleIndex = Random.Range(0, femaleHairStyles.Count);
                hairStyle.sprite = femaleHairStyles[hairStyleIndex];

                clothesIndex = Random.Range(0, femaleClothesList.Count);
                clothesStyle.sprite = femaleClothesList[clothesIndex];
            }
            else//male
            {
                head.transform.localPosition = Vector3.up * 1.2f;
                hairStyleIndex = Random.Range(0, maleHairStyles.Count);
                hairStyle.sprite = maleHairStyles[hairStyleIndex];

                clothesIndex = Random.Range(0, maleClothesList.Count);
                clothesStyle.sprite = maleClothesList[clothesIndex];
            }

            bodyType.sprite = bodyTypeList[gender];

            int maskIndex = Random.Range(0, maskSprites.Count);
            mask.sprite = maskSprites[maskIndex];

            int hairColorIndex = Random.Range(0, hairColorList.Count);
            hairStyle.color = hairColorList[hairColorIndex];

            int bodyColorIndex = Random.Range(0, clothesColorList.Count);
            bodyType.color = clothesColorList[bodyColorIndex];

            outfitHash = $"{gender}-{maskIndex}-{bodyColorIndex}-{clothesIndex}-{hairStyleIndex}-{hairColorIndex}";
            if(!usedOutfitHashes.Contains(outfitHash))
            {
                usedOutfitHashes.Add(outfitHash);
                UniqueFound = true;
            }
        }
    }

    public string getUniqueHash()
    {
        return outfitHash;
    }
}
