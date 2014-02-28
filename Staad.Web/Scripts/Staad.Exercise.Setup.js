Staad.namespace('Staad.Exercise.Setup');
Staad.Exercise.Setup = function () {
  var slider,
      container,
      wordsCountCur,
      wordsCountMax,
      hdnWordsCountCur,
      btnShowSelectExerDlg,
      exerSelectDialog;

  return {
    Init: function (params) {
      container = $(params.containerSelector);
      slider = container.find(params.sliderSelector);
      wordsCountCur = container.find(params.wordsCountCurSelector);
      wordsCountMax = container.find(params.wordsCountMaxSelector);
      hdnWordsCountCur = container.find(params.hdnwordsCountCur_slc);
      btnShowSelectExerDlg = container.find(params.btnShowSelectExerDlg_slc);
      exerSelectDialog = container.find(params.exerSelectDialog_slc);

      slider.slider({
        value: wordsCountCur.text(),
        max: wordsCountMax.text(),
        slide: function (ev, ui) {
          wordsCountCur.text(ui.value);
          hdnWordsCountCur.val(ui.value);
        }
      });

      //container is accordion
      $('#accordion').accordion({
        collapsible: true,
        animate: false,
        active: 1
      });

      $(".b-exercises").sortable({
        placeholder: "ui-state-highlight"
      });
      $(".b-exercises").disableSelection();

      btnShowSelectExerDlg.click(function () {
        exerSelectDialog.dialog({
          width: 500,
          height: 500,
          modal: true,
          resizable: false
        });
      });
      btnShowSelectExerDlg.click();
    }
  };
};

(function ($) {
  $(document).ready(function() {
    var control = new Staad.Exercise.Setup();
    control.Init({
      containerSelector: '.exer-settings',
      sliderSelector: '.b-slider',
      wordsCountCurSelector: '.exer__settings__words__cur',
      wordsCountMaxSelector: '.exer__settings__words__max',
      hdnwordsCountCur_slc: 'input[type=hidden]#WordsInLesson',
      btnShowSelectExerDlg_slc: 'a.b-exercises-add',
      exerSelectDialog_slc: '.exercises-select'
    });
  });
})(jQuery);