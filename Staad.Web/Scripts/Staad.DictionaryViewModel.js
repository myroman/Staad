Staad.namespace('Staad.Utils.WordFactory');
Staad.Utils.WordFactory = (function () {
  return {
    CreateObservableFromModel: function (m) {
      return {
        Id: m.Id,
        Original: ko.observable(m.Original),
        Definition: ko.observable(m.Definition),
        Example: ko.observable(m.Example),
        Checked: ko.observable(false)
      };
    },
    CopyObservable: function (m) {
      if (m == null) {
        return {
          Original: ko.observable(''),
          Definition: ko.observable(''),
          Example: ko.observable(''),
          Checked: ko.observable(false)
        };
      }
      return {
        Original: ko.observable(m.Original()),
        Definition: ko.observable(m.Definition()),
        Example: ko.observable(m.Example()),
        Checked: ko.observable(m.Checked())
      };
    }
  };
})();

//
function DictionaryViewModel(model) {
  var that = this,
      wordDlg = new Staad.WordDialog(),
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
  $.each(model.WordsToRenderFirst, function (i, v) {
    var x = wordFactory.CreateObservableFromModel(v);
    that.words.push(x);
  });

  if (model.NeedMoreWords === true) {
    getExtraItems();
  }

  // *** Definitions ***
  function getCurrentDictionaryId() {
    return model.Id;
  }
  var getWordById = function(id) {
    var matches = that.words().filter(function(x) { return x.Id == id; });
    if (matches.length == 0) {
      return { Id: 0 };
    }
    return matches[0];
  };
  var getIndexOfWord = function(wordId) {
    for (var i = 0; i < that.words().length; ++i) {
      if (that.words()[i].Id == wordId) {
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
    currentWordId = it.Id;
    that.dlgWord = function(found) {
      return wordFactory.CopyObservable(found);
    }(getWordById(currentWordId));
    that.canEdit(true);
    wordDlg.Show();
  };

  that.saveWordInDlg = function () {
    var indexOfWord;
    that.dlgWord.DictionaryId = getCurrentDictionaryId();
    that.dlgWord.Id = 0;
    if (currentWordId != 0) {
      indexOfWord = getIndexOfWord(currentWordId);

      var tableWordRef = getWordById(currentWordId);
      tableWordRef.Original(that.dlgWord.Original());
      tableWordRef.Definition(that.dlgWord.Definition());
      tableWordRef.Example(that.dlgWord.Example());
      that.dlgWord.Id = currentWordId;
    } else {
      that.words.push(wordFactory.CopyObservable(that.dlgWord));
      indexOfWord = that.words().length - 1;
    }
    wordDlg.Close();
    
    saveWordsToServer([ko.toJS(that.dlgWord)], [indexOfWord]);

    that.canEdit(false);
    that.dlgWord = null;
  };

  // requests to handler
  function saveWordsToServer(wordsToSave, theirIndexes) {
    var onSuccess = function(data) {
      setErr();
      var i, ind;
      for (i = 0; i < data.length; ++i) {
        ind = data[i].index;
        if (ind >= that.words().length) continue;

        that.words()[ind].Id = data[i].Id;
      }
    };
    doRequest({
      entity: 'word',
      action: 'save',
      words: JSON.stringify(wordsToSave),
      theirIndexes: JSON.stringify(theirIndexes)
    }, onSuccess);
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
      if (this.words()[i].Checked()) {
        return true;
      }
    }
    return false;
  }, that);

  that.getSelected = function() {
    var res = [];
    $.each(that.words(), function(i, v) {
      if (v.Checked()) {
        res.push(ko.toJS(v));
      }
    });
    return res;
  };

  that.removeChecked = function() {
    var ids = that.getSelected().map(function(x) {
      return x.Id;
    });

    var onSuccess = function(resp) {
      setErr();
      that.words.remove(function(x) {
        return x.Checked() && $.inArray(x.Id, resp) != -1;
      });
    };

    doRequest({
      entity: 'word',
      action: 'delete',
      ids: JSON.stringify(ids)
    }, onSuccess);
  };
  
  function doRequest(data, successCallback) {
    $.ajax('/DictionaryHandler.axd', {
      data: data,
      dataType: 'json',
      type: 'post',
      success: function (resp) {
        setErr();
        if (successCallback) successCallback(resp);
      },
      error: function (resp) {
        if (resp && typeof resp.responseText == 'string') {
          setErr(resp.responseText);
        }
      }
    });
  }
  
  that.allWordsChecked = ko.observable(false);
  that.checkAllWords = function () {
    $.each(that.words(), function (i, v) {
      v.Checked(that.allWordsChecked());
    });
    return true;
  };
  
  that.getClassForDeleteBtn = ko.computed(function() {
    return that.hasSelectedItems() ? 'button_blue' : 'button_grey';
  });

  that.currentError = ko.observable('');
  function setErr(s) {
    var errorElem = $('div.msg_error');
    if (s) {
      that.currentError(s);
      errorElem.show();
    } else {
      that.currentError('');
      errorElem.hide();
    }
  }
  
  function getExtraItems() {
    
    var onGetNewItems = function (resp) {
      if (typeof resp != 'object') return;
      $.each(resp, function (i, v) {
        var x = wordFactory.CreateObservableFromModel(v);
        v.DictionaryId = getCurrentDictionaryId();
        that.words.push(x);
      });
    };
    
    doRequest({
      entity: 'word',
      action: 'loadmore',
      offset: that.words().length,
      dictId: getCurrentDictionaryId()
    }, onGetNewItems);
  }
}

$(document).ready(function () {
  var hdnData = $('.dict input[type=hidden]');
  if (hdnData.length == 0) return;
  if (typeof hdnData.val() === 'string') {
    var model = $.parseJSON(hdnData.val());
    var viewModel = new DictionaryViewModel(model);
    ko.applyBindings(viewModel);
  }
});