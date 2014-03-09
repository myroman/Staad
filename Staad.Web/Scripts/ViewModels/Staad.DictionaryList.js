Staad.namespace('Staad.Utils.DictFactory');
Staad.Utils.DictFactory = (function () {
  return {
    CreateObsFromModel: function (m) {
      return {
        id: m.Id,
        created: m.Created,
        name: ko.observable(m.Name),
        wordsCount: m.WordsCount,
        checked: ko.observable(false),
        url: m.Url
      };
    },
    CreatePoco: function (m) {
      return {
        Id: m.id,
        Checked: m.checked()
      };
    }
  };
})();
function DictionaryListViewModel(model) {
  var that = this,
      dictFactory = Staad.Utils.DictFactory,
      request = new Staad.DictRequest();

  that.dicts = ko.observableArray();
  $.each(model, function(i, v) {
    var x = dictFactory.CreateObsFromModel(v);
    that.dicts.push(x);
  });

  //removing
  that.hasSelectedItems = ko.computed(function () {
    var i;
    for (i = 0; i < that.dicts().length; ++i) {
      if (this.dicts()[i].checked()) {
        return true;
      }
    }
    return false;
  }, that);

  that.getSelected = function (transform) {
    var res = [];
    $.each(that.dicts(), function (i, v) {
      if (v.checked()) {

        var pocoWord = dictFactory.CreatePoco(v);
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
        entity: 'dict',
        action: 'delete',
        ids: JSON.stringify(ids)
      },
      function(resp) {
        that.dicts.remove(function(x) {
          return x.checked();
        });
      },
      function(resp) {
      });
  };
  that.allChecked = ko.observable(false);
  that.toggleCheckAll = function() {
    $.each(that.dicts(), function (i, v) {
      v.checked(that.allChecked());
    });
    return true;
  };

  that.getClassForDeleteBtn = ko.computed(function() {
    return that.hasSelectedItems() ? 'button_blue' : 'button_grey';
  });
}

$(document).ready(function () {
  var hdn = $('input[id=hdnDicts][type=hidden]');
  if (hdn.length == 0) return;
  var model = $.parseJSON(hdn.val());
  var vm = new DictionaryListViewModel(model);
  ko.applyBindings(vm);
});