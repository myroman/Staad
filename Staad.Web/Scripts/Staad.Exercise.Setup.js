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
    }
  };
};

function ExerciseSetup(model) {
  var that = this,
      container = $('.exer-settings'),
      exerSelectDialog = container.find('.exercises-select');
      
  that.showAddExerciseDlg = function() {
    exerSelectDialog.dialog({
      width: 500,
      height: 500,
      modal: true,
      resizable: false
    });
  };
  
  $('#accordion').accordion({
    collapsible: true,
    animate: false,
    active: 0
  });

  $('.b-exercises').sortable({
    placeholder: 'ui-state-highlight'
  });
  $('.b-exercises').disableSelection();

  ko.bindingHandlers.slider = {
    init: function (element, valueAccessor, allBindingsAccessor) {
      var options = allBindingsAccessor().sliderOptions || {};
      $(element).slider(options);
      
      ko.utils.registerEventHandler(element, 'slidechange', function (event, ui) {
        var observable = valueAccessor();
        observable(ui.value);
      });
      ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
        $(element).slider('destroy');
      });
      ko.utils.registerEventHandler(element, 'slide', function (event, ui) {
        var observable = valueAccessor();
        observable(ui.value);
      });
    },
    update: function (element, valueAccessor) {
      var value = ko.utils.unwrapObservable(valueAccessor());
      if (isNaN(value)) value = 0;
      $(element).slider('value', value);

    }
  };
  that.wordsCountCurrent = ko.observable(model.WordsInLesson);
  that.wordsCountMax = model.WordsMaxCount;
  
}

(function ($) {
  $(document).ready(function () {
    var hidden = $('.exer-settings #hdnModel');
    if (hidden.length == 0) {
      return;
    }
    var model = hidden.val(),
        vm = new ExerciseSetup($.parseJSON(model));
    ko.applyBindings(vm);

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