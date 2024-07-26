using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreenScript : MonoBehaviour
{
    public Image image; 
    public float fadeDuration = 1f;
    private bool isFadingIn = true;

    private void Start()
    {
        if (image == null)
            image = GetComponent<Image>();

        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        while (true)
        {
            Color startColor = isFadingIn ? new Color(image.color.r, image.color.g, image.color.b, 0f) : image.color;
            Color endColor = isFadingIn ? image.color : new Color(image.color.r, image.color.g, image.color.b, 0f);
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(startColor.a, endColor.a, elapsed / fadeDuration);
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }

            image.color = endColor;
            isFadingIn = !isFadingIn;
            yield return new WaitForSeconds(1f); // Delay between fades
        }
    }
}
