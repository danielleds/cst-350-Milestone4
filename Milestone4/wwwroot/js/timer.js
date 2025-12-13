let startTime;
let timerInterval;
let loadedTime = 0;

function startStopwatch() {
    startTime = Date.now();
    console.log(loadedTime)
    timerInterval = setInterval(updateDisplay, 250); // Update every 1/4 second
}

function updateDisplay() {
    const elapsedTime = (Date.now() - startTime);

    // get rounded values
    const displaySeconds = Math.round(elapsedTime / 1000) + Math.round(loadedTime / 1000);
    let milliseconds = (Math.round(elapsedTime / 250) * 250) + parseInt(loadedTime);

    document.getElementById("stopwatch").textContent = displaySeconds + "s";
    document.getElementById("internalStopwatchTime").setAttribute("value", milliseconds);
}

function stopStopwatch() {
    clearInterval(timerInterval);
}

function setLoadedTime(time) {
    loadedTime = time;
    console.log(loadedTime)
}