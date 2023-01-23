(function (bootstrap) {
    // apply hover state to linked elements
    document.addEventListener('mouseover', e => {
        const target = e.target;
        const hoverId = target.getAttribute('data-hover');
        if (!hoverId) {
            return;
        }

        const elements = document.querySelectorAll('[data-hover="' + hoverId + '"]');
        elements.forEach(el => el.classList.add('hover'));
    });

    document.addEventListener('mouseout', e => {
        const target = e.target;
        const hoverId = target.getAttribute('data-hover');
        if (!hoverId) {
            return;
        }

        const elements = document.querySelectorAll('[data-hover="' + hoverId + '"]');
        elements.forEach(el => el.classList.remove('hover'));
    });

    // tooltips
    document.addEventListener('mouseover', e => {
        const target = e.target;        

        const text = target.getAttribute('data-tooltip');
        if (!text) {
            return;
        }

        const hasTooltip = target.getAttribute('data-tooltip-ready');
        if (hasTooltip) {
            return;
        }
        
        const tooltip = new bootstrap.Tooltip(target, {
            title: text,
            trigger: 'hover'
        });

        tooltip.show();

        target.setAttribute('data-tooltip-ready', 'true');
    });
})(bootstrap);