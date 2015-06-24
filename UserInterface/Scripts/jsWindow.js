(function (global, undefined) {
    var RESTRICTION_ZONE = "main";

    function confirmCallBackFn(arg) {
        radalert("<strong>radconfirm</strong> returned the following result: <h3 style='color: #ff0000;'>" + arg + "</h3>", 420, 220, "Result");
    }

    function promptCallBackFn(arg) {
        radalert("After 7.5 million years, <strong>Deep Thought</strong> answers:<h3 style='color: #ff0000;'>" + arg + "</h3>", 360, 250, "Deep Thought");
    }

    function OpenAlert() {
        radalert('<h4>Welcome to <strong>RadWindow</strong>!</h4>', 330, 200, 'RadAlert custom title');
        return false;
    }
    function OpenConfirm() {
        radconfirm('<h3 style=\'color: #333399;\'>Are you sure?</h3>', confirmCallBackFn, 330, 200, null, 'RadConfirm custom title');
        return false;
    }
    function OpenPrompt() {
        radprompt('<span style=\'color: #333399;\'>What is the answer of Life, Universe and Everything?</span>', promptCallBackFn, 330, 230, null, 'The Question', '42');
        return false;
    }

    function OpenWindow() {
        var wnd = window.radopen("http://www.bing.com", null);
        wnd.set_offsetElementID(wnd.get_windowManager().get_offsetElementID());
        wnd.setSize(400, 400);
        return false;
    }

    function controlWindowButtons(sender, args) {
        GetRadWindowManager()[sender.get_value()]();
    }

    function showWindow(sender, args) {
        sender.set_restrictionZoneID(RESTRICTION_ZONE);
    }

    global.showWindow = showWindow;
    global.OpenWindow = OpenWindow;
    global.confirmCallBackFn = confirmCallBackFn;
    global.promptCallBackFn = promptCallBackFn;
    global.OpenAlert = OpenAlert;
    global.OpenConfirm = OpenConfirm;
    global.OpenPrompt = OpenPrompt;
    global.controlWindowButtons = controlWindowButtons;
})(window);