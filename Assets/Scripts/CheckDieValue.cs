using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckDieValue : MonoBehaviour
{
    #region Public Variables
    public Text upperSideText;  // Shows which side is facing up on the UI
    public Vector3Int directionValues;
    #endregion

    #region Private Variables
    private Vector3Int opposingDirectionValues;

    readonly List<string> FaceRepresent = new List<string>() { "?", "1", "2", "3", "4", "5", "6" };
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        opposingDirectionValues = 7 * Vector3Int.one - directionValues;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged)   // Check to see if the die transform has changed
        {
            if (Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f) //x axis a.b.sin theta <45
                                                                             //if ((int) Vector3.Cross(Vector3.up, transform.right).magnitude == 0) //Previously
            {
                if (Vector3.Dot(Vector3.up, transform.right) > 0)
                {
                    upperSideText.text = FaceRepresent[directionValues.x];
                }
                else
                {
                    upperSideText.text = FaceRepresent[opposingDirectionValues.x];
                }
            }
            else if (Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f) //y axis
            {
                if (Vector3.Dot(Vector3.up, transform.up) > 0)
                {
                    upperSideText.text = FaceRepresent[directionValues.y];
                }
                else
                {
                    upperSideText.text = FaceRepresent[opposingDirectionValues.y];
                }
            }
            else if (Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f) //z axis
            {
                if (Vector3.Dot(Vector3.up, transform.forward) > 0)
                {
                    upperSideText.text = FaceRepresent[directionValues.z];
                }
                else
                {
                    upperSideText.text = FaceRepresent[opposingDirectionValues.z];
                }
            }

            transform.hasChanged = false;
        }
    }
    #endregion
}