using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using SimpleFileBrowser;

public class PlayMusic : MonoBehaviour
{
    void Start() {
    }

    IEnumerator playAudio(string path, AudioType type) {
        //var path = "file:///Users/tatsuya/Downloads/sound.mp3";
        var url = "file://"+path;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, type))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log(www.error);
            } else {
                var audioClip = DownloadHandlerAudioClip.GetContent(www);
                AudioSource audio = GetComponent<AudioSource>();
                Debug.Log("audio : " + audio);
                Debug.Log("audioClip : " + audioClip);
                // audio.PlayOneShot(audioClip, 1.0f);
                audio.clip = audioClip;
                audio.Play();
            }
        }
    }

    public void OnClick() {
        Debug.Log("OnClick");
//        var paths = StandaloneFileBrowser.OpenFilePanel( "Open File", "", "", false);
//        Debug.Log("paths : " + paths);
        StartCoroutine( SelectFileAndPlayCoroutine() );
        //StartCoroutine(playAudio());
    }

    void Update() {
        
    }

    IEnumerator SelectFileAndPlayCoroutine()
	{
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Audio", ".mp3", ".wav") );
        FileBrowser.SetDefaultFilter( ".mp3" );
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.Files, false, null, null, "Load Files and Folders", "Load" );
		Debug.Log( FileBrowser.Success );

		if( FileBrowser.Success )
		{
            var path = FileBrowser.Result[0];
            var type = GetAudioType(path);
            if (type != AudioType.UNKNOWN) {
                StartCoroutine(playAudio(path, type));
            }
		}
	}
    static AudioType GetAudioType(string uri) {
        switch (Path.GetExtension(uri).ToLower())
        {
        case ".wav":
            return AudioType.WAV;
        case ".mp3":
            return AudioType.MPEG;
        default:
            return AudioType.UNKNOWN;
        }
    }
}
