// Determines whether js is enabled or not
function detectJs(){
  var nojs = document.getElementsByClassName('no-js');
  
  for (var i = 0; i < nojs.length; i++) {
     nojs[i].classList.toggle('no-js');
  }
};
detectJs();
