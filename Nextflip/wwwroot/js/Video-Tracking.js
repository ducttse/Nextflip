let videoEl;

function getVideoEl() {
    videoEl = document.getElementById("video");
}

function trackWatchTime() {
    if (videoEl == null) {
        getVideoEl();
    }
    return ((videoEl.currentTime / videoEl.duration) >= 0.9);
}