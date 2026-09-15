using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class GeminiManager : MonoBehaviour
{
    // ضعي مفتاحك هنا
    private string apiKey = "AQ.Ab8RN6KWNLGRWqWAtsqp_honjOd98282y-vB--JFwYPk2pYRFw";

    // رابط الموديل (نستخدم 1.5-flash لسرعته)
    private string url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key=";

    // دالة لاستقبال سؤال اللاعب
    public void AskGemini(string userPrompt)
    {
        StartCoroutine(SendRequest(userPrompt));
    }

    IEnumerator SendRequest(string prompt)
    {
        // 1. تجهيز السؤال بصيغة JSON تناسب Gemini
        string jsonBody = "{\"contents\":[{\"parts\":[{\"text\":\"" + prompt + "\"}]}]}";

        UnityWebRequest request = new UnityWebRequest(url + apiKey, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 2. إرسال الطلب والانتظار
        yield return request.SendWebRequest();

        // 3. استقبال الرد
        if (request.result == UnityWebRequest.Result.Success)
        {
            // سيتم طباعة الرد الكامل بصيغة JSON في الكونسول
            Debug.Log("رد جيمني: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("خطأ في الاتصال: " + request.error);
        }
    }
}