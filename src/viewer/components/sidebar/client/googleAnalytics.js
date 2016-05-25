var googleAnalytics = (function googleAnalytics() {
  return {
    send : function send(ga, events) {
       for (var i=0; i<events.length; i++) {
         var eventObj = events[i];
         ga.send(eventObj.category, eventObj.action, eventObj.label);
       }
     }
  };

})();
