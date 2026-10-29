function fnHandleAjaxRequest({ url, method = 'GET', dataType = 'html', contentType = 'application/json', processData = true, data = null, onSuccess, onError }) {
    $.ajax({
        url: url,
        method: method,
        contentType: contentType,
        dataType: dataType,
        processData: processData,
        data: data,
        beforeSend: function () {
            toggleLoading(true);
        },
        success: function (response) {
            if (typeof onSuccess === 'function') {
                onSuccess(response);
            }
        },
        error: function () {
            if (typeof onError === 'function') {
                onError();
            }
        },
        complete: function () {
            toggleLoading(false);
        }
    });
}
function toggleLoading(show) {
    $('#loading-element').toggle(show);
}