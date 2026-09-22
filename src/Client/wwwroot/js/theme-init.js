// Set theme before first paint to avoid a flash of the wrong colour scheme.
// Runs as an external, synchronous <script> so it executes before CSS/Blazor
// load, and so it works under a strict script-src 'self' CSP (no inline JS).
(function () {
    // Mark that JS is available, so reveal-on-scroll styling only applies
    // when we can actually un-hide the content again.
    document.documentElement.classList.add('js');
    try {
        var t = localStorage.getItem('theme') ||
            (window.matchMedia('(prefers-color-scheme: light)').matches ? 'light' : 'dark');
        document.documentElement.setAttribute('data-theme', t);
    } catch (e) { document.documentElement.setAttribute('data-theme', 'dark'); }
})();
