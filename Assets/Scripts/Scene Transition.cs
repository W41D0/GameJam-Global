using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    public RectTransform blackBar; 
    public float slideDuration = 0.5f;

    private Image barImage;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        barImage = blackBar.GetComponent<Image>();

        blackBar.anchorMin = Vector2.zero; 
        blackBar.anchorMax = Vector2.one;  
        blackBar.offsetMin = Vector2.zero;
        blackBar.offsetMax = Vector2.zero;

        blackBar.localScale = new Vector3(1.1f, 1.1f, 1f);

        barImage.enabled = false;
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(TransitionRoutine(sceneIndex));
    }

    IEnumerator TransitionRoutine(int sceneIndex)
    {
        float height = blackBar.rect.height;

        float startY = height * 1.2f;

        blackBar.anchoredPosition = new Vector2(0, startY);
        barImage.enabled = true; 

        yield return MoveBar(new Vector2(0, startY), Vector2.zero);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        while (!op.isDone) yield return null;

        yield return MoveBar(Vector2.zero, new Vector2(0, -startY));

        barImage.enabled = false; 
        blackBar.anchoredPosition = new Vector2(0, startY); 
    }

    IEnumerator MoveBar(Vector2 startPos, Vector2 endPos)
    {
        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slideDuration;
            t = Mathf.SmoothStep(0f, 1f, t); 
            
            blackBar.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }
        blackBar.anchoredPosition = endPos;
    }
}