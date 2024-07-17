using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectControll : MonoBehaviour
{
    private Material outline;
    Renderer renderers;
    List<Material> originalMaterials = new List<Material>();
    List<Material> materialList = new List<Material>();

    private void Start() {
        outline = new Material(Shader.Find("Draw/OutlineShader"));
        renderers = this.GetComponent<Renderer>();
        originalMaterials.AddRange(renderers.sharedMaterials);
    }

    public void selectItem() {
        materialList.Clear();
        materialList.AddRange(originalMaterials);
        materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }

    public void outSelect() {
        materialList.Clear();
        materialList.AddRange(originalMaterials);
        materialList.Remove(outline);

        renderers.materials = materialList.ToArray();
    }
}
