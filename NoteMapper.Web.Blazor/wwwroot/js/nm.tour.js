(function () {
    const attrPrefix = 'data-nm-tour';
    const attrs = {
        trigger: `${attrPrefix}-for`,
        container: `${attrPrefix}-container`,
        step: `${attrPrefix}-step`,
        stepArrow: `${attrPrefix}-step-arrow`,
        stepTitle: `${attrPrefix}-step-title`,
        stepMessage: `${attrPrefix}-step-message`
    };

    // Returns a step object for a given DOM element.
    // A step is not returned if it does not have a step attribute, or if its step number < 0
    function getStep(el) {
        if (!el.matches(`[${attrs.step}]`)) {
            return;
        }

        const number = el.getAttribute(attrs.step);
        if (number < 0) {
            return;
        }

        return {
            arrow: el.getAttribute(attrs.stepArrow) !== 'false',
            el: el,
            number: number,
            title: el.getAttribute(attrs.stepTitle),
            message: el.getAttribute(attrs.stepMessage)
        };
    }

    // Returns the valid step objects for the given DOM container.
    // The container itself can be a step.
    function getSteps(container) {
        const stepElements = container.querySelectorAll(`[${attrs.step}]`);
        
        const steps = [
            getStep(container)
        ];
        
        stepElements.forEach(el => {
            const step = getStep(el);
            steps.push(step);
        });

        // Sort steps in ascending order
        return steps
            .filter(x => !!x)
            .sort((a, b) => a.number > b.number);
    }

    // Listen to click events. 
    // Start the tour if the click event was a trigger for a valid tour.
    document.addEventListener('click', e => {
        if (Shepherd.activeTour) {
            return;
        }

        const trigger = e.target.closest(`[${attrs.trigger}]`);
        if (!trigger) {
            return;
        }

        const containerId = trigger.getAttribute(attrs.trigger);
        const container = document.querySelector(`[${attrs.container}="${containerId}"]`);
        if (!container) {
            return;
        }

        const steps = getSteps(container);
        if (steps.length === 0) {
            return;
        }

        const tour = new Shepherd.Tour({
            defaultStepOptions: {
                cancelIcon: {
                    enabled: true
                },
                classes: 'card',
                scrollTo: { behavior: 'smooth', block: 'center' }
            },
            useModalOverlay: true
        });

        steps.forEach((step, i) => {
            const buttons = [];
            if (steps.length > 1) {
                if (i > 0) {
                    buttons.push({
                        action: function () {
                            return this.back();
                        },
                        classes: 'btn btn btn-outline-secondary btn-sm',
                        text: 'Back'
                    });
                }

                buttons.push({
                    action: function () {
                        return this.next();
                    },
                    classes: 'btn btn btn-outline-primary btn-sm',
                    text: i === steps.length - 1 ? 'Close' : 'Next'
                });
            }
            
            tour.addStep({
                arrow: step.arrow !== false,
                attachTo: {
                    element: step.el,
                    on: 'bottom'
                },                
                buttons: buttons,
                title: step.title,
                text: step.message
            });
        });

        tour.start();
    });
})();