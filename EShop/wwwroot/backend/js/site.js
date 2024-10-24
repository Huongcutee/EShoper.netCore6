
$(function () {

    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Confirm deletion")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }

});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgpreview").attr("src", e.target.result).width(200).height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}

/*
Format tiền tệ
*/
// Hàm định dạng tiền VNĐ (không có ký hiệu tiền tệ)
function formatCurrencyVND(amount) {
    return amount.toLocaleString('vi-VN');
}

// Hàm định dạng giá trị trong các phần tử có class 'cart_total_price'
function formatPrices() {
    // Lấy tất cả các phần tử có class 'cart_total_price'
    var elements = document.querySelectorAll('.formatPrice');

    elements.forEach(function (element) {
        // Lấy giá trị của phần tử, convert thành số và định dạng
        var amount = parseFloat(element.innerText.replace(/\./g, ''));
        if (!isNaN(amount)) {
            element.innerText = formatCurrencyVND(amount) + " VNĐ";
        }
    });
}

// Gọi hàm khi trang được tải xong
window.onload = formatPrices;