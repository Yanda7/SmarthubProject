window.addEventListener('DOMContentLoaded', event => {

    const datatablesSimple = document.getElementById('forekBaseDT');
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple);
    }
});