using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemScript : MonoBehaviour
{
    public Mesh reference;
    public Material referenceMat;
    Particle[] particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = Emit(100);
    }

    void SetParticlePosition(Particle p, Vector3 pos)
    {
        p.SetPosition(pos);
    }

    Particle[] Emit(int count)
    {
        Particle[] p = new Particle[count];
        for(int i = 0; i < p.Length; i++)
        {
            p[i] = new Particle();
            //p[i].SetPosition(new Vector3(i, 0f, 0f));
        }
        foreach(Particle part in p)
        {
            SetParticlePosition(part, part.GetPosition() + Vector3.up * 10f);
            part.SetForce(Vector3.down * 9.8f * part.particleMass);
        }
        return p;
    }

    Quaternion PointToCamera(int index)
    {
        Vector3 dirVect = particles[index].GetPosition() - Camera.main.transform.position;
        float angleX = Mathf.Atan2(dirVect.y, dirVect.z) * Mathf.Rad2Deg;
        float angleY = Mathf.Atan2(dirVect.z, dirVect.x) * Mathf.Rad2Deg;
        float angleZ = Mathf.Atan2(dirVect.y, dirVect.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(-angleX, -angleY + 90f, -angleZ));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Particle p in particles)
        {
            RaycastHit hit;
            foreach (Particle a in particles)
            {
                if(p != a)
                {
                    p.IsIntersecting(a);
                }
            }
            if (Physics.Raycast(p.GetPosition(), p.GetDirection(), out hit, (p.particleRadius / 2f)))
            {
                float reactionForceX = -p.GetForce().x;
                float reactionForceY = 9.8f * p.particleMass;
                float reactionForceZ = 0f;

                p.AddForce(new Vector3(reactionForceX, reactionForceY, reactionForceZ));

                p.SetPosition(hit.point + Vector3.up * (p.particleRadius / 2f));
            }
            p.SetTime(Time.time);
            //Render particle
            Graphics.DrawMesh(reference, p.GetPosition(), Quaternion.identity, referenceMat, 0);
        }
    }
}
