using System.Collections;
using UnityEngine;

public class Entity2D : MonoBehaviour
{
    public GameObject entitySprite;
    Transform entityTransformAssigned;
    Vector3 Origin2D = new Vector3(0, -140, 0);

    public void InitiateMe(Transform transformAssigned)
    {
        entityTransformAssigned = transformAssigned;
        StartCoroutine(UpdateMyCopy());
    }


    IEnumerator UpdateMyCopy()
    {
        while (entityTransformAssigned)
        {
            entityTransformAssigned.position= new Vector3(transform.position.x, transform.position.y, 0) + Origin2D;
            yield return new WaitForFixedUpdate();
        }


    }

    private void OnDestroy()
    {
        if (entityTransformAssigned)
            Destroy(entityTransformAssigned.gameObject);
    }



}
