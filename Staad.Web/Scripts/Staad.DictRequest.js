Staad.namespace('Staad.DictRequest');
Staad.DictRequest = function (refreshUrl) {
  var url = '/DictionaryHandler.axd';

  //
  var deleteDict = function (data) {
    $.ajax(url, {
      data: data,
      type: 'post',
      success: onDeleted,
      error: onDeleteError
    });
  };

  //
  var onDeleted = function () {
    window.location = refreshUrl;
  };

  //
  var onDeleteError = function (data) {
//    console.log(data.statusText);
  };

  //
  var saveDict = function (data) {
    // TODO: implement
  };

  var post = function(data, onSuccess, onError) {
    $.ajax(url, {
      data: data,
      type: 'post',
      success: onSuccess,
      error: onError
    });
  };

  return {
    DeleteDict: deleteDict,
    SaveDict: saveDict,
    Post: post
  };
};