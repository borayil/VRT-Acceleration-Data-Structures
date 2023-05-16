using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOctree : MonoBehaviour
{

    public Octree ot;
    public int maxDepth = 3; // 0 for no splits, 3-4 works well overall.
    private bool drawOctree = false;

    
    void Start()
    {
        ot = new Octree(this.gameObject, maxDepth);
        Draw(ot.rootNode);
    }

    void Update()
    {
        if (!drawOctree)
        {
            return;
        }
        Draw(ot.rootNode);
        // Recalculate octree if transformed
        if (transform.hasChanged)
        {
            ot = new Octree(this.gameObject, maxDepth);
        }
    }

    public void Draw(OctreeNode node)
    {
        // Draw octree ot using Popcron Gizmos package
        Popcron.Gizmos.Bounds(node.nodeBounds, Color.green);
        
        if (node.drawHitpoint)
        {
            if (node.hitpointSphere == null)
            {
                node.hitpointSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Renderer renderer = node.hitpointSphere.GetComponent<Renderer>();
                Material material = new Material(Shader.Find("Diffuse"));
                material.color = Color.green;
                renderer.material = material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
            node.hitpointSphere.transform.position = node.hitpoint;
            node.hitpointSphere.transform.localScale = 0.05f * Vector3.one;
        } else
        {
            if (node.hitpointSphere != null) Object.Destroy(node.hitpointSphere);
        }
         
      
        if (node.children != null)
        {
            for (int i = 0; i < 8; i++)
            {
                if (node.children[i] != null)
                    Draw(node.children[i]);
            }
        }
    }

    public void showOctreeToggle(bool tog)
    {
        drawOctree = !drawOctree;
    }
}
