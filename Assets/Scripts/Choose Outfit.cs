using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class ChooseOutfit : MonoBehaviour
{
    
    SpriteRenderer mask;
    Color clothesColor;

    [SerializeField] List<Sprite> maskSprites;
    [SerializeField] List<Color> ColorList;

    public string outfitHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mask = transform.Find("Mask").GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
