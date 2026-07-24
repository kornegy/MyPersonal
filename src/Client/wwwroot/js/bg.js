// Interactive particle-network background.
// A field of slowly drifting nodes connect to their neighbours and reach out
// toward the cursor, forming a living "constellation" behind the content.
// Self-contained, theme-aware, and respectful of reduced-motion preferences.
(function () {
    var canvas = document.getElementById('bg-canvas');
    if (!canvas) return;
    var ctx = canvas.getContext('2d');
    if (!ctx) return;

    var reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    var w = 0, h = 0, dpr = Math.min(window.devicePixelRatio || 1, 2);
    var particles = [];
    var mouse = { x: -9999, y: -9999, active: false };

    // Accent colours (indigo + cyan) matched to the site palette.
    var C1 = [99, 102, 241];   // indigo
    var C2 = [34, 211, 238];   // cyan

    function theme() { return document.documentElement.getAttribute('data-theme') || 'dark'; }

    // Line/dot strength adapts so the effect reads on both themes.
    function strength() { return theme() === 'light' ? 0.85 : 1; }

    function targetCount() {
        var area = window.innerWidth * window.innerHeight;
        var n = Math.round(area / 16000);          // density
        return Math.max(24, Math.min(n, window.innerWidth < 700 ? 40 : 90));
    }

    function rand(min, max) { return Math.random() * (max - min) + min; }

    function makeParticles() {
        var count = targetCount();
        particles = [];
        for (var i = 0; i < count; i++) {
            particles.push({
                x: Math.random() * w,
                y: Math.random() * h,
                vx: rand(-0.25, 0.25),
                vy: rand(-0.25, 0.25),
                r: rand(1.1, 2.3),
                c: Math.random() < 0.5 ? C1 : C2
            });
        }
    }

    function resize() {
        w = window.innerWidth;
        h = window.innerHeight;
        dpr = Math.min(window.devicePixelRatio || 1, 2);
        canvas.width = Math.floor(w * dpr);
        canvas.height = Math.floor(h * dpr);
        canvas.style.width = w + 'px';
        canvas.style.height = h + 'px';
        ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
        makeParticles();
    }

    var LINK = 130;      // neighbour link distance
    var MOUSE = 190;     // cursor influence radius

    function draw() {
        ctx.clearRect(0, 0, w, h);
        var s = strength();

        for (var i = 0; i < particles.length; i++) {
            var p = particles[i];

            // Drift + gentle pull toward the cursor.
            if (mouse.active) {
                var mdx = mouse.x - p.x, mdy = mouse.y - p.y;
                var md2 = mdx * mdx + mdy * mdy;
                if (md2 < MOUSE * MOUSE && md2 > 1) {
                    var f = 0.6 / md2;
                    p.vx += mdx * f;
                    p.vy += mdy * f;
                }
            }
            p.x += p.vx; p.y += p.vy;
            p.vx *= 0.99; p.vy *= 0.99;

            // Wrap around the edges.
            if (p.x < -20) p.x = w + 20; else if (p.x > w + 20) p.x = -20;
            if (p.y < -20) p.y = h + 20; else if (p.y > h + 20) p.y = -20;

            // Neighbour links.
            for (var j = i + 1; j < particles.length; j++) {
                var q = particles[j];
                var dx = p.x - q.x, dy = p.y - q.y;
                var d2 = dx * dx + dy * dy;
                if (d2 < LINK * LINK) {
                    var a = (1 - Math.sqrt(d2) / LINK) * 0.32 * s;
                    ctx.strokeStyle = 'rgba(' + p.c[0] + ',' + p.c[1] + ',' + p.c[2] + ',' + a + ')';
                    ctx.lineWidth = 1;
                    ctx.beginPath();
                    ctx.moveTo(p.x, p.y);
                    ctx.lineTo(q.x, q.y);
                    ctx.stroke();
                }
            }

            // Reach toward the cursor.
            if (mouse.active) {
                var cdx = p.x - mouse.x, cdy = p.y - mouse.y;
                var cd2 = cdx * cdx + cdy * cdy;
                if (cd2 < MOUSE * MOUSE) {
                    var ca = (1 - Math.sqrt(cd2) / MOUSE) * 0.55 * s;
                    ctx.strokeStyle = 'rgba(' + C2[0] + ',' + C2[1] + ',' + C2[2] + ',' + ca + ')';
                    ctx.lineWidth = 1;
                    ctx.beginPath();
                    ctx.moveTo(p.x, p.y);
                    ctx.lineTo(mouse.x, mouse.y);
                    ctx.stroke();
                }
            }

            // The node itself.
            ctx.fillStyle = 'rgba(' + p.c[0] + ',' + p.c[1] + ',' + p.c[2] + ',' + (0.55 * s) + ')';
            ctx.beginPath();
            ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2);
            ctx.fill();
        }
    }

    var running = true;
    function loop() {
        if (running) draw();
        requestAnimationFrame(loop);
    }

    // ---- wiring ----
    window.addEventListener('resize', resize);
    window.addEventListener('mousemove', function (e) {
        mouse.x = e.clientX; mouse.y = e.clientY; mouse.active = true;
    }, { passive: true });
    window.addEventListener('mouseout', function () { mouse.active = false; });
    document.addEventListener('visibilitychange', function () {
        running = !document.hidden;
    });

    resize();

    if (reduceMotion) {
        // Static single frame, no animation or cursor interaction.
        draw();
    } else {
        requestAnimationFrame(loop);
    }
})();
