$(document).ready(function () {
    var amountInserted = 0;
    var dollars = 0;
    var quarters = 0;
    var dimes = 0;
    var nickels = 0;
    var currentItem = '';
    var highlightedItem = null;
    var itemsLoaded;

    function loadItems() {
        $.ajax({
            type: 'GET',
            url: 'http://localhost:55524/items',
            success: function (itemsArray) {
                var itemsDiv = $('#vendingItems');
                itemsDiv.empty();
                itemsLoaded = true;

                $.each(itemsArray, function (index, item) {
                    var itemInfo = '';
                    //where I generate the vending items
                    itemInfo += '<div class="col-md-4 item" style="transition:background-color 1s ease">';
                    itemInfo += '<div class="itemID">' + item.id + '</div><br>';
                    itemInfo += '<div class="text-center">';
                    itemInfo += '<h5 class="itemName">' + item.name + '</h5><br>';
                    itemInfo += '<h5>$' + item.price + '</h5><br>';
                    itemInfo += '<h5>Quantity Left: ' + item.quantity + '</h5><br>';
                    itemInfo += '</div></div>';
                    itemsDiv.append(itemInfo);
                });
            },
            error: function () {
                alert("The vending delivery guy hasn't restocked the machine yet.");
                itemsLoaded = false;
            }
        });
    };

    loadItems();

    $('#addDollar').on('click', function () {
        amountInserted += 1;
        dollars++;
        $('#moneyIn').val('$' + amountInserted.toFixed(2));
        $('#shakeMachine').hide('slow');
    });
    $('#addQuarter').on('click', function () {
        amountInserted += .25;
        quarters++;
        $('#moneyIn').val('$' + amountInserted.toFixed(2));
        $('#shakeMachine').hide('slow');
    });
    $('#addDime').on('click', function () {
        amountInserted += .1;
        dimes++;
        $('#moneyIn').val('$' + amountInserted.toFixed(2));
        $('#shakeMachine').hide('slow');
    });
    $('#addNickel').on('click', function () {
        amountInserted += .05;
        nickels++;
        $('#moneyIn').val('$' + amountInserted.toFixed(2));
        $('#shakeMachine').hide('slow');
    });

    $('#makePurchase').on('click', function () {
        if (currentItem != '') {
            vendItem();
        }
        else {
            if (itemsLoaded) {
                $('#messageBox').val('Please make a selection');
            } else {
                $('#messageBox').val('NO ITEMS');
                alert("Sorry, the vending service is currently down. Contact customer service at 1-800-AVENDER");
            }
        }
    });

    function vendItem() {
        $.ajax({
            type: 'GET',
            url: 'http://localhost:55524/money/' + amountInserted + '/item/' + $('#itemIDBox').val(),
            success: function (itemToGet) {
                loadItems();

                var changeReturned = '';

                if (itemToGet.quarters != 0) {
                    changeReturned += "Q's:" + itemToGet.quarters + ' ';
                }
                if (itemToGet.dimes != 0) {
                    changeReturned += "D's:" + itemToGet.dimes + ' ';
                }
                if (itemToGet.nickels != 0) {
                    changeReturned += "N's:" + itemToGet.nickels + ' ';
                }

                currentItem = '';
                amountInserted = 0;
                dollars = 0;
                quarters = 0;
                dimes = 0;
                nickels = 0;
                highlightedItem = null;
                $('#moneyIn').val('$' + 0);
                $('#itemIDBox').val('');
                $('#changeBox').val(changeReturned);

                if (itemToGet.stuck == 'Y') {
                    $('#messageBox').val('ITEM STUCK');
                    $('#shakeMachine').show('slow');
                }
                else {
                    $('#messageBox').val('Thank you!!!');
                }

                if (itemToGet.stuck != 'Y' && itemToGet.stuck != 'N') {
                    alert(itemToGet.stuck);
                }
            },
            error: function (response) {
                $('#messageBox').val(response.responseJSON.message);
            }
        });
    };

    function shakeSuccess() {
        $.ajax({
            type: 'GET',
            url: 'http://localhost:55524/shakesuccess',
            success: function (response) {
                loadItems();
                alert(response)
            },
            error: function (response) {
                alert(response.responseJSON.message);
            }
        })
    }

    $('#changeReturn').on('click', function () {
        amountInserted = 0;
        var changeReturned = '';
        if (dollars != 0) {
            quarters += dollars * 4;
        }
        if (quarters != 0) {
            changeReturned += "Q's:" + quarters + ' ';
        }
        if (dimes != 0) {
            changeReturned += "D's:" + dimes + ' ';
        }
        if (nickels != 0) {
            changeReturned += "N's:" + nickels + ' ';
        }

        dollars = 0;
        quarters = 0;
        dimes = 0;
        nickels = 0;
        if (changeReturned == '') {
            $('#changeBox').val("There's no change to return");
        }
        else {
            $('#changeBox').val(changeReturned);
        }

        $('#moneyIn').val('$' + 0);
        $('#itemIDBox').val('');
        $('#messageBox').val('');
        $('#shakeMachine').hide('slow');
        currentItem = '';
        highlightedItem.css('background-color', 'aliceblue');
        highlightedItem = null;
    });

    $('#vendingItems').on('click', '.item', function () {
        $('#itemIDBox').val($(this).find('.itemID').text());
        currentItem = $(this).find('.itemName').text();
        $('#messageBox').val(currentItem);
        if (highlightedItem != null) {
            highlightedItem.css('background-color', 'aliceblue');
        }
        highlightedItem = $(this);
        highlightedItem.css('background-color', 'plum');
    });

    $('#shakeMachine').on('click', function () {
        alert('You got pissed at a vending machine and attempted to get free food by shaking it.');
        var getItem = Math.floor(Math.random() * 10)
        if (getItem < 4) {
            shakeSuccess();
        } else {
            if (currentItem == 'Snickers') {
                alert("Snickers satisfies, but you however won't be satisfied because you shook the machine too hard and it fell on top of you killing you instantly.");
            } else if (currentItem == 'Take 5') {
                alert('You tried to take 5, but the machine took your life when it fell on top of you kiling you instantly.');
            } else if (currentItem == 'Doritos') {
                alert('¡Ay caramba! You shook the machine too hard esse. It fell on top of you killing you instantly.');
            } else {
                alert('Unfortunately, you shook the machine too hard and it fell on top of you killing you instantly.');
            }

            $('#messageBox').val('lol, I killed you XD');
        }

        amountInserted = 0;
        dollars = 0;
        quarters = 0;
        dimes = 0;
        nickels = 0;
        $('#moneyIn').val('$' + 0);
        $('#itemIDBox').val('');
        $('#shakeMachine').hide('slow');
        currentItem = '';
        highlightedItem.css('background-color', 'aliceblue');
        highlightedItem = null;
    })

    //     $('#vendingItems').hover(function () {
    //         $(this).css('background-color', 'orchid');
    // },
    //     function () {
    //         $(this).css('background-color', 'aquamarine');
    //     });   
});