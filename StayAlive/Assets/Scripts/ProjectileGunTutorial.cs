using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileGunTutorial : MonoBehaviour
{
    //Gun stats
    [SerializeField]
    private int damage, magazineSize, bulletsPerTap;
    [SerializeField]
    private float spread, reloadTime, timeBetweenShooting, timeBetweenShots;
    [SerializeField]
    private bool allowButtonHold;
    
    //bullet
    [SerializeField]
    private GameObject bullet;
    
    //bullet force
    [SerializeField]
    private float shootForce;
    private Queue<GameObject> query;

    int bulletsLeft, bulletsShot;
    
    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    [SerializeField]
    private Camera fpsCam;
    [SerializeField]
    private Transform attackPoint;
    [SerializeField]
    private AudioClip GunShotClip;
    [SerializeField]
    private AudioSource source;
    private Vector2 audioPitch = new Vector2(.9f, 1.1f);

    //Graphics
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private TextMeshProUGUI ammunitionDisplay;
    
    [SerializeField]
    private bool allowInvoke = true;

    private void Awake()
    {
        query = new Queue<GameObject>();
        if (source != null) source.clip = GunShotClip;
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
        
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            GameObject zomb = hit.collider.gameObject;
            if (zomb.tag == "Zombie")
            {
                zomb.GetComponent<ZombieAI>().Damaged(damage);
            }
        }
        else
            targetPoint = ray.GetPoint(75);
        
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        query.Enqueue(currentBullet);
        
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        
        if (muzzleFlash != null)
        {
            var muzzle = Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
            muzzle.transform.parent = attackPoint.transform;
            Destroy(muzzle, 0.5f);
        }

        //Audio
        if (source != null)
        {
            if (source.transform.IsChildOf(transform))
            {
                source.Play();
            }
            else
            {
                AudioSource newAS = Instantiate(source);
                if ((newAS = Instantiate(source)) != null && newAS.outputAudioMixerGroup != null && newAS.outputAudioMixerGroup.audioMixer != null)
                {
                    newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                    newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);
                    
                    newAS.PlayOneShot(GunShotClip);

                    Destroy(newAS.gameObject, 4);
                }
            }
        }

        bulletsLeft--;
        bulletsShot++;
        
        if (allowInvoke)
        {
            Invoke(nameof(ResetShot), timeBetweenShooting);
            allowInvoke = false;
        }

        if (query.Count >= 10)
        {
            Destroy(query.Dequeue());
        }
        
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }
    
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
