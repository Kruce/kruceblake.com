const video = document.getElementById(`backgroundVideo`);

//1. pause video using intersection observer api 
const handleIntersection = (entries, observer) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) { //video is visible in viewport
            video.play();
        } else { //video is not visible in viewport
            video.pause();
        }
    });
};

const observer = new IntersectionObserver(handleIntersection,
    { //options
        root: null, // relative to the viewport
        rootMargin: `0px`,
        threshold: 0
    });

observer.observe(video);

//2. pause video when page is hidden using page visibility api
let hidden, visibilityChange;
if (typeof document.hidden !== "undefined") {
    hidden = "hidden";
    visibilityChange = "visibilitychange";
} else if (typeof document.msHidden !== "undefined") {
    hidden = "msHidden";
    visibilityChange = "msvisibilitychange";
} else if (typeof document.webkitHidden !== "undefined") {
    hidden = "webkitHidden";
    visibilityChange = "webkitvisibilitychange";
}

// pause video if page is hidden
function handleVisibilityChange() {
    if (document[hidden]) {
        video.pause();
    } else {
        video.play();
    }
}

// add an event listener if supported
if (typeof document.addEventListener !== "undefined" && hidden !== undefined) {
    document.addEventListener(visibilityChange, handleVisibilityChange, false);
}