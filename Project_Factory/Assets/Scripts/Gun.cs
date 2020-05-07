using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public float bulletsPerMinute;
    public bool autoFire;
    public float damage;
    public float startSpreadFactor;
    public int maxMagAmmo;
    public int maxReserveAmmo;
    public int projectileAmount;
    public float reloadTime;
    public float recoil;
    public float recoilTime;
    public int recoilDivitions;
    public GameObject bulletColitionParticles;
    public AudioClip fieringSound;
    public AudioClip reloadFinish;
    public AudioClip pump;
    public AudioSource audioSourceFire;
    public AudioSource audioSourceReload;
    public AudioSource audioPump;
    public Animator animator;
    public int currentReserveAmmo;
    public Sprite defaultSprite;
    public Sprite noAmmoSprite;
    public GameObject mag;
    public GameObject magPos;
    public SpriteRenderer sr;

    private float nextTimeToFire = 0f;
    private bool releasedFire;

    Scene scene;

    private int currentMagAmmo;
    private bool isRealoding = false;
    private Text ammoText;
    private float startPosX;
    private float currentRecoil;
    private float spreadFactor;

    private void Start()
    {
        if (!pump)
        {
            pump = reloadFinish;
        }

        audioPump = GameObject.Find("Audio Source (Pump)").GetComponent<AudioSource>();
        audioSourceFire = GameObject.Find("Audio Source (Fire)").GetComponent<AudioSource>();
        audioSourceReload = GameObject.Find("Audio Source (Reload)").GetComponent<AudioSource>();
        animator = GameObject.Find("Player").GetComponent<Animator>();

        spreadFactor = startSpreadFactor;

        startPosX = sr.transform.localPosition.x;
        currentMagAmmo = maxMagAmmo;
        ammoText = GameObject.Find("Ammo Display Text").GetComponent<Text>();

        if (animator.GetBool("FacingRight") == false)
        {
            facingRight = false;

        }
        else
        {
            facingRight = true;
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {

        GetComponentInChildren<SpriteRenderer>().sprite = defaultSprite;
        spreadFactor = startSpreadFactor;

        isRealoding = false;

        if (animator.GetBool("FacingRight") == false)
        {
            facingRight = false;

        }
        else
        {
            facingRight = true;
        }

        currentRecoil = 0;
    }

    void Update()
    {

        if (scene != null)
        {
            if (scene != SceneManager.GetActiveScene())
            {
                ammoText = GameObject.Find("Ammo Display Text").GetComponent<Text>();
            }
        }

        scene = SceneManager.GetActiveScene();

        if (maxReserveAmmo < currentReserveAmmo)
        {
            currentReserveAmmo = maxReserveAmmo;
        }

        if (animator.GetBool("FacingRight") == false)
        {
            facingRight = false;

        }
        else
        {
            facingRight = true;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && releasedFire && currentMagAmmo > 0 && isRealoding == false)
        {
            nextTimeToFire = Time.time + 60/bulletsPerMinute;
            Shoot();
            if (autoFire == false)
            {
                releasedFire = false;
            }
        }

        if (Input.GetButton("Fire1") == false)
        {
            releasedFire = true;
        }

        if (Input.GetButtonDown("Fire1") && currentReserveAmmo > 0 && currentMagAmmo <= 0 && isRealoding == false)
        {
            StartCoroutine("Reload");
        }

        if (Input.GetKeyDown("r") && currentReserveAmmo > 0 && currentMagAmmo != maxMagAmmo && isRealoding == false)
        {
            StartCoroutine("Reload");
        }

        var pos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - pos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += currentRecoil;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (isRealoding == false)
        ammoText.text = "Ammo: " + currentMagAmmo + " / " + currentReserveAmmo;

        if(spreadFactor < 0.00001f)
        {
            spreadFactor = startSpreadFactor;
        }

    }

    void Shoot()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            Vector2 direction = transform.right;

            direction.x += Random.Range(-spreadFactor, spreadFactor);
            direction.y += Random.Range(-spreadFactor, spreadFactor);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity);

            if (hit.collider)
            {
                Instantiate(bulletColitionParticles, hit.point, Quaternion.LookRotation(hit.normal, Vector2.up));
                Enemy enemy = hit.transform.GetComponent<Enemy>(); 
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
        currentMagAmmo--;
        audioSourceFire.clip = fieringSound;
        audioSourceFire.Play();
        StartCoroutine("Recoil");

        if (gameObject.name == "Shotgun")
        {
            StartCoroutine("Pump");
        }
    }

    IEnumerator Recoil()
    {
        spreadFactor += recoil;

        for (int i = 0; i < recoilDivitions; i++)
        {
            spreadFactor -= recoil / recoilDivitions;

            yield return new WaitForSeconds(recoilTime/recoilDivitions);
        }
    }

    IEnumerator Pump()
    {
        yield return new WaitForSeconds(nextTimeToFire - Time.time - pump.length);
        audioPump.clip = pump;
        audioPump.Play();

        GameObject go = Instantiate(mag, magPos.transform.position, magPos.transform.rotation);
        go.transform.localScale = new Vector3(magPos.transform.localScale.x, magPos.transform.localScale.y, magPos.transform.localScale.z);
    }

    IEnumerator Reload()
    {
        isRealoding = true;
        ammoText.text = "Reloading";
        GetComponentInChildren<SpriteRenderer>().sprite = noAmmoSprite;


        if (gameObject.name == "Shotgun")
        {
            var magTemp = currentMagAmmo;

            for (int i = 0; i < maxMagAmmo-magTemp; i++)
            {
                if (currentReserveAmmo > 0)
                {
                    yield return new WaitForSeconds(reloadTime / maxMagAmmo);

                    audioSourceReload.clip = reloadFinish;
                    audioSourceReload.Play();

                    currentMagAmmo += 1;
                    currentReserveAmmo -= 1;
                }
            }

            isRealoding = false;

            GetComponentInChildren<SpriteRenderer>().sprite = defaultSprite;

        }
        else
        {
            GameObject go = Instantiate(mag, magPos.transform.position, magPos.transform.rotation);

            if (!facingRight)
                go.transform.localScale = new Vector3(magPos.transform.localScale.x, -magPos.transform.localScale.y, magPos.transform.localScale.z);

            yield return new WaitForSeconds(reloadTime - reloadFinish.length);

            audioSourceReload.clip = reloadFinish;
            audioSourceReload.Play();

            yield return new WaitForSeconds(reloadFinish.length);

            if (currentMagAmmo + currentReserveAmmo >= maxMagAmmo)
            {
                currentReserveAmmo -= maxMagAmmo - currentMagAmmo;
                currentMagAmmo = maxMagAmmo;
            }
            else
            {
                currentMagAmmo += currentReserveAmmo;
                currentReserveAmmo = 0;
            }
            isRealoding = false;

            GetComponentInChildren<SpriteRenderer>().sprite = defaultSprite;
        }
    }

    private bool _facingRight = false;
    public bool facingRight
    {
        get { return _facingRight; }
        set
        {
            if (_facingRight != value)
            {
                _facingRight = value;
                
                if (_facingRight == true)
                {
                    sr.transform.Rotate(-180, 0, 0);
                    magPos.transform.localPosition = new Vector2(magPos.transform.localPosition.x,magPos.transform.localPosition.y*-1);
                }
                else
                {
                    sr.transform.Rotate(180, 0, 0);
                    magPos.transform.localPosition = new Vector2(magPos.transform.localPosition.x, magPos.transform.localPosition.y * -1);
                }

            }
        }
    }
}
