using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    Vector3 direction;

    Vector3 initalPosition;

    Vector3 position;

    Vector3 velocity;

    Vector3 displacement;

    Vector3 acceleration;

    Vector3 force;

    Vector3 momentum;

    public float particleRadius = 1f;

    public float particleMass = 1f;

    float time = 0f;

    public void SetTime(float nTime)
    {
        acceleration = force / particleMass;
        time = nTime;
        velocity = acceleration * time;
        displacement = velocity * time;
        momentum = velocity * particleMass;
        position = initalPosition + displacement;
    }

    public void IsIntersecting (Particle other)
    {
        float distance = (GetDisplacement() - other.GetDisplacement()).magnitude;
        Vector3 direction = (GetDisplacement() - other.GetDisplacement()).normalized;
        if (distance <= particleRadius && distance > 0f)
        {
            
        }
        else if(distance == 0f)
        {
            other.AddForce(Vector3.right);
            AddForce(Vector3.left);
        }
    }

    public void SetPosition(Vector3 nP)
    {
        initalPosition = nP;
    }

    public void SetForce(Vector3 Force)
    {
        force = Force;
    }

    public void AddForce(Vector3 Force)
    {
        force += Force;
    }

    public void AddImpulseForce(Vector3 Force)
    {
        force += Force;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetDisplacement()
    {
        return displacement;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public Vector3 GetForce()
    {
        return force;
    }

    public Vector3 GetDirection()
    {
        Vector3 currentPosition = (force / particleMass) * Time.time * Time.time;
        direction = (currentPosition - displacement).normalized;
        return direction;
    }
}
