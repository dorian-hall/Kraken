using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class UIAnimalChcker : MonoBehaviour
{
    public List<AnimalImage> AnimalImages = new List<AnimalImage>();
    
    // Start is called before the first frame update
    void Start()
    {
        AnimalChecker.Instance.AnimalCaught += UpdateAnimal;
    }
    void UpdateAnimal(AnimalChecker.Animal animal)
    {
        for (int i = 0; i < AnimalImages.Count; i++)
        {
            if (AnimalImages[i].animal == animal)
            {
                AnimalImages[i].image.color = Color.white;
                break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    [Serializable]
    public struct AnimalImage
    {
        public AnimalChecker.Animal animal;
        public RawImage image;
        public bool isCaught;
    }
}
