window.getClipboard = async () => {
    var contents = await navigator.clipboard.readText();
    return contents;
}

window.setClipboard = async (text) => {
    await navigator.clipboard.writeText(text);
}

window.invokeConfirm = async (message) => {
    var response = confirm(message);
    return response;
}