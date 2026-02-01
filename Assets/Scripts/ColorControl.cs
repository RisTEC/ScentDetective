using UnityEngine;
using System.Collections.Generic;

public class ColorControl : MonoBehaviour
{
    public void SetGrayscale(bool isGray)
    {
        Material[] materials = GetComponent<Renderer>().materials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_StencilID", isGray ? 0.0f : 1.0f);
        }
    }
}
