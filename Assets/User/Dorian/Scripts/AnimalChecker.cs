using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalChecker : MonoBehaviour
{
    public enum Animal { Fish,Chicken,Pig}
    public List<Animal> animals = new List<Animal>();
    public List<GameObject> CaughtAnimals = new List<GameObject>();
    public static AnimalChecker Instance;
    public Dictionary<string, Animal> Tag2Animals = new Dictionary<string, Animal>();
    public UnityAction<Animal> AnimalCaught;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Tag2Animals.Add("Fish",Animal.Fish);
        Tag2Animals.Add("Chicken",Animal.Chicken);
        Tag2Animals.Add("Pig", Animal.Pig);
        AnimalCaught += dummy;
    }

    void dummy(Animal animal)
    {
        
    }
    // this has no checkif the game object has already been Checked FIX Later 
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (!Tag2Animals.ContainsKey(other.tag)) {  return; }

        animals.Add(Tag2Animals[other.tag]);
        AnimalCaught.Invoke(Tag2Animals[other.tag]);
    }
}
