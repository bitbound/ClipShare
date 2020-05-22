window.getClipboard = async () => {
    var contents = await navigator.clipboard.readText();
    return contents;
}

window.setClipboard = async (text) => {
    await navigator.clipboard.writeText(text);
}