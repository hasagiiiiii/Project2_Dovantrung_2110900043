$.ajax({
    url: "/Home/GetCard",
    type: "get",
    contentType: "application/json",
    beforeSend: function () {
        
    },
    success: function (res) {
        let subtotal = 0;
        if (res && res.length > 0) {
            for (var i = 0; i < res.length; i++) {
                var view = res[i];
                subtotal = view.sum
                let show = `
                 <div class="order-card">
                <img src="/${view.img_product}" alt="" class="order-image" />
                <div class="order-detail">
                    <p>${view.detail}</p>
                    <a href="/Home/DeleteCard?id=${view.id_card}"><i class="fas fa-times"></i></a><input data-id=${view.id_card} type="text" class="input-quantity" value="${view.quantity}" />
                </div>
                <h6 class="order-price">${view.siglePrice}</h6>
                </div>
                `;
                $('.order-wrapper').append(show);
            }
            let abc = `
             <p>Subtotal <span>$156</span></p>
            <p>Tax(10%) <span>$15.6</span></p>
            <p>Delivery Free <span>$3</span></p>
            <div class="order-promo">
                <input type="text" placeholder="Apply Voucher" class="input-promo" />
                <button class="button-promo">Find Voucher</button>
            </div>
            <hr class="divider" />
            <p>Total <span class="sumtotal">${subtotal}</span></p>
            `
            $('.order-total').append(abc);

        }
        },
        error: function (err) {
            console.log()
        }

})

