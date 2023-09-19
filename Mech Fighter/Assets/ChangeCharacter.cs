using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /*    public void SelectCharacter(int character)
        {
            //CurrentMechModel = GetComponent<SkinnedMeshRenderer>();
            switch (character)
            {
                case 0: //spawn first character

                    break;
                case 1: //spawn 2nd character

                    break;
                default: //spawn first character as a failsafe

                    break;
            }
        }*/
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.getCharacter() == 0)
        {
            PlayersMech.sharedMesh = HeadlessMech1; //change mesh
            PlayersMech.sharedMaterial = Mech1Mat;
            EnemyMech.sharedMesh = Mech2; //change enemy mesh
            EnemyMech.sharedMaterial = Mech2Mat;
            //change textures
        }
        else if(GameManager.instance.getCharacter() == 1)
        {
            PlayersMech.sharedMesh = HeadlessMech2; //change mesh
            PlayersMech.sharedMaterial = Mech2Mat;
            EnemyMech.sharedMesh = Mech1; //change enemy mesh
            EnemyMech.sharedMaterial = Mech1Mat;
            //change textures
        }
    }
}
