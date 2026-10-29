$(document).ready(function () {
    $("#importFile").change(function () {
        var fileUpload = $("#importFile").get(0);
        var files = fileUpload.files;

        if (files.length === 0) return;

        var fileData = new FormData();
        fileData.append("file", files[0]);
        fnHandleAjaxRequest({
            url: "/Home/Import",
            method: "POST",
            dataType: "json", 
            contentType: false,
            processData: false,
            data: fileData,
            onSuccess: function (resultData) {
                var $tbody = $("#tblImportInvoice tbody");
                $tbody.empty();
                if (resultData) {
                    var validationCount = 0;
                    $.each(resultData, function (index, item) {
                        var errorHtml = "";
                        if (item.Validation && item.Validation.length > 0) {
                            errorHtml = `<ol>`;
                            $.each(item.Validation, function (i, msg) {
                                errorHtml += `<li>${msg.replace(/"/g, '&quot;')}</li>`;
                            });
                            errorHtml += `</ol>`;
                            validationCount = item.Validation.length;
                        }
                        var row = `
                                <tr id="tr_${index}" class="main-row">
                                 <td class="text-success">${errorHtml}</td>
                                    <td>${item.CompanyName}</td>
                                    <td>${item.ContactPerson}</td>
                                    <td>${item.Category}</td>
                                    <td>${item.PhoneNumber}</td>
                                    <td>${item.EmailAddress}</td>
                                    <td>${item.Website}</td>
                                </tr>
                            `;
                        $tbody.append(row);
                    });
                }
            },
            onError: function () {
                showToastr("Failed to import!");
            }
        });

    });

    let processedRecords = 0;
    const totalRecord = $("#tblImportInvoice tbody tr").length;

    $("#btn-upload").on("click", function () {
        const templateType = $("#categoryDropdown").val();

        if (!templateType) {
            alert("Please select an email template.");
            return;
        }

        toggleLoading(true); // optional spinner

        $("#tblImportInvoice tbody tr").each(function (index) {
            const $row = $(this);
            const contactPerson = $row.find("td:eq(2)").text();
            const emailAddress = $row.find("td:eq(5)").text();
            const loaderTD = $row.find("td:eq(0)"); // status column

            // Inline validation per row
            if (!contactPerson || !emailAddress) {
                loaderTD
                    .removeClass("alert alert-success alert-danger")
                    .addClass("alert alert-warning")
                    .html("Missing contact or email");
                processedRecords++;
                return;
            }

            // Show "Sending..." while waiting
            loaderTD
                .removeClass("alert alert-success alert-danger")
                .addClass("alert alert-info")
                .html("Sending...");

            // Staggered AJAX call
            setTimeout(function () {
                $.ajax({
                    url: '/Home/SendEmail',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({
                        templateType: templateType,
                        ContactPerson: contactPerson,
                        EmailAddress: emailAddress
                    }),
                    success: function (responseData) {
                        debugger;
                        if (responseData) {
                            if (responseData.success == true) {
                                loaderTD.removeClass("alert alert-info alert-danger").addClass("alert alert-success").html("✅ Email sent successfully!");
                            } else {
                                loaderTD.removeClass("alert alert-info alert-danger").addClass("alert alert-danger").html("❌ Failed to send email.");
                            }
                        }
                        processedRecords++;
                        if (processedRecords === totalRecord)
                            toggleLoading(false);
                    },
                    error: function () {
                        loaderTD
                            .removeClass("alert alert-info alert-success")
                            .addClass("alert alert-danger")
                            .html("❌ Failed to send email.");
                        processedRecords++;
                        if (processedRecords === totalRecord) toggleLoading(false);
                    }
                });
            }, 1000 * index); // stagger each call by 300ms
        });
    });

});


function fnValidation() {
    var isValid = true;

    var templateType = $.trim($("#categoryDropdown").val());
    
    if (templateType == null || templateType == "" || templateType == undefined) {
        $("#categoryDropdown").next("span").text("Please select an email template!");
        isValid = false;
    } else {
        $("#categoryDropdown").next("span").text("");
    }
    
    return isValid;
}
