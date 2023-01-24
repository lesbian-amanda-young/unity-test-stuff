using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photon : MonoBehaviour
{
    public LayerMask contact;
    public LayerMask ZapPlayer;
    public float damageAmount;

    public float rotationAmount;
    public float speed;

    void Update()
    {
        float rotationZ = Mathf.SmoothStep(0, rotationAmount, Mathf.PingPong(Time.time * speed, 1));
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        RaycastHit2D hitTerrain = Physics2D.Raycast(transform.position, dir, 150f, contact);
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, dir, 150f, ZapPlayer);

        if (hitTerrain.collider == null)
        {
            return;
        }
        transform.localScale = new Vector3(hitTerrain.distance, transform.localScale.y, 1f);

        if (hitPlayer.collider.CompareTag("Player"))
            hitPlayer.collider.GetComponent<HP>().Damage(damageAmount);
    }
}
