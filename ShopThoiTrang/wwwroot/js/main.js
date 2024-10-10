/*price range*/

 $('#sl2').slider();

	var RGBChange = function() {
	  $('#RGB').css('background', 'rgb('+r.getValue()+','+g.getValue()+','+b.getValue()+')')
	};	
		
/*scroll to top*/

$(document).ready(function(){
	$(function () {
		$.scrollUp({
	        scrollName: 'scrollUp', // Element ID
	        scrollDistance: 300, // Distance from top/bottom before showing element (px)
	        scrollFrom: 'top', // 'top' or 'bottom'
	        scrollSpeed: 300, // Speed back to top (ms)
	        easingType: 'linear', // Scroll to top easing (see http://easings.net/)
	        animation: 'fade', // Fade, slide, none
	        animationSpeed: 200, // Animation in speed (ms)
	        scrollTrigger: false, // Set a custom triggering element. Can be an HTML string or jQuery object
					//scrollTarget: false, // Set a custom target element for scrolling to the top
	        scrollText: '<i class="fa fa-angle-up"></i>', // Text for element, can contain HTML
	        scrollTitle: false, // Set a custom <a> title if required.
	        scrollImg: false, // Set true to use image
	        activeOverlay: false, // Set CSS color to display scrollUp active point, e.g '#00FFFF'
	        zIndex: 2147483647 // Z-Index for the overlay
		});
	});
});


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