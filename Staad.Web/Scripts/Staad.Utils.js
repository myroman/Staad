var Staad = Staad || { };
Staad.namespace = function (namespaceStr) {
  var components = namespaceStr.split('.');

  var context = window;

  for (var i = 0; i < components.length; ++i) {
    context[components[i]] = context[components[i]] || {};
    context = context[components[i]];
  }
};