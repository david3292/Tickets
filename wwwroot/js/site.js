let firstConection = true;
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/servicio-tickets")
    .build();

connection.start()
    .then(function () {
        console.log("Coneccion exitosa");
        alert('Conexión exitosa');
    })
    .catch(err => console.error(err.toString()));

connection.on('startView', (jsonList) => {
    if (firstConection) {
        console.log(jsonList);
        let listQueue = JSON.parse(jsonList);
        listQueue.forEach(client => {
            agregarACola(client.nameQueue, client.idClient, client.nameCient);
        });
        firstConection = false;
    }    
});

connection.on('agregarCola', (nombreCola, idCliente, nombreCliente) => {
    agregarACola(nombreCola, idCliente, nombreCliente);
});

connection.on('eliminarCola', (idCliente) => {
    console.log(`cola: ${idCliente}`);
    eliminarCliente(idCliente);
});

$(document).on('click', 'button', function () {
    let idCliente = $('#id-cliente').val();
    let nombreCliente = $('#nombre-cliente').val();

    connection.invoke('NuevoCliente', idCliente, nombreCliente);
    event.preventDefault();
    resetCinputs();
});

function agregarACola(nombreCola, idCliente, nombreCliente) {
    let colorItem;
    if (nombreCola === 'cola-1')
        colorItem = 'list-group-item-primary';
    else
        colorItem = 'list-group-item-success';

    let cliente = `<li id="${idCliente}" class="list-group-item ${colorItem}"><div class="form-group"><label class="lbl-id">${idCliente}</label><label class="lbl-name form-control-file">${nombreCliente}</label></div></li>`;
    $(`#${nombreCola}`).append(cliente);
};

function eliminarCliente(idQueue) {
    let colorItem;
    if (idQueue === 'cola-1')
        colorItem = 'text-primary';
    else
        colorItem = 'text-success';

    let idCliente = $(`#${idQueue} li`).first().find('.lbl-id').text();
    let nameClient = $(`#${idQueue} li`).first().find('.lbl-name').text();
    let attentionHour = new Date();
    $(`#${idQueue} li`).first().remove();
    let record = `<li class="list-group-item"><p class="${colorItem}">Atendido   ${attentionHour} ===> ${idQueue}:: Id ${idCliente}:: Nombre ${nameClient}</p></li>`;
    $('#historial-atencion').prepend(record);
};

function resetCinputs() {
    $('#id-cliente').val('');
    $('#nombre-cliente').val('');
    $('#id-cliente').focus();
}