let video;

function getVideoEl() {
    video = document.getElementById("video");
}

function trackWatchTime() {
    return ((video.currentTime / video.duration) >= 0.9);
}