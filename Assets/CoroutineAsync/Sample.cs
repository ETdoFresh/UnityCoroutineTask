using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CoroutineAsync
{
    public class Sample : MonoBehaviour
    {
        [SerializeField] private Text textUi;
        
        IEnumerator Start()
        {
            textUi.text = "";
            
            yield return CoroutineTask.Delay(1000);
            textUi.text += "You just waited 1 second...";

            yield return CoroutineTask.Run(WaitOneSecond());
            textUi.text += "\nYou just waited another second...";
            
            yield return CoroutineTask.Run(WaitOneSecond);
            textUi.text += "\nYou just waited yet another second...";

            var url = "http://www.example.com";
            yield return CoroutineTask<string>.Run(TryToLoadUrl(url), out var response);;
            textUi.text += $"\nYour download result of {url} is {response.Result}";

            var url2 = "http://www.someRFakeWeBSiTe-34yt5.com";
            yield return CoroutineTask<string>.Run(TryToLoadUrl(url2), out var response2);;
            textUi.text += $"\nYour download result of {url2} is {response2.Result}";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator WaitOneSecond()
        {
            yield return new WaitForSeconds(1);
        }

        private IEnumerator TryToLoadUrl(string url)
        {
            var www = new WWW(url);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                yield return "success";
            }
            else
            {
                yield return "fail";
            }
        }
    }
}