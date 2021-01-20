using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected Rigidbody rid;

    protected float velY;

    public void SetVelocity(Vector3 velocity)
    {
        rid.velocity = velocity;
    }
    public void SetYVelocity(float ySpeed)
    {
        Vector3 velocity = rid.velocity;
        velocity.y = ySpeed;
        rid.velocity = velocity;
    }
    public void SetVelocityWithoutY(Vector3 velocity)
    {
        velY = rid.velocity.y;
        Vector3 final_velocity = velocity;

        final_velocity.y = velY;

        rid.velocity = final_velocity;
    }
    public void AddForce(Vector3 force)
    {
        rid.AddForce(force);
    }
    public void AddForce(Vector3 force,ForceMode mode)
    {
        rid.AddForce(force,mode);
    }
    public void LimitYVelocity(float minY)
    {
        Vector3 velocity = rid.velocity;

        if (velocity.y < minY)
            velocity.y = minY;

        rid.velocity = velocity;
    }
    public Vector3 GetVelocity()
    {
        return rid.velocity;
    }

    public void EnableGravity(bool yes)
    {
        rid.useGravity = yes;
    }
}
