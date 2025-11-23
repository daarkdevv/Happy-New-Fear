using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class TvManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip[] allVideos;
    [SerializeField] public VideoClip EasAlarmVideo;

    private List<VideoClip> availableVideos;
    private int lastRandomIndex = -1; // Initialize with an invalid index
    private int consecutiveCount = 0; // Count of consecutive repeats

    void Start()
    {
        InitializeVideoList();
        PlayRandomVideo();
        
        // Subscribe to the VideoPlayer's loopPointReached event to know when a video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void InitializeVideoList()
    {
        availableVideos = new List<VideoClip>(allVideos);
    }

    void PlayRandomVideo()
    {
        if (availableVideos.Count == 0)
        {
            InitializeVideoList();
        }

        int randomIndex = GetUniqueRandomIndex();

        if (randomIndex >= 0 && randomIndex < availableVideos.Count)
        {
            VideoClip selectedVideo = availableVideos[randomIndex];
            videoPlayer.clip = selectedVideo;

            Debug.Log("Playing video: " + selectedVideo.name);

            videoPlayer.Play();

            availableVideos.RemoveAt(randomIndex);
        }
        else
        {
            Debug.LogError("Invalid random index: " + randomIndex);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab))
        playEasAlarmVideo();
    }

    public void playEasAlarmVideo()
    {



     videoPlayer.Stop();
     videoPlayer.clip = EasAlarmVideo;
     videoPlayer.Play();


    }

    int GetUniqueRandomIndex()
    {
        // If there's only one video, return its index directly
        if (availableVideos.Count == 1)
        {
            return 0;
        }

        int randomIndex = Random.Range(0, availableVideos.Count);

        // If there are only two videos, we may need a safeguard to avoid infinite loop
        if (availableVideos.Count == 2)
        {
            if (randomIndex == lastRandomIndex && consecutiveCount >= 1)
            {
                randomIndex = (randomIndex + 1) % availableVideos.Count; // Switch to the other video
            }
        }
        else
        {
            // For more than two videos, use a while loop to avoid repeating the same video consecutively
            while (randomIndex == lastRandomIndex && consecutiveCount >= 1)
            {
                randomIndex = Random.Range(0, availableVideos.Count);
            }
        }

        // Update consecutive count and lastRandomIndex
        if (randomIndex == lastRandomIndex)
        {
            consecutiveCount++;
        }
        else
        {
            consecutiveCount = 0;
        }

        lastRandomIndex = randomIndex;

        return randomIndex;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished. Playing next video.");
        PlayRandomVideo();
    }
}
