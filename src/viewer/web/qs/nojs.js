// Determines whether js is enabled or not
function detectJs(){
  var nojs = document.getElementsByClassName('no-js');
  while (nojs.length > 0) {
     nojs[0].classList.toggle('no-js');
  }
};
detectJs();
