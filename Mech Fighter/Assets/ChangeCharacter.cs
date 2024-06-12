using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeCharacter : MonoBehaviour
{
    private int character;

    [SerializeField] private SkinnedMeshRenderer PlayersMech;
    [SerializeField] private SkinnedMeshRenderer EnemyMech;
    [SerializeField] private Mesh HeadlessMech1;
    [SerializeField] private Mesh HeadlessMech2;
    [SerializeField] private Mesh Mech1;
    [SerializeField] private Mesh Mech2;
    [SerializeField] private Material Mech1Mat;
    [SerializeField] private Material Mech2Mat;

    private void Awake()
    {
        ServiceLocator.ProvideService(this);
    }

    public void SetChar()
    {
        Debug.Log("You are character " + GameManager.instance.getCharacter());
        switch (GameManager.instance.getCharacter())
        {
            case 0:
                PlayersMech.sharedMesh = HeadlessMech1; //change mesh
                PlayersMech.sharedMaterial = Mech1Mat;
                EnemyMech.sharedMesh = Mech2; //change enemy mesh
                EnemyMech.sharedMaterial = Mech2Mat;
                break;
            case 1:
                PlayersMech.sharedMesh = HeadlessMech2; //change mesh
                PlayersMech.sharedMaterial = Mech2Mat;
                EnemyMech.sharedMesh = Mech1; //change enemy mesh
                EnemyMech.sharedMaterial = Mech1Mat;
                break;
            default:
                Debug.Log("default character");
                break;
        }
    }
}
