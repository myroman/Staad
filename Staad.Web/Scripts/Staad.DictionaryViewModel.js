Staad.namespace('Staad.Utils.WordFactory');
Staad.Utils.WordFactory = (function () {
  return {
    CreateObservableFromModel: function (m) {
      return {
        id: m.Id,
        dictId: m.DictionaryId,
        original: ko.observable(m.Original),
        definition: ko.observable(m.Definition),
        example: ko.observable(m.Example),
        checked: ko.observable(false)
      };
    },
    CopyObservable: function (m) {
      if (m == null) {
        return {
          original: ko.observable(''),
          definition: ko.observable(''),
          example: ko.observable(''),
          checked: ko.observable(false)
        };
      }
      return {
        original: ko.observable(m.original()),
        definition: ko.observable(m.definition()),
        example: ko.observable(m.example()),
        checked: ko.observable(m.checked())
      };
    },
    CreatePoco: function (m, dictId) {
      return {
        Id: m.id,
        DictionaryId: dictId,
        Original: m.original(),
        Definition: m.definition(),
        Example: m.example(),
        Checked: m.checked()
      };
    }
  };
})();

//
function DictionaryViewModel(model) {
  var that = this,
      wordDlg = new Staad.WordDialog(),
      request = new Staad.DictRequest(),
      currentWordId = 0,
      onWordDlgClose = function() {
        that.canEdit(false);
      },
      wordFactory = Staad.Utils.WordFactory;

  wordDlg.Init({
    dialogModalSelector: '#dialog-modal',
    onCloseCallback: onWordDlgClose
  });

  that.words = ko.observableArray();
  $.each(model.Words, function(i, v) {
    var x = wordFactory.CreateObservableFromModel(v);
    that.words.push(x);
  });

  var getWordById = function(id) {
    var matches = that.words().filter(function(x) { return x.id == id; });
    if (matches.length == 0) {
      return { id: 0 };
    }
    return matches[0];
  };
  var getIndexOfWord = function(wordId) {
    for (var i = 0; i < that.words().length; ++i) {
      if (that.words()[i].id == wordId) {
        return i;
      }
    }
    return -1;
  };

  that.getWordToEdit = function() {
    return getWordById(currentWordId);
  };

  // Word dialog
  that.dlgWord = null;
  that.canEdit = ko.observable(false);
  that.editWord = function(it) {
    currentWordId = it.id;
    that.dlgWord = function(found) {
      return wordFactory.CopyObservable(found);
    }(getWordById(currentWordId));
    that.canEdit(true);
    wordDlg.Show();
  };

  that.saveWordInDlg = function () {
    var indexOfWord = -1;
    if (currentWordId != 0) {
      indexOfWord = getIndexOfWord(currentWordId);
      that.dlgWord.id = currentWordId;
    } else {
      that.words.push(wordFactory.CopyObservable(that.dlgWord));
      indexOfWord = that.words().length - 1;
    }
    wordDlg.Close();
    saveWordsToServer([wordFactory.CreatePoco(that.dlgWord, model.Id)], [indexOfWord]);

    that.canEdit(false);
    that.dlgWord = null;
  };

  // requests to handler
  function saveWordsToServer (wordsToSave, theirIndexes) {
    request.Post({
      entity: 'word',
      action: 'save',
      words: JSON.stringify(wordsToSave),
      theirIndexes: JSON.stringify(theirIndexes)
    },
      function (data) {
        var i, ind;
        for (i = 0; i < data.length; ++i) {
          ind = data[i].index;
          if (ind >= that.words().length) continue;

          that.words()[ind].id = data[i].Id;
        }
      },
      function (resp) {
      });
  }

  function deleteWordsFromSrv(ids) {
    
  }

  // add
  that.addWord = function() {
    currentWordId = 0;
    that.dlgWord = wordFactory.CopyObservable(null);
    that.canEdit(true);
    wordDlg.Show();
  };

  // checking/deleting
  that.hasSelectedItems = ko.computed(function() {
    var i;
    for (i = 0; i < this.words().length; ++i) {
      if (this.words()[i].checked()) {
        return true;
      }
    }
    return false;
  }, that);

  that.getSelected = function(transform) {
    var res = [];
    $.each(that.words(), function(i, v) {
      if (v.checked()) {

        var pocoWord = wordFactory.CreatePoco(v, model.Id);
        if (transform) {
          pocoWord = transform(pocoWord);
        }
        res.push(pocoWord);
      }
    });
    return res;
  };

  that.removeChecked = function() {
    var ids = that.getSelected().map(function(x) {
      return x.Id;
    });
    request.Post({
        entity: 'word',
        action: 'delete',
        ids: JSON.stringify(ids)
      },
      function(resp) {
        that.words.remove(function(x) {
          return x.checked();
        });
      },
      function(resp) {
      });
  };
  
  that.allWordsChecked = ko.observable(false);
  that.checkAllWords = function () {
    $.each(that.words(), function (i, v) {
      v.checked(that.allWordsChecked());
    });
    return true;
  };
  
  that.getClassForDeleteBtn = ko.computed(function() {
    return that.hasSelectedItems() ? 'button_blue' : 'button_grey';
  });
}

$(document).ready(function () {
  var hdnData = $('.dict input[type=hidden]');
  if (hdnData.length == 0) return;
  if (typeof hdnData.val())
    var model = $.parseJSON(hdnData.val());
  var viewModel = new DictionaryViewModel(model);
  ko.applyBindings(viewModel);
});