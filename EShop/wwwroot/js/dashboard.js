"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();
$(function () {
    connection.start().then(function () {
        IvokeProducts();
        IvokeOrders();

    }).catch(function (err) {
        return console.error(err.toString());
    });
})
function IvokeProducts(){
    connection.invoke("SendProducts").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivedProducts", function (products) {
    BindProductsToGrid(products)
});

function BindProductsToGrid(products)
{
    $('tblProduct tbody').empty();
    var tr;
    $.each(products, function (index, product) {
        tr = $('<tr/>');
        tr.append(`<td>${index + 1}</td>`);
        tr.append(`<td>${product.id}</td>`);
        tr.append(`<td>${product.name}</td>`);
        tr.append(`<td>${product.price}</td>`);
        tr.append(`<td>${product.quantity}</td>`);
        $('#tblProduct').append(tr);
    })
}

connection.on("ReceivedProductsForGraph", function (productsForGraph) {
    BindProductsToGraph(productsForGraph);
});

function BindProductsToGraph(productsForGraph) {
    var labels = [];
    var data = [];

    $.each(productsForGraph, function (index, item) {
        labels.push(item.category);
        data.push(item.products);
    });

    DestroyCanvasIfExists('canvasProudcts');

    const context = $('#canvasProudcts');
    const myChart = new Chart(context, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: '# of Products',
                data: data,
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}

//Order
function IvokeOrders() {
    connection.invoke("SendOrders").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivedOrders", function (orders) {
    BindOrdersToGrid(orders)
})
function BindOrdersToGrid(orders)
{
    $('tblOrder tbody').empty();
    var tr;
    $.each(orders, function (index, order) {
        tr = $('<tr/>');
        tr.append(`<td>${index + 1}</td>`)
        tr.append(`<td>${order.userName}</td>`)
        tr.append(`<td>${order.totalPrice}</td>`)
        tr.append(`<td>${order.createDate}</td>`)
        $('#tblOrder').append(tr);
    })
}


connection.on("ReceivedOrdersForGraph", function (ordersForGraph) {
    console.log("Received orders for graph:", ordersForGraph);
    BindSalesToGraph(ordersForGraph);
});

function BindSalesToGraph(ordersForGraph) {
    var labels = [];
    var data = [];

    $.each(ordersForGraph, function (index, item) {
        var purchaseDate = new Date(item.purchaseOn);
        var purchaseDateString = purchaseDate.toLocaleDateString('vi-VN'); 
        labels.push(purchaseDateString); 
        data.push(item.totalPrice);
    });
    DestroyCanvasIfExists('canvasSales');

    const context = $('#canvasSales');
    const myChart = new Chart(context, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: '# of Sales',
                data: data,
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}



function DestroyCanvasIfExists(canvasId) {
    let chartStatus = Chart.getChart(canvasId);
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }
}

var backgroundColors = [
    'rgba(255, 99, 132, 0.2)',
    'rgba(54, 162, 235, 0.2)',
    'rgba(255, 206, 86, 0.2)',
    'rgba(75, 192, 192, 0.2)',
    'rgba(153, 102, 255, 0.2)',
    'rgba(255, 159, 64, 0.2)'
];
var borderColors = [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
];