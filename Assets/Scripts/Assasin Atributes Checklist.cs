using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AssasinAtributesChecklist : MonoBehaviour
{
    List<string> assasinHashList = new List<string>();
    string hashToShow;

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

    GameObject head;
    GameObject body;
    Image mask;

    int gender;
    Image bodyType;
    Image clothesStyle;
    Image hairStyle;

    void Start()
    {
        head = gameObject.transform.Find("Head").gameObject;
        body = gameObject.transform.Find("Body").gameObject;
        mask = head.transform.Find("Mask").GetComponent<Image>();
        bodyType = body.GetComponent<Image>();
        clothesStyle = body.transform.Find("Clothes").GetComponent<Image>();
        hairStyle = head.transform.Find("Hair").GetComponent<Image>();

        StartCoroutine(pickAssasin());

    }

    public void addAssasinToHashList(string assasinHash)
    {
        assasinHashList.Add(assasinHash);
    }
    
    public void removeAssasinFromList(string assasinHash)
    {
        assasinHashList.Remove(assasinHash);
    }

    public IEnumerator pickAssasin()
    {
        yield return new WaitUntil(() => assasinHashList.Count > 0);
        int chosenAssasin = Random.Range(0, AttendeeBehaviour.numOfAssasinsAlive);
        hashToShow = assasinHashList[chosenAssasin];
        revealAssasinTraits();
    }

    void revealAssasinTraits()
    {
        string[] data = hashToShow.Split("-");

        gender = int.Parse(data[0]);
        int maskIndex = int.Parse(data[1]);
        int bodyColorIndex = int.Parse(data[2]);
        int clothesIndex = int.Parse(data[3]);
        int hairStyleIndex = int.Parse(data[4]);
        int hairColorIndex = int.Parse(data[5]);

        if(gender == 0)//female
            {
                hairStyle.sprite = femaleHairStyles[hairStyleIndex];
                clothesStyle.sprite = femaleClothesList[clothesIndex];
            }
            else//male
            {
                hairStyle.sprite = maleHairStyles[hairStyleIndex];
                clothesStyle.sprite = maleClothesList[clothesIndex];
            }

            bodyType.sprite = bodyTypeList[gender];
            mask.sprite = maskSprites[maskIndex];
            hairStyle.color = hairColorList[hairColorIndex];
            bodyType.color = clothesColorList[bodyColorIndex];
    }
}
