using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class ChooseOutfit : MonoBehaviour
{
    
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

    

    public string outfitHash;

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
        int gender = Random.Range(0, 2);
        if(gender == 0)
        {
            head.transform.localPosition = Vector3.up * 1;
        }
        else
        {
            head.transform.localPosition = Vector3.up * 1;
        }

        bodyType.sprite = bodyTypeList[gender];
        Debug.Log("my gender is: " + gender);

        int chosenIndex = Random.Range(0, maskSprites.Count);
        mask.sprite = maskSprites[chosenIndex];

        chosenIndex = Random.Range(0, clothesColorList.Count);
        bodyType.color = clothesColorList[chosenIndex];
    }
}
