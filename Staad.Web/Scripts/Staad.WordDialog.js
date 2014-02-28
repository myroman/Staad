Staad.namespace('Staad.WordDialog');
Staad.WordDialog = function () {

  var dlgModal,
      btnSaveWordDlg,
      btnCancelSaveWordDlg,
      txtOriginalWordDlg,
      txtDefinitionDlg,
      txtExample,
      validationSummary,
      onWordUpdateCallback,
    onCloseCallback;

  //
  var init = function (params) {
    // setup
    dlgModal = $(params.dialogModalSelector);
    onWordUpdateCallback = params.onWordUpdateCallback;
    onCloseCallback = params.onCloseCallback;
    
    btnSaveWordDlg = dlgModal.find('.b-wddialog__save');
    btnCancelSaveWordDlg = dlgModal.find('.b-wddialog__cancel');
    txtOriginalWordDlg = dlgModal.find('.b-wddialog__original');
    txtDefinitionDlg = dlgModal.find('.b-wddialog__definition');
    txtExample = dlgModal.find('.b-wddialog__example');
    validationSummary = dlgModal.find('.validation-summary-errors');
    
    //btnSaveWordDlg.click(saveWord);
  };

  //
  var showWord = function () {
    dlgModal.dialog({
      width: 340,
      height: 290,
      modal: true,
      resizable: true,
      close: function (ev, ui) {
        if (onCloseCallback) {
          onCloseCallback(ev, ui);
        }
      }
    });
  };

  var close = function () {
    validationSummary.text('');
    dlgModal.dialog('close');
  };

  //
  var saveWord = function () {


    /*
    $.ajax({
    type: 'post',
    data: $.extend(currWord, {
    action: 'save',
    dataType: 'json'
    }),
    url: '/WordHandler.axd',
    success: function (details) {
    details = $.parseJSON(details);
        
    if (onWordUpdateCallback) {
    if (currWord.id == 0) {
    currWord.id = details.WordId;
    }
    onWordUpdateCallback(currWord);
    }
    dlgModal.dialog('close');
    currWord = null;
    },
    error: function (why) {
    why = $.parseJSON(why);
    validationSummary.text(why.ErrorMsg);
    }
    });*/

  };

  return {
    Init: init,
    Show: showWord,
    Close: close
  };
};