function fadeInModal(modalContent) {
    modalContent.animate([
        { opacity: 0 },
        { opacity: 1 }
    ], {
        duration: 200,
        easing: "ease-in-out",
        fill: "forwards"
    });
}