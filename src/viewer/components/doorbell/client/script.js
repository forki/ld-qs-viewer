(function(){
    <!--Doorbell User feedback module-->
    window.doorbellOptions = {
        strings: {
            'feedback-button-text': 'Leave feedback',

            'title': 'Your feedback',
            'feedback-textarea-placeholder': '',
            'feedback-label': 'Your comments, suggestions and issues (if applicable)...',
            'email-input-placeholder': '',
            'email-label': 'Your email address',
            'attach-a-screenshot': 'Include screenshot',
            'submit-button-text': 'Submit feedback',
            'add-attachments-label': 'Add an attachment'
        },
        appKey: '0Kr8cH0IhEL3L3W6mxNjQIKFxNP1Ic78RsDPYmxmle77ACqDCOq34CIWcVVnDpDp'
    };


    (function(d, t) {var g = d.createElement(t);g.id = 'doorbellScript';g.type = 'text/javascript';g.async = true;g.src = 'https://embed.doorbell.io/button/2274?t='+(new Date().getTime());(d.getElementsByTagName('head')[0]||d.getElementsByTagName('body')[0]).appendChild(g);}(document, 'script'));

    function ShowDoorbellForm() {
        doorbell.show();
    }
})();
