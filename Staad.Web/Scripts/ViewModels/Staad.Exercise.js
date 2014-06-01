function Exercise(model) {
  var that = this;

  that.visWord = null;
  that.inited = ko.observable(that.visWord != null);
  console.log(model);
  
  function showSetup() {
    
  }
}

(function($) {
  $(function() {
    var hdnData = $('.b-exr input[type=hidden]');
    if (hdnData.length == 0) return;
    if (typeof hdnData.val() === 'string') {
      var vm = new Exercise($.parseJSON(hdnData.val()));
      ko.applyBindings(vm);
    }
  });
})(jQuery);