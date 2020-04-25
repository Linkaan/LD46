using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// like have the fire reduce in size if we have a lot of enemies around because of reasons
public class fireSizeReducer : MonoBehaviour
{

    public float likeHowMuchOneEnemyIsWorth;
    public float likeTheStrengthOfTheFire;

    public GameObject buttonToHide;
    public GameObject playerToEnable;

    public int fuelCountInverse;

    public Slider sliderUi;

    public Light theLight;

    int lastEnemyCount;

    /*ParticleSystem myParticleSystem;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.MainModule mainModule;*/

    // Start is called before the first frame update
    void Start()
    {
        /*myParticleSystem = this.GetComponent<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
        mainModule = myParticleSystem.main;*/
        Time.timeScale = 0;
    }

    public void addFuel()
    {
        fuelCountInverse -= 2;

        if (fuelCountInverse < 0) fuelCountInverse = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var value = likeTheStrengthOfTheFire - likeHowMuchOneEnemyIsWorth * fuelCountInverse;
        
        sliderUi.value = value;
        if (lastEnemyCount != fuelCountInverse && value >= 0)
        {
            //emissionModule.rateOverTime = value;
            theLight.range = value / 10;
            lastEnemyCount = fuelCountInverse;

            if (value == 0)
            {
                StartCoroutine(gameOverInSomeTime());
            }
        }
    }

    IEnumerator gameOverInSomeTime()
    {
        Debug.Log("GAMEOVER");
        yield return new WaitForSeconds(3f);
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        buttonToHide.SetActive(false);
        playerToEnable.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
    }
}
