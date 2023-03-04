﻿// CREDIT: https://www.peug.net/en/blazor-manage-disconnects/

// Wait until a reload button appears
new MutationObserver((mutations, observer) => {
    if (document.querySelector('#components-reconnect-modal h5 a')) {
        // Now every 10 seconds, see if the server appears to be back, and if so, reload
        async function attemptReload() {
            // Check the server really is back
            await fetch('');
            location.reload();
        }
        observer.disconnect();
        attemptReload();
        setInterval(attemptReload, 10000);
    }
}).observe(document.body, { childList: true, subtree: true });