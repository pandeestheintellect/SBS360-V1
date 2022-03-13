function confirmDialogYesNoDelete(question, deletecallback,  option) {
    confirmDialogDelete("Confirm", question, "Submit", "Cancel", deletecallback,  option);
}
function confirmDialogDelete(heading, question, submitButtonText, cancelButtonText, deletecallback,  option) {
    var selectOption = "";
    var confirmModalDelete = "";
    if (option == 1) {
        selectOption =
            '<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
              '<div class="modal-header">' +
                '<button id="closeButton" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                '<h4 class="modal-title">' + heading + '</h4>' +
              '</div>' +

              '<div class="modal-body">' +
                '<p>' + question + '</p>' +
                 '<p>' +
                 '<label style="font-weight: normal !important;">' +
                '<input type="radio"  name="feed_option" id="radio_1" />' +
                 ' Delete all unpublished articles </br>' +
                 '</label>' +
                 '<label style="font-weight: normal !important;">' +
                 '<input type="radio"  name="feed_option" id="radio_2" checked="checked" />' +
                 ' Delete only those that are not tagged to projects </br>' +
                 '</label>' +
                 '</p>' +
              '</div>' +
              '<div class="modal-footer">' +
                '<button id="submitButton" class="btn btn-primary">' +
                  submitButtonText +
                '</button>' +
                '<button  id="cancelButton" class="btn" data-dismiss="modal">' +
                  cancelButtonText +
                '</button>' +
              '</div>' +

           '</div>' +
           '</div>' +
        '</div>';
    } else {
        selectOption =
            '<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
              '<div class="modal-header">' +
                '<button id="closeButton" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                '<h4 class="modal-title">' + heading + '</h4>' +
              '</div>' +

              '<div class="modal-body">' +
                '<p>' + question + '</p>' +
                '<p>' +
                '<input type="radio"  name="feed_option" id="radio_1" />' +
                 ' Delete all unpublished articles </br>' +
                 '<input type="radio"  name="feed_option" id="radio_2" checked="checked"/>' +
                 ' Delete only those that are not tagged to other projects </br>' +
                 '</p>' +
              '</div>' +
              '<div class="modal-footer">' +
                '<button id="submitButton" class="btn btn-primary">' +
                  submitButtonText +
                '</button>' +
                '<button  id="cancelButton" class="btn" data-dismiss="modal">' +
                  cancelButtonText +
                '</button>' +
              '</div>' +

           '</div>' +
           '</div>' +
        '</div>'
    }
    confirmModalDelete =
         $(selectOption);

    confirmModalDelete.find('#submitButton').click(function (event) {

        if ($('input[id="radio_1"]').is(':checked')) {
            deletecallback();
            confirmModalDelete.modal('hide');
        }
       
    });
    confirmModalDelete.modal({ show: true });
};

function confirmDialogYesorNoClipper(confirmMsg, question, yescallback, nocallback) {
    confirmDialogClipper(confirmMsg, question, "Cancel", "Yes", yescallback, nocallback);
}
function confirmDialogClipper(heading, question, cancelButtonTxt, okButtonTxt, yescallback, nocallback) {
    var confirmModalClipper =
        $('<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
              '<div class="modal-header">' +
                '<button id="closeButton" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                '<h4 class="modal-title">' + heading + '</h4>' +
              '</div>' +

              '<div class="modal-body">' +
                '<p>' + question + '</p>' +
              '</div>' +

              '<div class="modal-footer">' +
                '<button  id="cancelButton" class="btn" data-dismiss="modal">' +
                  cancelButtonTxt +
                '</button>' +
                '<button id="okButton" class="btn btn-primary">' +
                  okButtonTxt +
                '</button>' +
              '</div>' +

           '</div>' +
           '</div>' +
        '</div>');

    confirmModalClipper.find('#okButton').click(function (event) {
        yescallback();
        confirmModalClipper.modal('hide');
    });

    confirmModalClipper.find('#cancelButton').click(function (event) {
        nocallback();
        confirmModalClipper.modal('hide');
    });
    confirmModalClipper.find('#closeButton').click(function (event) {
        nocallback();
        confirmModalClipper.modal('hide');
    });
    confirmModalClipper.modal({ show: true });
};

function confirmDialogYesorNo(question, callback, pos) {
    confirmDialog("Confirm", question, "Cancel", "Yes", callback, pos);
}
function confirmDialog(heading, question, cancelButtonTxt, okButtonTxt, callback, pos) {

    var stylePos = "";

    if (pos == 'Y') {
        stylePos = 'style="left: 110px;top: 250px"';
    }

    var confirmModal =
        $('<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content"' + stylePos + '>' +
              '<div class="modal-header">' +
                '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                '<h4 class="modal-title">' + heading + '</h4>' +
              '</div>' +

              '<div class="modal-body">' +
                '<p>' + question + '</p>' +
              '</div>' +

              '<div class="modal-footer">' +
                '<button class="btn" data-dismiss="modal">' +
                  cancelButtonTxt +
                '</button>' +
                '<button id="okButton" class="btn btn-primary">' +
                  okButtonTxt +
                '</button>' +
              '</div>' +

           '</div>' +
           '</div>' +
        '</div>');

    confirmModal.find('#okButton').click(function (event) {
        callback();
        confirmModal.modal('hide');
    });

    confirmModal.modal({ show: true });

};

function alertDialog(message) {
    alertDialogDefault("Alert", message, "Ok", null);
}
function alertDialogCallBack(message, callback) {
    alertDialogDefault("Alert", message, "Ok", callback);
}
function alertDialogCallBackCancel(message, callback) {
    alertDialogDefaultCancel("Alert", message, "Ok", callback);
}
function alertDialogDefault(heading, message, okButtonTxt, callback) {
    var alertModal =
          $('<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
          '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
            '<h4 class="modal-title">' + heading + '</h4>' +
          '</div>' +

          '<div class="modal-body">' +
            '<p>' + message + '</p>' +
          '</div>' +

          '<div class="modal-footer">' +
            '<button id="okButton" class="btn btn-primary">' +
                  okButtonTxt +
            '</button>' +
          '</div>' +
          '</div>' +
          '</div>' +
        '</div>');

    alertModal.find('#okButton').click(function (event) {
        if (callback != null)
            callback();
        alertModal.modal('hide');
    });

    alertModal.modal({ show: true });
};
function alertDialogDefaultCancel(heading, message, okButtonTxt, callback) {
    var alertModal =
          $('<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
          '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
            '<h4 class="modal-title">' + heading + '</h4>' +
          '</div>' +

          '<div class="modal-body">' +
            '<p>' + message + '</p>' +
          '</div>' +

          '<div class="modal-footer">' +
          '<button id="cancelButton" class="btn btn-primary">Cancel</button>' +
            '<button id="okButton" class="btn btn-primary">' +
                  okButtonTxt +
            '</button>' +
          '</div>' +
          '</div>' +
          '</div>' +
        '</div>');

    alertModal.find('#okButton').click(function (event) {
        if (callback != null)
            callback();
        alertModal.modal('hide');
    });

    alertModal.find('#cancelButton').click(function (event) {

        alertModal.modal('hide');
    });

    alertModal.modal({ show: true });
};