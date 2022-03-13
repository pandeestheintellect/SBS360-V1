function confirmDialogYesorNo(question, callback, pos, id) {

    confirmDialog("Confirm", question, "Cancel", "Yes", callback, pos, id);
}
function confirmDialog(heading, question, cancelButtonTxt, okButtonTxt, callback, pos, id) {

    var stylePos = "";

    if (pos == 'Y') {
        stylePos = 'style="left: 110px;top: 250px"';
    }

    var confirmModal =
        $('<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content"' + stylePos + '>' +
              '<div class="modal-header" style="background-color:  lightgray ;">' +
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
        if (id != null)
            callback(id);
        else
            callback();
        confirmModal.modal('hide');
    });

    confirmModal.modal({ show: true });

};

function alertDialog(message) {
    alertDialogDefault("Alert", message, "Ok", null);
}
function alertDialogDefault(heading, message, okButtonTxt, callback) {
    var alertModal =
          $('<div class="modal fade in">' +
          '<div class="modal-dialog">' +
          '<div class="modal-content">' +
          '<div class="modal-header" style="background-color:  lightgray ;">' +
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