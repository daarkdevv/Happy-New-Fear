using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float TranstionSpeed;
    [SerializeField] float threshold = 0.01f; // Small value to determine when to stop the loop
    [SerializeField] public float AlphaV;
    [SerializeField] public bool WillTranstionToSecne;
    [SerializeField] string SecneName;
    [SerializeField] public float TranstionToSecneTime;
    [SerializeField] bool WillPlaySound;
    [SerializeField] AudioSource playerSource;
    [SerializeField] AudioClip[] audioClips;
    

    void Start()
    {
        // Assuming there's only one CanvasGroup in the scene with the tag "CanvasGroup"
        canvasGroup = GameObject.FindGameObjectWithTag("CanvasGroup").GetComponent<CanvasGroup>();
        playerSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        StartCoroutine(LoadLevel(AlphaV,false,0,null,false,null,TranstionSpeed));
    }

    public IEnumerator LoadLevel(float alphaValue , bool TranstionToSecne , float TranstionToSecneTimE,string SecneNamE,bool WillPlaySoundd,AudioClip clipToPlay ,float transtionSpeed)
    {
        Debug.Log("sasa");
        while (Mathf.Abs(canvasGroup.alpha - alphaValue) > threshold)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, alphaValue, transtionSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }

        // Ensure the final alpha is set correctly
        canvasGroup.alpha = alphaValue;

        if(!TranstionToSecne) yield break;

        if(WillPlaySoundd) playerSource.PlayOneShot(clipToPlay);
        
        yield return new WaitForSeconds(TranstionToSecneTimE);

        SceneManager.LoadScene(SecneNamE);

        
        
    }
}
