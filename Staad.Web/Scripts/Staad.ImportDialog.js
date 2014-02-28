Staad.namespace('Staad.ImportDialog');
Staad.ImportDialog = function () {
  var dlg;

  var init = function (params) {

  };

  var show = function() {

  };

  return {
    Init: init,
    Show: show
  };
};

$(document).ready(function () {
  var control = new Staad.ImportDialog();
  control.Init({
    dlgSelector: '#import-dlg'
  });
});