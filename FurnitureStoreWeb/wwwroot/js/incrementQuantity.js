$(document).ready(function () {
    // Increment button click event
    $('.quantity-btn[data-operation="increment"]').click(function () {
        var inputId = $(this).data('id');
        var inputField = $('#quantityInput_' + inputId);
        var currentValue = parseInt(inputField.val());

        // Increment the value and update the input field
        inputField.val(currentValue + 1);
    });

    // Decrement button click event
    $('.quantity-btn[data-operation="decrement"]').click(function () {
        var inputId = $(this).data('id');
        var inputField = $('#quantityInput_' + inputId);
        var currentValue = parseInt(inputField.val());

        // Decrement the value if it's greater than 0
        if (currentValue > 0) {
            inputField.val(currentValue - 1);
        }
    });
});