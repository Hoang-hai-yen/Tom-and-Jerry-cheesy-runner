using UnityEngine;
using UnityEngine.Video;

public class MenuVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/menu_bg.mp4";
        videoPlayer.isLooping = true;
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }
}
