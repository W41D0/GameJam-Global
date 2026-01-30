using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class AssasinAtributesChecklist : MonoBehaviour
{
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
}
