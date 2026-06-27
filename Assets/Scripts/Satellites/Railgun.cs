using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class Railgun : DefensiveSatellite
{
    [SerializeField] LineRenderer line;
    [SerializeField] Transform firePoint;
    Camera mc;
    bool cooldown;

    [SerializeField] float maxRange = 24f;
    //0 is rail beam sfx
    [SerializeField] AudioClip[] sfx;
    //using this as a start surrogate because idk the consequences of overriding start in a grandchild of satellite and i dont want to reckon with the consequence with 5 hours on the line.
    void OnEnable()
    {
        GameObject mcObj = GameObject.FindGameObjectWithTag("MainCamera");
        mc = mcObj.GetComponent<Camera>();

        
    }

    protected override void Update()
    {
        base.Update();
        RotateToMouse();
    }

    void RotateToMouse()
    {
        Vector2 dist = (mc.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        float angle = Mathf.Atan2(dist.y, dist.x);
        Vector3 rotateV = new Vector3(transform.rotation.x, transform.rotation.y, ((angle * Mathf.Rad2Deg) - 90));
        Quaternion rotation = Quaternion.Euler(rotateV);
        transform.rotation = rotation;
    }

    IEnumerator LateStart()
    {
        yield return null;
        rb.angularVelocity = 0f;
    }

    protected override void SeekAliens()
    {
        if (Input.GetButtonDown("Jump") && cooldown == false)
        {
            StartCoroutine(FireRailgun());
        }
    }

    IEnumerator FireRailgun()
    {
        cooldown = true;

        SoundFXManager.instance.PlaySoundEffectClip(sfx[0], transform.position, 1f);

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, ((transform.position).normalized), maxRange, alienLayers);
        StartCoroutine(LineEffect(attackRate));
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                GameObject thisAlien = hit[i].collider.gameObject;
                mc.gameObject.GetComponent<CameraM>().StartCoroutine(thisAlien.GetComponent<Health>().TakeDamage(attackDamage));
            }
        }

        yield return new WaitForSeconds(attackRate);
        cooldown = false;

    }

    IEnumerator LineEffect(float dur)
    {
        GameObject lrObj = GameObject.Instantiate(line.gameObject, firePoint.position, Quaternion.identity);
        Destroy(lrObj, (dur + 0.1f));

        LineRenderer lr = lrObj.GetComponent<LineRenderer>();

        lr.SetPosition(0, firePoint.position);
        Vector2 dir = (mc.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        Vector2 endPoint = dir * maxRange;
        lr.SetPosition(1, endPoint);

        lr.startWidth = 0.1f;
        lr.endWidth = 0.25f;
        for (float i = 0; i < dur; i += Time.deltaTime)
        {
            float norm = i / dur;
            lr.startWidth = Mathf.Lerp(0.1f, 0, norm);
            lr.endWidth = Mathf.Lerp(0.25f, 0, norm);
            yield return null;
        }

        lr.startWidth = 0f;
        lr.endWidth = 0f;

    }
}
