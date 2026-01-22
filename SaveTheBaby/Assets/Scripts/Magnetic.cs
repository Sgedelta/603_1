using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Magnetic : MonoBehaviour
{

    [SerializeField] public bool Repellent = false;
    [SerializeField] private Collider2D electricCollider;
    private List<Collider2D> collisionList;

    public bool IsElectric
    {
        get
        {
            collisionList.Clear();
            electricCollider.Overlap(collisionList);
            return( (from collider in collisionList where (collider.gameObject.GetComponent<ElectricSource>() != null) select collider).ToList().Count > 0 );
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collisionList = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
