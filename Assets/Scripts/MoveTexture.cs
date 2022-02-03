using UnityEngine;

public class MoveTexture : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2( .5f, 0f );
    public string textureName = "_BaseMap";


 Vector2 uvOffset = Vector2.zero;
 void LateUpdate() 
 {
     uvOffset += ( uvAnimationRate * Time.deltaTime );
     if( GetComponent<MeshRenderer>().enabled )
     {
         GetComponent<MeshRenderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
     }
 }
}
