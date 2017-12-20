function ConfirmMsg(item, action) {
    var srv = item.data("service");
    var msg = item.data("msg");
    if (action == undefined) action = item.data("action");

    if (action == undefined) action = "deleteit";

    if (localization[srv] == undefined) srv = "common";

    if (msg == undefined) {
        return localization[srv].confirm[action];
    }
    else {
        return localization[srv].confirm.param[action].replace("{msg}", msg);
    }
}

function ConfirmMsg(item, service, action) {
    var srv = service;
    var msg = item.data("msg");

    if (service == undefined) srv = item.data("service");

    if (action == undefined) action = item.data("action");

    if (action == undefined) action = "deleteit";

    if (localization[srv] == undefined) srv = "common";

    if (msg == undefined) {
        return localization[srv].confirm[action];
    }
    else {
        return localization[srv].confirm.param[action].replace("{msg}", msg);
    }
}