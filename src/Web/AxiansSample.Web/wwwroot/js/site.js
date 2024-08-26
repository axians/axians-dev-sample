// The site.js hold commonly used functions and prototypes

// Post and Get are used to call backend Controller API's
function post(url, payload) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(payload),
            success: function (response) {
                resolve(response);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                reject(thrownError);
                if (xhr.status === 401) {
                    location.reload();
                }
            },
            processData: false,
            async: true
        });
    });
}
function get(url) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                resolve(response);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                reject(thrownError);

                if (xhr.status === 401) {
                    location.reload();
                }
            },
            processData: false,
            async: true
        });
    });
}